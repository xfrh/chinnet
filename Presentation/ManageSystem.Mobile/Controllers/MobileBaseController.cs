using ManageSystem.Services.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    /// <summary>
    /// 移动端控制基类
    /// </summary>
    public class MobileBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}