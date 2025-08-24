using ManageSystem.Admin.Models.SystemInfo;
using ManageSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class SystemInfoController : Controller
    {
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;

        public SystemInfoController(
              HttpContextBase httpContext,
              IWebHelper webHelper)
        {
            this._httpContext = httpContext;
            this._webHelper = webHelper;
        }


        public ActionResult SystemInfo()
        {
            var model = new SystemInfoModel();
            model.NopVersion = ManageSystemVersion.CurrentVersion;
            try
            {
                model.OperatingSystem = Environment.OSVersion.VersionString;

            }
            catch (Exception)
            {
            }

            try
            {
                model.AspNetInfo = RuntimeEnvironment.GetSystemVersion();
            }
            catch (Exception)
            {

            }
            try
            {
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted.ToString();
            }
            catch (Exception)
            {

            }

            model.ServerTimeZone = TimeZone.CurrentTimeZone.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.UtcTime = DateTime.UtcNow;
            model.HttpHost = _webHelper.ServerVariables("HTTP_HOST");

            foreach (var key in _httpContext.Request.ServerVariables.AllKeys)
            {
                model.ServerVariables.Add(new SystemInfoModel.ServerVariableModel
                {
                    Name = key,
                    Value = _httpContext.Request.ServerVariables[key]
                });
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                model.LoadedAssemblies.Add(new SystemInfoModel.LoadedAssembly
                {
                    FullName = assembly.FullName,
                });
            }

            return View(model);
        }
    }
}