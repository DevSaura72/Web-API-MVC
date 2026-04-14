using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_Core.Mappings.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using API_Arch_Core.DataBaseObjects.Areas.Masters;
using API_Arch_Core.DataBaseObjects.Areas.Foundation;

namespace API_Arch_DataAccessLayer
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AppUser>(b =>
            //{
            //    b.ToTable("AspNetUsers");
            //});

            //modelBuilder.Entity<AppRoleMap>(b =>
            //{
            //    b.ToTable("AspNetRoles");
            //});

            modelBuilder.Entity<IdentityUserRole<int>>(b =>
            {
                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(b =>
            {
                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<int>>(b =>
            {
                b.ToTable("AspNetUserTokens");
            });


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppRoleMap).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppUserMap).Assembly);

            #region MasterConfig
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UDCMaster).Assembly);
            #endregion


            #region Foundation
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Languages).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Translations).Assembly);
            #endregion
        }

        #region Foundation
        public DbSet<Languages> Languages { get; set; }
        public DbSet<Translations> Translations { get; set; }
        #endregion


        #region Masters
        public DbSet<UDCMaster> UDCMaster { get; set; } 
        #endregion
    }
}
