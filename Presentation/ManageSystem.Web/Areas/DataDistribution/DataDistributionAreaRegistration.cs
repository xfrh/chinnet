using System.Web.Mvc;

namespace ManageSystem.Web.Areas.DataDistribution
{
    public class DataDistributionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DataDistribution";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "Data_Distribution",
               "DataDistribution",
                new { area = "DataDistribution", controller = "MicDistribution", action = "Index", id = UrlParameter.Optional },
                new string[] { "ManageSystem.Web.Areas.DataDistribution.Controllers" }
           );
            context.MapRoute(
                "DataDistribution_default",
                "DataDistribution/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}