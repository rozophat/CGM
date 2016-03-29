using AutoMapper;
using Root.Models;

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
		}
	}
}