using AutoMapper;
using Root.Models;
using Website.ViewModels.Account;
using Website.ViewModels.Banner;
using Website.ViewModels.Category;
using Website.ViewModels.Keyword;
using Website.ViewModels.Listing;

namespace Service.Mappers
{
	public class ViewModelToDomainMappingProfile : Profile
	{
		public override string ProfileName
		{
			get
			{
				return "ViewModelToDomainMappingProfile";
			}
		}

		protected override void Configure()
		{
			//Mapper.CreateMap<ResourceActivityViewModel, ResourceActivity>();
			//Mapper.CreateMap<RegisterViewModel, ApplicationUser>().ForMember(user => user.UserName, vm => vm.MapFrom(rm => rm.Email));
			Mapper.CreateMap<AccountViewModel, Account>();
			Mapper.CreateMap<ListingViewModel, Listing>();
			Mapper.CreateMap<BannerViewModel, Banner>();
			Mapper.CreateMap<ListingAddressViewModel, ListingAddress>();
			Mapper.CreateMap<KeywordViewModel, Keyword>();
			Mapper.CreateMap<CategoryViewModel, Category>();
			Mapper.CreateMap<ListingImageViewModel, ListingImage>();
			Mapper.CreateMap<BannerImageViewModel, BannerImage>();
		}
	}
}