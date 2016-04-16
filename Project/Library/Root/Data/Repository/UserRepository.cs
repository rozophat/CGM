using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Root.AuthenticationModels;
using Root.Data.Infrastructure;
using Root.Models;

namespace Root.Data.Repository
{
	public class UserRepository : RepositoryBase<User>, IUserRepository, IDisposable
	{
		private CGMContext _ctx;
		private UserManager<User> _userManager;
		private UserStore<User> _userStore;
		private RoleManager<IdentityRole> _roleManager;
		public UserRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
			DataContext.Set<User>();
			_ctx = DataContext;
			_userStore = _userStore ?? new UserStore<User>(_ctx);
			_userManager = _userManager ?? new UserManager<User>(_userStore);
			_roleManager = _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
			var provider = new DpapiDataProtectionProvider("YourAppName");
			_userManager.UserTokenProvider = new DataProtectorTokenProvider<User, string>(provider.Create("UserToken"));
		}
		public IList<string> GetRoles(string id)
		{
			return _userManager.GetRoles(id);
		}

		public Task<IdentityRole> GetRoleByName(string name)
		{
			return _roleManager.FindByIdAsync(name);
		}

		public int RegisterUser(UserModel userModel)
		{
			var checkUserExist = _userManager.FindByName(userModel.UserName);
			if (checkUserExist != null)
			{
				return 1;
			}

			if (!string.IsNullOrEmpty(userModel.Email))
			{
				var checkEmailExist = _userManager.FindByEmail(userModel.Email);
				if (checkEmailExist != null)
				{
					return 2;
				}
			}
			
			User user = new User
			{
				UserName = userModel.UserName,
				Email = userModel.Email,
				IsActive = "1"
			};

			_userManager.Create(user, userModel.Password);

			return 0;
		}

		public async Task<IdentityResult> UpdateUser(UserModel userModel)
		{
			IdentityResult result = null;
			// update Users table
			var userList = _ctx.Users.Where(u => u.UserName == userModel.UserName);
			var userUpdate = userList.FirstOrDefault();
			if (userUpdate != null)
			{
				userUpdate.IsActive = "1";
				userUpdate.Email = userModel.Email;
				result = await _userManager.UpdateAsync(userUpdate);
			}

			return result;
		}

		public async Task<IdentityResult> DeleteUser(UserModel userModel)
		{
			IdentityResult result = null;
			// update Users table
			var userList = _ctx.Users.Where(u => u.UserName == userModel.UserName);
			var userDelete = userList.FirstOrDefault();
			if (userDelete != null)
			{
				result = await _userManager.DeleteAsync(userDelete);
			}

			return result;
		}

		public async Task<IdentityResult> SetActiveStatus(UserModel userModel)
		{
			IdentityResult result = null;

			var userUpdate = await _userManager.FindByNameAsync(userModel.UserName);
			if (userUpdate != null)
			{
				userUpdate.IsActive = userModel.IsActive;
				result = await _userManager.UpdateAsync(userUpdate);
				_userStore.Context.SaveChanges();
			}

			return result;
		}

		public async Task<IdentityResult> ResetPassword(string userName, string defaultPassword)
		{
			IdentityResult result = null;
			// update Users table
			var userList = _ctx.Users.Where(u => u.UserName == userName);
			var userUpdate = userList.FirstOrDefault();
			if (userUpdate != null)
			{
				var hashedNewPassword = _userManager.PasswordHasher.HashPassword(defaultPassword);

				userUpdate.PasswordHash = hashedNewPassword;
				result = await _userManager.UpdateAsync(userUpdate);
			}

			return result;
		}

		public Task<IdentityResult> ForgetPassword(string userName, string defaultPassword)
		{
			throw new NotImplementedException();
		}

		public async Task<IdentityResult> ChangePassword(string userName, string password, string newPassword)
		{
			var userId = _userManager.FindByName(userName).Id;
			var result = await _userManager.ChangePasswordAsync(userId, password, newPassword);
			return result;
		}

		public async Task<User> FindUser(string userName, string password)
		{
			//Every connection is using singleton design pattern, so when we find one user, the user is cached in this 
			//dbcontext. When we change this user from another connection, and get back to previous connection, the user with data changed
			//is not apply
			//Solution: create new DBcontext everytime we need to get all user information.
			var context = new CGMContext();
			using (var userManager = new UserManager<User>(new UserStore<User>(context)))
			{
				var user = await userManager.FindAsync(userName, password);
				return user;
			}
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

		public string GetUserIdByEmail(string email)
		{
			var user = _userManager.FindByEmail(email);
			if (user != null) return user.Id;
			return null;
		}

		public IdentityResult ResetPassword(string userId, string code, string password)
		{
			var result = _userManager.ResetPassword(userId, code, password);
			return result;
		}

		public string GetResetPasswordToken(string userId)
		{
			
			string code = _userManager.GeneratePasswordResetToken(userId);
			return code;
		}

		public void SendMailResetPassword(string userId, string content)
		{
			_userManager.SendEmail(userId, "Reset Password", content);
		}

		public string GetUserEmailById(string userId)
		{
			var user = _userManager.FindById(userId);
			if (user != null) return user.Email;
			return null;
		}

		public void Dispose()
		{
			_ctx.Dispose();
			_userManager.Dispose();
		}
	}
	public interface IUserRepository : IRepository<User>
	{
		IList<string> GetRoles(string id);
		Task<IdentityRole> GetRoleByName(string id);
		int RegisterUser(UserModel userModel);
		Task<IdentityResult> UpdateUser(UserModel userModel);
		Task<IdentityResult> ResetPassword(string userName, string defaultPassword);
		Task<IdentityResult> ForgetPassword(string userName, string defaultPassword);
		Task<IdentityResult> ChangePassword(string userName, string password, string newPassword);
		Task<IdentityResult> DeleteUser(UserModel userModel);
		Task<IdentityResult> SetActiveStatus(UserModel userModel);
		Task<User> FindUser(string userName, string password);
		Task<User> FindAsync(UserLoginInfo loginInfo);
		Task<IdentityResult> CreateAsync(User user);
		Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
		string GetUserIdByEmail(string email);
		IdentityResult ResetPassword(string userId, string code, string password);
		string GetResetPasswordToken(string userId);
		void SendMailResetPassword(string userId, string content);
		string GetUserEmailById(string userId);
	}
}
