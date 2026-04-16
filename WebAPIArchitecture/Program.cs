using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_DataAccessLayer;
using API_Arch_DataAccessLayer.GenericRepository;
using API_Arch_DataAccessLayer.Interphases.Areas.Masters;
using API_Arch_DataAccessLayer.Services.Areas.Masters;
using API_Arch_Framework.AutoMappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;


var builder = WebApplication.CreateBuilder(args);

ServiceRegistery.RegisterAllServices(builder);

var app = builder.Build();
app.UseStaticFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("hi"),
    new CultureInfo("mr")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

app.UseStaticFiles();

// ---- MVC Route (for admin web app) ----
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AuthMVC}/{action=Login}/{id?}",
    defaults: new { area = "" }
);


// ---- API Controllers (automatically mapped) ----
app.MapControllerRoute(
    name: "API",
    pattern: "api/{controller}/{action}/{id?}"
);
app.MapControllers();
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "DENY";
    await next();
});
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Startup Error: " + ex);
        throw;
    }
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedSystemAdminAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{   
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Home/Index");
    return Task.CompletedTask;
});

app.Run();
