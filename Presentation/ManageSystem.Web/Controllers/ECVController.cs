using ManageSystem.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class ECVController : Controller
    {
        // GET: Epidemiology
        [CheckRole(false)]
        public ActionResult Index()
        {
            return View();
        }
    }
}