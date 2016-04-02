using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class CardGroupMap : EntityTypeConfiguration<CardGroup>
	{
		public CardGroupMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsRequired()
				.IsUnicode(false)
				.HasMaxLength(50);

			this.Property(t => t.Name)
				.IsRequired()
				.IsUnicode(false)
				.HasMaxLength(50);

			this.Property(t => t.Description)
				.IsUnicode(true);

			this.Property(t => t.AppleProductCode)
				.IsUnicode(false)
				.HasMaxLength(200);

			this.Property(t => t.GoogleProductCode)
				.IsUnicode(false)
				.HasMaxLength(200);

			this.Property(t => t.Type)
				.IsUnicode(false)
				.IsFixedLength()
				.HasMaxLength(1);

			this.Property(t => t.Price)
				.HasPrecision(12,0);

            this.Property(t => t.Active)
                .IsUnicode(false)
               .IsFixedLength()
               .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("CardGroup");
		}
	}
}
