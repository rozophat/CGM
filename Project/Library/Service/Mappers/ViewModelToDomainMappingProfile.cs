using AutoMapper;
using Root.Models;
using Website.ViewModels.Card;
using Website.ViewModels.CardGroup;

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
            Mapper.CreateMap<CardGroupViewModel, CardGroup>();
            Mapper.CreateMap<CardViewModel, Card>();
        }
	}
}