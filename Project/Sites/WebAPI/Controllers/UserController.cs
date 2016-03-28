using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Root.AuthenticationModels;
using Root.Data.Repository;
using Service.Services;
using WebAPI.Handlers;
using Website.ViewModels.User;

namespace WebAPI.Controllers
{
	public class UserController : ApiController
	{
		public IUserService _userService;
		public IUserRepository _userRepository;
		private IAuthenticationManager Authentication
		{
			get { return Request.GetOwinContext().Authentication; }
		}
		public UserController() { }
		public UserController(IUserService userService, IUserRepository userRepository)
		{
			this._userRepository = userRepository;
			this._userService = userService;
		}

		[System.Web.Http.AllowAnonymous]
		[System.Web.Http.Route("api/User/Register")]
		public IHttpActionResult Register(UserModel userModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var errorStatus = _userRepository.RegisterUser(userModel);

			return Ok(errorStatus);
		}

		[System.Web.Http.Authorize]
		[System.Web.Http.HttpPut]
		[System.Web.Http.Route("api/User/Update")]
		public async Task<IHttpActionResult> Update(UserViewModel userViewModel)
		{
			var user = new UserModel {UserName = userViewModel.UserName, Email = userViewModel.Email};

			IdentityResult result = await _userRepository.UpdateUser(user);

			IHttpActionResult errorResult = GetErrorResult(result);

			if (errorResult != null)
			{
				return errorResult;
			}

			return Ok(0);
		}

		[System.Web.Http.Authorize]
		[System.Web.Http.Route("api/User/DoDeleteAccount")]
		public async Task<IHttpActionResult> DoDeleteAccount(UserViewModel userViewModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			UserModel user = new UserModel();
			user.UserName = userViewModel.UserName;

			IdentityResult result = await _userRepository.DeleteUser(user);

			IHttpActionResult errorResult = GetErrorResult(result);

			if (errorResult != null)
			{
				return errorResult;
			}

			return Ok();
		}

		[System.Web.Http.Authorize]
		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/User/ChangePassword/{userName}/{password}/{newPassword}")]
		public async Task<HttpResponseMessage> ChangePassword(string userName, string password, string newPassword)
		{
			IdentityResult result = await _userRepository.ChangePassword(userName, password, newPassword);

			string strResult = "OK";

			if (!result.Succeeded)
			{
				strResult = result.Errors.First();
			}

			return new HttpResponseMessage()
			{
				Content = new StringContent(
					strResult,
					Encoding.UTF8,
					"text/html"
				)
			};
		}

		private IHttpActionResult GetErrorResult(IdentityResult result)
		{
			if (result == null)
			{
				return InternalServerError();
			}

			if (!result.Succeeded)
			{
				if (result.Errors != null)
				{
					foreach (string error in result.Errors)
					{
						ModelState.AddModelError("", error);
					}
				}

				if (ModelState.IsValid)
				{
					// No ModelState errors are available to send, so just return an empty BadRequest.
					return BadRequest();
				}

				return BadRequest(ModelState);
			}

			return null;
		}

		[System.Web.Http.Authorize]
		[System.Web.Http.Route("api/User/Datatable")]
		public IHttpActionResult Get(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "UserName",
				  bool reverse = false,
				  string search = null)
		{
			var userDatatable = _userService.GetUsersForTable(page, itemsPerPage, sortBy, reverse, search);
			if (userDatatable == null)
			{
				return NotFound();
			}
			return Ok(userDatatable);
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/User/GetUserByName")]
		public IHttpActionResult GetUserByName(string userName)
		{
			var user = _userService.GetUserByName(userName);
			return Ok(user);
		}

		[System.Web.Http.Authorize]
		public IEnumerable<UserViewModel> Get(string value)
		{
			return _userService.GetUsers(value);
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/User/ForgotPassword")]
		public IHttpActionResult ForgotPassword(string email)
		{
			if (ModelState.IsValid)
			{
				var userId = _userRepository.GetUserIdByEmail(email);
				if (userId == null)
				{
					return Ok(2);
				}

				var code = _userRepository.GetResetPasswordToken(userId);
				var token = HttpUtility.UrlEncode(code);
				var callbackUrl = ConfigurationManager.AppSettings["resetPasswordBaseUrl"] + "userid=" + userId + "&code=" + token;
				var sentContent = new IdentityMessage();
				sentContent.Subject = "Reset Password";
				sentContent.Body = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";
				sentContent.Destination = email;
				Mail.SendEmail(sentContent);
				return Ok(callbackUrl);
			}
			return Ok(1);
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/User/GetUserEmail")]
		public IHttpActionResult GetUserEmail(string id)
		{
			var email = _userRepository.GetUserEmailById(id);
			if (email == null)
			{
				return Ok(2);
			}

			return Ok(new{email});
		}

		[System.Web.Http.HttpPost]
		[ValidateAntiForgeryToken]
		public IHttpActionResult ResetPassword(ResetPasswordViewModel model)
		{
			var userId = _userRepository.GetUserIdByEmail(model.Email);
			var error = 0;
			if (userId == null)
			{
				error = 1;
				const string message = "Email doesn't exist";
				return Ok(new { error, message });
			}
			var result = _userRepository.ResetPassword(userId, model.Code, model.Password);
			if (result.Succeeded)
			{
				return Ok(new { error });
			}
			error = 1;
			var errorList = result.Errors;
			return Ok(new{error, errorList});
		}

	}
}