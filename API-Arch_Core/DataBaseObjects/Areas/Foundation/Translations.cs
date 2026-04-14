using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Core.DataBaseObjects.Areas.Foundation
{
    public class Translations : BaseEntity
    {
        public string CultureCode { get; set; }

        public string TranslationKey { get; set; }
        public string TranslationValue { get; set; }

        public virtual Languages Language { get; set; }
    }
}
