using ManageSystem.Admin.App_Start;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Installation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class InstallController : Controller
    {
        [CheckRole(false, false)]
        public ActionResult Index()
        {
            this.CreateData();
            return View();
        }


        /// <summary>
        /// 初始化基础数据
        /// </summary>
        private void CreateData()
        {
            var installationService = EngineContext.Current.Resolve<IInstallationService>();
            installationService.InstallData(true);
        }

    }
}