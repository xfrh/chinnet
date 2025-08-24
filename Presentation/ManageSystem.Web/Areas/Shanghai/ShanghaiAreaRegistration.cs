using System.Web.Mvc;

namespace ManageSystem.Web.Areas.Shanghai
{
    public class ShanghaiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Shanghai";
            }
        }

        public override void RegisterArea(AreaRegistrationContext routes)
        {
            //http://localhost:56249/Shanghai
            routes.MapRoute(
               "Shanghai_home",
               "Shanghai",
                new { area = "Shanghai", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "ManageSystem.Web.Areas.Shanghai.Controllers" }
           );

            //http://localhost:56249/Shanghai/
            routes.MapRoute(
              "Shanghai_Home_Index",
              "Shanghai/Index",
               new { area = "Shanghai", controller = "Home", action = "Index", id = UrlParameter.Optional },
               new string[] { "ManageSystem.Web.Areas.Shanghai.Controllers" }
          );

            //http://localhost:56249/Shanghai/About
            routes.MapRoute(
              "Shanghai_about",
              "Shanghai/About",
               new { area = "Shanghai", controller = "Home", action = "About" },
               new string[] { "ManageSystem.Web.Areas.Shanghai.Controllers" }
          );

            //http://localhost:56249/Shanghai/Home/Index
            routes.MapRoute(
              "Shanghai_default",
              "Shanghai/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional },
               new string[] { "ManageSystem.Web.Areas.Shanghai.Controllers" }
          );

            
        }
    }
}