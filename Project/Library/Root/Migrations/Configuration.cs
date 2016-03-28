using System.Collections.Generic;
using Root.AuthenticationModels;
using Root.Models;

namespace Root.Migrations
{
	using System.Data.Entity.Migrations;
	using System.Linq;

	public class Configuration : DbMigrationsConfiguration<Root.Data.CGMContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			//Because we are developing product so lossingn data is not a big issue
			AutomaticMigrationDataLossAllowed = true;
		}

		protected override void Seed(Root.Data.CGMContext context)
		{
		}
	}
}
