using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Service.Services;
using Website.ViewModels.Keyword;

namespace WebAPI.Controllers
{
	[Authorize]
	public class KeywordController : ApiController
	{
		public IKeywordService _keywordService;

		public KeywordController() { }

		public KeywordController(IKeywordService keywordService)
		{
			this._keywordService = keywordService;
		}

		[Route("api/Keyword/GetAutoSuggestKeywords")]
		public IEnumerable<KeywordViewModel> GetAutoSuggestKeywords(string value)
		{
			return _keywordService.GetAutoSuggestKeywords(value);
		}

		[Route("api/Keyword/Datatable")]
		public IHttpActionResult Get(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "KeywordName",
				  bool reverse = false,
				  string search = null)
		{
			var accountTable = _keywordService.GetKeywordDatatable(page, itemsPerPage, sortBy, reverse, search);
			if (accountTable == null)
			{
				return NotFound();
			}
			return Ok(accountTable);
		}

		public IHttpActionResult Post(KeywordViewModel account)
		{
			var errorStatus = _keywordService.CreateKeyword(account);
			return Ok(errorStatus); 
		}

		public void Put(KeywordViewModel vmKeyword)
		{
			_keywordService.UpdateKeyword(vmKeyword);
		}

		public void Delete(int id)
		{
			_keywordService.DeleteKeyword(id);
		}
	}
}