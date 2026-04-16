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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API_Arch_DataAccessLayer
{
    public class ServiceRegistery
    {
        public static void RegisterAllServices(WebApplicationBuilder builder)
        {
            // =========================
            // DATABASE
            // =========================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("ConnectionString"),
                    b => b.MigrationsAssembly("WebAPIArchitecture")));

            // =========================
            // IDENTITY (LOCAL USERS)
            // =========================
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
            });

            // =========================
            // AUTHENTICATION (FIXED ✅)
            // =========================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme; // ✅ use Identity cookie
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAD"));

            builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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

            // =========================
            // AUTHORIZATION
            // =========================
            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // =========================
            // MVC + UI
            // =========================
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages()
                .AddMicrosoftIdentityUI(); // Azure login UI

            builder.Services.AddControllers(); // APIs

            // =========================
            // SWAGGER + JWT
            // =========================
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your_token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });

            // =========================
            // OTHER SERVICES
            // =========================
            builder.Services.AddLocalization();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IDbLocalizer, DbLocalizer>();

            builder.Services.AddScoped<SiteMapService>();

            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 3;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            builder.Services.AddScoped(typeof(IGenericServices<>), typeof(GenericServices<>));

            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUDCServices, UDCServices>();
            builder.Services.AddScoped<IRoleServices, RoleServices>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.Configure<OpenIdConnectOptions>(
            OpenIdConnectDefaults.AuthenticationScheme,
            options =>
            {
                options.Events.OnTokenValidated = async context =>
                {
                    var userManager = context.HttpContext.RequestServices
                        .GetRequiredService<UserManager<AppUser>>();
            
                    var signInManager = context.HttpContext.RequestServices
                        .GetRequiredService<SignInManager<AppUser>>();

                    var email = context.Principal.FindFirst("email")?.Value
                                ?? context.Principal.FindFirst(ClaimTypes.Email)?.Value
                                ?? context.Principal.FindFirst("preferred_username")?.Value
                                ?? context.Principal.FindFirst("upn")?.Value;

                    if (email != null && email.Contains("#EXT#"))
                    {
                        var parts = email.Split('#')[0];
                        email = parts.Replace("_", "@");
                    }

                    if (email == null)
                        return;
            
                    var user = await userManager.FindByEmailAsync(email);
            
                    if (user == null)
                    {
                        user = new AppUser
                        {
                            UserName = email,
                            Email = email,

                            EmailConfirmed = true,
                            PhoneNumber = "0000000000",
                            PhoneNumberConfirmed = true
                        };
            
                        var result = await userManager.CreateAsync(user);
                    }
            
                    await signInManager.SignInAsync(user, isPersistent: false);
                };
            });

            builder.Services.Configure<OpenIdConnectOptions>(
            OpenIdConnectDefaults.AuthenticationScheme,
            options =>
            {
                options.Events.OnTicketReceived = context =>
                {
                    context.ReturnUri = "/Home/Index"; 
                    return Task.CompletedTask;
                };
            });

        }
    }

    //public class ServiceRegistery
    //{
    //    public static void RegisterAllServices(WebApplicationBuilder builder)
    //    {


    //        //FOR Azure Entra ID/ Active directory
    //        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    //            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAD"));

    //        builder.Services.AddControllersWithViews(options =>
    //        {
    //            var policy = new AuthorizationPolicyBuilder().
    //            RequireAuthenticatedUser().
    //            Build();
    //            options.Filters.Add(new AuthorizeFilter(policy));
    //        });

    //        builder.Services.AddRazorPages().AddMicrosoftIdentityUI();



    //        // Add services to the container.
    //        builder.Services.AddControllers();
    //        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //        builder.Services.AddEndpointsApiExplorer();
    //        builder.Services.AddSwaggerGen();


    //        //Add DbContext sercvice
    //        //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
    //        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"), b => b.MigrationsAssembly("WebAPIArchitecture")));

    //        builder.Services.AddLocalization();
    //        builder.Services.AddHttpContextAccessor();
    //        builder.Services.AddScoped<IDbLocalizer, DbLocalizer>();

    //        builder.Services.AddAuthorization();
    //        //builder.Services.AddDbContext<AppDbContext>(options =>
    //        //    options.UseJet(builder.Configuration.GetConnectionString("AccessDb")));

    //        #region MVC
    //        builder.Services.AddControllersWithViews();
    //        builder.Services.AddRazorPages();

    //        builder.Services.AddScoped<SiteMapService>();

    //        builder.Services.AddNotyf(config => {
    //            config.DurationInSeconds = 3;
    //            config.IsDismissable = true;
    //            config.Position = NotyfPosition.BottomRight;
    //        });
    //        #endregion


    //        #region JWT

    //        // Configure JWT authentication
    //        builder.Services.AddAuthentication(options =>
    //        {
    //            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //        })
    //        .AddJwtBearer(options =>
    //        {
    //            options.TokenValidationParameters = new TokenValidationParameters
    //            {
    //                ValidateIssuer = true,
    //                ValidateAudience = true,
    //                ValidateLifetime = true,
    //                ValidateIssuerSigningKey = true,
    //                ValidIssuer = builder.Configuration["Jwt:Issuer"],
    //                ValidAudience = builder.Configuration["Jwt:Audience"],
    //                IssuerSigningKey = new SymmetricSecurityKey(
    //                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    //            };
    //        });


    //        builder.Services.AddSwaggerGen(c =>
    //        {
    //            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    //            // Enable JWT token authentication in Swagger
    //            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //            {
    //                Name = "Authorization",
    //                Type = SecuritySchemeType.Http,
    //                Scheme = "Bearer",
    //                BearerFormat = "JWT",
    //                In = ParameterLocation.Header,
    //                Description = "Enter JWT token with **Bearer** prefix. Example: `Bearer abc123xyz`"
    //            });

    //            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    //            {
    //                new OpenApiSecurityScheme {
    //                    Reference = new OpenApiReference {
    //                        Type = ReferenceType.SecurityScheme,
    //                        Id = "Bearer"
    //                    }
    //                },
    //                new string[] { }
    //            } });
    //        });

    //        #endregion

    //        #region Identity

    //        builder.Services.AddIdentity<AppUser, AppRole>(options =>
    //        {
    //            options.Password.RequireDigit = true;
    //            options.Password.RequiredLength = 6;
    //        })
    //        .AddEntityFrameworkStores<AppDbContext>()
    //        .AddDefaultTokenProviders();

    //        builder.Services.Configure<IdentityOptions>(options =>
    //        {
    //            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //            options.Lockout.MaxFailedAccessAttempts = 5;
    //            options.Lockout.AllowedForNewUsers = true;
    //        });

    //        #endregion

    //        builder.Services.AddScoped(typeof(IGenericServices<>), typeof(GenericServices<>));

    //        #region Masters
    //        builder.Services.AddScoped<IUserServices, UserServices>();
    //        builder.Services.AddScoped<IUDCServices, UDCServices>();
    //        builder.Services.AddScoped<IRoleServices, RoleServices>();

    //        #endregion

    //        #region Foundation


    //        #endregion

    //        builder.Services.AddAutoMapper(typeof(MappingProfile));

    //    }
    //}
}
