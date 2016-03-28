using AutoMapper;
using Root.Models;
using Website.ViewModels.Account;
using Website.ViewModels.Banner;
using Website.ViewModels.Category;
using Website.ViewModels.Keyword;
using Website.ViewModels.Listing;

namespace Service.Mappers
{
	public class DomainToViewModelMappingProfile : Profile
	{
		public override string ProfileName
		{
			get
			{
				return "DomainToViewModelMappingProfile";
			}
		}


		protected override void Configure()
		{
			//Mapper.CreateMap<ResourceActivity,ResourceActivityViewModel>()
			//    .ForMember(vm => vm.ActivityDateString, dm=> dm.MapFrom(dModel => dModel.ActivityDate.ToLongDateString()));

			//Mapper.CreateMap<Department_M, EmployeeViewModel>()
			//    .ForMember(src => src.DepC, dest => dest.MapFrom(s => s.DepC));
			Mapper.CreateMap<Account, AccountViewModel>();
			Mapper.CreateMap<Keyword, KeywordViewModel>();
			Mapper.CreateMap<Listing, ListingViewModel>();
			Mapper.CreateMap<Banner, BannerViewModel>();
			Mapper.CreateMap<ListingAddress, ListingAddressViewModel>();
			Mapper.CreateMap<Category, CategoryViewModel>();
		}
	}
}