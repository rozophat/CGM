using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Root.Migrations;
using Root.Models;
using Root.Models.Mapping;

namespace Root.Data
{
	public partial class CGMContext : IdentityDbContext<User>
	{
		private string schemaName = "leepsi";

		public CGMContext()
			: base("Name=CGMContext", false)
		{
			//Database.SetInitializer(new MigrateDatabaseToLatestVersion<CGMContext, Configuration>());
		}

		public DbSet<Asset> Asset { get; set; }
		public DbSet<Card> Card { get; set; }
		public DbSet<CardGroup> CardGroup { get; set; }
		public DbSet<Level> Level { get; set; }
		public DbSet<PlayerAsset> PlayerAsset { get; set; }
		public DbSet<Player> Player { get; set; }
		public DbSet<PlayerCardGroup> PlayerCardGroup { get; set; }
		public DbSet<PlayerLog> PlayerLog { get; set; }
		public DbSet<PlayerStar> PlayerStar { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new AssetMap());
			modelBuilder.Configurations.Add(new CardMap());
			modelBuilder.Configurations.Add(new CardGroupMap());
			modelBuilder.Configurations.Add(new LevelMap());
			modelBuilder.Configurations.Add(new PlayerAssetMap());
			modelBuilder.Configurations.Add(new PlayerMap());
			modelBuilder.Configurations.Add(new PlayerCardGroupMap());
			modelBuilder.Configurations.Add(new PlayerLogMap());
			modelBuilder.Configurations.Add(new PlayerStarMap());

			base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
			//authentication db
			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
			modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
			modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
			modelBuilder.Entity<IdentityRole>().ToTable("Roles");
		}

		public virtual void Commit()
		{
			base.SaveChanges();
		}
	}
}
