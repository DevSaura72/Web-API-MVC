    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Xml.Linq;

    namespace Architecture.Web.Services
    {
        public class SiteMapService
        {
            private readonly IWebHostEnvironment _env;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public SiteMapService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
            {
                _env = env;
                _httpContextAccessor = httpContextAccessor;
            }

            public List<SiteMapNode> LoadSiteMap()
        {
            //string pa = Path.Combine(_env.WebRootPath);
            string filePath = Path.Combine(_env.ContentRootPath, "Views", "Shared", "sitemap.config");
                if (!File.Exists(filePath))
                {
                    return new List<SiteMapNode>();
                }

                var doc = XDocument.Load(filePath);
                var allNodes = doc.Descendants("siteMapNodeParent").Select(ParseNode).ToList();

                return FilterNodesByUserPermissions(allNodes);
            }

            private SiteMapNode ParseNode(XElement element)
            {
                var node = new SiteMapNode
                {
                    SystemName = element.Attribute("SystemName")?.Value,
                    Controller = element.Attribute("Controller")?.Value,
                    Action = element.Attribute("Action")?.Value,
                    Area = element.Attribute("Area")?.Value,
                    IsHeader = bool.TryParse(element.Attribute("IsHeader")?.Value, out bool isHeader) && isHeader,
                    PermissionNames = element.Attribute("PermissionNames")?.Value,
                    IconClass = element.Attribute("IconClass")?.Value
                };

                var childNodes = element.Elements("siteMapNodeChild").Select(ParseNode).ToList();
                if (childNodes.Any())
                {
                    node.ChildNodes.AddRange(childNodes);
                }

                return node;
            }

            private List<SiteMapNode> FilterNodesByUserPermissions(List<SiteMapNode> nodes)
            {
                var userPermissions = GetUserPermissions();
                return nodes
                    .Where(node => string.IsNullOrEmpty(node.PermissionNames) || node.PermissionNames.Split(',').Any(userPermissions.Contains))
                    .Select(node =>
                    {
                        node.ChildNodes = FilterNodesByUserPermissions(node.ChildNodes);
                        node.Visible = node.ChildNodes.Any() || !string.IsNullOrEmpty(node.Controller);
                        return node;
                    })
                    .Where(node => node.Visible)
                    .ToList();
            }

            private HashSet<string> GetUserPermissions()
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    return new HashSet<string>();
                }

                return user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToHashSet();
            }
        }

        public class SiteMapNode
        {
            public string SystemName { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public string Area { get; set; }
            public bool IsHeader { get; set; } = false;
            public bool Visible { get; set; } = true;
            public string PermissionNames { get; set; }
            public string IconClass { get; set; }
            public List<SiteMapNode> ChildNodes { get; set; } = new List<SiteMapNode>();
        }
    }
