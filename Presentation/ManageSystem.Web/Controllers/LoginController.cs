using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.MvcCaptcha;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class LoginController : WebBaseController
    {
        private readonly IMemberService MemberService;
        private readonly IValidateCodeService ValidateCodeService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IArticleService ArticleService;
        private readonly IEncryptionService EncryptionService;
        private readonly ISatelliteUserService satelliteUserService;
        private readonly IProjectHospitalService ProjectHospitalService;

        public LoginController(
                 IMemberService _memberService,
                 IValidateCodeService _validateCodeService,
                 IAuthenticationService _authenticationService,
                 ISatelliteUserService _satelliteUserService,
                IArticleService _articleService,
               IEncryptionService _encryptionService, IProjectHospitalService _projectHospitalService
            )
        {
            this.MemberService = _memberService;
            this.ValidateCodeService = _validateCodeService;
            this.AuthenticationService = _authenticationService;
            this.ArticleService = _articleService;
            this.satelliteUserService = _satelliteUserService;
            this.EncryptionService = _encryptionService;
            this.ProjectHospitalService = _projectHospitalService;
        }

        #region 登录页面

        /// <summary>
        /// 登录页面的登录
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Index()
        {
            LoginModel model = new LoginModel();
            model.ValidateCode = "";

            return View(model);
        }

        /// <summary>
        /// 弹窗登录
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Open()
        {
            LoginModel model = new LoginModel();
            model.ValidateCode = "";

            return View(model);
        }

        /// <summary>
        /// 手机号码和验证码登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="validateCode"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult PhoneLogin(string phone, string validateCode, string returnUrl = "")
        {
            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符
            if (string.IsNullOrEmpty(phone) || !(new Regex(@"^[1-9]{1}[0-9]{10}$").IsMatch(phone)))
                return this.Content(JsonHelper.GetBaseMessage(false, "手机号码不正确"));

            if (string.IsNullOrEmpty(validateCode) || !(new Regex(@"^[0-9]{6}$").IsMatch(validateCode)))
                return this.Content(JsonHelper.GetBaseMessage(false, "验证码格式不正确"));

            //检查帐号
            var memberEntity = this.MemberService.QueryModelByPhone(phone);
            if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                return this.Content(JsonHelper.GetBaseMessage(false, "用户不存在"));

            //检查验证码
            DateTime time = DateTime.Now;
            int type = (int)ValidateCodeType.Phone;
            var validateCodeEntity = this.ValidateCodeService.Query(m => m.Value.Equals(memberEntity.Phone) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).FirstOrDefault();
            if (validateCodeEntity == null || validateCodeEntity.Id <= 0 || !validateCodeEntity.Code.Equals(validateCode))
                return this.Content(JsonHelper.GetBaseMessage(false, "验证码不正确"));

            //设置登录
            this.AuthenticationService.SignIn(memberEntity, true);
            if (!memberEntity.IsSettingPassword)
            {
                if (String.IsNullOrEmpty(returnUrl) || returnUrl.ToString().ToLower().Contains("localhost") || returnUrl.ToString().ToLower().Contains("login"))
                {
                    returnUrl = $"/Chinet/Cipher?token={EncryptionService.EncryptText(memberEntity.Id.ToString())}";
                }
                else
                {
                    returnUrl = $"/Chinet/Cipher?token={EncryptionService.EncryptText(memberEntity.Id.ToString())}&returnUrl={returnUrl}";
                }
            }
            else if (String.IsNullOrEmpty(returnUrl) || returnUrl.ToString().ToLower().Contains("localhost") || returnUrl.ToString().ToLower().Contains("login"))
                returnUrl = "/";

            //if(memberEntity.LoginId.IndexOf("CRFW") == 0)
            //    returnUrl = "/CRProject/Index"; //CR项目的用户登录成功则自动跳转到 CR项目页面

            return this.Content(JsonHelper.GetBaseMessage(true, returnUrl));
        }

        /// <summary>
        /// 帐号密码登录
        /// </summary>
        /// <param name="account">用户名/手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="validationCode"></param>
        /// <param name="MvcCaptchaGuid"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult SatelliteUserLogin(string account, string password, string validationCode, string mvcCaptchaGuid, string returnUrl = "")
        {
            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符
            if (string.IsNullOrEmpty(account) || account.Length < 3)
                return this.Content(JsonHelper.GetBaseMessage(false, "用户名/手机号码格式不正确"));

            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return this.Content(JsonHelper.GetBaseMessage(false, "密码长度必须大于等于6位"));

            if (string.IsNullOrWhiteSpace(validationCode) || validationCode.Length != 4)
                return this.Content(JsonHelper.GetBaseMessage(false, "输入验证码不正确"));

            // get values 
            var image = MvcCaptchaImage.GetCachedCaptcha(mvcCaptchaGuid);
            string expectedValue = image == null ? String.Empty : image.Text;

            // removes the captch from Session so it cannot be used again 
            Session.Remove(mvcCaptchaGuid);

            bool isValid = !String.IsNullOrEmpty(validationCode)
                           && !String.IsNullOrEmpty(expectedValue)
                           && String.Equals(validationCode, expectedValue, StringComparison.OrdinalIgnoreCase);
            if (!isValid)
                return this.Content(JsonHelper.GetBaseMessage(false, "输入验证码不正确"));

            //检查帐号
            var memberEntity = this.satelliteUserService.QueryEntity(m => m.UserName.Equals(account) );
            if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.State != 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "用户名或密码不正确"));

            //检查密码
            if (!password.Equals(memberEntity.PassWord))
                return this.Content(JsonHelper.GetBaseMessage(false, "用户名或密码不正确"));

            //设置登录
            this.AuthenticationService.SatelliteSignIn(memberEntity, true);

            // 强制设置密码
            if (String.IsNullOrEmpty(returnUrl) || returnUrl.ToString().ToLower().Contains("localhost") || returnUrl.ToString().ToLower().Contains("login"))
                returnUrl = "/";

            //if (memberEntity.LoginId.IndexOf("CRFW") == 0)
            //    returnUrl = "/CRProject/Index"; //CR项目的用户登录成功则自动跳转到 CR项目页面
            returnUrl = "/Data/AntibioticDrugFastMap";
            return this.Content(JsonHelper.GetBaseMessage(true, returnUrl));
        }

        /// <summary>
        /// 帐号密码登录
        /// </summary>
        /// <param name="account">用户名/手机号码</param>
        /// <param name="password">密码</param>
        /// <param name="validationCode"></param>
        /// <param name="MvcCaptchaGuid"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult AccountLogin(string account, string password, string validationCode, string mvcCaptchaGuid, string returnUrl = "")
        {
            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符
            if (string.IsNullOrEmpty(account) || account.Length < 3)
                return this.Content(JsonHelper.GetBaseMessage(false, "用户名/手机号码格式不正确"));

            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return this.Content(JsonHelper.GetBaseMessage(false, "密码长度必须大于等于6位"));

            if (string.IsNullOrWhiteSpace(validationCode) || validationCode.Length != 4)
                return this.Content(JsonHelper.GetBaseMessage(false, "输入验证码不正确"));

            // get values 
            var image = MvcCaptchaImage.GetCachedCaptcha(mvcCaptchaGuid);
            string expectedValue = image == null ? String.Empty : image.Text;

            // removes the captch from Session so it cannot be used again 
            Session.Remove(mvcCaptchaGuid);

            bool isValid = !String.IsNullOrEmpty(validationCode)
                           && !String.IsNullOrEmpty(expectedValue)
                           && String.Equals(validationCode, expectedValue, StringComparison.OrdinalIgnoreCase);
            if (!isValid)
                return this.Content(JsonHelper.GetBaseMessage(false, "输入验证码不正确"));

            //检查帐号
            var memberEntity = this.MemberService.QueryEntity(m => m.LoginId.Equals(account) || m.Phone.Equals(account));
            if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                return this.Content(JsonHelper.GetBaseMessage(false, "用户名/手机号码或密码不正确"));

            //检查密码
            string newPassword = this.EncryptionService.EncryptText(password);

            if (!newPassword.Equals(memberEntity.Password))
                return this.Content(JsonHelper.GetBaseMessage(false, "用户名/手机号码或密码不正确"));

            //设置登录
            this.AuthenticationService.SignIn(memberEntity, true);

            // 强制设置密码
            //if (String.IsNullOrEmpty(returnUrl) || returnUrl.ToString().ToLower().Contains("localhost") || returnUrl.ToString().ToLower().Contains("login"))
            //    returnUrl = "/";

            if (String.IsNullOrEmpty(returnUrl) || returnUrl.ToString().ToLower().Contains("login"))
                returnUrl = "/";

            //if (memberEntity.LoginId.IndexOf("CRFW") == 0)
            //    returnUrl = "/CRProject/Index"; //CR项目的用户登录成功则自动跳转到 CR项目页面

            return this.Content(JsonHelper.GetBaseMessage(true, returnUrl));
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult SendValidateCode(string phone)
        {
            try
            {
                //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
                //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
                //用户名长度为4～18个字符
                if (string.IsNullOrEmpty(phone) || !(new Regex(@"^[1-9]{1}[0-9]{10}$").IsMatch(phone)))
                    return this.Content(JsonHelper.GetBaseMessage(false, "手机号码格式不正确"));

                phone = phone.Trim();

                var memberEntity = this.MemberService.QueryModelByPhone(phone);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                {
                    //return this.Content(JsonHelper.GetBaseMessage(false, "用户名不存在"));
                    memberEntity = this.MemberService.Regist(phone, phone, phone, 1);
                }

                if (string.IsNullOrWhiteSpace(memberEntity.Phone))
                    return this.Content(JsonHelper.GetBaseMessage(false, "未设置手机号码"));

                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.Phone;
                var listCount = this.ValidateCodeService.Query(m => m.Value.Equals(memberEntity.Phone) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).Count();
                if (listCount > 0) return this.Content(JsonHelper.GetBaseMessage(false, "验证码已发送过"));

                //验证码
                string code = this.ValidateCodeService.GetCode(ValidateCodeType.Phone);

                //调用API发送短信
                string smsResult = this.ValidateCodeService.SendPhoneMessage("Login", code, memberEntity.Phone);
                //   string smsResult = "success";
                if (!smsResult.Equals("success")) return this.Content(JsonHelper.GetBaseMessage(false, smsResult));

                //将短信内容写入到数据库
                ValidateCode model = new ValidateCode();
                model.Id = CommonHelper.GuidToLongID;
                model.Code = code;
                model.StartTime = time;
                model.OutTime = time.AddMinutes(1);
                model.Source = (int)ValidateCodeSource.Weixin;
                model.Type = (int)ValidateCodeType.Phone;
                model.Value = memberEntity.Phone;
                model.Describe = "会员登录发送验证码，手机号码：" + phone;

                this.ValidateCodeService.Insert(model);

                return this.Content(JsonHelper.GetBaseMessage(true, "发送成功，1分钟内有效"));
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(JsonHelper.GetBaseMessage(false, "系统发送错误"));
            }
        }

        [CheckRole(false)]
        public ActionResult Out()
        {
            this.AuthenticationService.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        #endregion

        #region 登录页面 2019版

        [CheckRole(false)]
        public ActionResult OpenIndex()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost, CheckRole(false)]
        public JsonResult CheckMobile(string mobile)
        {
            try
            {
                Member member = this.MemberService.QueryEntity(m => m.LoginId.Equals(mobile) || m.Phone.Equals(mobile));
                //Member member = MemberService.QueryModelByLoginId(mobile);
                return Json(new
                {
                    status = true,
                    setting = member != null && member.Id > 0 && string.IsNullOrWhiteSpace(member.Password)
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        /// <summary>
        /// 设置密码并登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="newPassword"></param>
        /// <param name="cfmPassword"></param>
        /// <param name="validationCode"></param>
        /// <param name="mvcCaptchaGuid"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost, CheckRole(false)]
        public JsonResult AccountLoginWithPassword(string mobile, string new_Password, string cfm_Password, string validationCode, string mvcCaptchaGuid, string returnUrl = "")
        {
            #region 基础验证
            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符

            if (!string.IsNullOrEmpty(mobile) && (new Regex(@"^[1](([3|5|8][\d])|([4][1,4,5,6,7,8,9])|([6][5,6])|([7][3,4,5,6,7,8])|([9][8,9]))[\d]{8}$").IsMatch(mobile)))
            {
                // 手机号登录
            }
            else
            {
                // 用户名登陆
            }
            //if (string.IsNullOrEmpty(mobile) || !(new Regex(@"^[1-9]{1}[0-9]{10}$").IsMatch(mobile)))
            //{
            //    return Json(new { status = false, message = "手机号码格式不正确" });
            //}

            if (string.IsNullOrWhiteSpace(new_Password) || new_Password.Length < 6)
            {
                return Json(new { status = false, message = "密码长度必须大于等于6位" });
            }

            if (string.IsNullOrEmpty(cfm_Password) || cfm_Password.Length < 6)
            {
                return Json(new { status = false, message = "再次输入密码长度必须大于等于6位" });
            }

            if (!EncryptionService.EncryptText(new_Password).Equals(EncryptionService.EncryptText(cfm_Password)))
            {
                return Json(new { status = false, message = "两次密码输入不一致" });
            }

            if (string.IsNullOrWhiteSpace(validationCode) || validationCode.Length != 4)
            {
                return Json(new { status = false, message = "输入验证码不正确" });
            }

            // get values 
            var image = MvcCaptchaImage.GetCachedCaptcha(mvcCaptchaGuid);
            string expectedValue = image == null ? String.Empty : image.Text;

            // removes the captch from Session so it cannot be used again 
            Session.Remove(mvcCaptchaGuid);

            bool isValid = !String.IsNullOrEmpty(validationCode)
                           && !String.IsNullOrEmpty(expectedValue)
                           && String.Equals(validationCode, expectedValue, StringComparison.OrdinalIgnoreCase);
            if (!isValid)
            {
                return Json(new { status = false, message = "输入验证码不正确" });
            }

            //检查帐号
            var memberEntity = this.MemberService.QueryEntity(m => m.LoginId.Equals(mobile) || m.Phone.Equals(mobile));
            var memberEntity3 = this.MemberService.QueryEntity(m => m.LoginId.Equals(mobile));
            var memberEntity4 = this.MemberService.QueryEntity(m => m.Phone.Equals(mobile));


            if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
            {
                return Json(new { status = false, message = "手机号码/用户名或密码不正确" });
            }
            #endregion

            #region 更新实体
            if (memberEntity.LoginId != mobile)
            {
                //memberEntity.LoginId = mobile;
            }

            memberEntity.IsSettingPassword = true;
            memberEntity.Password = EncryptionService.EncryptText(cfm_Password);

            // 更新密码
            MemberService.Update(memberEntity);
            #endregion

            //设置登录
            this.AuthenticationService.SignIn(memberEntity, true);

            if (String.IsNullOrEmpty(returnUrl) || returnUrl.ToString().ToLower().Contains("localhost") || returnUrl.ToString().ToLower().Contains("login"))
            {
                returnUrl = "/";
            }
            return Json(new { status = true, message = "登录成功", returnUrl = returnUrl });
        }

        #endregion

        #region //多中心获取登用户信息

        /// <summary>
        /// 获取登用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckRole(false)]
        public ActionResult GetLoginMember()
        {
            try
            {
                var _loginUserInfo = base.LoginUserinfo;
                if (_loginUserInfo == null || _loginUserInfo.Id <= 0)
                {
                    return Json(new Member() { Id = 0 }, JsonRequestBehavior.AllowGet);
                }

                //返回当前用户信息
                return Json(_loginUserInfo, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //出错了
                return Json(new Member() { Id = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region //多中心获取支持医院列表

        /// <summary>
        /// 获取多中心研究支持医院列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckRole(false)]
        public ActionResult GetHospital()
        {
            //枚举MemberProjectType：0/CHINET、1/SUGAR多中心、2/CRAB多中心、3/ERA多中心,4/CRE多中心
            var projectType = Utility.ToInt(QueryString.Q("type"));
            if (projectType <= 0)
            {
                Response.Write("项目类型参数为空<br/>");
                Response.Write("type：0/CHINET、1/SUGAR多中心、2/CRAB多中心、3/ERA多中心、4/CRE多中心");
                Response.End();
            }

            List<string> returnList = new List<string>();
            var datalist = this.ProjectHospitalService.Query(t => t.ProjectType == projectType);
            if (datalist != null && datalist.ToList().Count > 0)
            {
                foreach (var item in datalist)
                {
                    returnList.Add(item.HospitalTitle);
                }
            }
            if (returnList.Count <= 0)
            {
                returnList.Add("待更新");
            }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}