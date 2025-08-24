using System.Web.Mvc;

namespace ManageSystem.Mobile.Areas.CRE
{
    public class CREAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CRE";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //context.MapMvcAttributeRoutes();

            context.MapRoute(
                  "Cre_Data",
                  "CRE",
                   new { area = "CRE", controller = "CreData", action = "HeatMap", id = UrlParameter.Optional },
                   new string[] { "ManageSystem.Mobile.Areas.CRE.Controllers" }
              );
            context.MapRoute(
                "CRE_default",
                "CRE/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "ManageSystem.Mobile.Areas.CRE.Controllers" }
          );
           
        }
    }
}