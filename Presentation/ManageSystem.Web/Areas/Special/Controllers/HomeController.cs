using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.Special.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [CheckRole(false)]
    public class HomeController : WebBaseController
    {
        [CheckRole(false)]
        public ActionResult Index()
        {
            return this.Redirect("/Special/Data/Project1");
        }
    }
}