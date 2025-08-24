using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Core.Infrastructure.DependencyManagement;
using ManageSystem.Core;
using System.Web.Optimization;
using System;
using System.Web;
using ManageSystem.Services.Log;

using ManageSystem.Core.Domain.Log;
using ManageSystem.Admin.Controllers;
using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.App_Start;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Common;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.Threads;
using System.IO;

using log4net;
using log4net.Config;
using ManageSystem.Services.Configuration;
using ManageSystem.Services.Tasks;
using Senparc.Weixin.MP.Containers;

namespace ManageSystem.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //禁用“X-AspNetMvc-Version”标题名称
            MvcHandler.DisableMvcResponseHeader = true;

            //initialize engine context
            EngineContext.Initialize(false);

            //注册 AutoMapper 映射
            AutoMapperAdminRegistrar.Register();  //admin

            //系统相关
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); // 注册筛选器

            //添加一些功能默认ModelMetadataProvider 之上
            ModelMetadataProviders.Current = new MetadataProvider();

            // fluent validation 设置
            FluentValidationModelValidatorProvider provider = new FluentValidationModelValidatorProvider(new ValidatorFactory());
            ModelValidatorProviders.Providers.Add(provider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            RegisterWeixinThreads();//激活微信缓存（必须）
            RegisterSenparcWeixin();//注册Demo所用微信公众号的账号信息
            
            //Log4net  注册config文件， 注意配置文件在网站根目录下面
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            //加载系统配置缓存
            this.LoadSetting();

            //计划任务
            TaskManager.Instance.Initialize();
            TaskManager.Instance.Start();
        }

        /// <summary>
        ///应用程序异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            var systemLog = EngineContext.Current.Resolve<ISystemLogService>();
            systemLog.Insert(exception, SystemLogLevel.Error,this.Request.Url.ToString());
            

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                //跳转到404错误页面
                IController errorController = EngineContext.Current.Resolve<CommonController>();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Common");
                routeData.Values.Add("action", "PageNotFound");

                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 激活微信缓存
        /// </summary>
        private void RegisterWeixinThreads()
        {
            ThreadUtility.Register();
        }

        /// <summary>
        /// 注册Demo所用微信公众号的账号信息
        /// </summary>
        private void RegisterSenparcWeixin()
        {
            ISettingService    settingService = EngineContext.Current.Resolve<ISettingService>();
            AccessTokenContainer.Register(WeixinSettingService.GetAppId(), WeixinSettingService.GetAppSecret());
        }

        /// <summary>
        /// 加载系统配置缓存
        /// </summary>
        private void LoadSetting()
        {
            EngineContext.Current.Resolve<ISettingService>().SetAllSettingsCached();
        }


    }
}
