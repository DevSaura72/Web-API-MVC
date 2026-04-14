using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_DataAccessLayer.GenericRepository;
using API_Arch_DataAccessLayer.Interphases.Areas.Foundation;
using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_DataAccessLayer.Services.Areas.Foundation;
using API_Arch_DataAccessLayer.Services.Areas.Masters;
using API_Arch_Framework.AutoMappers;
using Architecture.Web.Services;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API_Arch_DataAccessLayer
{
    public class ServiceRegistery
    {
        public static void RegisterAllServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //Add DbContext sercvice
            //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"), b => b.MigrationsAssembly("WebAPIArchitecture")));

            builder.Services.AddLocalization();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IDbLocalizer, DbLocalizer>();

            builder.Services.AddAuthorization();
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseJet(builder.Configuration.GetConnectionString("AccessDb")));

            #region MVC
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<SiteMapService>();

            builder.Services.AddNotyf(config => {
                config.DurationInSeconds = 3;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });
            #endregion

            #region JWT

            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Enable JWT token authentication in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token with **Bearer** prefix. Example: `Bearer abc123xyz`"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                } });
            });

            #endregion

            #region Identity

            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            #endregion

            builder.Services.AddScoped(typeof(IGenericServices<>), typeof(GenericServices<>));

            #region Masters
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUDCServices, UDCServices>();
            builder.Services.AddScoped<IRoleServices, RoleServices>();

            #endregion

            #region Foundation


            #endregion

            builder.Services.AddAutoMapper(typeof(MappingProfile));

        }
    }
}
