using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Admin.Extensions;
using ManageSystem.Services.Users;
using ManageSystem.Services.Authentication;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Framework.API.Ali;
using ManageSystem.Admin.Models.Log;
using ManageSystem.Framework.API.Baidu;
using ManageSystem.Admin.App_Start;
using ManageSystem.Core.Domain.Seo;
using ManageSystem.Services.SystemSet;
using Newtonsoft.Json;
using System.Web.Security;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Configuration;
using ManageSystem.Framework.MvcCaptcha;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.HttpContext;
using ManageSystem.Core.Utility.FastDBF;
using System.IO;
using System.Text;
using ManageSystem.Services.Security;
using NPOI.POIFS.FileSystem;
using Microsoft.Ajax.Utilities;

namespace ManageSystem.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IUserinfoService userinfoService;
        private readonly IAuthenticationService authenticationService;
        private readonly IUserRegistrationService userRegistrationService;
        private readonly IActionLogService actionLogService;
        private readonly IFunctionService functionService;
        private readonly ISettingService settingService;

        private readonly IEncryptionService encryptionService;

        public HomeController(IUserinfoService _userinfoService,
            IAuthenticationService _authenticationService,
            IUserRegistrationService _userRegistrationService,
            IActionLogService _actionLogService,
            IFunctionService _functionService,
            ISettingService _settingService,
            ISessionManager _sessionManager)
        {
            this.userinfoService = _userinfoService;
            this.authenticationService = _authenticationService;
            this.userRegistrationService = _userRegistrationService;
            this.actionLogService = _actionLogService;
            this.functionService = _functionService;
            this.settingService = _settingService;
        }

        private readonly IEncryptionService EncryptionService;


        #region  用户登录 和退出

        [CheckRole(false, false)]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        [CheckRole(false, false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateMvcCaptcha("Captcha")]
        public ActionResult Login(LoginModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                model.LoginId = model.LoginId.Trim();
                var loginResult = this.userRegistrationService.ValidateUser(model.LoginId, model.Password);

                switch (loginResult)
                {
                    case UserLoginResults.UserNotExist:
                        ModelState.AddModelError("", "没有找到的客户帐户");
                        break;
                    case UserLoginResults.Deleted:
                        ModelState.AddModelError("", "删除客户");
                        break;
                    case UserLoginResults.NotActive:
                        ModelState.AddModelError("", "客户状态未启用");
                        break;
                    case UserLoginResults.NotRegistered:
                        ModelState.AddModelError("", "没有注册帐户");
                        break;

                    case UserLoginResults.Successful:
                        var customer = this.userinfoService.QueryModelByLoginId(model.LoginId);

                        if (customer == null || string.IsNullOrEmpty(customer.LoginId))
                        {
                            ModelState.AddModelError("", "用户名或者密码错误");
                            return View();
                        }
                        returnUrl = customer.Url;
                        this.authenticationService.SignIn(customer, true);

                        //清理菜单缓存
                        SessionLibrariy.MenuHtml = "";

                        //系统日志
                        this.SetLoginLog();

                        if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                            returnUrl = "/MgtChart/BarChartList";
                            //return RedirectToAction("Index");

                        return Redirect(returnUrl);

                    case UserLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", "用户名或密码错误，请重试");
                        break;

                }
            }

            return View(new LoginModel());
        }

        /// <summary>
        /// 设置登录日志
        /// </summary>
        public void SetLoginLog()
        {
            string logContent = "";

            try
            {
                //登录地址
                string ip = this.HttpContext.Request.UserHostAddress;
                IpAddressModel addressDetail = AliIpAddressApi.GetIpAddress(ip);
                logContent = "登录地址：" + addressDetail.data.region + "   " + addressDetail.data.city + "   " + addressDetail.data.county + "，运营商：" + addressDetail.data.isp;
            }
            catch (Exception)
            {

            }

            base.InsetActionLog(ActionType.Login, logContent);
        }

        /// <summary>
        ///  退出登录
        /// </summary>
        /// <returns></returns>
        [CheckRole(false, false)]
        public ActionResult Logout()
        {
            this.authenticationService.SignOut();
            return RedirectToAction("Login");
        }

        #endregion

        #region 后台首页

        [CheckRole(true, false)]
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// 用户登录相关信息，用于显示在首页
        /// </summary>
        /// <returns></returns>
        [CheckRole(true, false)]
        [ChildActionOnly]
        public PartialViewResult _LoginActionLog()
        {
            //最后2个的登录日志
            var list = this.actionLogService.QueryActionByType(ActionType.Login, 2).Select(m => m.ToModel()).ToList();

            return PartialView(list);
        }


        /// <summary>
        /// 天气预报相关信息，用于显示在首页
        /// </summary>
        /// <returns></returns>
        [CheckRole(true, false)]
        [ChildActionOnly]
        public PartialViewResult _WeatherMessage()
        {
            return PartialView();
        }

        /// <summary>
        /// 获取天气数据
        /// </summary>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ContentResult GetWeatherData()
        {
            List<Weather_Data> list = new List<Weather_Data>();

            try
            {
                string ip = this.HttpContext.Request.UserHostAddress.ToString();
                if (ip.Equals("127.0.0.1")) ip = "112.74.108.100";

                IpAddressModel ipAddress = AliIpAddressApi.GetIpAddress(ip);

                list = new BaiduWeather(base.GetSettingValue("baidu.app.key")).GetWeatherData(ipAddress.data.city);
            }
            catch (Exception ex)
            {

            }

            return this.Content(JsonConvert.SerializeObject(list));
        }
        #endregion

        #region 获取登录信息

        [HttpGet]
        [CheckRole(false)]
        public ActionResult GetLoginMember()
        {
            try
            {
                var _loginUserInfo = base.LoginUserinfo;
                if (_loginUserInfo == null || _loginUserInfo.Id <= 0)
                {
                    return Json(new Userinfo() { Id = 0 }, JsonRequestBehavior.AllowGet);
                }

                //返回实体
                return Json(new Userinfo()
                {
                    Id = _loginUserInfo.Id,
                    LoginId = _loginUserInfo.LoginId,
                    NickName = _loginUserInfo.NickName,
                    Email = _loginUserInfo.Email,
                    Phone = _loginUserInfo.Phone,
                    State = _loginUserInfo.State,
                    UserinfoState = _loginUserInfo.UserinfoState,
                    Version = _loginUserInfo.Version,
                    Mark = _loginUserInfo.Mark,
                    Birthday = _loginUserInfo.Birthday,
                    Sex = _loginUserInfo.Sex,
                    Url = _loginUserInfo.Url,
                    LastLoginDate = _loginUserInfo.LastLoginDate,
                    Name = _loginUserInfo.Name,
                    InsertTime = _loginUserInfo.InsertTime,
                    UpdateTime = _loginUserInfo.UpdateTime,
                    DeleteTime = _loginUserInfo.DeleteTime,
                    Describe = _loginUserInfo.Describe,
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Userinfo() { Id = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}