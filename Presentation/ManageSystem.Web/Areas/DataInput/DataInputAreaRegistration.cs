using System.Web.Mvc;

namespace ManageSystem.Web.Areas.DataInput
{
    public class DataInputAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DataInput";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DataInput_default",
                "DataInput/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
               "OLDataInput",
               "DataInput",
                new { area = "DataInput", controller = "OLDataInput", action = "OLproject", id = UrlParameter.Optional },
                new string[] { "ManageSystem.Web.Areas.DataInput.Controllers" }
           );
        }
    }
}