using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class PlayerStarMap : EntityTypeConfiguration<PlayerStar>
	{
		public PlayerStarMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsRequired()
				.IsUnicode(false)
				.HasMaxLength(50);

			this.Property(t => t.PlayerId)
				.IsRequired()
				.IsUnicode(false)
				.HasMaxLength(50);

			this.Property(t => t.PlayerCardGroupId)
				.IsRequired()
				.IsUnicode(false)
				.HasMaxLength(50);

			this.Property(t => t.PurchaseTransactionId)
				.HasMaxLength(100);

			// Table & Column Mappings
			this.ToTable("PlayerStar");
		}
	}
}
