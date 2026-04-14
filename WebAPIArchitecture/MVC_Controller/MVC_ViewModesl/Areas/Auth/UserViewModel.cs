using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace WebAPIArchitecture.MVC_Controller.MVC_ViewModesl.Areas.Auth
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Roles { get; set; }

        public int UserRole { get; set; }
    }
}
