using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Service.Services;
using Website.ViewModels.Category;

namespace WebAPI.Controllers
{
	[Authorize]
    public class CategoryController : ApiController
    {
		public ICategoryService _categoryService;
		public CategoryController() { }

		public CategoryController(ICategoryService categoryService)
		{
			this._categoryService = categoryService;
		}

		public IHttpActionResult Get(int id)
		{
			var vmCategory = _categoryService.GetCategoryInfo(id);
			if (vmCategory == null)
			{
				return NotFound();
			}
			return Ok(vmCategory);
		}

		[Route("api/Category/GetAutoSuggestCategories")]
		public IEnumerable<CategoryViewModel> GetAutoSuggestCategories(string value)
		{
			return _categoryService.GetAutoSuggestCategories(value);
		}

		[Route("api/Category/GetCategories")]
		public IEnumerable<CategoryViewModel> GetCategories()
		{
			return _categoryService.GetCategories();
		}

		[Route("api/Category/Datatable")]
		public IHttpActionResult Get(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "Name",
				  bool reverse = false,
				  string search = null)
		{
			var cateTable = _categoryService.GetCategoryDatatable(page, itemsPerPage, sortBy, reverse, search);
			if (cateTable == null)
			{
				return NotFound();
			}
			return Ok(cateTable);
		}

		public IHttpActionResult Post(CategoryViewModel vmCate)
		{
			return Ok(_categoryService.CreateCategory(vmCate));
		}

		public IHttpActionResult Put(CategoryViewModel vmCate)
		{
			return Ok(_categoryService.UpdateCategory(vmCate));
		}

		public void Delete(int id)
		{
			_categoryService.DeleteCategory(id);
		}
    }
}
