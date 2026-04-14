using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer.Interphases.Areas.Foundation
{
    public interface IDbLocalizer
    {
        string Get(string key);
    }
}
