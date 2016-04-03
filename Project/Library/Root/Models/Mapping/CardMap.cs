using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class CardMap : EntityTypeConfiguration<Card>
	{
		public CardMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.IsUnicode(false)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.GroupId)
				.IsUnicode(false)
				.HasMaxLength(50);

			this.Property(t => t.Type)
				.IsFixedLength()
				.HasMaxLength(1);

            this.Property(t => t.Difficulty)
                .IsFixedLength()
                .HasMaxLength(1);

            //this.Property(t => t.Question1)
            //	.IsUnicode(false);

            //this.Property(t => t.Question2)
            //	.IsUnicode(false);

            //this.Property(t => t.Question3)
            //	.IsUnicode(false);

            //this.Property(t => t.Answer1)
            //	.IsUnicode(false);

            //this.Property(t => t.Answer2)
            //	.IsUnicode(false);

            //this.Property(t => t.Answer3)
            //	.IsUnicode(false);

            //this.Property(t => t.Hint1)
            //	.IsUnicode(false);

            //this.Property(t => t.Hint2)
            //	.IsUnicode(false);

            //this.Property(t => t.Hint3)
            //	.IsUnicode(false);

            //this.Property(t => t.AnsweredCorrectCount1)
            //	.IsUnicode(false);

            //this.Property(t => t.AnsweredCorrectCount2)
            //	.IsUnicode(false);

            //this.Property(t => t.AnsweredCorrectCount3)
            //	.IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Card");
		}
	}
}
