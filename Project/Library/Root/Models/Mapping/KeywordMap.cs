using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
    public class KeywordMap : EntityTypeConfiguration<Keyword>
    {
        public KeywordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(t => t.KeywordName)
                .HasMaxLength(10)
                .IsRequired();
            
            // Table & Column Mappings
			this.ToTable("Keyword", "leepsi");
            this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.KeywordName).HasColumnName("KeywordName");
			this.Property(t => t.KeywordType).HasColumnName("KeywordType");
        }
    }
}
