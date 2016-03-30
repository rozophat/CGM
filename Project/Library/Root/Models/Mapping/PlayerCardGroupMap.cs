using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class PlayerCardGroupMap : EntityTypeConfiguration<PlayerCardGroup>
	{
		public PlayerCardGroupMap()
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

			this.Property(t => t.CardGroupId)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.PurchaseSource)
				.IsUnicode(false)
				.HasMaxLength(200);

			this.Property(t => t.TransactionId)
				.IsUnicode(false)
				.HasMaxLength(200);

			this.Property(t => t.StoreCost)
				.HasMaxLength(15);

			// Table & Column Mappings
			this.ToTable("PlayerCardGroup");
		}
	}
}
