using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Models.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }


        [CheckRole(false, false)]
        public ActionResult Image()
        {
            ProductPictureModel model = new ProductPictureModel();
            return View(model);
        }
    }
}