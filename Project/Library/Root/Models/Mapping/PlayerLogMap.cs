using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class PlayerLogMap : EntityTypeConfiguration<PlayerLog>
	{
		public PlayerLogMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.PlayerId)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			// Table & Column Mappings
			this.ToTable("PlayerLog");
		}
	}
}
