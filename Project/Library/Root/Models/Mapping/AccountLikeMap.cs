using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
    public class AccountLikeMap : EntityTypeConfiguration<AccountLike>
    {
        public AccountLikeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.ByAccountId)
                .IsRequired();

            // Table & Column Mappings
			this.ToTable("AccountLike", "leepsi");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
			this.Property(t => t.ByAccountId).HasColumnName("ByAccountId");
        }
    }
}
