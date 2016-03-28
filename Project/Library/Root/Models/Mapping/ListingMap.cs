using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Root.Models.Mapping
{
	public class ListingMap : EntityTypeConfiguration<Listing>
	{
		public ListingMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(t => t.AccountId)
				.IsRequired();

			this.Property(t => t.Title)
				.HasMaxLength(200);

			this.Property(t => t.ListingType)
				.IsUnicode(false)
				.IsFixedLength()
				.HasMaxLength(1);

			this.Property(t => t.TransactionId)
				.IsUnicode(false)
				.HasMaxLength(200);

			this.Property(t => t.Active)
				.IsUnicode(false)
				.IsFixedLength()
				.HasMaxLength(1);

			// Table & Column Mappings
			this.ToTable("Listing", "leepsi");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.AccountId).HasColumnName("AccountId");
			this.Property(t => t.Title).HasColumnName("Title");
			this.Property(t => t.Description).HasColumnName("Description");
			this.Property(t => t.Paid).HasColumnName("Paid");
			this.Property(t => t.TransactionId).HasColumnName("TransactionId");
			this.Property(t => t.Active).HasColumnName("Active");
			this.Property(t => t.CategoryId).HasColumnName("CategoryId");
			this.Property(t => t.GPSLatitude).HasColumnName("GPSLatitude");
			this.Property(t => t.GPSLongitude).HasColumnName("GPSLongitude");
		}
	}
}
