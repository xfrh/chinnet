using ManageSystem.Core.Domain.Cre;
using ManageSystem.Services.CRE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Areas.CRE.Controllers
{
    public class MobileBaseController : Controller
    {      
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }      
    }
}