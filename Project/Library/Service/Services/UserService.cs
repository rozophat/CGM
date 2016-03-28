using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Root.AuthenticationModels;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Website.ViewModels.User;

namespace Service.Services
{
	public interface IUserService
	{
		UserDatatable GetUsersForTable(int page, int itemsPerPage, string sortBy, bool reverse,
			 string custSearchValue);
		UserViewModel GetUserByName(string userName);
		IEnumerable<UserViewModel> GetUsers(string value);
		void SaveUser();
	}
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UserService(IUserRepository userRepository,
						   IUnitOfWork unitOfWork)
		{
			this._userRepository = userRepository;
			this._unitOfWork = unitOfWork;
		}

		public UserDatatable GetUsersForTable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			var users = from a in _userRepository.GetAllQueryable()
						where (string.IsNullOrEmpty(searchValue) || a.UserName.Contains(searchValue))
						select new UserViewModel()
						{
							UserName = a.UserName,
							Email = a.Email,
							IsActive = a.IsActive
						};
			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var usersOrdered = users.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var usersPaged = usersOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var data = new UserDatatable()
			{
				Data = usersPaged,
				Total = usersPaged.Count()
			};
			return data;
		}

		public UserViewModel GetUserByName(string userName)
		{
			var user = (from a in _userRepository.GetAllQueryable()
						
						where (a.UserName == userName)
						select new UserViewModel()
						{
							UserName = a.UserName,
							IsActive = a.IsActive
						}).ToList();

			if (user.Count > 0)
			{
				return user[0];
			}

			return null;
		}

		public IEnumerable<UserViewModel> GetUsers(string value)
		{
			var users = (from a in _userRepository.GetAllQueryable()
						 where (a.UserName.Contains(value))
						 select new UserViewModel()
						 {
							 UserName = a.UserName,
							 IsActive = a.IsActive
						 }).ToList();

			return users;
		}

		public void SaveUser()
		{
			_unitOfWork.Commit();
		}
	}
}
