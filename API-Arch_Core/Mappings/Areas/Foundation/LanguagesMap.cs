using API_Arch_Core.DataBaseObjects.Areas.Foundation;
using API_Arch_Core.DataBaseObjects.Areas.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Core.Mappings.Areas.Foundation
{
    public class LanguagesMap : IEntityTypeConfiguration<Languages>
    {
        public void Configure(EntityTypeBuilder<Languages> builder)
        {
            builder.ToTable("Languages");

            builder.Property(x => x.Language).HasMaxLength(25);
            builder.Property(x => x.CultureCode).HasMaxLength(5);

            builder.HasAlternateKey(x => x.CultureCode);

            builder.HasOne(x => x.AddedBy).WithMany().HasForeignKey(x => x.DeletedById).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.UpdatedBy).WithMany().HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey(x => x.DeletedById).OnDelete(DeleteBehavior.Restrict);
        }


    }
}
