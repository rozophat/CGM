using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class CategoryMap : EntityTypeConfiguration<Category>
	{
		public CategoryMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(t => t.Name)
				.HasMaxLength(200)
				.IsRequired();

			this.Property(t => t.Name_FR)
				.HasMaxLength(200);

			this.Property(t => t.Name_ES)
				.HasMaxLength(200);

			this.Property(t => t.Name_PT)
				.HasMaxLength(200);


			// Table & Column Mappings
			this.ToTable("Category", "leepsi");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.ParentId).HasColumnName("ParentId");
			this.Property(t => t.Type).HasColumnName("Type");
			this.Property(t => t.Name_FR).HasColumnName("Name_FR");
			this.Property(t => t.Name_ES).HasColumnName("Name_ES");
			this.Property(t => t.Name_PT).HasColumnName("Name_PT");
		}
	}
}
