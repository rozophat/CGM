using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class LevelMap : EntityTypeConfiguration<Level>
	{
		public LevelMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.Name)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.FrenchLevel)
				.IsUnicode(false)
				.IsFixedLength()
				.HasMaxLength(1);

			// Table & Column Mappings
			this.ToTable("Level");
		}
	}
}
