using AutoMapper;
using Root.Models;
using Website.ViewModels.Asset;
using Website.ViewModels.Card;
using Website.ViewModels.CardGroup;
using Website.ViewModels.Player;

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
            Mapper.CreateMap<CardGroup, CardGroupViewModel>();
            Mapper.CreateMap<Card, CardViewModel>();
			Mapper.CreateMap<Player, PlayerViewModel>();
			Mapper.CreateMap<PlayerAsset, PlayerAssetViewModel>();
			Mapper.CreateMap<PlayerStar, PlayerStarViewModel>();
			Mapper.CreateMap<PlayerCardGroup, PlayerCardGroupViewModel>();
            Mapper.CreateMap<Asset, AssetViewModel>();
        }
	}
}