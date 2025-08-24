using ManageSystem.Core.Caching;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Configuration;
using ManageSystem.Services.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    public class MobileController : MobileBaseController
    {
        protected readonly IActionLogService ActionLogService;
        protected readonly ICacheManager CacheManager;
        protected readonly ISettingService SettingService;
        protected readonly ISystemLogService SystemLogService;

        public MobileController()
        {
            //this.ActionLogService = EngineContext.Current.Resolve<IActionLogService>();
            //this.SettingService = EngineContext.Current.Resolve<ISettingService>();
            //this.CacheManager = EngineContext.Current.Resolve<ICacheManager>();
            //this.SystemLogService = EngineContext.Current.Resolve<ISystemLogService>();
        }

    }
}