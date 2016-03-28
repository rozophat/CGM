using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class ListingImageMap : EntityTypeConfiguration<ListingImage>
	{
		public ListingImageMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(t => t.ListingId)
				.IsRequired();

			this.Property(t => t.FileName)
				.IsUnicode(false)
				.HasMaxLength(200);

			// Table & Column Mappings
			this.ToTable("ListingImage", "leepsi");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.ListingId).HasColumnName("ListingId");
			this.Property(t => t.FileName).HasColumnName("FileName");
		}
	}
}
