using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Core.WebApi;
using ManageSystem.Framework.MvcCaptcha;
using ManageSystem.Services.Configuration;
using ManageSystem.Services.Log;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Mobile.MobileBaseController
{
    /// <summary>
    /// 个人中心相关控制器
    /// </summary>
    public class UserController : Controller
    {
        #region 相关业务层声明
        /// <summary>
        /// 用户业务层
        /// </summary>
        private readonly IMemberService MemberService;
        /// <summary>
        /// 加解密服务
        /// </summary>
        private readonly IEncryptionService EncryptionService;
        /// <summary>
        /// 区域业务层
        /// </summary>
        private readonly IAreaService AreaService;
        /// <summary>
        /// 反馈业务层
        /// </summary>
        private readonly IFeedbackService FeedbackService;
        /// <summary>
        /// 操作日志业务层
        /// </summary>
        private readonly IActionLogService ActionLogService;
        /// <summary>
        /// 用户认证业务层
        /// </summary>
        private readonly IMemberAttestationService MemberAttestationService;
        /// <summary>
        /// 医院部分业务层
        /// </summary>
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        /// <summary>
        /// 医生职务业务层
        /// </summary>
        private readonly IDoctorTitleService DoctorTitleService;
        /// <summary>
        /// 医院业务层
        /// </summary>
        private readonly IHospitalService HospitalService;
        /// <summary>
        /// 验证码业务层
        /// </summary>
        private readonly IValidateCodeService ValidateCodeService;
        /// <summary>
        /// 系统日志业务层
        /// </summary>
        private readonly ISystemLogService SystemLogService;
        /// <summary>
        /// 上传数据业务层
        /// </summary>
        private readonly IMedicalDataService MedicalDataService;
        /// <summary>
        /// 邮件业务层
        /// </summary>
        private readonly IMessageEmailService EmailService;
        /// <summary>
        /// 缓存
        /// </summary>
        private readonly ICacheManager cacheManager;
        /// <summary>
        /// 网站配置业务
        /// </summary>
        private readonly ISettingService settingService;

        private const string JWTSECRET = "com.chinets.wiaWQiOjQ4MTY3OTM1MTY5NjA0MDg";
        #endregion

        /// <summary>
        /// 时辰段声明
        /// </summary>
        private static readonly Dictionary<string, List<int>> TimeTextTange = new Dictionary<string, List<int>>
        {
            ["凌晨好"] = new List<int> { 3, 4, 5 },
            ["早晨好"] = new List<int> { 6, 7 },
            ["上午好"] = new List<int> { 8, 9, 10 },
            ["中午好"] = new List<int> { 11, 12 },
            ["下午好"] = new List<int> { 13, 14, 15, 16 },
            ["傍晚好"] = new List<int> { 17, 18 },
            ["晚上好"] = new List<int> { 19, 20, 21, 22 },
            ["深夜了,早些休息"] = new List<int> { 23, 24, 0, 1, 2 }
        };

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="_memberService"></param>
        /// <param name="_encryptionService"></param>
        /// <param name="_areaService"></param>
        /// <param name="_feedbackService"></param>
        /// <param name="_memberAttestationService"></param>
        /// <param name="_hospitalDepartmentService"></param>
        /// <param name="_doctorTitleService"></param>
        /// <param name="_hospitalService"></param>
        /// <param name="_validateCodeService"></param>
        /// <param name="_systemLogService"></param>
        /// <param name="_medicalDataService"></param>
        public UserController(IMemberService _memberService, IEncryptionService _encryptionService, IAreaService _areaService, IFeedbackService _feedbackService, IMemberAttestationService _memberAttestationService, IHospitalDepartmentService _hospitalDepartmentService, IDoctorTitleService _doctorTitleService, IHospitalService _hospitalService, IValidateCodeService _validateCodeService, ISystemLogService _systemLogService, IMedicalDataService _medicalDataService, IMessageEmailService _emailService, ICacheManager _cacheManager, ISettingService _settingService)
        {
            MemberService = _memberService;
            EncryptionService = _encryptionService;
            AreaService = _areaService;
            FeedbackService = _feedbackService;
            MemberAttestationService = _memberAttestationService;
            HospitalDepartmentService = _hospitalDepartmentService;
            DoctorTitleService = _doctorTitleService;
            HospitalService = _hospitalService;
            ValidateCodeService = _validateCodeService;
            SystemLogService = _systemLogService;
            MedicalDataService = _medicalDataService;
            EmailService = _emailService;
            this.cacheManager = _cacheManager;
            settingService = _settingService;
        }

        #region 账号登录
        /// <summary>
        /// 账号登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("login.cshtml")]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 提交账号登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(string mobile, string password, string validationCode, string mvcCaptchaGuid)
        {
            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符
            //if (string.IsNullOrEmpty(mobile) || !(new Regex(@"^[1-9]{1}[0-9]{10}$").IsMatch(mobile)))
            //{
            //    return Json(new { status = false, message = "手机号码格式不正确" });
            //}
            if (string.IsNullOrEmpty(mobile) || mobile.Length < 3)
            {
                return Json(new { status = false, message = "用户名/手机号码格式不正确" });
            }

            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return Json(new { status = false, message = "密码长度必须大于等于6位" });

            //if (string.IsNullOrWhiteSpace(validationCode) || validationCode.Length != 4)
            //{
            //    return Json(new { status = false, message = "输入验证码不正确" });
            //}

            //// get values 
            //var image = MvcCaptchaImage.GetCachedCaptcha(mvcCaptchaGuid);
            //string expectedValue = image == null ? String.Empty : image.Text;

            // removes the captch from Session so it cannot be used again 
            Session.Remove(mvcCaptchaGuid);

            //bool isValid = !String.IsNullOrEmpty(validationCode)
            //               && !String.IsNullOrEmpty(expectedValue)
            //               && String.Equals(validationCode, expectedValue, StringComparison.OrdinalIgnoreCase);
            //if (!isValid)
            //{
            //    return Json(new { status = false, message = "输入验证码不正确" });
            //}

            //检查帐号
            var memberEntity = this.MemberService.QueryEntity(m => m.LoginId.Equals(mobile) || m.Phone.Equals(mobile));

            if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                return Json(new { status = false, message = "用户名/手机号码或密码不正确" });

            //检查密码
            string newPassword = this.EncryptionService.EncryptText(password);

            if (!newPassword.Equals(memberEntity.Password))
                return Json(new { status = false, message = "用户名/手机号码或密码不正确" });

            return Json(new
            {
                status = true,
                url = "/",
                data = this.EncryptionService.EncryptText($"{memberEntity.Id}@@{memberEntity.LoginId}$${CommonHelper.GuidToLongID}")
            });
        }

        [HttpPost]
        public JsonResult SettingPasswordAndLogin(string mobile, string new_password, string cfm_password, string validationCode, string mvcCaptchaGuid)
        {
            #region 基础验证
            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符
            //if (string.IsNullOrEmpty(mobile) || !(new Regex(@"^[1-9]{1}[0-9]{10}$").IsMatch(mobile)))
            //{
            //    return Json(new { status = false, message = "手机号码格式不正确" });
            //}

            if (string.IsNullOrEmpty(mobile) || mobile.Length < 3)
            {
                return Json(new { status = false, message = "用户名/手机号码格式不正确" });
            }

            if (string.IsNullOrWhiteSpace(new_password) || new_password.Length < 6)
            {
                return Json(new { status = false, message = "密码长度必须大于等于6位" });
            }

            //if (string.IsNullOrEmpty(cfm_password) || cfm_password.Length < 6)
            //{
            //    return Json(new { status = false, message = "再次输入密码长度必须大于等于6位" });
            //}

            //if (!EncryptionService.EncryptText(new_password).Equals(EncryptionService.EncryptText(cfm_password)))
            //{
            //    return Json(new { status = false, message = "两次密码输入不一致" });
            //}

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
            if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
            {
                return Json(new { status = false, message = "用户名/手机号码或密码不正确" });
            }
            #endregion

            #region 更新实体
            if (memberEntity.LoginId != mobile)
            {
                //memberEntity.LoginId = mobile;
            }

            memberEntity.IsSettingPassword = true;
            memberEntity.Password = EncryptionService.EncryptText(cfm_password);

            // 更新密码
            MemberService.Update(memberEntity);
            #endregion

            return Json(new
            {
                status = true,
                url = "/",
                data = this.EncryptionService.EncryptText($"{memberEntity.Id}@@{memberEntity.LoginId}$${CommonHelper.GuidToLongID}")
            });
        }
        /// <summary>
        /// 检查手机号是否设置密码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckLoginMobile(string mobile)
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
        #endregion

        #region 验证码登录
        /// <summary>
        /// 验证码登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("login-sms.cshtml")]
        public ViewResult SmsLogin()
        {
            return View();
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SmsLoginSmsCode(string mobile)
        {
            try
            {
                mobile = mobile.Trim();
                if (string.IsNullOrEmpty(mobile) || !(new Regex(@"^[1-9]{1}[0-9]{10}$").IsMatch(mobile)))
                    return Json(new { status = false, message = "手机号码格式不正确" });

                var memberEntity = MemberService.QueryModelByPhone(mobile);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                {
                    // 手机号不存在时新增一个账号
                    memberEntity = this.MemberService.Regist(mobile, mobile, mobile, 2, "");
                }

                if (string.IsNullOrWhiteSpace(memberEntity.Phone))
                    return Json(new { status = false, message = "未设置手机号码" });

                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.Phone;
                var listCount = ValidateCodeService.Query(m => m.Value.Equals(memberEntity.Phone) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).Count();
                if (listCount > 0) return Json(new { status = false, message = "验证码已发送过" });

                //验证码
                string code = ValidateCodeService.GetCode(ValidateCodeType.Phone);

                //调用API发送短信
                string smsResult = ValidateCodeService.SendPhoneMessage("Login", code, memberEntity.Phone);

                if (!smsResult.Equals("success")) return Json(new { status = false, message = smsResult });

                //将短信内容写入到数据库
                ValidateCode model = new ValidateCode
                {
                    Id = CommonHelper.GuidToLongID,
                    Code = code,
                    StartTime = time,
                    OutTime = time.AddMinutes(1),
                    Source = (int)ValidateCodeSource.Weixin,
                    Type = (int)ValidateCodeType.Phone,
                    Value = memberEntity.Phone,
                    Describe = $"会员登录发送验证码，手机号码：{mobile}"
                };

                ValidateCodeService.Insert(model);

                return Json(new { status = true, message = "发送成功，1分钟内有效" });
            }
            catch (Exception ex)
            {
                SystemLogService.Insert(ex, SystemLogLevel.Error, Request.Url.ToString());
                return Json(new { status = false, message = "系统发送错误" });
            }
        }

        /// <summary>
        /// 提交验证码登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="smsCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SmsLogin(string mobile, string smsCode)
        {
            try
            {
                mobile = mobile.Trim();
                if (string.IsNullOrEmpty(mobile) || !RegexHelper.IsPhone(mobile))
                    return Json(new { status = false, message = "手机号码格式不正确" });

                var memberEntity = MemberService.QueryModelByPhone(mobile);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                {
                    return Json(new { status = false, message = "帐号或手机号码不存在" });
                }

                if (string.IsNullOrWhiteSpace(memberEntity.Phone))
                    return Json(new { status = false, message = "未设置手机号码" });

                if (string.IsNullOrWhiteSpace(smsCode))
                    return Json(new { status = false, message = "请输入手机验证码" });

                if (string.IsNullOrEmpty(smsCode) || !(new Regex(@"^[0-9]{6}$").IsMatch(smsCode)))
                    return Json(new { status = false, message = "验证码格式不正确" });

                //检查验证码
                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.Phone;
                var validateCodeEntity = this.ValidateCodeService.Query(m => m.Value.Equals(mobile) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).FirstOrDefault();
                if (validateCodeEntity == null || validateCodeEntity.Id <= 0 || !validateCodeEntity.Code.Equals(smsCode))
                    return Json(new { status = false, message = "验证码不正确或已过期" });

                return Json(new
                {
                    status = true,
                    message = "手机号登录成功",
                    url = "/",
                    token = EncryptionService.EncryptText($"{memberEntity.Id}@@{memberEntity.LoginId}$${CommonHelper.GuidToLongID}")
                });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                SystemLogService.Insert(ex, SystemLogLevel.Error, Request.Url.ToString());
                return Json(new { status = false, message = "系统处理发生错误" });
            }
        }
        #endregion

        #region 账号注册
        /// <summary>
        /// 账号注册页面
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("register.cshtml")]
        public ViewResult Register()
        {
            Log4Helper.Info("ManageSystemException");
            //List<SelectListItem> dropProvince = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            //dropProvince.Insert(0, new SelectListItem { Value = "-1", Text = "所属省份" });
            //ViewData["DropProvince"] = dropProvince;
            return View();
        }

        /// <summary>
        /// 提交账号注册
        /// </summary>
        /// <param name="loginid"></param>
        /// <param name="mobile"></param>
        /// <param name="newPassword"></param>
        /// <param name="cfmPassword"></param>
        /// <param name="provinceId"></param>
        /// <param name="hospital"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register(string loginid, string name, string mobile, string newPassword, string cfmPassword)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(name)) return Json(new { status = false, message = "请输入您的姓名" });

                if (string.IsNullOrWhiteSpace(loginid))
                {
                    return Json(new { status = false, message = "请输入您的登录用户名" });
                }

                // 判断用户名是否存在
                if (MemberService.Count(r => r.LoginId == loginid) > 0)
                {
                    return Json(new { status = false, message = "此登录用户名已经存在，请更换其他用户名" });
                }


                //mobile = mobile.Trim();
                //if (string.IsNullOrEmpty(mobile) || !RegexHelper.IsPhone(mobile))
                //    return Json(new { status = false, message = "手机号码格式不正确" });

                if (string.IsNullOrWhiteSpace(newPassword))
                    return Json(new { status = false, message = "请输入新密码" });

                //if (string.IsNullOrWhiteSpace(cfmPassword))
                //    return Json(new { status = false, message = "请再次输入新密码" });

                if (newPassword.Trim().Length < 6)
                    return Json(new { status = false, message = "密码长度必须大于或等于6位" });

                newPassword = EncryptionService.EncryptText(newPassword);
                //cfmPassword = EncryptionService.EncryptText(cfmPassword);
                //if (!newPassword.Equals(cfmPassword))
                //{
                //    return Json(new { status = false, message = "两次密码输入不一致" });
                //}

              //  newPassword = EncryptionService.DecryptText(newPassword);
                //cfmPassword = EncryptionService.DecryptText(cfmPassword);

                //注册用户
                Member entity = this.MemberService.Regist(loginId: loginid, name: name, phone: mobile, password: newPassword, password2: cfmPassword, source: 1);

                //注册用户
                //Member entity = this.MemberService.Regist(mobile, newPassword, cfmPassword, provinceId, hospital, 2, "");

                return Json(new
                {
                    status = true,
                    message = "注册成功,正在跳转",
                    url = "/",
                    token = EncryptionService.EncryptText($"{entity.Id}@@{entity.LoginId}$${CommonHelper.GuidToLongID}")
                });
            }
            catch (ManageSystemException ex)
            {
                Log4Helper.Info("ManageSystemException");
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                SystemLogService.Insert(ex, SystemLogLevel.Error);
                Log4Helper.Info("Exception");
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new { status = false, message = ex.Message });
            }
        }
        #endregion

        #region 忘记密码 -- 手机验证
        /// <summary>
        /// 忘记密码页面
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("forget.cshtml")]
        public ViewResult ForgetPassword()
        {
            return View();
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForgetPasswordSmsCode(string mobile)
        {
            try
            {
                mobile = !string.IsNullOrWhiteSpace(mobile) ? mobile.Trim() : "";

                //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
                //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
                //用户名长度为4～18个字符
                if (string.IsNullOrEmpty(mobile) || !(new Regex(@"^[1-9]{1}[0-9]{10}$").IsMatch(mobile)))
                    return Json(new { status = false, message = "手机号码格式不正确" });

                var memberEntity = MemberService.QueryModelByPhone(mobile);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                {
                    return Json(new { status = false, message = "帐号或手机号码不存在" });
                }

                if (string.IsNullOrWhiteSpace(memberEntity.Phone))
                    return Json(new { status = false, message = "未设置手机号码" });

                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.FindPasswordPhone;
                var listCount = ValidateCodeService.Query(m => m.Value.Equals(memberEntity.Phone) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).Count();
                if (listCount > 0) return Json(new { status = false, message = "验证码已发送过" });

                //验证码
                string code = ValidateCodeService.GetCode(ValidateCodeType.Phone);

                //调用API发送短信
                string smsResult = ValidateCodeService.SendPhoneMessage("Login", code, memberEntity.Phone);

                if (!smsResult.Equals("success")) return Json(new { status = false, message = smsResult });

                //将短信内容写入到数据库
                ValidateCode model = new ValidateCode
                {
                    Id = CommonHelper.GuidToLongID,
                    Code = code,
                    StartTime = time,
                    OutTime = time.AddMinutes(3),
                    Source = (int)ValidateCodeSource.Weixin,
                    Type = (int)ValidateCodeType.FindPasswordPhone,
                    Value = memberEntity.Phone,
                    Describe = "会员找回密码发送验证码，手机号码：" + mobile
                };

                ValidateCodeService.Insert(model);

                return Json(new { status = true, message = "发送成功，3分钟内有效" });
            }
            catch (Exception ex)
            {
                SystemLogService.Insert(ex, SystemLogLevel.Error, Request.Url.ToString());
                return Json(new { status = false, message = "系统发送错误" });
            }
        }

        /// <summary>
        /// 提交重置登录密码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="smsCode"></param>
        /// <param name="newPassword"></param>
        /// <param name="cfmPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForgetPassword(string mobile, string smsCode, string newPassword, string cfmPassword)
        {
            try
            {
                mobile = mobile.Trim();
                if (string.IsNullOrEmpty(mobile) || !RegexHelper.IsPhone(mobile))
                {
                    return Json(new { status = false, message = "手机号码格式不正确" });
                }

                var memberEntity = MemberService.QueryModelByPhone(mobile);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                {
                    return Json(new { status = false, message = "帐号或手机号码不存在" });
                }

                if (string.IsNullOrWhiteSpace(memberEntity.Phone))
                    return Json(new { status = false, message = "未设置手机号码" });

                if (string.IsNullOrWhiteSpace(smsCode))
                    return Json(new { status = false, message = "请输入手机验证码" });

                if (string.IsNullOrEmpty(smsCode) || !(new Regex(@"^[0-9]{6}$").IsMatch(smsCode)))
                    return Json(new { status = false, message = "验证码格式不正确" });

                //检查验证码
                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.FindPasswordPhone;
                var validateCodeEntity = this.ValidateCodeService.Query(m => m.Value.Equals(mobile) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).FirstOrDefault();
                if (validateCodeEntity == null || validateCodeEntity.Id <= 0 || !validateCodeEntity.Code.Equals(smsCode))
                    return Json(new { status = false, message = "验证码不正确或已过期" });

                if (string.IsNullOrWhiteSpace(newPassword))
                    return Json(new { status = false, message = "请输入新的登录密码" });

                if (newPassword.Trim().Length < 6)
                    return Json(new { status = false, message = "密码长度必须大于等于6位" });

                //if (string.IsNullOrWhiteSpace(cfmPassword))
                //    return Json(new { status = false, message = "请再次输入新的登录密码" });

                //if (!EncryptionService.EncryptText(newPassword).Equals(EncryptionService.EncryptText(cfmPassword)))
                //{
                //    return Json(new { status = false, message = "两次密码输入不一致" });
                //}

                memberEntity.Password = EncryptionService.EncryptText(cfmPassword);
                MemberService.Update(memberEntity);

                try
                {
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Mobile, memberEntity.Id, memberEntity.Name + "（" + memberEntity.LoginId + "）", $"用户找回密码，手机号码：{memberEntity.Phone}", memberEntity.SerializeObject());
                }
                catch (System.Exception)
                {

                }
                return Json(new { status = true, message = "密码重置成功", url = Url.Action("Login").ToLowerInvariant() });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                SystemLogService.Insert(ex, SystemLogLevel.Error, Request.Url.ToString());
                return Json(new { status = false, message = "系统处理发生错误" });
            }
        }
        #endregion

        #region 忘记密码 -- 邮箱验证
        [HttpGet, Route("forget-email.cshtml")]
        public ActionResult EmailForgetPassword()
        {
            return View();
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForgetPasswordEmailCode(string email)
        {
            try
            {
                email = string.IsNullOrWhiteSpace(email) ? "" : email.Trim();

                //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
                //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
                //用户名长度为4～18个字符
                if (string.IsNullOrEmpty(email) || !(new Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$").IsMatch(email)))
                    return Json(new { status = false, message = "邮箱格式不正确" });

                var memberEntity = MemberService.QueryModelByEmail(email);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                    return Json(new { status = false, message = "帐号或邮箱地址不存在" });

                if (string.IsNullOrWhiteSpace(memberEntity.Email))
                    return Json(new { status = false, message = "未设置邮箱地址" });

                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.FindPasswordPhone;
                var listCount = ValidateCodeService.Query(m => m.Value.Equals(memberEntity.Email) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).Count();
                if (listCount > 0) return Json(new { status = false, message = "请前往邮箱查收验证码" });

                //验证码
                string code = ValidateCodeService.GetCode(ValidateCodeType.Email);

                // 发送邮件验证码
                string smsResult = this.ValidateCodeService.SendEmailMessage(memberEntity.Email, code);

                if (!smsResult.Equals("success")) return Json(new { status = false, message = smsResult });

                //将短信内容写入到数据库
                ValidateCode model = new ValidateCode
                {
                    Id = CommonHelper.GuidToLongID,
                    Code = code,
                    StartTime = time,
                    OutTime = time.AddMinutes(3),
                    Source = (int)ValidateCodeSource.PC,
                    Type = (int)ValidateCodeType.FindPasswordPhone,
                    Value = memberEntity.Email,
                    Describe = $"会员通过邮箱找回密码发送验证码，邮箱地址：{email}"
                };

                ValidateCodeService.Insert(model);

                return Json(new { status = true, message = "发送成功，请前往邮箱查收验证码，3分钟内有效" });
            }
            catch (Exception ex)
            {
                SystemLogService.Insert(ex, SystemLogLevel.Error, Request.Url.ToString());
                return Json(new { status = false, message = "系统发送错误" });
            }
        }

        /// <summary>
        /// 提交重置登录密码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="smsCode"></param>
        /// <param name="newPassword"></param>
        /// <param name="cfmPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmailForgetPassword(string email, string smsCode, string newPassword, string cfmPassword)
        {
            try
            {
                email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
                if (string.IsNullOrEmpty(email) || !(new Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$").IsMatch(email)))
                {
                    return Json(new { status = false, message = "邮箱格式不正确" });
                }

                var memberEntity = MemberService.QueryModelByEmail(email);
                if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
                {
                    return Json(new { status = false, message = "帐号或邮箱地址不存在" });
                }

                if (string.IsNullOrWhiteSpace(memberEntity.Email))
                    return Json(new { status = false, message = "未设置邮箱地址" });

                if (string.IsNullOrWhiteSpace(smsCode))
                    return Json(new { status = false, message = "请输入验证码" });

                if (string.IsNullOrEmpty(smsCode) || smsCode.Length != 6)
                    return Json(new { status = false, message = "验证码格式不正确" });

                //检查验证码
                DateTime time = DateTime.Now;
                int type = (int)ValidateCodeType.FindPasswordPhone;
                var validateCodeEntity = this.ValidateCodeService.Query(m => m.Value.Equals(email) && m.Mark > 0 && m.Type == type && time >= m.StartTime && time <= m.OutTime).FirstOrDefault();
                if (validateCodeEntity == null || validateCodeEntity.Id <= 0 || !validateCodeEntity.Code.Equals(smsCode))
                    return Json(new { status = false, message = "验证码不正确或已过期" });

                if (string.IsNullOrWhiteSpace(newPassword))
                    return Json(new { status = false, message = "请输入新的登录密码" });

                if (newPassword.Trim().Length < 6)
                    return Json(new { status = false, message = "密码长度必须大于等于6位" });

                //if (string.IsNullOrWhiteSpace(cfmPassword))
                //    return Json(new { status = false, message = "请再次输入新的登录密码" });

                //if (!EncryptionService.EncryptText(newPassword).Equals(EncryptionService.EncryptText(cfmPassword)))
                //{
                //    return Json(new { status = false, message = "两次密码输入不一致" });
                //}
                memberEntity.IsSettingPassword = true;
                memberEntity.Password = EncryptionService.EncryptText(cfmPassword);
                MemberService.Update(memberEntity);

                try
                {
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Mobile, memberEntity.Id, memberEntity.Name + "（" + memberEntity.LoginId + "）", $"用户通过邮箱找回密码，邮箱：{memberEntity.Email}", memberEntity.SerializeObject());
                }
                catch (System.Exception)
                {

                }
                return Json(new { status = true, message = "密码重置成功", url = Url.Action("Login").ToLowerInvariant() });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                SystemLogService.Insert(ex, SystemLogLevel.Error, Request.Url.ToString());
                return Json(new { status = false, message = "系统处理发生错误" });
            }
        }
        #endregion

        #region 退出登录
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("logout.cshtml")]
        public ActionResult LogOut()
        {
            return RedirectToAction("Login");
        }
        #endregion

        #region 个人中心
        /// <summary>
        /// 个人中心页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult MyCenter()
        {
            return View();
        }

        /// <summary>
        /// 请求个人中心初始化数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MyCenter(string token)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            return Json(new
            {
                status = true,
                user = new
                {
                    photo = member.HeadImage,
                    username = string.IsNullOrWhiteSpace(member.Name) ? member.LoginId : member.Name,
                    timetext = TimeTextTange.Where(r => r.Value.Contains(DateTime.Now.Hour)).Select(r => r.Key).FirstOrDefault()
                }
            });
        }
        #endregion

        #region 账户设置
        /// <summary>
        /// 账户设置页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Setting()
        {
            return View();
        }
        #endregion

        #region 设置登录密码
        [HttpGet, Route("setting/cipher.cshtml")]
        public ActionResult ForceCipher()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForceCipher(string token, string password, string password2)
        {
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return Json(new { status = false, message = "请输入密码" });
            }

            if (string.IsNullOrWhiteSpace(password2))
            {
                return Json(new { status = false, message = "请再次输入密码" });
            }
            if (password.Trim().Length < 6)
            {
                return Json(new { status = false, message = "密码长度必须大于等于6位" });
            }

            //if (!EncryptionService.EncryptText(password).Equals(EncryptionService.EncryptText(password2)))
            //{
            //    return Json(new { status = false, message = "两次密码输入不一致" });
            //}

            member.IsSettingPassword = true;
            member.Password = EncryptionService.EncryptText(password);
            MemberService.Update(member);

            string logContent = "用户设置密码成功，操作者：" + member.Name;
            try
            {
                this.ActionLogService.Insert(ActionType.Create, ActionSource.Mobile, member.Id, member.Name + "（" + member.LoginId + "）", logContent, member.SerializeObject());
            }
            catch (System.Exception)
            {

            }

            return Json(new { status = true, message = "操作已完成", url = "/" });
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Cipher()
        {
            return View();
        }

        /// <summary>
        /// 提交修改密码
        /// </summary>
        /// <param name="token"></param>
        /// <param name="orgPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="cfmPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Cipher(string token, string orgPassword, string newPassword, string cfmPassword)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            try
            {
                if (string.IsNullOrWhiteSpace(orgPassword))
                {
                    return Json(new { status = false, message = "请输入原始密码" });
                }
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    return Json(new { status = false, message = "请输入新密码" });
                }
                //if (string.IsNullOrWhiteSpace(cfmPassword))
                //{
                //    return Json(new { status = false, message = "请再次输入新密码" });
                //}
                if (newPassword.Trim().Length < 6)
                {
                    return Json(new { status = false, message = "密码长度必须大于等于6位" });
                }
                if (!EncryptionService.EncryptText(orgPassword).Equals(member.Password))
                {
                    return Json(new { status = false, message = "原密码输入不正确" });
                }
                //if (!EncryptionService.EncryptText(newPassword).Equals(EncryptionService.EncryptText(cfmPassword)))
                //{
                //    return Json(new { status = false, message = "两次密码输入不一致" });
                //}

                member.Password = EncryptionService.EncryptText(newPassword);
                MemberService.Update(member);

                string logContent = "用户修改密码成功，操作者：" + member.Name;
                try
                {
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Mobile, member.Id, member.Name + "（" + member.LoginId + "）", logContent, member.SerializeObject());
                }
                catch (System.Exception)
                {

                }
                return Json(new { status = true, message = "修改密码成功", url = Url.Action("Login").ToLowerInvariant() });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new { status = false, message = "提交发生异常" });
            }
        }
        #endregion

        #region 修改个人信息
        /// <summary>
        /// 个人信息页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public new ViewResult Profile()
        {
            return View();
        }

        /// <summary>
        /// 请求个人信息数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProfileData(string token)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            if (!Enum.IsDefined(MemberSex.Female.GetType(), member.Sex))
            {
                member.Sex = (int)MemberSex.Unknown;
            }

            List<SelectListItem> provinceList = AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == member.ProvinceId }).ToList();
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            return Json(new
            {
                status = true,
                data = new
                {
                    userphoto = member.HeadImage,
                    username = member.LoginId,
                    realname = member.Name,
                    gender = member.Sex,
                    genderText = ((MemberSex)member.Sex).GetDescription(),
                    email = member.Email,
                    member.IsCheckEmail,
                    mobile = member.Phone,
                    province = member.ProvinceId,
                    hospital = HospitalService.QueryEntity(r => r.Id == member.HospitalId)?.Name,
                    provinceList = provinceList
                }
            });
        }

        /// <summary>
        /// 提交修改个人信息数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="realname"></param>
        /// <param name="gender"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="province"></param>
        /// <param name="hospital"></param>
        /// <param name="postFile"></param>
        /// <returns></returns>
        [HttpPost]
        public new JsonResult Profile(string token, string realname, MemberSex gender, string email, string mobile, long province, string hospital, HttpPostedFileBase postFile = null)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }

                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    if (MemberService.Count(r => r.Id != member.Id && r.Phone == mobile && r.Mark > 0) > 0)
                    {
                        return Json(new { status = false, message = "此手机号已被他人使用" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            try
            {
                if (postFile != null && postFile.ContentLength > 0)
                {
                    //上传文件
                    string path = "/Content/Upload/Member";
                    UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(postFile, path, 0, UpLoadType.Image, "");
                    if (uploadResult == null || !uploadResult.State)
                    {
                        return Json(new { status = false, message = uploadResult.ErrorMessage.Replace("|", "、") });
                    }

                    member.HeadImage = path + "/" + uploadResult.Name;
                }

                if (!string.IsNullOrWhiteSpace(realname))
                {
                    member.Name = realname;
                }
                member.Sex = (int)gender;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    if (email != member.Email)
                        member.IsCheckEmail = false;
                    member.Email = email;
                }
                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    member.Phone = mobile;
                }

                #region 省份、医院
                var areaEntity = AreaService.QueryEntity(province);

                Hospital hospitalEntity = HospitalService.QueryEntity(r => r.Name.Equals(hospital)) ?? new Hospital { Id = 0 };
                if (hospitalEntity.Id <= 0)
                {
                    //填写的医院不存在，创建新的医院
                    hospitalEntity = new Hospital()
                    {
                        Id = CommonHelper.GuidToLongID,
                        Address = "",
                        Code = "",
                        ContactsTel = "",
                        ContactsUser = "",
                        Content = "",
                        MemberId = 0,
                        Name = hospital,
                        ProvinceId = areaEntity.Id,
                        ProvinceName = areaEntity.Name,
                        Sort = 10000,
                        State = true,
                        Describe = "",
                        IsTeam = false
                    };

                    HospitalService.Insert(hospitalEntity);
                }

                member.AreaId = areaEntity.Id;
                member.ProvinceId = areaEntity.Id;
                member.HospitalId = hospitalEntity.Id;
                #endregion

                MemberService.Update(member);
                try
                {
                    this.ActionLogService.Insert(ActionType.Edit, ActionSource.Mobile, member.Id, member.Name + "（" + member.LoginId + "）", $"用户修改个人信息", member.SerializeObject());
                }
                catch (System.Exception)
                {

                }

                return Json(new { status = true, message = "修改成功，正在跳转", url = Url.Action("MyCenter").ToLowerInvariant() });

            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new { status = false, message = "上传发生错误" });
            }
        }
        #endregion


        #region


        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCheckEmail(string Token, string Email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    return Json(new
                    {
                        status = false,
                        message = "请填写有效邮箱"
                    });
                }

                #region 用户信息
                string tokenText = EncryptionService.DecryptText(Token);
                long member_id = 0;
                Member member = null;
                try
                {
                    string[] sArray = Regex.Split(tokenText, "@@");
                    long.TryParse(sArray[0], out member_id);
                    if (member_id <= 0)
                    {
                        return Json(new { status = false, message = "未能获取用户信息" });
                    }
                    member = MemberService.QueryEntity(member_id);
                    if (member == null || member.Id <= 0)
                    {
                        return Json(new { status = false, message = "未能获取用户信息" });
                    }
                    if (member.Mark != 1 && member.Mark != 2)
                    {
                        return Json(new { status = false, message = "用户信息状态异常" });
                    }
                }
                catch (Exception)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                #endregion

                JWT.IJwtAlgorithm algorithm = new JWT.Algorithms.HMACSHA256Algorithm();
                JWT.IJsonSerializer serializer = new JWT.Serializers.JsonNetSerializer();
                JWT.IBase64UrlEncoder urlEncoder = new JWT.JwtBase64UrlEncoder();
                JWT.IJwtEncoder encoder = new JWT.JwtEncoder(algorithm, serializer, urlEncoder);

                long timestamp = DateHelper.UtcTimestamp;
                var payload = new Dictionary<string, object>
            {
                { "iss", "CHINET数据云" },
                { "iat",  timestamp },
                { "exp", timestamp + 600 }, // 10分钟有效
                { "id", member.Id },
                { "email", member.Email },
                {  "jti", "CHINET数据云" }
            };

                var token = encoder.Encode(payload, JWTSECRET);

                string _key = token.Split('.')[2];
                string _cacheKey = $"web.email.validate.{_key}";

                // 缓存10分钟有效
                this.cacheManager.Set(_cacheKey, token, 10);

                string validateUrl = $"{WebConfigurationManager.AppSettings["FileWebUrl"].TrimEnd('/')}/center/checkemail?token={_key}";
                string email_template_path = Server.MapPath("/Content/File/usercenter_email_validate_template.html");
                string html = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(email_template_path))
                {
                    html = sr.ReadToEnd();
                }
                html = html.Replace("$UserName$", member.Name)
                    .Replace("$ValidateLink$", validateUrl);

                //var key = "web.email." + DateTime.Now.ToFileTime().ToString() + member.Email;

                //this.cacheManager.Set(key, member.Id.ToString(), 1);

                //StringBuilder s = new StringBuilder();
                //string serviceWebUrl = ConfigHelper.GetConfigString("FileWebUrl");
                //s.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:700px;\">");
                //s.Append("<tr style=\"font-size:16px;font-weight: bold;text-indent:33px;\"><td colspan=\"3\" style=\"font-size:16px;font-weight: bold;text-indent:33px;line-height: 50px;text-indent: 0;width:700px;\">" + member.Name + "&nbsp;您好,</td></tr>");

                //s.Append("<tr><td colspan=\"3\" style=\"font-size:16px;font-weight: bold;text-indent:33px;line-height: 30px;width:700px;\">请确认邮箱验证</td></tr>");

                //s.Append("<tr><td colspan=\"3\" style=\"padding: 10px 0;text-indent: 0;font-weight: bold;\"></td></tr>");

                //s.Append("<tr style=\"width: 33px;\"><td style=\"width: 33px;\"></td><td style=\"font-size:16px;font-weight:bold;width: 200px;text-align: center;background-color:#337ab7;color:white;border-radius:4px;text-decoration:none;padding: 10px 0;\"><a style=\"color:white;text-decoration:none;font-weight:bold;font-size:16px;border-radius:4px;\" href=" + WebConfigurationManager.AppSettings["FileWebUrl"] + "Center/CheckEmail?key=" + key + ">点击确认</a></td><td style=\"width: 467px;\"></td></tr>");
                //s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">感谢!</td></tr>");

                //s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">CHINET数据云</td></tr>");
                //s.Append("</table>");

                MessageEmail email = new MessageEmail()
                {
                    //Content = s.ToString(),
                    Content = html,
                    Email = Email,
                    MemberId = member.Id,
                    MemberName = member.Name,
                    Remark = "",
                    SceneType = "",
                    SendType = 1,
                    Source = "web",
                    Status = 1,//状态：1、待发送   2：已发送   3：失败
                    Title = "CHINET邮箱验证"
                };

                //发送邮件
                string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
                string servicePassword = ConfigHelper.GetConfigString("message.email.password");
                int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
                bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
                string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
                string displayname = ConfigHelper.GetConfigString("message.email.displayname");

                bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { Email }, email.Title, email.Content);
                if (result)
                    email.Status = 2;
                else
                    email.Status = 3;

                //保存发送邮件记录
                EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);

                return Json("发送成功");
            }
            catch (Exception)
            {
                return Json("用户状态异常");

            }
        }
        #endregion

        #region 认证信息
        /// <summary>
        /// 用户认证信息页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Authentication()
        {
            return View();
        }

        /// <summary>
        /// 请求用户认证信息数据
        /// </summary>
        /// <param name="token">用户Token</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Authentication(string token)
        {
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }

            try
            {
                List<int> markValue = new List<int> { 1, 2 };
                MemberAttestation attestation = MemberAttestationService.QueryEntity(r => r.MemberId == member.Id && markValue.Contains(r.Mark));

                if (attestation == null || attestation.Id <= 0)
                {
                    // 没有认证信息
                    List<SelectListItem> provinceList = AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToList();

                    List<SelectListItem> doctorTitleList = DoctorTitleService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
                    List<SelectListItem> hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
                    List<SelectListItem> hospitalDepartmentList = this.HospitalDepartmentService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();

                    provinceList.Insert(0, new SelectListItem { Value = "0", Text = "-- 请选择 --", Selected = true });
                    doctorTitleList.Insert(0, new SelectListItem() { Value = "0", Text = "-- 请选择 --", Selected = true });
                    hospitalList.Insert(0, new SelectListItem() { Value = "0", Text = "-- 请选择 --", Selected = true });
                    hospitalDepartmentList.Insert(0, new SelectListItem() { Value = "0", Text = "-- 请选择 --", Selected = true });


                    return Json(new
                    {
                        status = true,
                        attestation = false,
                        data = new
                        {
                            Date = string.Empty,
                            Status = string.Empty,
                            AreaName = attestation?.AreaName,
                            HospitalName = attestation?.HospitalName,
                            HospitalDepartmentName = attestation?.HospitalDepartmentName,
                            DoctorTitleName = attestation?.DoctorTitleName
                        },
                        FormInitData = new
                        {
                            ProvinceList = provinceList,
                            CityList = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "-- 请选择 --", Selected = true } },
                            DoctorTitleList = doctorTitleList,
                            HospitalList = hospitalList,
                            HospitalDepartmentList = hospitalDepartmentList
                        }
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = true,
                        attestation = true,
                        data = new
                        {
                            Date = attestation.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                            Status = ((MemberAttestationStatus)attestation.Status).GetDescription(),
                            attestation.AreaName,
                            attestation.HospitalName,
                            attestation.HospitalDepartmentName,
                            attestation.DoctorTitleName
                        }
                    });
                }
            }
            catch (Exception)
            {
            }
            return Json(false);
        }

        /// <summary>
        /// 提交用户认证请求
        /// </summary>
        /// <param name="token"></param>
        /// <param name="areaId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="departmentId"></param>
        /// <param name="doctorTitleId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitAuthentication(string token, long areaId, long hospitalId, long departmentId, long doctorTitleId)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            try
            {
                //检查是否重复提交
                if (MemberAttestationService.Count(m => m.Mark > 0 && m.Status != (int)MemberAttestationStatus.Cancel && m.MemberId == member.Id) > 0)
                {
                    return Json(new { status = false, message = "您已经提交过，请勿重复提交" });
                }

                var city = this.AreaService.QueryEntity(areaId);
                if (city == null || city.Id <= 0) return Json(new { status = false, message = "选择的城市不存在，请刷新重试" });

                var hospital = this.HospitalService.QueryEntity(hospitalId);
                if (hospital == null || hospital.Id <= 0) return Json(new { status = false, message = "选择的医院不存在，请刷新重试" });

                var department = this.HospitalDepartmentService.QueryEntity(departmentId);
                if (department == null || department.Id <= 0) return Json(new { status = false, message = "选择的所属科室不存在，请刷新重试" });

                var title = this.DoctorTitleService.QueryEntity(doctorTitleId);
                if (title == null || title.Id <= 0) return Json(new { status = false, message = "选择的医生职称不存在，请刷新重试" });

                MemberAttestation entity = new MemberAttestation
                {
                    AreaId = city.Id,
                    AreaName = city.Name,
                    DoctorTitleId = title.Id,
                    DoctorTitleName = title.Name,
                    HospitalDepartmentId = department.Id,
                    HospitalDepartmentName = department.Name,
                    HospitalId = hospital.Id,
                    HospitalName = hospital.Name,
                    Status = (int)MemberAttestationStatus.WaitCheck,
                    MemberId = member.Id,
                    MemberName = member.Name
                };

                this.MemberAttestationService.Insert(entity);

                string logContent = "用户提交建议反馈成功，反馈者：" + entity.MemberName;
                try
                {
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Mobile, entity.MemberId, entity.MemberName + "（" + member.LoginId + "）", logContent, entity.SerializeObject());
                }
                catch (System.Exception)
                {

                }

                return Json(new { status = true, message = "提交认证申请成功，请等待审核", url = Url.Action("MyCenter").ToLowerInvariant() });

            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new { status = false, message = "提交发生异常" });
            }

        }

        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DistrictList(long parentId)
        {
            List<SelectListItem> provinceList = AreaService.QueryByParentId(parentId).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToList();
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "-- 请选择 --", Selected = true });
            return Json(provinceList);
        }
        #endregion

        #region 建议反馈
        /// <summary>
        /// 建议/反馈页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Feedback()
        {
            return View();
        }

        /// <summary>
        /// 提交建议/反馈
        /// </summary>
        /// <param name="token"></param>
        /// <param name="title"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Feedback(string token, string title, string mobile, string email, string content)
        {
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }

            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return Json(new { status = false, message = "必须填写反馈的标题" });
                }

                Regex regex = new Regex(ConfigHelper.GetConfigString("regex.mobile"));
                if (string.IsNullOrWhiteSpace(mobile) || !regex.IsMatch(mobile))
                {
                    return Json(new { status = false, message = "请输入正确的手机号码" });
                }

                Feedback entity = new Feedback
                {
                    Content = string.IsNullOrWhiteSpace(content) ? "" : content,
                    Describe = Newtonsoft.Json.JsonConvert.SerializeObject(new { Mobile = mobile, Email = email }),
                    Name = title,
                    MemberId = member.Id,
                    MemberName = member.Name,
                    Status = false
                };

                FeedbackService.Insert(entity);

                string logContent = "用户提交建议反馈成功，反馈者：" + entity.MemberName;
                try
                {
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Mobile, entity.MemberId, entity.MemberName + "（" + member.LoginId + "）", logContent, entity.SerializeObject());
                }
                catch (System.Exception)
                {

                }

                // 发送邮箱
                FeedbackEmail(entity, member, mobile, email);

                return Json(new { status = true, message = "提交成功，感谢您的反馈", url = Url.Action("MyCenter").ToLowerInvariant() });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new { status = false, message = "提交发生异常" });
            }

        }

        private void FeedbackEmail(Feedback feedback, Member user, string inputMobile, string inputEmail)
        {
            string htmlStyle = @"<style>
                                table {
                                    background-color: transparent;
                                    border-spacing: 0;
                                    border-collapse: collapse;
                                    font-size: 14px;
                                }

                                .table {
                                    width: 100%;
                                    max-width: 100%;
                                    margin-bottom: 20px;
                                }

                                .table-bordered {
                                    border: 1px solid #EBEBEB;
                                }

                                    .table-bordered > thead > tr > th,
                                    .table-bordered > thead > tr > td {
                                        background-color: #F5F5F6;
                                        border-bottom-width: 1px;
                                    }

                                    .table-bordered > thead > tr > th,
                                    .table-bordered > tbody > tr > th,
                                    .table-bordered > tfoot > tr > th,
                                    .table-bordered > thead > tr > td,
                                    .table-bordered > tbody > tr > td,
                                    .table-bordered > tfoot > tr > td {
                                        border: 1px solid #e7e7e7;
                                    }

                                .table > thead > tr > th {
                                    border-bottom: 1px solid #DDDDDD;
                                    vertical-align: bottom;
                                }

                                .table > thead > tr > th,
                                .table > tbody > tr > th,
                                .table > tfoot > tr > th,
                                .table > thead > tr > td,
                                .table > tbody > tr > td,
                                .table > tfoot > tr > td {
                                    border-top: 1px solid #e7eaec;
                                    line-height: 1.42857;
                                    padding: 8px;
                                    vertical-align: middle;
                                }

                                .w80px {
                                    width: 80px;
                                }
                            </style>";
            string htmlBody = $@"<table class='table table-bordered\'>
                                <tbody>
                                    <tr>
                                        <td class='w80px'>反&ensp;馈&ensp;者：</td>
                                        <td>{user.Name}</td>
                                    </tr>
                                    <tr>
                                        <td>手&ensp;机&ensp;号：</td>
                                        <td>{inputMobile}</td>
                                    </tr>
                                    <tr>
                                        <td>邮&emsp;&emsp;箱：</td>
                                        <td>{inputEmail}</td>
                                    </tr>
                                    <tr>
                                        <td>反馈时间：</td>
                                        <td>{feedback.InsertTime.ToString("yyyy-MM-dd HH:mm:ss")}</td>
                                    </tr>
                                    <tr>
                                        <td>反馈标题：</td>
                                        <td>{feedback.Name}</td>
                                    </tr>
                                    <tr>
                                        <td>反馈内容：</td>
                                        <td>{feedback.Content}</td>
                                    </tr>
                                </tbody>
                            </table>";

            string noticeEmails = settingService.QueryValue<string>("web.notice.email");
            List<MessageEmail> emails = new List<MessageEmail>();
            if (string.IsNullOrWhiteSpace(noticeEmails))
            {
                emails.Add(new MessageEmail()
                {
                    Id = CommonHelper.GuidToLongID,
                    Content = htmlStyle + htmlBody,
                    Email = "chinets@ccast.com.cn",
                    MemberId = user.Id,
                    MemberName = user.Name,
                    Remark = "",
                    SceneType = "CHINETS数据云 建议反馈",
                    SendType = 1,
                    Source = "web",
                    Status = 1,//状态：1、待发送   2：已发送   3：失败
                    Title = "CHINET用户提交意见反馈"
                });
            }
            else
            {
                noticeEmails.Split(',').ToList().ForEach(item =>
                {
                    // 验证是否邮箱
                    if (Regex.IsMatch(item, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                    {
                        emails.Add(new MessageEmail()
                        {
                            Id = CommonHelper.GuidToLongID,
                            Content = htmlStyle + htmlBody,
                            Email = item,
                            MemberId = user.Id,
                            MemberName = user.Name,
                            Remark = "",
                            SceneType = "CHINETS数据云 建议反馈",
                            SendType = 1,
                            Source = "web",
                            Status = 1,//状态：1、待发送   2：已发送   3：失败
                            Title = "CHINET用户提交意见反馈"
                        });
                    }
                });
            }

            //发送邮件
            string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
            string servicePassword = ConfigHelper.GetConfigString("message.email.password");
            int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
            bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
            string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
            string displayname = ConfigHelper.GetConfigString("message.email.displayname");

            foreach (var item in emails)
            {
                try
                {
                    bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL)
                        .WebMailSend(new string[] { item.Email }, item.Title, item.Content);
                    if (result)
                    {
                        item.Status = 2;
                    }
                    else
                    {
                        item.Status = 3;
                    }
                }
                catch (Exception e)
                {
                    item.Remark = $"发送邮件发生异常，异常信息：{e.Message}";
                }
            }


            //保存发送邮件记录
            EmailService.Insert(emails);
        }
        #endregion

        #region 数据上传记录
        /// <summary>
        /// 上传数据记录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult MedicineList()
        {
            return View();
        }

        /// <summary>
        /// 分页请求上传记录
        /// </summary>
        /// <param name="token">用户Token</param>
        /// <param name="page">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MedicineList(string token, int page = 1, int pageSize = 15)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            Dictionary<string, List<int>> quarterConfig = new Dictionary<string, List<int>>
            {
                ["全年"] = new List<int> { 5 },
                ["上半年(1月-6月)"] = new List<int> { 1, 2, 6 },
                ["下半年(7月-12月)"] = new List<int> { 3, 4, 7 }
            };

            var pageList = MedicalDataService.Query(member.Id, 0, 0, null).ToPagedList(page, pageSize);
            List<dynamic> data = new List<dynamic>();
            foreach (var item in pageList)
            {
                if (!Enum.IsDefined(MedicalDataStatusEnum.Invalid.GetType(), item.Status))
                {
                    item.Status = (int)MedicalDataStatusEnum.Invalid;
                }

                var uploadResult = item.UploadMessage.DeserializeObject<UploadMedicalResult>();
                if (uploadResult != null && uploadResult.BaseMessage != null)
                {
                    item.FileName = uploadResult.BaseMessage.FileName;
                }

                data.Add(new
                {
                    id = item.Id.ToString(),
                    filename = item.FileName,
                    quarter = $"{item.Year}年{quarterConfig.Where(r => r.Value.Contains(item.Quarter)).Select(r => r.Key).FirstOrDefault()}",
                    datetime = item.InsertTime.ToString("yyyy/MM/dd HH:mm"),
                    valid = ((MedicalDataStatusEnum)item.Status) == MedicalDataStatusEnum.Effective,
                    invalid = ((MedicalDataStatusEnum)item.Status) == MedicalDataStatusEnum.Invalid,
                    statusName = ((MedicalDataStatusEnum)item.Status).GetDescription()
                });
            }

            return Json(new
            {
                status = true,
                data = data,
                nextPage = page + 1,
                existNextPage = pageList.TotalPageCount > page
            });
        }
        #endregion
    }
}