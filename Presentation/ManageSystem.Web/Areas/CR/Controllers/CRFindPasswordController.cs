using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using ManageSystem.Web.Models.FindPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.CR.Controllers
{
    public class CRFindPasswordController : WebBaseController
    {
        // GET: CR/FindPassword
        private readonly IMemberService MemberService;
        private readonly IValidateCodeService ValidateCodeService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IArticleService ArticleService;
        private readonly IEncryptionService EncryptionService;

        public CRFindPasswordController(
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

        [CheckRole(false)]
        public ActionResult Open()
        {
            return View();
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
                    return this.Content(JsonHelper.GetBaseMessage(false, "帐号或手机号码不存在"));

                if (string.IsNullOrWhiteSpace(memberEntity.Phone))
                    return this.Content(JsonHelper.GetBaseMessage(false, "未设置手机号码"));

                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.FindPasswordPhone;
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
                model.OutTime = time.AddMinutes(3);
                model.Source = (int)ValidateCodeSource.PC;
                model.Type = (int)ValidateCodeType.FindPasswordPhone;
                model.Value = memberEntity.Phone;
                model.Describe = "会员找回密码发送验证码，手机号码：" + phone;

                this.ValidateCodeService.Insert(model);

                return this.Content(JsonHelper.GetBaseMessage(true, "发送成功，3分钟内有效"));
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(JsonHelper.GetBaseMessage(false, "系统发送错误"));
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="model">提交的数据</param>
        /// <returns></returns>
        [CheckRole(false)]
        [HttpPost]
        public ContentResult Submit(FindPasswordModel model)
        {
            #region 验证数据

            if (string.IsNullOrWhiteSpace(model.Phone) || !RegexHelper.IsPhone(model.Phone))
                return this.Content(JsonHelper.GetBaseMessage(false, "手机号码格式不正确"));

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Trim().Length < 6)
                return this.Content(JsonHelper.GetBaseMessage(false, "密码长度必须大于等于6位"));

            if (string.IsNullOrWhiteSpace(model.Password2) || model.Password2.Trim().Length < 6)
                return this.Content(JsonHelper.GetBaseMessage(false, "确认密码长度必须大于等于6位"));

            if (!model.Password.Equals(model.Password2))
                return this.Content(JsonHelper.GetBaseMessage(false, "2次密码不一致"));

            if (string.IsNullOrEmpty(model.ValidateCode) || !(new Regex(@"^[0-9]{6}$").IsMatch(model.ValidateCode)))
                return this.Content(JsonHelper.GetBaseMessage(false, "验证码格式不正确"));

            //检查帐号
            var entity = this.MemberService.QueryModelByPhone(model.Phone);
            if (entity == null || entity.Id <= 0 || entity.Status != (int)MemberStatus.Normal)
                return this.Content(JsonHelper.GetBaseMessage(false, "帐号或手机号码不存在"));

            //检查验证码
            DateTime time = DateTime.Now;
            int type = (int)ValidateCodeType.FindPasswordPhone;
            var validateCodeEntity = this.ValidateCodeService.Query(m => m.Value.Equals(entity.Phone) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).FirstOrDefault();
            if (validateCodeEntity == null || validateCodeEntity.Id <= 0 || !validateCodeEntity.Code.Equals(model.ValidateCode))
                return this.Content(JsonHelper.GetBaseMessage(false, "验证码不正确或已过期"));

            #endregion
            entity.Password = this.EncryptionService.EncryptText(model.Password2);
            this.MemberService.Update(entity);

            //添加日志
            base.InsetActionLog(Core.Domain.Log.ActionType.Create, "用户找回密码，手机号码：" + model.Phone, entity.SerializeObject());

            return this.Content(JsonHelper.GetBaseMessage(true, ""));
        }

        #region 找回密码 2019版
        [CheckRole(false)]
        public ActionResult OpenIndex()
        {
            return View();
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost, CheckRole(false)]
        public ContentResult SendEmailValidateCode(string email)
        {
            try
            {
                //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
                //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
                //用户名长度为4～18个字符
                if (string.IsNullOrEmpty(email) || !(new Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$").IsMatch(email)))
                    return this.Content(JsonHelper.GetBaseMessage(false, "邮箱格式不正确"));

                email = email.Trim();

                var memberEntity = this.MemberService.QueryModelByEmail(email);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                    return this.Content(JsonHelper.GetBaseMessage(false, "帐号或邮箱地址不存在"));

                if (string.IsNullOrWhiteSpace(memberEntity.Email))
                    return this.Content(JsonHelper.GetBaseMessage(false, "未设置邮箱地址"));

                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.FindPasswordPhone;
                var listCount = this.ValidateCodeService.Query(m => m.Value.Equals(memberEntity.Email) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).Count();
                if (listCount > 0) return this.Content(JsonHelper.GetBaseMessage(false, "请前往邮箱查收验证码"));

                // 验证码
                string code = this.ValidateCodeService.GetCode(ValidateCodeType.Email);

                // 发送邮件验证码
                string smsResult = this.ValidateCodeService.SendEmailMessage(memberEntity.Email, code);
                //   string smsResult = "success";
                if (!smsResult.Equals("success")) return this.Content(JsonHelper.GetBaseMessage(false, smsResult));

                //将短信内容写入到数据库
                ValidateCode model = new ValidateCode();
                model.Id = CommonHelper.GuidToLongID;
                model.Code = code;
                model.StartTime = time;
                model.OutTime = time.AddMinutes(3);
                model.Source = (int)ValidateCodeSource.PC;
                model.Type = (int)ValidateCodeType.FindPasswordPhone;
                model.Value = memberEntity.Email;
                model.Describe = $"会员通过邮箱找回密码发送验证码，邮箱地址：{email}";

                this.ValidateCodeService.Insert(model);

                return this.Content(JsonHelper.GetBaseMessage(true, "发送成功，请前往邮箱查收验证码，3分钟内有效"));
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(JsonHelper.GetBaseMessage(false, "系统发送错误"));
            }
        }

        [HttpPost, CheckRole(false)]
        public ContentResult EmailSubmit(FindPasswordModel model)
        {
            #region 验证数据

            if (string.IsNullOrWhiteSpace(model.Phone) || !RegexHelper.IsEmail(model.Phone))
                return this.Content(JsonHelper.GetBaseMessage(false, "邮箱码格式不正确"));

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Trim().Length < 6)
                return this.Content(JsonHelper.GetBaseMessage(false, "密码长度必须大于等于6位"));

            if (string.IsNullOrWhiteSpace(model.Password2) || model.Password2.Trim().Length < 6)
                return this.Content(JsonHelper.GetBaseMessage(false, "确认密码长度必须大于等于6位"));

            if (!model.Password.Equals(model.Password2))
                return this.Content(JsonHelper.GetBaseMessage(false, "2次密码不一致"));

            if (string.IsNullOrEmpty(model.ValidateCode) || model.ValidateCode.Length != 6)
                return this.Content(JsonHelper.GetBaseMessage(false, "验证码格式不正确"));

            // 此处Phone 存入数据是Email
            model.Phone = model.Phone.Trim();

            // 检查帐号
            var entity = this.MemberService.QueryModelByEmail(model.Phone);
            if (entity == null || entity.Id <= 0 || entity.Status != (int)MemberStatus.Normal)
                return this.Content(JsonHelper.GetBaseMessage(false, "帐号或邮箱不存在"));

            // 检查验证码
            DateTime time = DateTime.Now;
            int type = (int)ValidateCodeType.FindPasswordPhone;
            var validateCodeEntity = this.ValidateCodeService.Query(m => m.Value.Equals(entity.Email) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).FirstOrDefault();
            if (validateCodeEntity == null || validateCodeEntity.Id <= 0 || !validateCodeEntity.Code.Equals(model.ValidateCode))
                return this.Content(JsonHelper.GetBaseMessage(false, "验证码不正确或已过期"));

            #endregion
            entity.IsSettingPassword = true;
            entity.Password = this.EncryptionService.EncryptText(model.Password2);
            this.MemberService.Update(entity);

            //添加日志
            base.InsetActionLog(Core.Domain.Log.ActionType.Create, "用户通过邮箱找回密码，邮箱地址：" + model.Phone, entity.SerializeObject());

            return this.Content(JsonHelper.GetBaseMessage(true, ""));
        }
        #endregion
    }
}