using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using WebAPI.Models;

namespace WebAPI.Photo
{
	public interface IPhotoManager
	{
		Task<IEnumerable<PhotoViewModel>> Get();
		Task<PhotoActionResult> DeleteListingImage(int listingId, int id);
		Task<PhotoActionResult> DeleteBannerImage(int bannerId, int id);
		Task<string> AddListingImage(HttpRequestMessage request, int listingId);
		Task<string> AddBannerImage(HttpRequestMessage request, int bannerId);
		bool ListingFileExists(int listingId, int id);
		bool BannerFileExists(int bannerId, int id);
		void SavePhoto();
	}
	public class LocalPhotoManager : IPhotoManager
	{
		public IListingImageRepository _listingImageRepository;
		public IBannerImageRepository _bannerImageRepository;
		private readonly IUnitOfWork _unitOfWork;
		private string workingListingFolder { get; set; }
		private string workingBannerFolder { get; set; }

		public LocalPhotoManager()
		{

		}

		public LocalPhotoManager(IListingImageRepository listingImageRepository, IBannerImageRepository bannerImageRepository, IUnitOfWork unitOfWork)
		{
			this.workingListingFolder = HttpRuntime.AppDomainAppPath + @"\Album\Listing";
			this.workingBannerFolder = HttpRuntime.AppDomainAppPath + @"\Album\Banner";
			this._listingImageRepository = listingImageRepository;
			this._bannerImageRepository = bannerImageRepository;
			this._unitOfWork = unitOfWork;
			CheckListingTargetDirectory();
			CheckBannerTargetDirectory();
		}

		public async Task<IEnumerable<PhotoViewModel>> Get()
		{
			List<PhotoViewModel> photos = new List<PhotoViewModel>();

			DirectoryInfo photoFolder = new DirectoryInfo(this.workingListingFolder);

			await Task.Factory.StartNew(() =>
			{
				photos = photoFolder.EnumerateFiles()
											.Where(fi => new[] { ".jpg", ".bmp", ".png", ".gif", ".tiff" }.Contains(fi.Extension.ToLower()))
											.Select(fi => new PhotoViewModel
											{
												Name = fi.Name,
												Created = fi.CreationTime,
												Modified = fi.LastWriteTime,
												Size = fi.Length / 1024
											})
											.ToList();
			});

			return photos;
		}

		public async Task<PhotoActionResult> DeleteListingImage(int listingId, int id)
		{
			try
			{
				var picture = _listingImageRepository.Query(p => p.Id == id).FirstOrDefault();
				if (picture != null)
				{
					var listingFolder = this.workingListingFolder + @"\" + listingId;
					var filePath = Directory.GetFiles(listingFolder, picture.FileName)
									.FirstOrDefault();

					await Task.Factory.StartNew(() =>
					{ if (filePath != null) File.Delete(filePath); });

					_listingImageRepository.Delete(p => p.Id == id);
					SavePhoto();

					return new PhotoActionResult { Successful = true, Message = "Deleted successfully" };
				}
				return null;
			}
			catch (Exception ex)
			{
				return new PhotoActionResult { Successful = false, Message = "error deleting fileName " + ex.GetBaseException().Message };
			}
		}

		public async Task<PhotoActionResult> DeleteBannerImage(int bannerId, int id)
		{
			try
			{
				var picture = _bannerImageRepository.Query(p => p.Id == id).FirstOrDefault();
				if (picture != null)
				{
					var bannerFolder = this.workingBannerFolder + @"\" + bannerId;
					var filePath = Directory.GetFiles(bannerFolder, picture.FileName)
									.FirstOrDefault();

					await Task.Factory.StartNew(() =>
					{ if (filePath != null) File.Delete(filePath); });

					_bannerImageRepository.Delete(p => p.Id == id);
					SavePhoto();

					return new PhotoActionResult { Successful = true, Message = "Deleted successfully" };
				}
				return null;
			}
			catch (Exception ex)
			{
				return new PhotoActionResult { Successful = false, Message = "error deleting fileName " + ex.GetBaseException().Message };
			}
		}

		public async Task<string> AddListingImage(HttpRequestMessage request, int listingId)
		{
			var listingFolder = this.workingListingFolder + @"\" + listingId;
			if (!Directory.Exists(listingFolder))
			{
				Directory.CreateDirectory(listingFolder);
			}

			var provider = new PhotoMultipartFormDataStreamProvider(listingFolder);

			await request.Content.ReadAsMultipartAsync(provider);

			foreach (var file in provider.FileData)
			{
				var fileInfo = new FileInfo(file.LocalFileName);

				var picture = new ListingImage()
				{
					ListingId = listingId,
					FileName = fileInfo.Name
				};

				_listingImageRepository.Add(picture);
			}

			SavePhoto();

			return "1";
		}

		public async Task<string> AddBannerImage(HttpRequestMessage request, int bannerId)
		{
			var bannerFolder = this.workingBannerFolder + @"\" + bannerId;
			if (!Directory.Exists(bannerFolder))
			{
				Directory.CreateDirectory(bannerFolder);
			}

			var provider = new PhotoMultipartFormDataStreamProvider(bannerFolder);

			await request.Content.ReadAsMultipartAsync(provider);

			foreach (var file in provider.FileData)
			{
				var fileInfo = new FileInfo(file.LocalFileName);

				var picture = new BannerImage()
				{
					BannerId = bannerId,
					FileName = fileInfo.Name
				};

				_bannerImageRepository.Add(picture);
			}

			SavePhoto();

			return "1";
		}

		public bool ListingFileExists(int listingId, int id)
		{
			var picture = _listingImageRepository.Query(p => p.Id == id).FirstOrDefault();
			if (picture != null)
			{
				var listingFolder = this.workingListingFolder + @"\" + listingId;
				var file = Directory.GetFiles(listingFolder, picture.FileName)
									.FirstOrDefault();

				return file != null;
			}
			return false;
		}

		public bool BannerFileExists(int bannerId, int id)
		{
			var picture = _bannerImageRepository.Query(p => p.Id == id).FirstOrDefault();
			if (picture != null)
			{
				var bannerFolder = this.workingBannerFolder + @"\" + bannerId;
				var file = Directory.GetFiles(bannerFolder, picture.FileName)
									.FirstOrDefault();

				return file != null;
			}
			return false;
		}

		private void CheckListingTargetDirectory()
		{
			if (!Directory.Exists(this.workingListingFolder))
			{
				Directory.CreateDirectory(this.workingListingFolder);
			}
		}

		private void CheckBannerTargetDirectory()
		{
			if (!Directory.Exists(this.workingBannerFolder))
			{
				Directory.CreateDirectory(this.workingBannerFolder);
			}
		}

		public void SavePhoto()
		{
			_unitOfWork.Commit();
		}
	}
}