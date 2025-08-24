using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ManageSystem.Core;
using ManageSystem.Framework.UI;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;

namespace ManageSystem.Framework.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 将部分视图转换成字符串
        /// </summary>
        /// <returns></returns>
        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        /// <summary>
        ///将部分视图转换成字符串
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        /// <summary>
        /// 将部分视图转换成字符串
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        /// <summary>
        /// 将部分视图转换成字符串
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="model">对象</param>
        /// <returns></returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="exc">异常</param>
        protected void LogException(Exception exc)
        {
            var logger = EngineContext.Current.Resolve<ISystemLogService>();
            logger.Insert(exc,SystemLogLevel.Error);
        }

        /// <summary>
        /// 显示操作成功通知
        /// </summary>
        /// <param name="message">内容</param>
        /// <param name="persistForTheNextRequest">一个值指示是否一个消息应该持续下一个请求</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// 显示操作失败通知
        /// </summary>
        /// <param name="message">内容</param>
        /// <param name="persistForTheNextRequest">一个值指示是否一个消息应该持续下一个请求</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// 显示操作失败通知
        /// </summary>
        /// <param name="exception">内容</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">一个值指示是否一个消息应该持续下一个请求</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        /// <param name="type">通知类型</param>
        /// <param name="message">内容</param>
        /// <param name="persistForTheNextRequest">一个值指示是否一个消息应该持续下一个请求</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("nop.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }


     
    }
}
