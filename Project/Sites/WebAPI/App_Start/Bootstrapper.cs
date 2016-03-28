using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Service.Mappers;
using Service.Services;
using WebAPI.Photo;

namespace WebAPI.App_Start
{
	public class Bootstrapper
	{
		public static void Run()
		{
			AutoMapperConfiguration.Configure();
		}

		public static IContainer SetAutofacContainer()
		{
			var builder = new ContainerBuilder();

			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
			builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
			builder.RegisterAssemblyTypes(typeof(AccountRepository).Assembly)
			.Where(t => t.Name.EndsWith("Repository"))
			.AsImplementedInterfaces().InstancePerRequest();
			builder.RegisterAssemblyTypes(typeof(AccountService).Assembly)
			.Where(t => t.Name.EndsWith("Service"))
			.AsImplementedInterfaces().InstancePerRequest();
			builder.RegisterAssemblyTypes(typeof(LocalPhotoManager).Assembly)
			.Where(t => t.Name.EndsWith("Manager"))
			.AsImplementedInterfaces().InstancePerRequest();

			var container = builder.Build();
			return container;
		}
	}
}