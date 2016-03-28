using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using Root.Data.Repository;
using Root.Data.Infrastructure;
using Root.Models;
using Website.ViewModels.Account;
using Website.ViewModels.Banner;
using Website.ViewModels.Common;
using Website.ViewModels.Keyword;
using Website.ViewModels.Listing;

namespace Service.Services
{
	public interface IAccountService
	{
		AccountViewModel GetAccountInfo(int id);
		ListingViewModel GetListingInfo(int id);
		BannerViewModel GetBannerInfo(int id);
		ListingAddressViewModel GetListingAddressInfo(int id);
		List<ListingImageTypeViewModel> GetListingImageType();
		List<BannerImageTypeViewModel> GetBannerImageType();
		ResponseStatus AssignKeywordToAccount(int accountId, string keywordName);
		ResponseStatus AssignKeywordToListing(int listingId, string keywordName);
		bool AddContactToAccount(int accountId, int contactId);
		IEnumerable<AccountViewModel> GetAutoSuggestContacts(int accountId, string value);
		AccountDatatable GetAccountDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
		AccountKeywordDatatable GetAccountKeywordDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		ListingKeywordDatatable GetListingKeywordDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		AccountContactDatatable GetAccountContactDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		AccountCommentDatatable GetAccountCommentDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		AccountLikeDatatable GetAccountLikeDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		AccountListingDatatable GetAccountListingDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		AccountBannerDatatable GetAccountBannerDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		ListingCommentDatatable GetListingCommentDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		ListingLikeDatatable GetListingLikeDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		ListingAddressDatatable GetListingAddressDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id, string searchValue);
		void CreateAccount(AccountViewModel account);
		void CreateListing(ListingViewModel listing);
		void CreateListingAddress(ListingAddressViewModel address);
		void CreateBanner(BannerViewModel banner);
		void UpdateAccount(AccountViewModel account);
		void UpdateListing(ListingViewModel listing);
		void UpdateListingAddress(ListingAddressViewModel address);
		void UpdateBanner(BannerViewModel banner);
		void UpdateListingImage(ListingImageViewModel image);
		void UpdateBannerImage(BannerImageViewModel image);
		void DeleteAccount(int id);
		void DeleteAccountKeyword(int id);
		void DeleteListingKeyword(int id);
		void DeleteAccountContact(int id);
		void DeleteAccountComment(int id);
		void DeleteAccountListing(int id);
		void DeleteAccountBanner(int id);
		void DeleteListingAddress(int id);
		void SaveAccount();
	}

	public class AccountService : IAccountService
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IAccountContactRepository _accountContactRepository;
		private readonly IAccountKeywordRepository _accountKeywordRepository;
		private readonly IAccountLikeRepository _accountLikeRepository;
		private readonly IKeywordRepository _keywordRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IListingRepository _listingRepository;
		private readonly IListingLikeRepository _listingLikeRepository;
		private readonly IListingAddressRepository _listingAddressRepository;
		private readonly IBannerRepository _bannerRepository;
		private readonly IBannerImageRepository _bannerImageRepository;
		private readonly IBannerImageTypeRepository _bannerImageTypeRepository;
		private readonly IListingKeywordRepository _listingKeywordRepository;
		private readonly IListingImageRepository _listingImageRepository;
		private readonly IListingImageTypeRepository _listingImageTypeRepository;
		private readonly ICategoryRepository _categoryRepository;

		private readonly IUnitOfWork _unitOfWork;

		public AccountService(IAccountRepository accountRepository, IAccountContactRepository accountContactRepository,
								IAccountKeywordRepository accountKeywordRepository, IBannerRepository bannerRepository, IListingImageRepository listingImageRepository, IBannerImageTypeRepository bannerImageTypeRepository,
								IKeywordRepository keywordRepository, ICommentRepository commentRepository, IListingRepository listingRepository, IBannerImageRepository bannerImageRepository,
								IAccountLikeRepository accountLikeRepository, IListingLikeRepository listingLikeRepository, IListingImageTypeRepository listingImageTypeRepository,
								IListingAddressRepository listingAddressRepository, IListingKeywordRepository listingKeywordRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
		{
			this._accountRepository = accountRepository;
			this._accountContactRepository = accountContactRepository;
			this._accountKeywordRepository = accountKeywordRepository;
			this._accountLikeRepository = accountLikeRepository;
			this._keywordRepository = keywordRepository;
			this._commentRepository = commentRepository;
			this._listingRepository = listingRepository;
			this._listingLikeRepository = listingLikeRepository;
			this._listingAddressRepository = listingAddressRepository;
			this._bannerRepository = bannerRepository;
			this._bannerImageRepository = bannerImageRepository;
			this._bannerImageTypeRepository = bannerImageTypeRepository;
			this._listingKeywordRepository = listingKeywordRepository;
			this._listingImageTypeRepository = listingImageTypeRepository;
			this._listingImageRepository = listingImageRepository;
			this._categoryRepository = categoryRepository;
			this._unitOfWork = unitOfWork;
		}

		public AccountViewModel GetAccountInfo(int id)
		{
			var account = _accountRepository.Query(p => p.Id == id).FirstOrDefault();
			if (account != null)
			{
				var vmAccount = Mapper.Map<Account, AccountViewModel>(account);
				return vmAccount;
			}
			return null;
		}

		public ListingViewModel GetListingInfo(int id)
		{
			var listing = _listingRepository.Query(p => p.Id == id).FirstOrDefault();
			if (listing != null)
			{
				var vmListing = Mapper.Map<Listing, ListingViewModel>(listing);
				return vmListing;
			}
			return null;
		}

		public BannerViewModel GetBannerInfo(int id)
		{
			var banner = _bannerRepository.Query(p => p.Id == id).FirstOrDefault();
			if (banner != null)
			{
				var vmBanner = Mapper.Map<Banner, BannerViewModel>(banner);
				return vmBanner;
			}
			return null;
		}

		public ListingAddressViewModel GetListingAddressInfo(int id)
		{
			var address = _listingAddressRepository.Query(p => p.Id == id).FirstOrDefault();
			if (address != null)
			{
				var vmAddress = Mapper.Map<ListingAddress, ListingAddressViewModel>(address);
				return vmAddress;
			}
			return null; ;
		}

		public List<ListingImageTypeViewModel> GetListingImageType()
		{
			var listingImageType = (from p in _listingImageTypeRepository.GetAll()
									select new ListingImageTypeViewModel()
										{
											Id = p.Id,
											Name = p.Name
										}).ToList();
			return listingImageType;
		}

		public List<BannerImageTypeViewModel> GetBannerImageType()
		{
			var bannerImageType = (from p in _bannerImageTypeRepository.GetAll()
								   select new BannerImageTypeViewModel()
								   {
									   Id = p.Id,
									   Name = p.Name
								   }).ToList();
			return bannerImageType;
		}

		public ResponseStatus AssignKeywordToAccount(int accountId, string keywordName)
		{
			var keyword = _keywordRepository.Query(p => p.KeywordName == keywordName).FirstOrDefault();
			if (keyword != null)
			{
				var accountKeywod = new AccountKeyword
					{
						AccountId = accountId,
						KeywordId = keyword.Id
					};
				_accountKeywordRepository.Add(accountKeywod);
				SaveAccount();
				return new ResponseStatus { Successful = true, Message = "" };
			}
			return new ResponseStatus { Successful = false, Message = "nexist" };
		}

		public ResponseStatus AssignKeywordToListing(int listingId, string keywordName)
		{
			var keyword = _keywordRepository.Query(p => p.KeywordName == keywordName).FirstOrDefault();
			if (keyword != null)
			{
				var listingKeywod = new ListingKeyword()
				{
					ListingId = listingId,
					KeywordId = keyword.Id
				};
				_listingKeywordRepository.Add(listingKeywod);
				SaveAccount();
				return new ResponseStatus { Successful = true, Message = "" };
			}
			return new ResponseStatus { Successful = false, Message = "nexist" };
		}

		public bool AddContactToAccount(int accountId, int contactId)
		{
			var contact = _accountRepository.Query(p => p.Id == contactId).FirstOrDefault();
			if (contact != null)
			{
				var accountContact = new AccountContact
				{
					AccountId = accountId,
					ContactId = contactId
				};
				_accountContactRepository.Add(accountContact);
				SaveAccount();
				return true;
			}
			return false;
		}

		public IEnumerable<AccountViewModel> GetAutoSuggestContacts(int accountId, string value)
		{
			var contacts = _accountRepository.Query(i => ((i.FirstName.Contains(value)) || (i.LastName.Contains(value))
															|| i.Email.ToLower() == value.ToLower()) && i.Id != accountId);
			var destination = Mapper.Map<IEnumerable<Account>, IEnumerable<AccountViewModel>>(contacts);
			return destination;
		}

		public AccountDatatable GetAccountDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			var accounts = _accountRepository.GetAllQueryable();
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				accounts = accounts.Where(p => p.FirstName.ToLower().Contains(searchValue) ||
												p.LastName.ToLower().Contains(searchValue) ||
												p.Email.ToLower().Equals(searchValue.ToLower()));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var accountsOrdered = accounts.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var accountsPaged = accountsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var destination = Mapper.Map<List<Account>, List<AccountViewModel>>(accountsPaged);
			var accountDatatable = new AccountDatatable()
			{
				Data = destination,
				Total = accounts.Count()
			};
			return accountDatatable;
		}

		public AccountKeywordDatatable GetAccountKeywordDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
																  string searchValue)
		{
			var accountKeywords = from p in _accountKeywordRepository.GetAllQueryable()
								  join q in _keywordRepository.GetAllQueryable() on p.KeywordId equals q.Id into k
								  from q in k.DefaultIfEmpty()
								  where p.AccountId == id
								  select new AccountKeywordViewModel()
									  {
										  Id = p.Id,
										  AccountId = p.AccountId,
										  KeywordId = p.KeywordId,
										  KeywordName = q.KeywordName,
										  KeywordType = q.KeywordType
									  };
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				accountKeywords = accountKeywords.Where(p => p.KeywordName.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var accountKeywordsOrdered = accountKeywords.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var accountKeywordsPaged = accountKeywordsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var accountKeywordDatatable = new AccountKeywordDatatable()
			{
				Data = accountKeywordsPaged,
				Total = accountKeywords.Count()
			};
			return accountKeywordDatatable;
		}

		public ListingKeywordDatatable GetListingKeywordDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
																  string searchValue)
		{
			var listingKeywords = from p in _listingKeywordRepository.GetAllQueryable()
								  join q in _keywordRepository.GetAllQueryable() on p.KeywordId equals q.Id into k
								  from q in k.DefaultIfEmpty()
								  where p.ListingId == id
								  select new ListingKeywordViewModel()
								  {
									  Id = p.Id,
									  ListingId = p.ListingId,
									  KeywordId = p.KeywordId,
									  KeywordName = q.KeywordName,
									  KeywordType = q.KeywordType
								  };
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				listingKeywords = listingKeywords.Where(p => p.KeywordName.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var listingKeywordsOrdered = listingKeywords.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var listingKeywordsPaged = listingKeywordsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var listingKeywordDatatable = new ListingKeywordDatatable()
			{
				Data = listingKeywordsPaged,
				Total = listingKeywords.Count()
			};
			return listingKeywordDatatable;
		}

		public AccountContactDatatable GetAccountContactDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
			string searchValue)
		{
			var accountContacts = from p in _accountContactRepository.GetAllQueryable()
								  join q in _accountRepository.GetAllQueryable() on p.ContactId equals q.Id into k
								  from q in k.DefaultIfEmpty()
								  where p.AccountId == id
								  select new AccountContactViewModel()
								  {
									  Id = p.Id,
									  AccountId = p.AccountId,
									  ContactId = p.ContactId,
									  ContactName = q.FirstName + " " + q.LastName
								  };
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				accountContacts = accountContacts.Where(p => p.ContactName.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var accountContactsOrdered = accountContacts.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var accountContactsPaged = accountContactsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var accountContactDatatable = new AccountContactDatatable()
			{
				Data = accountContactsPaged,
				Total = accountContacts.Count()
			};
			return accountContactDatatable;
		}

		public AccountCommentDatatable GetAccountCommentDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
																  string searchValue)
		{
			var accountComments = from c in _commentRepository.GetAllQueryable()
								  join a in _accountRepository.GetAllQueryable() on c.ByAccountId equals a.Id into act
								  from t in act.DefaultIfEmpty()
								  where c.ObjectId == id & c.CommentType.Equals("A")
								  select new AccountCommentViewModel()
								  {
									  Id = c.Id,
									  CommentType = c.CommentType,
									  ObjectId = c.ObjectId,
									  ByAccountId = c.ByAccountId,
									  ByAccountName = t.FirstName + " " + t.LastName,
									  Title = c.Title,
									  Description = c.Description,
									  Rating = c.Rating
								  };
			// searching
			//if (!string.IsNullOrWhiteSpace(searchValue))
			//{
			//	searchValue = searchValue.ToLower();
			//	accountComments = accountComments.Where(p => p.Rating.ToLower().Contains(searchValue));
			//}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var accountCommentsOrdered = accountComments.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var accountCommentsPaged = accountCommentsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var accountCommentDatatable = new AccountCommentDatatable()
			{
				Data = accountCommentsPaged,
				Total = accountComments.Count()
			};
			return accountCommentDatatable;
		}

		public AccountLikeDatatable GetAccountLikeDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
															string searchValue)
		{
			var accountLikes = from l in _accountLikeRepository.GetAllQueryable()
							   join a in _accountRepository.GetAllQueryable() on l.ByAccountId equals a.Id into la
							   from a in la.DefaultIfEmpty()
							   where l.AccountId == id
							   select new AccountLikeViewModel()
							   {
								   Id = l.Id,
								   AccountId = l.AccountId,
								   ByAccountId = l.ByAccountId,
								   ByAccountName = a.FirstName + " " + a.LastName
							   };
			// searching
			//if (!string.IsNullOrWhiteSpace(searchValue))
			//{
			//	searchValue = searchValue.ToLower();
			//	accountComments = accountComments.Where(p => p.Rating.ToLower().Contains(searchValue));
			//}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var accountLikesOrdered = accountLikes.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var accountLikesPaged = accountLikesOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var accountLikesDatatable = new AccountLikeDatatable()
			{
				Data = accountLikesPaged,
				Total = accountLikes.Count()
			};
			return accountLikesDatatable;
		}

		public AccountListingDatatable GetAccountListingDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
			string searchValue)
		{
			var accountListings = from l in _listingRepository.GetAllQueryable()
								  join a in _accountRepository.GetAllQueryable() on l.AccountId equals a.Id into la
								  from a in la.DefaultIfEmpty()
								  join c in _categoryRepository.GetAllQueryable() on l.CategoryId equals c.Id into lc
								  from c in lc.DefaultIfEmpty()
								  where l.AccountId == id
								  select new ListingViewModel()
								  {
									  Id = l.Id,
									  AccountId = l.AccountId,
									  AccountName = a.FirstName + " " + a.LastName,
									  Title = l.Title,
									  Description = l.Description,
									  Active = l.Active,
									  CategoryId = l.CategoryId,
									  CategoryName = c.Name
								  };
			// searching
			//if (!string.IsNullOrWhiteSpace(searchValue))
			//{
			//	searchValue = searchValue.ToLower();
			//	accountComments = accountComments.Where(p => p.Rating.ToLower().Contains(searchValue));
			//}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var accountListingsOrdered = accountListings.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var accountListingsPaged = accountListingsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var accountListingDatatable = new AccountListingDatatable()
			{
				Data = accountListingsPaged,
				Total = accountListings.Count()
			};
			return accountListingDatatable;
		}

		public AccountBannerDatatable GetAccountBannerDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
			string searchValue)
		{
			var accountBanners = from b in _bannerRepository.GetAllQueryable()
								 join a in _accountRepository.GetAllQueryable() on b.AccountId equals a.Id into ba
								 from a in ba.DefaultIfEmpty()
								 join c in _categoryRepository.GetAllQueryable() on b.CategoryId equals c.Id into bc
								 from c in bc.DefaultIfEmpty()
								 where b.AccountId == id
								 select new BannerViewModel()
								 {
									 Id = b.Id,
									 AccountId = b.AccountId,
									 AccountName = a.FirstName + " " + a.LastName,
									 Title = b.Title,
									 Description = b.Description,
									 DestinationType = b.DestinationType,
									 Destination = b.Destination,
									 Active = b.Active,
									 CategoryId = b.CategoryId,
									 CategoryName = c.Name,
									 StartDateTime = b.StartDateTime,
									 EndDateTime = b.EndDateTime
								 };
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				accountBanners = accountBanners.Where(p => p.AccountName.ToLower().Contains(searchValue) || p.Title.ToLower().Contains(searchValue) || p.Description.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var accountBannersOrdered = accountBanners.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var accountBannersPaged = accountBannersOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var accountBannerDatatable = new AccountBannerDatatable()
			{
				Data = accountBannersPaged,
				Total = accountBanners.Count()
			};
			return accountBannerDatatable;
		}

		public ListingCommentDatatable GetListingCommentDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
			string searchValue)
		{
			var listingComments = from c in _commentRepository.GetAllQueryable()
								  join a in _accountRepository.GetAllQueryable() on c.ByAccountId equals a.Id into act
								  from t in act.DefaultIfEmpty()
								  where c.ObjectId == id & c.CommentType.Equals("L")
								  select new ListingCommentViewModel()
								  {
									  Id = c.Id,
									  CommentType = c.CommentType,
									  ObjectId = c.ObjectId,
									  ByAccountId = c.ByAccountId,
									  ByAccountName = t.FirstName + " " + t.LastName,
									  Title = c.Title,
									  Description = c.Description,
									  Rating = c.Rating
								  };
			// searching
			//if (!string.IsNullOrWhiteSpace(searchValue))
			//{
			//	searchValue = searchValue.ToLower();
			//	accountComments = accountComments.Where(p => p.Rating.ToLower().Contains(searchValue));
			//}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var listingCommentsOrdered = listingComments.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var listingCommentsPaged = listingCommentsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var listingCommentDatatable = new ListingCommentDatatable()
			{
				Data = listingCommentsPaged,
				Total = listingComments.Count()
			};
			return listingCommentDatatable;
		}

		public ListingLikeDatatable GetListingLikeDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
			string searchValue)
		{
			var listingLikes = from l in _listingLikeRepository.GetAllQueryable()
							   join a in _accountRepository.GetAllQueryable() on l.ByAccountId equals a.Id into la
							   from a in la.DefaultIfEmpty()
							   where l.ListingId == id
							   select new ListingLikeViewModel()
							   {
								   Id = l.Id,
								   ListingId = l.ListingId,
								   ByAccountId = l.ByAccountId,
								   ByAccountName = a.FirstName + " " + a.LastName
							   };
			// searching
			//if (!string.IsNullOrWhiteSpace(searchValue))
			//{
			//	searchValue = searchValue.ToLower();
			//	accountComments = accountComments.Where(p => p.Rating.ToLower().Contains(searchValue));
			//}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var listingLikesOrdered = listingLikes.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var listingLikesPaged = listingLikesOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var listingLikesDatatable = new ListingLikeDatatable()
			{
				Data = listingLikesPaged,
				Total = listingLikes.Count()
			};
			return listingLikesDatatable;
		}

		public ListingAddressDatatable GetListingAddressDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int id,
			string searchValue)
		{
			var listingAddresses = _listingAddressRepository.Query(p => p.ListingId == id);
			// searching
			//if (!string.IsNullOrWhiteSpace(searchValue))
			//{
			//	searchValue = searchValue.ToLower();
			//	accountComments = accountComments.Where(p => p.Rating.ToLower().Contains(searchValue));
			//}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var listingAddressesOrdered = listingAddresses.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var listingAddressesPaged = listingAddressesOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var destination = Mapper.Map<List<ListingAddress>, List<ListingAddressViewModel>>(listingAddressesPaged);

			var listingAddressDatatable = new ListingAddressDatatable()
			{
				Data = destination,
				Total = listingAddresses.Count()
			};
			return listingAddressDatatable;
		}

		public void CreateAccount(AccountViewModel vmAccount)
		{
			var account = Mapper.Map<AccountViewModel, Account>(vmAccount);
			_accountRepository.Add(account);
			SaveAccount();
		}

		public void CreateListing(ListingViewModel vmListing)
		{
			var listing = Mapper.Map<ListingViewModel, Listing>(vmListing);
			_listingRepository.Add(listing);
			SaveAccount();
		}

		public void CreateListingAddress(ListingAddressViewModel vmAddress)
		{
			var address = Mapper.Map<ListingAddressViewModel, ListingAddress>(vmAddress);
			_listingAddressRepository.Add(address);
			SaveAccount();
		}

		public void CreateBanner(BannerViewModel vmBanner)
		{
			var banner = Mapper.Map<BannerViewModel, Banner>(vmBanner);
			_bannerRepository.Add(banner);
			SaveAccount();
		}

		public void UpdateAccount(AccountViewModel vmAccount)
		{
			var account = Mapper.Map<AccountViewModel, Account>(vmAccount);
			_accountRepository.Update(account);
			SaveAccount();
		}

		public void UpdateListing(ListingViewModel vmListing)
		{
			var listing = Mapper.Map<ListingViewModel, Listing>(vmListing);
			_listingRepository.Update(listing);
			SaveAccount();
		}

		public void UpdateListingAddress(ListingAddressViewModel vmAddress)
		{
			var address = Mapper.Map<ListingAddressViewModel, ListingAddress>(vmAddress);
			_listingAddressRepository.Update(address);
			SaveAccount();
		}

		public void UpdateBanner(BannerViewModel vmBanner)
		{
			var banner = Mapper.Map<BannerViewModel, Banner>(vmBanner);
			_bannerRepository.Update(banner);
			SaveAccount();
		}

		public void UpdateListingImage(ListingImageViewModel vmImage)
		{
			var image = Mapper.Map<ListingImageViewModel, ListingImage>(vmImage);
			_listingImageRepository.Update(image);
			SaveAccount();
		}

		public void UpdateBannerImage(BannerImageViewModel vmImage)
		{
			var image = Mapper.Map<BannerImageViewModel, BannerImage>(vmImage);
			_bannerImageRepository.Update(image);
			SaveAccount();
		}

		public void DeleteAccount(int id)
		{
			_accountRepository.Delete(p => p.Id == id);
			_commentRepository.Delete(p => p.ObjectId == id & p.CommentType == "A");
			_accountContactRepository.Delete(p => p.AccountId == id);
			_accountKeywordRepository.Delete(p => p.AccountId == id);
			_accountLikeRepository.Delete(p => p.AccountId == id);
			SaveAccount();
		}

		public void DeleteAccountKeyword(int id)
		{
			_accountKeywordRepository.Delete(p => p.Id == id);
			SaveAccount();
		}

		public void DeleteListingKeyword(int id)
		{
			_listingKeywordRepository.Delete(p => p.Id == id);
			SaveAccount();
		}

		public void DeleteAccountContact(int id)
		{
			_accountContactRepository.Delete(p => p.Id == id);
			SaveAccount();
		}

		public void DeleteAccountComment(int id)
		{
			_commentRepository.Delete(p => p.Id == id);
			SaveAccount();
		}

		public void DeleteAccountListing(int id)
		{
			_listingRepository.Delete(p => p.Id == id);
			SaveAccount();
		}

		public void DeleteAccountBanner(int id)
		{
			_bannerRepository.Delete(p => p.Id == id);
			SaveAccount();
		}

		public void DeleteListingAddress(int id)
		{
			_listingAddressRepository.Delete(p => p.Id == id);
			SaveAccount();
		}

		public void SaveAccount()
		{
			_unitOfWork.Commit();
		}
	}
}
