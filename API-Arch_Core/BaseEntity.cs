using API_Arch_Core.DataBaseObjects.Areas.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Core
{
    [PrimaryKey(nameof(Id))]
    public class BaseEntity
    {
        public int Id { get; set; } = new int();
        public int AddedById { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? DeletedById { get; set; }
        public DateTime? DeletedOn { get; set; }


        public AppUser AddedBy { get; set; }
        public AppUser? UpdatedBy { get; set; }
        public AppUser? DeletedBy { get; set; }
    }
}
