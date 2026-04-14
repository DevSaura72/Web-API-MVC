using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_Framework.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer.GenericRepository
{
    public class GenericServices<T> : IGenericServices<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public GenericServices(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) 
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Result> CreateAsync(T entity)
        {
            try
            {
                await _appDbContext.Set<T>().AddAsync(entity);
                await _appDbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var entities = await _appDbContext.Set<T>().ToListAsync();
                return Result.Success<IEnumerable<T>>(entities);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<T>>(new Error(ex.Message));
            }
         }

        public async Task<Result<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _appDbContext.Set<T>().FindAsync(id);
                if (entity == null)
                    return Result.Failure<T>("Entity not found");

                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure<T>(new Error(ex.Message));
            }
        }

        public int? GetCurrentUser()
        {
            try
            {
                //var userIdClaim = _httpContextAccessor.HttpContext.User;

                //if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                //    return Result.Failure<AppUser>("User ID claim is missing or invalid.");

                var user = _httpContextAccessor.HttpContext.User;
                string NAME = user.Identity.Name;
                if (string.IsNullOrEmpty(NAME))
                    return null;
                else
                    return Convert.ToInt32(_userManager.GetUserId(user));
                 
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Result> UpdateAsync(T entity)
        {
            try
            {
                _appDbContext.Set<T>().Update(entity);
                await _appDbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result> DelteAsync(int id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ex.Message));
            }
        }

    }
}
