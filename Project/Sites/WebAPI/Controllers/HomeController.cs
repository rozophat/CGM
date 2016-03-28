using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Service.Services;

namespace WebAPI.Controllers
{
	public class HomeController : Controller
	{
		public IAccountService _accountService;

		public HomeController(){}
		public HomeController(IAccountService accountService)
		{
			this._accountService = accountService;
		}

		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";
			return View();
		}
	}
}