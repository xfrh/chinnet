using System.Web.Mvc;

namespace ManageSystem.Web.Areas.CR
{
    public class CRAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Cre_Data",
                "CR",
                 new { area = "CR", controller = "CRData", action = "Index", id = UrlParameter.Optional },
                 new string[] { "ManageSystem.Web.Areas.CR.Controllers" }
            );

            context.MapRoute(
                "CR_default",
                "CR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}