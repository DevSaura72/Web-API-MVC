using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_Core.DataBaseObjects.Areas.Masters;
using API_Arch_DataAccessLayer.GenericRepository;
using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_Framework.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer.Services.Areas.Masters
{
    public class UDCServices : IUDCServices
    {
        private readonly IGenericServices<UDCMaster> _UDCServices;
        public UDCServices(IGenericServices<UDCMaster> uDCServices) 
        {
            _UDCServices = uDCServices;
        }
        public Task<Result> CreateAsync(UDCMaster entity)
        {
            var user = GetCurrentUser();
            //entity.AddedById = user.IsSuccess

            
            entity.AddedOn = DateTime.Now;
            entity.AddedById = user != null ? Convert.ToInt32(user) : 0;
            return _UDCServices.CreateAsync(entity);
        }

        public Task<Result<IEnumerable<UDCMaster>>> GetAllAsync()
        {
            return _UDCServices.GetAllAsync();
        }

        public Task<Result<UDCMaster>> GetByIdAsync(int id)
        {
            return _UDCServices.GetByIdAsync(id);
        }

        public int? GetCurrentUser()
        {
            var user = _UDCServices.GetCurrentUser();
            return user;
        }

        public Task<Result> UpdateAsync(UDCMaster entity)
        {
            return (_UDCServices.UpdateAsync(entity));
        }
        public Task<Result> DelteAsync(int id)
        {
            return _UDCServices.DelteAsync(id);
        }

    }
}
