using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Configuration;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.App_Start
{
    /// <summary>
    /// 对访问的action进行IP访问检测
    /// </summary>
    public class CheckIPAttribute : System.Web.Mvc.ActionFilterAttribute
    {

        /// <summary>
        /// 是否启用登录检查IP
        /// </summary>
        public bool CheckIP = true;

        public CheckIPAttribute(bool checkIP = true)
        {
            this.CheckIP = checkIP;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            try
            {
                //if (!this.CheckIP) return;

                //string value = WebSettingService.GetWebAdminAllowIp();
                //if (string.IsNullOrWhiteSpace(value)) return;  //不需要验证IP

                //string[] array = value.Split(',');
                //string ip = HttpHelper.GetIp();

                //if (ip.Equals("127.0.0.1") || ip.Equals("localhost")) return;

                //foreach (var item in array)
                //    if (item.Equals(ip)) return;

                //throw new Exception("您的IP：" + ip + "，被拒绝访问");
                return;
            }
            catch (Exception ex)
            {
                ContentResult temp = new ContentResult();
                temp.Content = "<script>alert('" + ex.Message + "'); location.href='/Common/Error?message=" + ex.Message + "';</script>";
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