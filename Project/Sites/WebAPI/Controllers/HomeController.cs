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
		public IUserService _userService;
		public HomeController(){}

		public HomeController(IUserService userService)
		{
			this._userService = userService;
		}
		public ActionResult Index()
		{
			var user = new UserRepository(new DatabaseFactory());
			var a = user.GetAll();
			ViewBag.Title = "Home Page";
			return View();
		}
	}
}