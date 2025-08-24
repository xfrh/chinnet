using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Log;
using ManageSystem.Services.Medicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.InnerApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMedicalDataService MedicalDataService;
        private readonly IMedicalAntibioticResultService MedicalAntibioticResultService;
        private readonly IActionLogService ActionLogService;

        public HomeController()
        {
            this.MedicalDataService = EngineContext.Current.Resolve<IMedicalDataService>();
            this.ActionLogService = EngineContext.Current.Resolve<IActionLogService>();
            this.MedicalAntibioticResultService = EngineContext.Current.Resolve<IMedicalAntibioticResultService>();
        }

        public ActionResult Index(long id=0)
        {   
            ViewBag.Title = "HomePage";
            return View();
        }
    }
}
