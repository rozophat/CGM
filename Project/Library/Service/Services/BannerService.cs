using System.Linq;
using System.Linq.Dynamic;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Website.ViewModels.Banner;

namespace Service.Services
{
	public interface IBannerService
	{
		BannerDatatable GetBannerDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
		BannerImageDatatable GetBannerImageDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int bannerId, string searchValue);
		void SaveBanner();
	}

	public class BannerService : IBannerService
	{
		private readonly IBannerRepository _bannerRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IBannerImageRepository _bannerImageRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public BannerService(IAccountRepository accountRepository, IBannerRepository bannerRepository, IBannerImageRepository bannerImageRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
		{
			this._accountRepository = accountRepository;
			this._bannerRepository = bannerRepository;
			this._bannerImageRepository = bannerImageRepository;
			this._categoryRepository = categoryRepository;
			this._unitOfWork = unitOfWork;
		}

		public BannerDatatable GetBannerDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			var banners = from b in _bannerRepository.GetAllQueryable()
						  join a in _accountRepository.GetAllQueryable() on b.AccountId equals a.Id into ba
						  from a in ba.DefaultIfEmpty()
						  join c in _categoryRepository.GetAllQueryable() on b.CategoryId equals c.Id into bc
						  from c in bc.DefaultIfEmpty()
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
				banners = banners.Where(p => p.AccountName.ToLower().Contains(searchValue) || p.Title.ToLower().Contains(searchValue) || p.Description.ToLower().Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var bannersOrdered = banners.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var bannersPaged = bannersOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var bannerDatatable = new BannerDatatable()
			{
				Data = bannersPaged,
				Total = banners.Count()
			};
			return bannerDatatable;
		}

		public BannerImageDatatable GetBannerImageDatatable(int page, int itemsPerPage, string sortBy, bool reverse, int bannerId,
															string searchValue)
		{
			var pictures = _bannerImageRepository.Query(p => p.BannerId == bannerId);

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var picturesOrdered = pictures.OrderBy("Id descending");

			// paging
			var picturesPaged = picturesOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var destination = from p in picturesPaged
							  select new BannerImageViewModel()
							  {
								  Id = p.Id,
								  BannerId = p.BannerId,
								  FileName = p.FileName,
								  PictureDestination = @"Album/Banner/" + p.BannerId + @"/" + p.FileName,
								  ImageType = p.ImageType
							  };


			var bannerImageDatatable = new BannerImageDatatable()
			{
				Data = destination.ToList(),
				Total = pictures.Count()
			};
			return bannerImageDatatable;
		}

		public void SaveBanner()
		{
			_unitOfWork.Commit();
		}

	}
}
