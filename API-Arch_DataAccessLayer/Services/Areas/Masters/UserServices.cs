using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_DataAccessLayer.GenericRepository;
using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_Framework.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer.Services.Areas.Masters
{
    public class UserServices : IUserServices
    {
        private readonly IGenericServices<AppUser> _services;
        public UserServices(IGenericServices<AppUser> services) 
        {
            _services = services;
        }

        public Task<Result> CreateAsync(AppUser entity)
        {
            return _services.CreateAsync(entity);
        }

        public Task<Result<IEnumerable<AppUser>>> GetAllAsync()
        {
            return _services.GetAllAsync();
        }

        public Task<Result<AppUser>> GetByIdAsync(int id)
        {
            return _services.GetByIdAsync(id);
        }

        public int? GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(AppUser entity)
        {

            return _services.UpdateAsync(entity);
        }
        public Task<Result> DelteAsync(int id)
        {
            var user = GetByIdAsync(id).Result.Data;
            user.IsDeleted = true;

            var res = UpdateAsync(user);

            return res;
        }


    }
}
