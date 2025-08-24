using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.MvcCaptcha;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.CR.Controllers
{
    public class CRLoginController : WebBaseController
    {
        // GET: CRs/CRLogin
        private readonly IMemberService MemberService;
        private readonly IValidateCodeService ValidateCodeService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IArticleService ArticleService;
        private readonly IEncryptionService EncryptionService;

        public CRLoginController(
                 IMemberService _memberService,
                 IValidateCodeService _validateCodeService,
                 IAuthenticationService _authenticationService,
                IArticleService _articleService,
               IEncryptionService _encryptionService
            )
        {
            this.MemberService = _memberService;
            this.ValidateCodeService = _validateCodeService;
            this.AuthenticationService = _authenticationService;
            this.ArticleService = _articleService;
            this.EncryptionService = _encryptionService;
        }

        // GET: CR/Login
        public ActionResult Index()
        {
            return View();
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

            if (memberEntity.LoginId.IndexOf("CRFW") == 0)
                returnUrl = "/CRProject/Index"; //CR项目的用户登录成功则自动跳转到 CR项目页面

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
            if (String.IsNullOrEmpty(returnUrl) || returnUrl.ToString().ToLower().Contains("localhost") || returnUrl.ToString().ToLower().Contains("login"))
                returnUrl = "/CR/CRData/Index";

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
            return this.RedirectToAction("Index", "CRData");
        }
    }
}