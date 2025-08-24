using System.Web.Mvc;

namespace ManageSystem.Web.Areas.Special
{
    public class SpecialAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Special";
            }
        }

        public override void RegisterArea(AreaRegistrationContext routes) 
        {

            //http://localhost:56249/Special
            routes.MapRoute(
               "Special_home",
               "Special",
                new { area = "Special", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "ManageSystem.Web.Areas.Special.Controllers" }
           );

            //http://localhost:56249/Special/
            routes.MapRoute(
              "Special_Home_Index",
              "Special/Index",
               new { area = "Special", controller = "Home", action = "Index", id = UrlParameter.Optional },
               new string[] { "ManageSystem.Web.Areas.Special.Controllers" }
          );

            //http://localhost:56249/Special/Home/Index
            routes.MapRoute(
              "Special_default",
              "Special/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional },
               new string[] { "ManageSystem.Web.Areas.Special.Controllers" }
          );

        }
    }
}