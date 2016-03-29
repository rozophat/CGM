using AutoMapper;
using Root.Models;

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
		}
	}
}