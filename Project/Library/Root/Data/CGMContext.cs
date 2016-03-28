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

        public DbSet<Account> Account { get; set; }
		public DbSet<AccountContact> AccountContact { get; set; }
		public DbSet<AccountKeyword> AccountKeyword { get; set; }
		public DbSet<AccountLike> AccountLike { get; set; }
		public DbSet<Banner> Banner { get; set; }
		public DbSet<BannerImage> BannerImage { get; set; }
		public DbSet<BannerImageType> BannerImageType { get; set; }
		public DbSet<Comment> Comment { get; set; }
		public DbSet<Keyword> Keyword { get; set; }
		public DbSet<Listing> Listing { get; set; }
		public DbSet<ListingKeyword> ListingKeyword { get; set; }
		public DbSet<ListingAddress> ListingAddress { get; set; }
		public DbSet<ListingImage> ListingImage { get; set; }
		public DbSet<ListingLike> ListingLike { get; set; }
		public DbSet<ListingImageType> ListingImageType { get; set; }
		public DbSet<Category> Category { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Configurations.Add(new AccountMap());
			modelBuilder.Configurations.Add(new AccountContactMap());
			modelBuilder.Configurations.Add(new AccountKeywordMap());
			modelBuilder.Configurations.Add(new AccountLikeMap());
			modelBuilder.Configurations.Add(new BannerMap());
			modelBuilder.Configurations.Add(new BannerImageMap());
			modelBuilder.Configurations.Add(new BannerImageTypeMap());
			modelBuilder.Configurations.Add(new CommentMap());
			modelBuilder.Configurations.Add(new KeywordMap());
			modelBuilder.Configurations.Add(new ListingMap());
			modelBuilder.Configurations.Add(new ListingKeywordMap());
			modelBuilder.Configurations.Add(new ListingAddressMap());
			modelBuilder.Configurations.Add(new ListingImageMap());
			modelBuilder.Configurations.Add(new ListingLikeMap());
			modelBuilder.Configurations.Add(new ListingImageTypeMap());
			modelBuilder.Configurations.Add(new CategoryMap());

			base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
			//authentication db
			modelBuilder.Entity<User>().ToTable("Users", schemaName);
			modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", schemaName);
			modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", schemaName);
			modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", schemaName);
			modelBuilder.Entity<IdentityRole>().ToTable("Roles", schemaName);
		}

		public virtual void Commit()
		{
			base.SaveChanges();
		}
	}
}
