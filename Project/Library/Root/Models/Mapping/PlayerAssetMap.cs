using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class PlayerAssetMap : EntityTypeConfiguration<PlayerAsset>
	{
		public PlayerAssetMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.AssetId)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.PlayerId)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.Used)
				.IsUnicode(false)
				.IsFixedLength()
				.HasMaxLength(1);

			this.Property(t => t.UsedCardId)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			// Table & Column Mappings
			this.ToTable("PlayerAsset");
		}
	}
}
