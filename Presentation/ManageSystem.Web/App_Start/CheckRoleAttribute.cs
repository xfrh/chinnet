using ManageSystem.Web.Extensions;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Authentication;
using ManageSystem.Web.Models.Members;
using System;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Core.Domain.Members;
using System.Net.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Policy;

namespace ManageSystem.Web.App_Start
{
    /// <summary>
    /// 对访问的action进行登录和权限检查权限筛选器
    /// </summary>
    public class CheckRoleAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 是否启用登录检查，默认启用，false表示不检查是否登录
        /// </summary>
        public bool CheckLogin = true;

        public CheckRoleAttribute(bool checkLogin = true)
        {
            this.CheckLogin = checkLogin;
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
                MemberModel userModel = (userEntity as Member).ToModel();
                
                var httpRequestBase = EngineContext.Current.Resolve<HttpRequestBase>();

                //检查登录
                if (userModel == null)
                {
                    returnUrl = httpRequestBase.Url.AbsolutePath;
                    throw new Exception();
                }

                base.OnActionExecuting(filterContext);

                return;
            }
            catch (Exception ex)
            {
                //returnUrl = HttpContext.Current.Request.Url.ToString();

                returnUrl = HttpContext.Current.Request.Url.AbsolutePath;
                if (HttpContext.Current.Request.Url.ToString().Contains("?")) {
                    returnUrl += $"?{HttpContext.Current.Request.Url.ToString().Split('?')[0]}";
                }
                string alert = string.IsNullOrEmpty(error) ? "请注册！" : error;

                ContentResult temp = new ContentResult();
                //  temp.Content = "<script>alert('" + alert + "'); location.href='/Home/Register?returnUrl=" + returnUrl + "';</script>";
                //temp.Content = "<script>location.href='/?type=login&returnUrl=" + returnUrl + "';</script>";
                temp.Content = "<script>location.href='/Login/OpenIndex?returnUrl=" + returnUrl + "';</script>";
                filterContext.Result = temp;
            }
        }

    }
}