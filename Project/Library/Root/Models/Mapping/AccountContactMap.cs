using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
    public class AccountContactMap : EntityTypeConfiguration<AccountContact>
    {
        public AccountContactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.ContactId)
                .IsRequired();

            // Table & Column Mappings
			this.ToTable("AccountContact", "leepsi");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
        }
    }
}
