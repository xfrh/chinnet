using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using ManageSystem.Web.Models.Regist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.CR.Controllers
{
    public class CRRegistController : WebBaseController
    {
        private readonly IHospitalService HospitalService;
        private readonly IMemberService MemberService;
        private readonly IValidateCodeService ValidateCodeService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IAreaService AreaService;
        private readonly IEncryptionService EncryptionService;

        public CRRegistController(
               IHospitalService _hospitalService,
                 IMemberService _memberService,
                 IValidateCodeService _validateCodeService,
                 IAuthenticationService _authenticationService,
                IAreaService _areaService,
                    IEncryptionService _encryptionService

            )
        {
            this.HospitalService = _hospitalService;
            this.MemberService = _memberService;
            this.ValidateCodeService = _validateCodeService;
            this.AuthenticationService = _authenticationService;
            this.AreaService = _areaService;
            this.EncryptionService = _encryptionService;
        }

        #region PC端注册

        /// <summary>
        /// 用户注册的 
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Open()
        {
            RegistModel model = new RegistModel();
            model.AreaList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();    //区域列表（顶级，省份）
            model.AreaList.Insert(0, new SelectListItem() { Text = "所属省份", Value = "" });

            return View(model);
        }

        /// <summary>
        /// 发送注册的短信验证码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult SendRegistValidateCode(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone) || !RegexHelper.IsPhone(phone))
                    return this.Content(JsonHelper.GetBaseMessage(false, "手机号码格式不正确"));

                phone = phone.Trim();

                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.RegistPhone;
                var listCount = this.ValidateCodeService.Query(m => m.Value.Equals(phone) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).Count();
                if (listCount > 0) return this.Content(JsonHelper.GetBaseMessage(false, "验证码已发送过"));

                //验证码
                string code = this.ValidateCodeService.GetCode(ValidateCodeType.RegistPhone);

                //调用API发送短信
                string smsResult = this.ValidateCodeService.SendPhoneMessage("Login", code, phone);
                //   string smsResult = "success";
                if (!smsResult.Equals("success")) return this.Content(JsonHelper.GetBaseMessage(false, smsResult));

                //将短信内容写入到数据库
                ValidateCode model = new ValidateCode();
                model.Id = CommonHelper.GuidToLongID;
                model.Code = code;
                model.StartTime = time;
                model.OutTime = time.AddMinutes(3);
                model.Source = (int)ValidateCodeSource.Weixin;
                model.Type = (int)ValidateCodeType.RegistPhone;
                model.Value = phone;
                model.Describe = "会员注册发送验证码，手机号码：" + phone;

                this.ValidateCodeService.Insert(model);

                return this.Content(JsonHelper.GetBaseMessage(true, "发送成功，3分钟内有效"));
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(JsonHelper.GetBaseMessage(false, "系统发送错误"));
            }
        }

        #endregion

        #region 移动端注册

        /// <summary>
        /// 移动端注册
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Mobile()
        {
            RegistModel model = new RegistModel();
            model.AreaList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();    //区域列表（顶级，省份）
            model.AreaList.Insert(0, new SelectListItem() { Text = "所属省份", Value = "" });

            return View(model);
        }

        #endregion

        #region 提交注册
        /// <summary>
        /// 提交用户注册
        /// </summary>
        /// <param name="model">提交的数据</param>
        /// <returns></returns>
        [CheckRole(false)]
        [HttpPost]
        public ContentResult Regist(RegistModel model)
        {
            try
            {
                //注册用户
                Member entity = this.MemberService.Regist(model.Phone, model.Password, model.Password2, model.AreaId, model.OtherHospital, model.Source, "");

                //设置登录
                this.AuthenticationService.SignIn(entity, true);

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }
        #endregion


        #region PC端注册 2019版
        [CheckRole(false)]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 提交注册 2019版
        /// <summary>
        /// 提交用户注册
        /// </summary>
        /// <param name="model">提交的数据</param>
        /// <returns></returns>
        [CheckRole(false)]
        [HttpPost]
        public ContentResult PostRegist(RegistModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.LoginId))
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "请输入您的登录用户名"));
                }

                // 判断用户名是否存在
                if (MemberService.Count(r => r.LoginId == model.LoginId) > 0)
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "此登录用户名已经存在，请更换其他用户名"));
                }

                //if (string.IsNullOrWhiteSpace(model.MemberName))
                //{
                //    return this.Content(JsonHelper.GetBaseMessage(false, "请输入您的姓名"));
                //}

                if (string.IsNullOrWhiteSpace(model.Phone))
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "请输入您的手机号"));
                }

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "请输入登录密码"));
                }
                if (model.Password.Trim().Length < 6)
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "密码长度必须大于等于6位"));
                }
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "请再次输入登录密码"));
                }

                model.Password = EncryptionService.EncryptText(model.Password);
                //model.Password2 = EncryptionService.EncryptText(model.Password2);

                //if (!model.Password.Equals(model.Password2))
                //{
                //    return this.Content(JsonHelper.GetBaseMessage(false, "两次输入登录密码不一致"));
                //}

                //model.Password = EncryptionService.DecryptText(model.Password);
                //model.Password2 = EncryptionService.DecryptText(model.Password2);

                //注册用户
                Member entity = this.MemberService.Regist(loginId: model.LoginId, name: model.MemberName, phone: model.Phone, password: model.Password, password2: model.Password2, source: model.Source);

                //设置登录
                this.AuthenticationService.SignIn(entity, true);

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }
        #endregion
    }
}