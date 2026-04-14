using API_Arch_Core.DataBaseObjects.Areas.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Core.DataBaseObjects.Areas.Foundation
{
    public class Languages : BaseEntity
    {

        public string Language { get; set; }
        public string CultureCode { get; set; }

        public ICollection<Translations> Translations { get; set; }
    }
}
