using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_Core.DataBaseObjects.Areas.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Core.Mappings.Areas.Masters
{
    public class UDCMap : IEntityTypeConfiguration<UDCMaster>
    {
        public void Configure(EntityTypeBuilder<UDCMaster> builder)
        {
            builder.ToTable("UDCMaster");

            builder.HasKey(x => x.Id);
            
            
            builder.HasOne(x => x.AddedBy).WithMany().HasForeignKey(x => x.AddedById).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.UpdatedBy).WithMany().HasForeignKey(x => x.UpdatedById).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey(x => x.DeletedById).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
