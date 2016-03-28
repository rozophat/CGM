using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Service.Services;
using WebAPI.Models;
using WebAPI.Photo;
using WebAPI.Providers;
using WebAPI.Results;
using Website.ViewModels.Account;
using Website.ViewModels.Banner;
using Website.ViewModels.Listing;

namespace WebAPI.Controllers
{
	[Authorize]
	public class AccountController : ApiController
	{
		public IAccountService _accountService;
		public IListingService _listingService;
		public IBannerService _bannerService;
		private IPhotoManager photoManager;

		public AccountController() { }

		public AccountController(IAccountService accountService, IListingService listingService, IBannerService bannerService, IPhotoManager photoManager)
		{
			this._accountService = accountService;
			this._listingService = listingService;
			this._bannerService = bannerService;
			this.photoManager = photoManager;
		}

		public IHttpActionResult Get(int id)
		{
			var vmAccount = _accountService.GetAccountInfo(id);
			if (vmAccount == null)
			{
				return NotFound();
			}
			return Ok(vmAccount);
		}

		[HttpGet]
		[Route("api/Account/GetListingInfo")]
		public IHttpActionResult GetListingInfo(int id)
		{
			var vmListing = _accountService.GetListingInfo(id);
			if (vmListing == null)
			{
				return NotFound();
			}
			return Ok(vmListing);
		}

		[HttpGet]
		[Route("api/Account/GetBannerInfo")]
		public IHttpActionResult GetBannerInfo(int id)
		{
			var vmBanner = _accountService.GetBannerInfo(id);
			if (vmBanner == null)
			{
				return NotFound();
			}
			return Ok(vmBanner);
		}

		[HttpGet]
		[Route("api/Account/GetListingAddressInfo")]
		public IHttpActionResult GetListingAddressInfo(int id)
		{
			var vmAddress = _accountService.GetListingAddressInfo(id);
			if (vmAddress == null)
			{
				return NotFound();
			}
			return Ok(vmAddress);
		}

		[HttpGet]
		[Route("api/Account/GetListingImageType")]
		public IHttpActionResult GetListingImageType()
		{
			var vmListingImageTypes = _accountService.GetListingImageType();
			if (vmListingImageTypes == null)
			{
				return NotFound();
			}
			return Ok(vmListingImageTypes);
		}

		[HttpGet]
		[Route("api/Account/GetBannerImageType")]
		public IHttpActionResult GetBannerImageType()
		{
			var vmBannerImageTypes = _accountService.GetBannerImageType();
			if (vmBannerImageTypes == null)
			{
				return NotFound();
			}
			return Ok(vmBannerImageTypes);
		}

		[HttpGet]
		[Route("api/Account/AssignKeywordToAccount")]
		public IHttpActionResult AssignKeywordToAccount(int accountId, string keywordName)
		{
			var status = _accountService.AssignKeywordToAccount(accountId, keywordName);
			return Ok(status);
		}

		[HttpGet]
		[Route("api/Account/AssignKeywordToListing")]
		public IHttpActionResult AssignKeywordToListing(int listingId, string keywordName)
		{
			var status = _accountService.AssignKeywordToListing(listingId, keywordName);
			return Ok(status);
		}

		[HttpGet]
		[Route("api/Account/AddContactToAccount")]
		public IHttpActionResult AddContactToAccount(int accountId, int contactId)
		{
			var status = _accountService.AddContactToAccount(accountId, contactId);
			return Ok(status);
		}

		[HttpGet]
		[Route("api/Account/GetAutoSuggestContacts")]
		public IEnumerable<AccountViewModel> GetAutoSuggestContacts(int accountId, string value)
		{
			return _accountService.GetAutoSuggestContacts(accountId, value);
		}

		[Route("api/Account/Datatable")]
		public IHttpActionResult Get(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "FirstName",
				  bool reverse = false,
				  string search = null)
		{
			var accountTable = _accountService.GetAccountDatatable(page, itemsPerPage, sortBy, reverse, search);
			if (accountTable == null)
			{
				return NotFound();
			}
			return Ok(accountTable);
		}

		[Route("api/Account/KeywordDatatable")]
		public IHttpActionResult Get(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "KeywordName",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var accountKeywordTable = _accountService.GetAccountKeywordDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (accountKeywordTable == null)
			{
				return NotFound();
			}
			return Ok(accountKeywordTable);
		}

		[HttpGet]
		[Route("api/Account/ListingKeywordDatatable")]
		public IHttpActionResult ListingKeywordDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "KeywordName",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var listingKeywordTable = _accountService.GetListingKeywordDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (listingKeywordTable == null)
			{
				return NotFound();
			}
			return Ok(listingKeywordTable);
		}

