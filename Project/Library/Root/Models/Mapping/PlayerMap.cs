using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class PlayerMap : EntityTypeConfiguration<Player>
	{
		public PlayerMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.FirstName)
				.HasMaxLength(50);

			this.Property(t => t.LastName)
				.HasMaxLength(50);

			this.Property(t => t.DeviceToken)
				.IsUnicode(false)
				.HasMaxLength(100);

			this.Property(t => t.FacebookId)
				.IsUnicode(false)
				.HasMaxLength(100);

			this.Property(t => t.FullName)
				.HasMaxLength(50);

			this.Property(t => t.NickName)
				.HasMaxLength(20);

			this.Property(t => t.Email)
				.IsUnicode(false)
				.HasMaxLength(50);

			this.Property(t => t.Password)
				.HasMaxLength(50);

			this.Property(t => t.City)
				.HasMaxLength(20);

			this.Property(t => t.ProvinceState)
				.HasMaxLength(20);

			this.Property(t => t.Country)
				.HasMaxLength(20);

			// Table & Column Mappings
			this.ToTable("Player");
		}
	}
}
