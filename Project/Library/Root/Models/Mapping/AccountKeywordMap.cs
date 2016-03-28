using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
    public class AccountKeywordMap : EntityTypeConfiguration<AccountKeyword>
    {
        public AccountKeywordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.KeywordId)
                .IsRequired();

            // Table & Column Mappings
			this.ToTable("AccountKeyword", "leepsi");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.KeywordId).HasColumnName("KeywordId");
        }
    }
}
