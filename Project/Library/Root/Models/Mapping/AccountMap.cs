using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.LastName)
                .HasMaxLength(20);

            this.Property(t => t.FirstName)
                .HasMaxLength(20);

            this.Property(t => t.PhoneNumber)
				.IsUnicode(false)
                .HasMaxLength(20);

            this.Property(t => t.Email)
				.IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.DOB)
                .HasColumnType("datetime2");

            this.Property(t => t.Sex)
                .IsUnicode(false)
                .HasMaxLength(1);

            this.Property(t => t.AddressLine1)
                .IsUnicode(false)
                .HasMaxLength(100);

            this.Property(t => t.AddressLine2)
                .IsUnicode(false)
                .HasMaxLength(100);

            this.Property(t => t.City)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.ProvinceState)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.Country)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.PostalZip)
                .IsUnicode(false)
                .HasMaxLength(30);

            this.Property(t => t.GPSLatitude)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(t => t.GPSLongitude)
                .IsUnicode(false)
                .HasMaxLength(50);

			this.Property(t => t.FBId)
				.IsUnicode(false)
				.HasMaxLength(150);

			this.Property(t => t.GId)
				.IsUnicode(false)
				.HasMaxLength(150);

			this.Property(t => t.Password)
				.IsUnicode(false)
				.HasMaxLength(50);

            // Table & Column Mappings
			this.ToTable("Account", "leepsi");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Email).HasColumnName("Email");
        }
    }
}
