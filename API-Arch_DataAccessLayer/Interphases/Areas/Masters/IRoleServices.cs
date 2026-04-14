using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_DataAccessLayer.GenericRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer.Interphases.Areas.Masters
{
    public interface IRoleServices : IGenericServices<AppRole>
    {
    }
}
