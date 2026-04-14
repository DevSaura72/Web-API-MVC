using Architecture.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Architecture.Web.Views.Shared.Components.SidebarMenu
{
    public class SidebarMenuViewComponent : ViewComponent
    {

        private readonly SiteMapService _siteMapService;

        public SidebarMenuViewComponent(SiteMapService siteMapService)
        {
            _siteMapService = siteMapService;
        }

        public IViewComponentResult Invoke()
        {
            var siteMap = _siteMapService.LoadSiteMap();
            return View(siteMap);
        }
    }
}
