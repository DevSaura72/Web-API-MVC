using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_Core.DataBaseObjects.Areas.Masters;
using API_Arch_Framework.Areas.Identity;
using API_Arch_Framework.Areas.Masters;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_Framework.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserDto>();
            CreateMap<AppUserDto, AppUser>();

            #region Roles
            CreateMap<AppRole, AppRoleDto>();
            CreateMap<AppRoleDto, AppRole>();
            #endregion

            #region UDCMaster
            CreateMap<UDCMaster, UDCDto>();
            CreateMap<UDCDto, UDCMaster>();
            #endregion
        }
    }
}
