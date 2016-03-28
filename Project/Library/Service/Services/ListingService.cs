using System.Linq;
using System.Linq.Dynamic;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Website.ViewModels.Listing;

namespace Service.Services
{
	public interface IListingService
	{

		ListingDatatable GetListingDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
		ListingImageDatatable GetListingImageDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int listingId, string searchValue);
		void SaveListing();
	}

	public class ListingService : IListingService
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IListingRepository _listingRepository;
		private readonly IListingImageRepository _listingImageRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ListingService(IAccountRepository accountRepository, IListingRepository listingRepository, ICategoryRepository categoryRepository,
								IListingImageRepository listingImageRepository, IUnitOfWork unitOfWork)
		{
			this._accountRepository = accountRepository;
			this._listingRepository = listingRepository;
			this._listingImageRepository = listingImageRepository;
			this._categoryRepository = categoryRepository;
			this._unitOfWork = unitOfWork;
		}

		public ListingDatatable GetListingDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			var listings = from l in _listingRepository.GetAllQueryable()
						   join a in _accountRepository.GetAllQueryable() on l.AccountId equals a.Id into la
						   from a in la.DefaultIfEmpty()
						   join c in _categoryRepository.GetAllQueryable() on l.CategoryId equals c.Id into lc
						   from c in lc.DefaultIfEmpty()
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
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				listings = listings.Where(p => p.AccountName.ToLower().Contains(searchValue) || p.Title.ToLower().Contains(searchValue) || p.Description.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var listingsOrdered = listings.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var listingsPaged = listingsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var listingDatatable = new ListingDatatable()
			{
				Data = listingsPaged,
				Total = listings.Count()
			};
			return listingDatatable;
		}

		public ListingImageDatatable GetListingImageDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int listingId,
															  string searchValue)
		{
			var pictures = _listingImageRepository.Query(p => p.ListingId == listingId);

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var picturesOrdered = pictures.OrderBy("Id descending");

			// paging
			var picturesPaged = picturesOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var destination = from p in picturesPaged
							  select new ListingImageViewModel()
							  {
								  Id = p.Id,
								  ListingId = p.ListingId,
								  FileName = p.FileName,
								  PictureDestination = @"Album/Listing/" + p.ListingId + @"/" + p.FileName,
								  ImageType = p.ImageType
							  };


			var listingImageDatatable = new ListingImageDatatable()
			{
				Data = destination.ToList(),
				Total = pictures.Count()
			};
			return listingImageDatatable;
		}

		public void SaveListing()
		{
			_unitOfWork.Commit();
		}
	}
}
