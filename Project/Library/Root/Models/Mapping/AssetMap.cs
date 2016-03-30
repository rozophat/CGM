﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class AssetMap : EntityTypeConfiguration<Asset>
	{
		public AssetMap()
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

			this.Property(t => t.Code)
				.IsRequired()
				.IsUnicode(false)
				.HasMaxLength(50);

			// Table & Column Mappings
			this.ToTable("Asset");
		}
	}
}
