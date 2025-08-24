using System.Web.Mvc;

namespace ManageSystem.Admin.Extensions
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            context.MapRoute(
                  "Admin_default",
                  "Admin/{controller}/{action}/{id}",
                  new { controller = "Home", action = "Index", area = "Admin", id = "" },
                  new[] { "ManageSystem.Web.Areas.Admin.Controllers" }
              );
        }
    }
}