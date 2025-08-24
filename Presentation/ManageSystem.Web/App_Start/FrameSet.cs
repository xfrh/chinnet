using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.App_Start
{

    public class FrameSetAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FrameSet";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FrameSet_default",
                "FrameSet/{controller}/{action}/{id}",
                new { controller = "Home", action = "Main", id = UrlParameter.Optional },
                new string[] { "ManageSystem.Web.App_Start.Controllers" }
            );
        }

    }
}