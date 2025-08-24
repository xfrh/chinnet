using log4net.Config;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;

namespace ManageSystem.Mobile
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //禁用“X-AspNetMvc-Version”标题名称
            MvcHandler.DisableMvcResponseHeader = true;

            //initialize engine context
            EngineContext.Initialize(false);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            EngineContext.Current.Resolve<ISettingService>().SetAllSettingsCached();

            //Log4net  注册config文件， 注意配置文件在网站根目录下面
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            //注册微信功能，功能暂时不使用，所以暂时注释
          //  this.RegisterWeixin();
        }

        /// <summary>
        /// 注册微信
        /// </summary>
        private void RegisterWeixin()
        {
            //设置全局 Debug 状态
            bool isGLobalDebug = true;
            SenparcSetting senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);
            IRegisterService register = RegisterService.Start(senparcSetting).UseSenparcGlobal(); // CO2NET全局注册，必须！

            //设置微信 Debug 状态
            bool isWeixinDebug = true;
            SenparcWeixinSetting senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting); // 微信全局注册，必须！

            // 微信sdk注册 -- 正式
            string appId = "";
            string appSecret = "";
            AccessTokenContainer.Register(appId, appSecret);
        }
    }
}
