using API_Arch_Core.DataBaseObjects.Areas.Foundation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Core.Mappings.Areas.Foundation
{
    public class TranslationsMap : IEntityTypeConfiguration<Translations>
    {
        public void Configure(EntityTypeBuilder<Translations> builder)
        {
            builder.ToTable("Translations");

            builder.Property(x => x.CultureCode).HasMaxLength(5).IsRequired();
            builder.Property(x => x.TranslationKey).HasMaxLength(250).IsRequired();
            builder.Property(x => x.TranslationValue).HasColumnType("nvarchar(500)").HasMaxLength(250).IsRequired();

            builder.HasOne(x => x.Language).WithMany(x => x.Translations).HasForeignKey(x => x.CultureCode).HasPrincipalKey(x => x.CultureCode).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AddedBy).WithMany().HasForeignKey(x => x.AddedById).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.UpdatedBy).WithMany().HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey(x => x.DeletedById).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
