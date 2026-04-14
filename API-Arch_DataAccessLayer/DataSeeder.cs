using API_Arch_Core.DataBaseObjects.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer
{
    public static class DataSeeder
    {
        public static async Task SeedSystemAdminAsync(IServiceProvider serviceProvider)
        {
			try
			{
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                var context = serviceProvider.GetRequiredService<AppDbContext>();

                if (!await roleManager.RoleExistsAsync("SystemAdmin"))
                {
                    var sysAdminRole = new AppRole
                    {
                        Name = "SystemAdmin"
                    };
                    await roleManager.CreateAsync(sysAdminRole);
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    var userRole = new AppRole
                    {
                        Name = "User"
                    };
                    await roleManager.CreateAsync(userRole);
                }

                var user = await userManager.FindByNameAsync("systemadmin");
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = "systemadmin",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                        PhoneNumber = "8055568659",
                        PhoneNumberConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Admin@123");

                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                if (!await userManager.IsInRoleAsync(user, "SystemAdmin"))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(user, "SystemAdmin");
                    if (!addToRoleResult.Succeeded)
                    {
                        throw new Exception("Failed to add user to SystemAdmin role: " + string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                    }
                }
            }
			catch (Exception ex)
			{
                Console.WriteLine("Startup Error: " + ex);
                throw;
			}
        }

    }
}

