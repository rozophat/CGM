using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using Website.ViewModels.Account;
using Website.ViewModels.Category;
using Website.ViewModels.Common;

namespace Service.Services
{
	public interface ICategoryService
	{
		CategoryViewModel GetCategoryInfo(int id);
		IEnumerable<CategoryViewModel> GetAutoSuggestCategories(string value);
		IEnumerable<CategoryViewModel> GetCategories();
		CategoryDatatable GetCategoryDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
		ResponseStatus CreateCategory(CategoryViewModel vmCate);
		ResponseStatus UpdateCategory(CategoryViewModel vmCate);
		void DeleteCategory(int id);
		void SaveCategory();
	}

	public class CategoryService : ICategoryService
	{
		private ICategoryRepository _categoryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
		{
			this._categoryRepository = categoryRepository;
			this._unitOfWork = unitOfWork;
		}

		public CategoryViewModel GetCategoryInfo(int id)
		{

			var cate = (from p in _categoryRepository.GetAllQueryable()
					   join q in _categoryRepository.GetAllQueryable() on p.ParentId equals q.Id into pq
					   from q in pq.DefaultIfEmpty()
					   where p.Id == id
					   select new CategoryViewModel()
					   {
						   Id = p.Id,
						   Name = p.Name,
						   ParentId = p.ParentId,
						   ParentCategoryName = q != null ? q.Name : "",
						   Type = p.Type,
						   Name_ES = p.Name_ES,
						   Name_FR = p.Name_FR,
						   Name_PT = p.Name_PT
						   
					   }).FirstOrDefault();
			if (cate != null)
			{
				return cate;
			}
			return null;
		}

		public IEnumerable<CategoryViewModel> GetAutoSuggestCategories(string value)
		{
			var categories = _categoryRepository.Query(i => (i.Name.Contains(value)));
			var destination = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories);
			return destination;
		}

		public IEnumerable<CategoryViewModel> GetCategories()
		{
			var parentCate = (from p in _categoryRepository.GetAllQueryable()
							 join q in _categoryRepository.GetAllQueryable() on p.ParentId equals q.Id into pq
							 from q in pq.DefaultIfEmpty()
							 where p.ParentId != null
							 select p.ParentId).Distinct();

			var cates = from p in _categoryRepository.GetAllQueryable()
						join q in _categoryRepository.GetAllQueryable() on p.ParentId equals q.Id into pq
						from q in pq.DefaultIfEmpty()
						where !parentCate.Contains(p.Id)
						select new CategoryViewModel()
						{
							Id = p.Id,
							Name = p.Name,
							ParentId = p.ParentId,
							ParentCategoryName = q != null ? q.Name : "",
						};
			return cates;
		}

		public CategoryDatatable GetCategoryDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			var cates = from p in _categoryRepository.GetAllQueryable()
						join q in _categoryRepository.GetAllQueryable() on p.ParentId equals q.Id into pq
						from q in pq.DefaultIfEmpty()
						select new CategoryViewModel()
						{
							Id = p.Id,
							Name = p.Name,
							ParentId = p.ParentId,
							ParentCategoryName = q != null ? q.Name : "",
							Type = p.Type,
							Name_ES = p.Name_ES,
							Name_FR = p.Name_FR,
							Name_PT = p.Name_PT
						};
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				cates = cates.Where(p => p.Name.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var catesOrdered = cates.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var catesPaged = catesOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var categoryDatatable = new CategoryDatatable()
			{
				Data = catesPaged,
				Total = cates.Count()
			};
			return categoryDatatable;
		}

		public ResponseStatus CreateCategory(CategoryViewModel vmCate)
		{
			//check exist category
			var existCate = _categoryRepository.Get(p => p.Name.ToLower().Equals(vmCate.Name.ToLower()));
			if(existCate != null)
			{
				return new ResponseStatus { Successful = false, Message = "The category have already existed." };
			}

			//check exist category
			if (string.IsNullOrEmpty(vmCate.ParentCategoryName))
			{
				vmCate.ParentId = null;
				vmCate.ParentCategoryName = "";
			}
			else
			{
				var existParentCate = _categoryRepository.Get(p => p.Name.ToLower().Equals(vmCate.ParentCategoryName.ToLower()));
				if (existParentCate != null)
				{
					vmCate.ParentId = existParentCate.Id;
				}
				else
				{
					return new ResponseStatus { Successful = false, Message = "The parent category doesn't exist." };
				}
			}
			

			var cate = Mapper.Map<CategoryViewModel, Category>(vmCate);
			_categoryRepository.Add(cate);
			SaveCategory();

			return new ResponseStatus { Successful = true, Message = "" };
		}

		public ResponseStatus UpdateCategory(CategoryViewModel vmCate)
		{
			//check exist category
			var existCate = _categoryRepository.Get(p => p.Name.ToLower().Equals(vmCate.Name.ToLower()) && p.Id != vmCate.Id);
			if (existCate != null)
			{
				return new ResponseStatus { Successful = false, Message = "The category have already existed." };
			}

			//check exist category
			if (string.IsNullOrEmpty(vmCate.ParentCategoryName))
			{
				vmCate.ParentId = null;
				vmCate.ParentCategoryName = "";
			}
			else
			{
				var existParentCate = _categoryRepository.Get(p => p.Name.ToLower().Equals(vmCate.ParentCategoryName.ToLower()));
				if (existParentCate != null)
				{
					vmCate.ParentId = existParentCate.Id;
				}
				else
				{
					return new ResponseStatus { Successful = false, Message = "The parent category doesn't exist." };
				}
			}

			var cate = Mapper.Map<CategoryViewModel, Category>(vmCate);
			_categoryRepository.Update(cate);
			SaveCategory();

			return new ResponseStatus { Successful = true, Message = "" };
		}

		public void DeleteCategory(int id)
		{
			_categoryRepository.Delete(p => p.Id == id);
			SaveCategory();
		}

		public void SaveCategory()
		{
			_unitOfWork.Commit();
		}
	}
}