		[HttpGet]
		[Route("api/Account/ContactDatatable")]
		public IHttpActionResult ContactDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "ContactName",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var accountContactTable = _accountService.GetAccountContactDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (accountContactTable == null)
			{
				return NotFound();
			}
			return Ok(accountContactTable);
		}

		[HttpGet]
		[Route("api/Account/CommentDatatable")]
		public IHttpActionResult CommentDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "Rating",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var accountCommentTable = _accountService.GetAccountCommentDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (accountCommentTable == null)
			{
				return NotFound();
			}
			return Ok(accountCommentTable);
		}

		[HttpGet]
		[Route("api/Account/LikeDatatable")]
		public IHttpActionResult LikeDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "ByAccountName",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var accountLikeTable = _accountService.GetAccountLikeDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (accountLikeTable == null)
			{
				return NotFound();
			}
			return Ok(accountLikeTable);
		}

		[HttpGet]
		[Route("api/Account/ListingDatatable")]
		public IHttpActionResult ListingDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "Title",
				  bool reverse = false,
				  string search = null)
		{
			var listingTable = _listingService.GetListingDatatable(page, itemsPerPage, sortBy, reverse, search);
			if (listingTable == null)
			{
				return NotFound();
			}
			return Ok(listingTable);
		}

		[HttpGet]
		[Route("api/Account/AccountListingDatatable")]
		public IHttpActionResult AccountListingDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "Title",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var accountListingTable = _accountService.GetAccountListingDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (accountListingTable == null)
			{
				return NotFound();
			}
			return Ok(accountListingTable);
		}

		[HttpGet]
		[Route("api/Account/ListingCommentDatatable")]
		public IHttpActionResult ListingCommentDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "Rating",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var listingCommentTable = _accountService.GetListingCommentDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (listingCommentTable == null)
			{
				return NotFound();
			}
			return Ok(listingCommentTable);
		}

		[HttpGet]
		[Route("api/Account/ListingLikeDatatable")]
		public IHttpActionResult ListingLikeDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "ByAccountName",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var listingLikeTable = _accountService.GetListingLikeDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (listingLikeTable == null)
			{
				return NotFound();
			}
			return Ok(listingLikeTable);
		}

		[HttpGet]
		[Route("api/Account/ListingAddressDatatable")]
		public IHttpActionResult ListingAddressDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "AddressLine1",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var listingAddressTable = _accountService.GetListingAddressDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (listingAddressTable == null)
			{
				return NotFound();
			}
			return Ok(listingAddressTable);
		}

		[HttpGet]
		[Route("api/Account/ListingImageDatatable")]
		public IHttpActionResult ListingImageDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "",
				  bool reverse = false,
				  int listingId = 0,
				  string search = null)
		{
			var listingImageDatatable = _listingService.GetListingImageDatatable(page, itemsPerPage, sortBy, reverse, listingId, search);
			if (listingImageDatatable == null)
			{
				return NotFound();
			}
			return Ok(listingImageDatatable);
		}

		[HttpGet]
		[Route("api/Account/BannerDatatable")]
		public IHttpActionResult BannerDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "Title",
				  bool reverse = false,
				  string search = null)
		{
			var bannerTable = _bannerService.GetBannerDatatable(page, itemsPerPage, sortBy, reverse, search);
			if (bannerTable == null)
			{
				return NotFound();
			}
			return Ok(bannerTable);
		}

		[HttpGet]
		[Route("api/Account/AccountBannerDatatable")]
		public IHttpActionResult AccountBannerDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "Title",
				  bool reverse = false,
				  int id = 0,
				  string search = null)
		{
			var accountBannerTable = _accountService.GetAccountBannerDatatable(page, itemsPerPage, sortBy, reverse, id, search);
			if (accountBannerTable == null)
			{
				return NotFound();
			}
			return Ok(accountBannerTable);
		}

		[HttpGet]
		[Route("api/Account/BannerImageDatatable")]
		public IHttpActionResult BannerImageDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "",
				  bool reverse = false,
				  int bannerId = 0,
				  string search = null)
		{
			var bannerImageDatatable = _bannerService.GetBannerImageDatatable(page, itemsPerPage, sortBy, reverse, bannerId, search);
			if (bannerImageDatatable == null)
			{
				return NotFound();
			}
			return Ok(bannerImageDatatable);
		}

		public void Post(AccountViewModel account)
		{
			_accountService.CreateAccount(account);
		}

		[HttpPost]
		[Route("api/Account/CreateListing")]
		public void CreateListing(ListingViewModel listing)
		{
			_accountService.CreateListing(listing);
		}

		[HttpPost]
		[Route("api/Account/CreateListingAddress")]
		public void CreateListingAddress(ListingAddressViewModel address)
		{
			_accountService.CreateListingAddress(address);
		}

		[HttpPost]
		[Route("api/Account/CreateBanner")]
		public void CreateBanner(BannerViewModel banner)
		{
			_accountService.CreateBanner(banner);
		}

		public void Put(AccountViewModel vmAccount)
		{
			_accountService.UpdateAccount(vmAccount);
		}

		[HttpPut]
		[Route("api/Account/UpdateListing")]
		public void UpdateListing(ListingViewModel vmListing)
		{
			_accountService.UpdateListing(vmListing);
		}

		[HttpPut]
		[Route("api/Account/UpdateListingAddress")]
		public void UpdateListingAddress(ListingAddressViewModel vmAddress)
		{
			_accountService.UpdateListingAddress(vmAddress);
		}

		[HttpPut]
		[Route("api/Account/UpdateBanner")]
		public void UpdateBanner(BannerViewModel vmBanner)
		{
			_accountService.UpdateBanner(vmBanner);
		}

		[HttpPut]
		[Route("api/Account/UpdateListingImage")]
		public void UpdateListingImage(ListingImageViewModel vmImage)
		{
			_accountService.UpdateListingImage(vmImage);
		}

		[HttpPut]
		[Route("api/Account/UpdateBannerImage")]
		public void UpdateBannerImage(BannerImageViewModel vmImage)
		{
			_accountService.UpdateBannerImage(vmImage);
		}

		public void Delete(int id)
		{
			_accountService.DeleteAccount(id);
		}

		[HttpDelete]
		[Route("api/Account/DeleteAccountKeyword/{id}")]
		public void DeleteAccountKeyword(int id)
		{
			_accountService.DeleteAccountKeyword(id);
		}

		[HttpDelete]
		[Route("api/Account/DeleteListingKeyword/{id}")]
		public void DeleteListingKeyword(int id)
		{
			_accountService.DeleteListingKeyword(id);
		}

		[HttpDelete]
		[Route("api/Account/DeleteAccountContact/{id}")]
		public void DeleteAccountContact(int id)
		{
			_accountService.DeleteAccountContact(id);
		}

		[HttpDelete]
		[Route("api/Account/DeleteAccountComment/{id}")]
		public void DeleteAccountComment(int id)
		{
			_accountService.DeleteAccountComment(id);
		}

		[HttpDelete]
		[Route("api/Account/DeleteAccountListing/{id}")]
		public void DeleteAccountListing(int id)
		{
			_accountService.DeleteAccountListing(id);
		}

		[HttpDelete]
		[Route("api/Account/DeleteAccountBanner/{id}")]
		public void DeleteAccountBanner(int id)
		{
			_accountService.DeleteAccountBanner(id);
		}

		[HttpDelete]
		[Route("api/Account/DeleteListingAddress/{id}")]
		public void DeleteListingAddress(int id)
		{
			_accountService.DeleteListingAddress(id);
		}

		#region UPLOADING PICTURE PROCESSING
		public async Task<IHttpActionResult> Get()
		{
			var results = await photoManager.Get();
			return Ok(new { photos = results });
		}

		// POST: api/Photo
		[HttpPost]
		[Route("api/Account/UploadListingFile/{listingId}")]
		public async Task<IHttpActionResult> UploadListingFile(int listingId)
		{
			// Check if the request contains multipart/form-data.
			if (!Request.Content.IsMimeMultipartContent("form-data"))
			{
				return BadRequest("Unsupported media type");
			}

			try
			{
				var photos = await photoManager.AddListingImage(Request, listingId);
				return Ok(new { Message = "Photos uploaded ok", Photos = photos });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.GetBaseException().Message);
			}
		}

		[HttpPost]
		[Route("api/Account/UploadBannerFile/{bannerId}")]
		public async Task<IHttpActionResult> UploadBannerFile(int bannerId)
		{
			// Check if the request contains multipart/form-data.
			if (!Request.Content.IsMimeMultipartContent("form-data"))
			{
				return BadRequest("Unsupported media type");
			}

			try
			{
				var photos = await photoManager.AddBannerImage(Request, bannerId);
				return Ok(new { Message = "Photos uploaded ok", Photos = photos });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.GetBaseException().Message);
			}
		}

		// DELETE: api/Photo/5
		[HttpDelete]
		[Route("api/Account/DeleteListingImage/{listingId}/{id}")]
		public async Task<IHttpActionResult> DeleteListingImage(int listingId, int id)
		{
			if (!this.photoManager.ListingFileExists(listingId, id))
			{
				return NotFound();
			}

			var result = await this.photoManager.DeleteListingImage(listingId, id);

			if (result.Successful)
			{
				return Ok(new { message = result.Message });
			}
			else
			{
				return BadRequest(result.Message);
			}
		}

		[HttpDelete]
		[Route("api/Account/DeleteBannerImage/{bannerId}/{id}")]
		public async Task<IHttpActionResult> DeleteBannerImage(int bannerId, int id)
		{
			if (!this.photoManager.BannerFileExists(bannerId, id))
			{
				return NotFound();
			}

			var result = await this.photoManager.DeleteBannerImage(bannerId, id);

			if (result.Successful)
			{
				return Ok(new { message = result.Message });
			}
			else
			{
				return BadRequest(result.Message);
			}
		}
		#endregion END UPLOAD PICTURE PROCESSING
	}

}
