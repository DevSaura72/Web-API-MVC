using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_Framework.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer.GenericRepository
{
    public interface IGenericServices<T> where T : class
    {
        public Task<Result<IEnumerable<T>>> GetAllAsync();

        public Task<Result<T>> GetByIdAsync(int id);

        public Task<Result> CreateAsync(T entity);

        public Task<Result> UpdateAsync(T entity);
        public Task<Result> DelteAsync(int id);

        public int? GetCurrentUser();

    }
}
