using System.Web.Mvc;
using ManageSystem.Core;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Framework.Controllers;
using ManageSystem.Framework.Mvc;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Services.Authentication;
using System.Collections.Generic;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.Configuration;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Caching;
using Newtonsoft.Json;
using System;
using ManageSystem.Core.Domain.Sate;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 后台所有控制器的基类
    /// </summary>
    public abstract partial class WebBaseController : BaseController
    {
        protected readonly IActionLogService ActionLogService;
        protected readonly ICacheManager CacheManager;
        protected readonly ISettingService SettingService;
        protected readonly ISystemLogService SystemLogService;

        public WebBaseController()
        {
            this.ActionLogService = EngineContext.Current.Resolve<IActionLogService>();
            this.SettingService = EngineContext.Current.Resolve<ISettingService>();
            this.CacheManager = EngineContext.Current.Resolve<ICacheManager>();
            this.SystemLogService = EngineContext.Current.Resolve<ISystemLogService>();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        ///  根据配置的key获取相关的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSettingValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return "";

            return this.SettingService.QueryValue<string>(key);
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        protected void InsetActionLog(ActionType type, string content = "", string detail = "")
        {
            try
            {
                var user = this.LoginUserinfo;
                if (user != null)
                {
                    this.ActionLogService.Insert(type, ActionSource.Web, user.Id, user.Name + "（" + user.LoginId + "）", content, detail);
                }
                else
                {
                    var user1=JsonConvert.DeserializeObject<Member>(detail);
                    this.ActionLogService.Insert(type, ActionSource.Web, user1.Id, user1.Name + "（" + user1.LoginId + "）", content, detail);
                }
            }
            catch (System.Exception)
            {

            }
        }

        /// <summary>
        /// 获取当前登录的用户
        /// </summary>
        protected Member LoginUserinfo
        {
            get
            {
                var account = EngineContext.Current.Resolve<IAuthenticationService>().GetAuthenticatedUser();
                if (account == null || account.Id <= 0) return null;

                return account as Member;
            }

        }

        protected SatelliteUser LoginSatelliteUserinfo
        {
            get
            {
                var account = EngineContext.Current.Resolve<IAuthenticationService>().GetAuthenticatedSatelliteUser();
                if (account == null || account.Id <= 0) return null;

                return account as SatelliteUser;
            }

        }

        /// <summary>
        /// 获取当前登录用户的完整姓名和用户名，格式：姓名（登录帐号）
        /// </summary>
        /// <returns></returns>
        protected string GetUserFullName()
        {
            var user = this.LoginUserinfo;

            return user.Name + "（" + user.LoginId + "）";
        }

        /// <summary>
        /// 初始化控制器
        /// </summary>
        /// <param name="requestContext">请求内容</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        /// <summary>
        ///记录异常数据
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
                LogException(filterContext.Exception);
            base.OnException(filterContext);
        }

        /// <summary>
        /// 拒绝访问视图，跳转到指定的错误页面
        /// </summary>
        /// <returns></returns>
        protected ActionResult AccessDeniedView()
        {
            return RedirectToAction("AccessDenied", "Common", new { pageUrl = this.Request.RawUrl });
        }

        /// <summary>
        /// 保存tab选中的标签索引
        /// </summary>
        /// <param name="index">索引保存;空自动检测</param>
        /// <param name="persistForTheNextRequest">一个值指示是否一个消息应该持续下一个请求</param>
        protected void SaveSelectedTabIndex(int? index = null, bool persistForTheNextRequest = true)
        {
            //keep this method synchronized with
            //"GetSelectedTabIndex" method of \Nop.Web.Framework\ViewEngines\Razor\WebViewPage.cs
            if (!index.HasValue)
            {
                int tmp;
                if (int.TryParse(this.Request.Form["selected-tab-index"], out tmp))
                {
                    index = tmp;
                }
            }
            if (index.HasValue)
            {
                string dataKey = "nop.selected-tab-index";
                if (persistForTheNextRequest)
                {
                    TempData[dataKey] = index;
                }
                else
                {
                    ViewData[dataKey] = index;
                }
            }
        }

        /// <summary>
        /// 设置新模型的5个基本字段为原来的数据，只能用户修改操作。
        /// </summary>
        /// <param name="model">新的模型</param>
        /// <param name="entity">原始实体</param>
        protected void SetDefaultValue(BaseEntityModel model, BaseEntity entity)
        {
            if (model == null || entity == null) return;

            model.InsertTime = entity.InsertTime;
            model.Mark = entity.Mark;
            model.Version = entity.Version;
            model.DeleteTime = entity.DeleteTime;
            model.UpdateTime = entity.UpdateTime;

        }

        /// <summary>
        /// 获取 是否启用的下拉列表数据，用于绑定下拉列表框
        /// </summary>
        /// <returns></returns>
        protected List<SelectListItem> GetEnabledSelectList(bool showTitle = true)
        {
            var list = new List<SelectListItem>() {
                   new SelectListItem { Text = "启用", Value = "1" },
                   new SelectListItem { Text = "禁用", Value = "0" },
         };

            if (showTitle)
                list.Insert(0, new SelectListItem { Text = "全部", Value = "" });

            return list;
        }

        /// <summary>
        /// 转发至错误页面
        /// </summary>
        /// <param name="message">错误信息</param>
        public void TransferError(string message)
        {
            this.HttpContext.Server.TransferRequest("/Error/Transfer?value=" + message, true);
        }


    }
}
