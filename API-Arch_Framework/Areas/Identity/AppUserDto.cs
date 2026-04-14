using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Arch_Framework.Areas.Identity
{
    public class AppUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public int RoleId { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }
    }
}
