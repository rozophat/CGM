using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
    public class ListingAddressMap : EntityTypeConfiguration<ListingAddress>
    {
        public ListingAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.ListingId)
                .IsRequired();

            this.Property(t => t.AddressLine1)
                .HasMaxLength(200);

			this.Property(t => t.AddressLine2)
				.HasMaxLength(200);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.ProvinceState)
                .HasMaxLength(50);

            this.Property(t => t.Country)
                .HasMaxLength(50);

            this.Property(t => t.PostalZip)
                .HasMaxLength(50);

            this.Property(t => t.GPSLatitude)
                .HasMaxLength(50);

			this.Property(t => t.GPSLongitude)
				.HasMaxLength(50);

            // Table & Column Mappings
			this.ToTable("ListingAddress", "leepsi");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ListingId).HasColumnName("ListingId");
            this.Property(t => t.AddressLine1).HasColumnName("AddressLine1");
			this.Property(t => t.AddressLine2).HasColumnName("AddressLine2");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.ProvinceState).HasColumnName("ProvinceState");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.PostalZip).HasColumnName("PostalZip");
			this.Property(t => t.GPSLatitude).HasColumnName("GPSLatitude");
			this.Property(t => t.GPSLongitude).HasColumnName("GPSLongitude");
        }
    }
}
