using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Root.AuthenticationModels;
using Root.Models;

namespace Root.Data.Repository
{
	public class AuthRepository : IDisposable
	{
		private CGMContext _ctx;

		private UserManager<User> _userManager;

		public AuthRepository()
		{
			_ctx = new CGMContext();
			_userManager = new UserManager<User>(new UserStore<User>(_ctx));
		}

		public IList<string> GetRoles(string id)
		{
			return _userManager.GetRoles(id);
		}
		public async Task<IdentityResult> RegisterUser(UserModel userModel)
		{
			User user = new User
			{
				UserName = userModel.UserName
			};

			var result = await _userManager.CreateAsync(user, userModel.Password);

			return result;
		}

		public async Task<User> FindUser(string userName, string password)
		{
			User user = await _userManager.FindAsync(userName, password);

			return user;
		}

		public async Task<User> FindAsync(UserLoginInfo loginInfo)
		{
			User user = await _userManager.FindAsync(loginInfo);

			return user;
		}

		public async Task<IdentityResult> CreateAsync(User user)
		{
			var result = await _userManager.CreateAsync(user);

			return result;
		}

		public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
		{
			var result = await _userManager.AddLoginAsync(userId, login);

			return result;
		}

		public void Dispose()
		{
			_ctx.Dispose();
			_userManager.Dispose();

		}
	}
}
