using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Arch_Core.DataBaseObjects.Areas.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

    }
}
