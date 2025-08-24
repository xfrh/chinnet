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
    /// 数据报表
    /// </summary>
    [CheckRole(false)]
    public class DataController : WebBaseController
    {
        [CheckRole(false)]
        public ActionResult Project1()
        {
            return View();
        }
    }
}