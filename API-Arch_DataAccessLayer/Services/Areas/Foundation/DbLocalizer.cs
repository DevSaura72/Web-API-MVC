using API_Arch_DataAccessLayer.Interphases.Areas.Foundation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Arch_DataAccessLayer.Services.Areas.Foundation
{
    public class DbLocalizer : IDbLocalizer
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbLocalizer(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public string Get(string key)
        {
            var culture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;

            var value = _appDbContext.Translations.Where(x => x.TranslationKey == key && x.CultureCode == culture).Select(x => x.TranslationValue).FirstOrDefault();

            return value ?? key;
        }
    }
}
