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
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Common;
using System.IO;

using log4net;
using log4net.Config;
using ManageSystem.Services.Configuration;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Controllers;
using System.Text.RegularExpressions;

namespace ManageSystem.Web
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
            AutoMapperRegistrar.Register();  //admin

            //系统相关
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); // 注册筛选器

            //添加一些功能默认ModelMetadataProvider 之上
            ModelMetadataProviders.Current = new MetadataProvider();

            // fluent validation 设置
            FluentValidationModelValidatorProvider provider = new FluentValidationModelValidatorProvider(new ValidatorFactory());
            ModelValidatorProviders.Providers.Add(provider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            //Log4net  注册config文件， 注意配置文件在网站根目录下面
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            //加载系统配置缓存
            this.LoadSetting();


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
            systemLog.Insert(exception, SystemLogLevel.Error, this.Request.Url.ToString());


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
        /// 加载系统配置缓存
        /// </summary>
        private void LoadSetting()
        {
            EngineContext.Current.Resolve<ISettingService>().SetAllSettingsCached();
        }

        protected void ReRewritePath(HttpContext context, string sendToUrl)
        {
            if (context.Request.QueryString.Count > 0)
            {
                if (sendToUrl.IndexOf('?') != -1)
                {
                    sendToUrl += "&" + context.Request.QueryString.ToString();
                }
                else
                {
                    sendToUrl += "?" + context.Request.QueryString.ToString();
                }
            }
            string queryString = String.Empty;
            string sendToUrlLessQString = sendToUrl;
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
            }
            context.RewritePath(sendToUrlLessQString, String.Empty, queryString);

        }



    }
}
