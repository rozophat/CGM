
using System;
using System.Linq;
using System.Net.Http.Formatting;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Root.Data;
using Root.Migrations;
using WebAPI.App_Start;
using WebAPI.Handlers;
using WebAPI.Providers;
using System.Data.Entity;
using System.Net.Http.Headers;

namespace WebAPI
{
	public partial class Startup
	{
		static Startup()
		{
		}

		public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

		public void ConfigureAuth(IAppBuilder app)
		{

			////Migration DB
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<CGMContext, Configuration>());

			var container = Bootstrapper.SetAutofacContainer();
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			log4net.Config.XmlConfigurator.Configure();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			ConfigureOAuth(app);

			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //json
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            app.UseAutofacMiddleware(container);
			app.UseAutofacWebApi(config);

			app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
			app.UseWebApi(config);

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			Bootstrapper.Run();
		}

		public void ConfigureOAuth(IAppBuilder app)
		{
			//use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
			OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

			var OAuthServerOptions = new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
				Provider = new SimpleAuthorizationServerProvider(),
			};

			// Token Generation
			app.UseOAuthAuthorizationServer(OAuthServerOptions);
			app.UseOAuthBearerAuthentication(OAuthBearerOptions);

		}
	}
}
