using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using Website.ViewModels.Keyword;

namespace Service.Services
{
	public interface IKeywordService
	{
		IEnumerable<KeywordViewModel> GetAutoSuggestKeywords(string value);
		KeywordDatatable GetKeywordDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
		int CreateKeyword(KeywordViewModel keyword);
		void UpdateKeyword(KeywordViewModel keyword);
		void DeleteKeyword(int id);
		void SaveKeyword();
	}

	public class KeywordService : IKeywordService
	{
		private readonly IKeywordRepository _keywordRepository;
		private readonly IUnitOfWork _unitOfWork;

		public KeywordService(IAccountRepository accountRepository, IKeywordRepository keywordRepository, IUnitOfWork unitOfWork)
		{
			this._keywordRepository = keywordRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<KeywordViewModel> GetAutoSuggestKeywords(string value)
		{
			var keywords = _keywordRepository.Query(i => (i.KeywordName.Contains(value)));
			var destination = Mapper.Map<IEnumerable<Keyword>, IEnumerable<KeywordViewModel>>(keywords);
			return destination;
		}

		public KeywordDatatable GetKeywordDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			var keywords = _keywordRepository.GetAllQueryable();
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				keywords = keywords.Where(p => p.KeywordName.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var keywordsOrdered = keywords.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var keywordsPaged = keywordsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var destination = Mapper.Map<List<Keyword>, List<KeywordViewModel>>(keywordsPaged);
			var keywordsDatatable = new KeywordDatatable()
			{
				Data = destination,
				Total = keywords.Count()
			};
			return keywordsDatatable;
		}

		public int CreateKeyword(KeywordViewModel vmKeyword)
		{
			var existKeyword = _keywordRepository.Query(p => p.KeywordName.Equals(vmKeyword.KeywordName)).FirstOrDefault();
			if (existKeyword != null)
			{
				return 1;
			}
			var keyword = Mapper.Map<KeywordViewModel, Keyword>(vmKeyword);
			_keywordRepository.Add(keyword);
			SaveKeyword();
			return 0;
		}

		public void UpdateKeyword(KeywordViewModel vmKeyword)
		{
			var keyword = Mapper.Map<KeywordViewModel, Keyword>(vmKeyword);
			_keywordRepository.Update(keyword);
			SaveKeyword();
		}

		public void DeleteKeyword(int id)
		{
			_keywordRepository.Delete(p=>p.Id == id);
			SaveKeyword();
		}

		public void SaveKeyword()
		{
			_unitOfWork.Commit();
		}
	}
}
