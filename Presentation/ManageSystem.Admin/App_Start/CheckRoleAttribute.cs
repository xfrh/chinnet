using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.App_Start
{
    /// <summary>
    /// 对访问的action进行登录和权限检查权限筛选器
    /// </summary>
    public class CheckRoleAttribute : System.Web.Mvc.ActionFilterAttribute
    {

        /// <summary>
        /// 是否启用登录检查，默认启用，false表示不检查是否登录
        /// </summary>
        public bool CheckLogin = true;

        /// <summary>
        /// 是否启用角色检查，默认启用，false表示检查是否有权限访问该action。启用了角色检查就必须启用检查登录
        /// </summary>
        public bool CheckRole = true;

        public CheckRoleAttribute(bool checkLogin = true, bool checkRole = true)
        {
            this.CheckLogin = checkLogin;
            this.CheckRole = checkRole;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //检查是否有登录
            string error = "";
            //回调地址
            string returnUrl = "";

            try
            {
                //是否检查登录，如果不检查登录就自由访问，不做任何限制
                if (!this.CheckLogin) return;

                //当前登录用户
                IAuthenticationService authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
                var userEntity = authenticationService.GetAuthenticatedUser();
                UserinfoModel userModel = (userEntity as Userinfo).ToModel();


                var httpRequestBase = EngineContext.Current.Resolve<HttpRequestBase>();

                //检查登录
                if (userModel == null)
                {
                    returnUrl = httpRequestBase.Url.AbsolutePath;
                    error = "请登录！";
                    throw new Exception();
                }

                //不检查功能的权限问题
                if (!this.CheckRole) return;

                //检查登录
                if (userModel.FunctionList == null || userModel.FunctionList.Count() <= 0)
                {
                    error = "用户无任何可用功能！";
                    throw new Exception();
                }

                //检查页面是否有权限访问
                var functionService = EngineContext.Current.Resolve<IFunctionService>();

                if (httpRequestBase.Url.AbsolutePath.Equals("/")) return;

                //当前访问的功能
                string url = this.GetLocationUrl(filterContext);
                if (string.IsNullOrWhiteSpace(url)) return;
                url = url.ToLower();
                FunctionModel functionModel = functionService.QueryEntity(m => m.Mark > 0 && url.Contains(m.Url.ToLower())).ToModel();
                var functionModelList = functionService.Query(m => m.Mark > 0 && url.Contains(m.Url.ToLower())).ToList();

                if (functionModel == null || string.IsNullOrEmpty(functionModel.Name))
                {
                    error = "访问错误，请检查重试！";
                    throw new Exception();
                }

                //检查用户是否可以访问该功能
                var userFunctionList = userModel.FunctionList.Where(m => functionModelList.Select(x=>x.Id).ToList().Contains(m.Id));
                if (userFunctionList == null || !userFunctionList.Any())
                {
                    //页面无权访问
                    error = "权限不足，请登录！";
                    throw new Exception();
                }

                base.OnActionExecuting(filterContext);

                return;
            }
            catch (Exception ex)
            {
                string returnURL = HttpContext.Current.Request.Url.ToString();
                string alert = string.IsNullOrEmpty(error) ? "系统错误，请联系管理员！" : error;

                ContentResult temp = new ContentResult();
                temp.Content = "<script>alert('" + alert + "'); location.href='/Home/Login?returnUrl=" + returnUrl + "';</script>";
                filterContext.Result = temp;
            }
        }


        /// <summary>
        /// 获取当前访问的url地址，解析 路由获得
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetLocationUrl(ActionExecutingContext filterContext)
        {
            string area = filterContext.RouteData.Values["area"] + "";
            string controller = filterContext.RouteData.Values["controller"] + "";
            string action = filterContext.RouteData.Values["action"] + "";

            string url = "";
            if (!string.IsNullOrWhiteSpace(area)) url += "/" + area;
            if (!string.IsNullOrWhiteSpace(controller)) url += "/" + controller;
            if (!string.IsNullOrWhiteSpace(action)) url += "/" + action;

            return url;
        }
    }
}