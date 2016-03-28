using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CommentType)
                .IsUnicode(false)
                .HasMaxLength(1)
                .IsFixedLength();

			this.Property(t => t.ObjectId)
			   .IsRequired();

            this.Property(t => t.ByAccountId)
                .IsRequired();

            this.Property(t => t.Title)
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(200);

            // Table & Column Mappings
			this.ToTable("Comment", "leepsi");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CommentType).HasColumnName("CommentType");
			this.Property(t => t.ObjectId).HasColumnName("ObjectId");
            this.Property(t => t.ByAccountId).HasColumnName("ByAccountId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Rating).HasColumnName("Rating");
        }
    }
}
