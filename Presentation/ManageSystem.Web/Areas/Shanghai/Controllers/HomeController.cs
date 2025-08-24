using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.Shanghai.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [CheckRole(false)]
    public class HomeController : WebBaseController
    {
        // GET: Shanghai/Home
        [CheckRole(false)]
        [Route("Shanghai/Home/Index")]
        public ActionResult Index()
        {
            return this.Redirect("/Shanghai/Data/Year");
        }

        [CheckRole(false)]
        public ActionResult About()
        {
            return View();
        }

       
    }

}