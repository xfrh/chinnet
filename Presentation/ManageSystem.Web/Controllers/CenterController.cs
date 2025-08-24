using ManageSystem.Core.Caching;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.Orders;
using ManageSystem.Core.Domain.Research;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Configuration;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Meetings;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.Orders;
using ManageSystem.Services.Researches;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Medicine;
using ManageSystem.Web.Models.Meetings;
using ManageSystem.Web.Models.Members;
using ManageSystem.Web.Models.Orders;
using ManageSystem.Web.Models.Researches;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class CenterController : WebBaseController
    {
        private readonly IMeetingService MeetingService;
        private readonly IMeetingCollectService MeetingCollectService;
        private readonly IMeetingApplyService MeetingApplyService;
        private readonly IMeetingCommentService MeetingCommentService;
        private readonly IAreaService AreaService;
        private readonly IResearchService ResearchService;
        private readonly IResearchTypeService ResearchTypeService;
        private readonly IResearchViewService ResearchViewService;
        private readonly IResearchCollectService ResearchCollectService;
        private readonly IResearchApplyService ResearchApplyService;
        private readonly IResearchCommentService ResearchCommentService;
        private readonly IMemberService MemberService;
        private readonly IMemberAddressService MemberAddressService;
        private readonly IEncryptionService encryptionService;
        private readonly IMedicalDataService MedicalDataService;
        private readonly IOrderService OrderService;
        private readonly IOrderItemService OrderItemService;
        private readonly IOrderAddressService OrderAddressService;
        private readonly IMemberIntegralLogService MemberIntegralLogService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly IDoctorTitleService DoctorTitleService;
        private readonly IHospitalService HospitalService;
        private readonly IMemberAttestationService MemberAttestationService;
        private readonly IFeedbackService FeedbackService;
        private readonly ICacheManager cacheManager;
        private readonly IMedicalDataProjectService medicalDataProjectService;

        private const string JWTSECRET = "com.chinets.wiaWQiOjQ4MTY3OTM1MTY5NjA0MDg";

        public CenterController(
                     IMeetingService _meetingService,
                     IMeetingCollectService _meetingCollectService,
                     IMeetingCommentService _meetingCommentService,
                     IMeetingApplyService _meetingApplyService,
                     IResearchTypeService _researchTypeService,
                     IResearchService _researchService,
                     IAreaService _areaService,
                     IResearchViewService _researchViewService,
                     IResearchCollectService _researchCollectService,
                     IResearchApplyService _researchApplyService,
                     IResearchCommentService _researchCommentService,
                     IEncryptionService _encryptionService,
                     IMemberService _memberService,
                     IMedicalDataService _medicalDataService,
                     IOrderService _orderService,
                     IOrderItemService _orderItemService,
                     IOrderAddressService _orderAddressService,
                     IMemberAddressService _memberAddressService,
                     IMemberIntegralLogService _memberIntegralLogService,
                     IHospitalDepartmentService _hospitalDepartmentService,
                     IDoctorTitleService _doctorTitleService,
                     IHospitalService _hospitalService,
                     IMemberAttestationService _memberAttestationService,
                     IFeedbackService _feedbackService,
                     ICacheManager _cacheManager,
                     IMedicalDataProjectService _medicalDataProjectService
                  )
        {
            this.MeetingService = _meetingService;
            this.MeetingCollectService = _meetingCollectService;
            this.MeetingApplyService = _meetingApplyService;
            this.ResearchService = _researchService;
            this.ResearchViewService = _researchViewService;
            this.ResearchCollectService = _researchCollectService;
            this.ResearchApplyService = _researchApplyService;
            this.ResearchCommentService = _researchCommentService;
            this.AreaService = _areaService;
            this.MeetingCommentService = _meetingCommentService;
            this.MemberService = _memberService;
            this.ResearchTypeService = _researchTypeService;
            this.encryptionService = _encryptionService;
            this.MedicalDataService = _medicalDataService;
            this.OrderService = _orderService;
            this.OrderItemService = _orderItemService;
            this.OrderAddressService = _orderAddressService;
            this.MemberAddressService = _memberAddressService;
            this.MemberIntegralLogService = _memberIntegralLogService;
            this.HospitalDepartmentService = _hospitalDepartmentService;
            this.HospitalService = _hospitalService;
            this.DoctorTitleService = _doctorTitleService;
            this.MemberAttestationService = _memberAttestationService;
            this.FeedbackService = _feedbackService;
            this.cacheManager = _cacheManager;
            this.medicalDataProjectService = _medicalDataProjectService;
        }

        #region 个人中心

        public ActionResult Index1()
        {
            var member = base.LoginUserinfo;
            var userinfo = this.MemberService.QueryEntity(member.Id);
            List<SelectListItem> provinceList = AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            List<SelectListItem> hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == userinfo.HospitalId }; }).ToList();
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            CenterIndexMemberModel model = new CenterIndexMemberModel()
            {
                Email = userinfo.Email,
                IsCheckEmail = userinfo.IsCheckEmail,
                LoginId = userinfo.LoginId,
                Id = userinfo.Id,
                Name = userinfo.Name,
                NickName = userinfo.NickName,
                Password = "",
                Phone = userinfo.Phone,
                Sex = userinfo.Sex,
                IntegralAmount = userinfo.IntegralAmount,
                HospitalId = userinfo.HospitalId,
                HospitalList = hospitalList,
                HospitalName = HospitalService.QueryEntity(userinfo.HospitalId)?.Name,
                ProvinceId = userinfo.ProvinceId,
                ProvinceList = provinceList,
                ProjectUploadItem = userinfo.ProjectUploadItem
            };
            ViewBag.HospitalId = userinfo.HospitalId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index1(CenterIndexMemberModel model, HttpPostedFileBase uploadFilePath)
        {
            var member = base.LoginUserinfo;
            var entity = this.MemberService.QueryEntity(member.Id);

            if (!string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.NewPassword2))
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                    ModelState.AddModelError("Password", "原密码不能为空");
                if (!string.IsNullOrWhiteSpace(model.Password) && model.Password.Length < 6)
                    ModelState.AddModelError("Password", "原密码长度必须大于等于6位");
                if (string.IsNullOrWhiteSpace(model.NewPassword))
                    ModelState.AddModelError("NewPassword", "新密码不能为空");
                if (!string.IsNullOrWhiteSpace(model.NewPassword) && model.NewPassword.Length < 6)
                    ModelState.AddModelError("NewPassword", "新密码长度必须大于等于6位");
                if (string.IsNullOrWhiteSpace(model.NewPassword2))
                    ModelState.AddModelError("NewPassword2", "确定密码不能为空");
                if (!string.IsNullOrWhiteSpace(model.NewPassword2) && model.NewPassword2.Length < 6)
                    ModelState.AddModelError("NewPassword2", "确定密码长度必须大于等于6位");
                if (!string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrWhiteSpace(model.NewPassword2) && !model.NewPassword.Equals(model.NewPassword2))
                    ModelState.AddModelError("NewPassword2", "确定密码和新密码不相等");

                var oldMd5 = this.encryptionService.EncryptText(model.Password);
                if (!entity.Password.Equals(oldMd5))
                {
                    ModelState.AddModelError("Password", "原始密码不正确");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                if (MemberService.Count(r => r.Id != member.Id && (r.Phone == model.Phone || r.LoginId == model.Phone) && r.Mark > 0) > 0)
                {
                    ModelState.AddModelError("Phone", "此手机号已被他人使用");
                }
            }

            var areaEntity = AreaService.QueryEntity(model.ProvinceId);

            //Hospital hospitalEntity = HospitalService.QueryEntity(r => r.Name.Equals(model.HospitalName)) ?? new Hospital { Id = 0 };
            Hospital hospitalEntity = new Hospital();
            //当医院id为空但医院名称不为空的情况下添加医院
            if (model.HospitalId == 0 && model.HospitalName != null)
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
                    Name = model.HospitalName,
                    ProvinceId = areaEntity.Id,
                    ProvinceName = areaEntity.Name,
                    Sort = 10000,
                    State = true,
                    Describe = "",
                    IsTeam = false
                };
                HospitalService.Insert(hospitalEntity);
            }


            if (!string.IsNullOrWhiteSpace(model.Password)&&!string.IsNullOrEmpty(model.NewPassword)&&!string.IsNullOrEmpty(model.NewPassword2))
            {
                var oldMd5 = this.encryptionService.EncryptText(model.Password);
                if (!entity.Password.Equals(oldMd5))
                {
                    ModelState.AddModelError("Password", "原始密码不正确");
                }
                entity.Password = oldMd5;

                if (!string.IsNullOrWhiteSpace(model.NewPassword2) && this.encryptionService.EncryptText(model.NewPassword).Equals(this.encryptionService.EncryptText(model.NewPassword2)))
                {
                    entity.Password = this.encryptionService.EncryptText(model.NewPassword2);
                }
            }

            if (ModelState.IsValid)
            {
                //entity.Name = model.Name;
                entity.Sex = model.Sex;
                entity.Phone = model.Phone;
                if (entity.Email != model.Email)
                    entity.IsCheckEmail = false;
                entity.Email = model.Email;
                entity.AreaId = areaEntity.Id;
                entity.ProvinceId = areaEntity.Id;
                //当医院id为空但医院名称不为空的时候绑定根据医院名称新增的医院id
                if (model.HospitalId == 0 && model.HospitalName != null)
                {
                    entity.HospitalId = hospitalEntity.Id;
                }
                else
                {
                    //帮顶医院id
                    entity.HospitalId = model.HospitalId;
                }

                model.LoginId = entity.LoginId;

                if (string.IsNullOrWhiteSpace(entity.MedicineEmail) && !string.IsNullOrWhiteSpace(model.Email))
                    entity.MedicineEmail = model.Email;

                if (uploadFilePath != null && uploadFilePath.ContentLength > 0)
                {
                    //上传文件
                    string path = "/Content/Upload/Member";
                    UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(uploadFilePath, path, 0, UpLoadType.Image, "");
                    if (uploadResult == null || !uploadResult.State) throw new Exception(uploadResult.ErrorMessage.Replace("|", "、"));

                    if (System.IO.File.Exists(Server.MapPath($"{path}/{uploadResult.Name}")))
                    {
                        entity.HeadImage = $"{path}/{uploadResult.Name}";
                    }
                }
                if (string.IsNullOrEmpty(model.Email))
                    entity.IsCheckEmail = true;
                this.MemberService.Update(entity);
                this.HttpContext.Session["LoginUserinfoSession"] = "";
                base.InsetActionLog(ActionType.Edit, "修改个人信息", entity.SerializeObject());
                base.SuccessNotification("修改成功");

                return this.RedirectToAction("Index1","Center");
            }
            else
            {
                if (ModelState.Values.Any(r => r.Errors.Count > 0))
                {
                    base.ErrorNotification(ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault().Errors.FirstOrDefault()?.ErrorMessage);
                }
                else
                {
                    base.ErrorNotification("信息验证不通过,修改失败");
                }
            }

            List<SelectListItem> provinceList = AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == member.ProvinceId }).ToList();
            List<SelectListItem> hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == member.HospitalId }; }).ToList();
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            model.HospitalList = hospitalList;
            model.ProvinceList = provinceList;
            model.IsCheckEmail = entity.IsCheckEmail;
            //return View("Index1",model);
            return this.RedirectToAction("Index1", "Center");
        }

        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCheckEmail(string Email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    return Json(new
                    {
                        status = false,
                        message = $"请填写有效邮箱"
                    });
                }

                Member member = this.MemberService.QueryEntity(base.LoginUserinfo.Id);

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
                Log4Helper.Debug($"cachekey:{_cacheKey},token:{token}");

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

                MessageEmail email = new MessageEmail()
                {
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

                return Json(new
                {
                    status = true,
                    message = $"CHINET邮箱验证通知已发送至\"{Email}\"，请留意查收邮件!"
                });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new
                {
                    status = false,
                    message = $"用户状态异常"
                });

            }
        }

        #endregion

        #region 邮箱验证
        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckEmail(string token)
        {
            Tuple<bool, string> tuple;
            try
            {
                string _cacheKey = $"web.email.validate.{token}";
                if (cacheManager.IsSet(_cacheKey))
                {
                    string jwtToken = cacheManager.Get<string>(_cacheKey);
                    JWT.IJsonSerializer serializer = new JWT.Serializers.JsonNetSerializer();
                    JWT.IDateTimeProvider provider = new JWT.UtcDateTimeProvider();
                    JWT.IJwtValidator validator = new JWT.JwtValidator(serializer, provider);
                    JWT.IBase64UrlEncoder urlEncoder = new JWT.JwtBase64UrlEncoder();
                    JWT.IJwtDecoder decoder = new JWT.JwtDecoder(serializer, validator, urlEncoder);

                    var strJson = decoder.Decode(jwtToken, JWTSECRET, verify: true);//token为之前生成的字符串
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strJson);
                    long id = json.id;
                    string email = json.email;

                    var entity = this.MemberService.QueryEntity(id);
                    entity.IsCheckEmail = true;
                    this.MemberService.Update(entity);
                    tuple = new Tuple<bool, string>(true, "恭喜您，您的邮箱已经验证成功！");
                    cacheManager.Remove(_cacheKey);
                }
                else
                {
                    tuple = new Tuple<bool, string>(false, "验证链接已失效，请重新发起验证！");
                }
            }
            catch (JWT.TokenExpiredException)
            {
                tuple = new Tuple<bool, string>(false, "验证链接已失效，请重新发起验证！");
            }
            catch (JWT.SignatureVerificationException)
            {
                tuple = new Tuple<bool, string>(false, "验证链接已失效，请重新发起验证！");
            }
            catch (Exception)
            {
                tuple = new Tuple<bool, string>(false, "验证链接已失效，请重新发起验证！");
            }

            return View(tuple);
        }


        #endregion

        #region 认证信息
        /// <summary>
        /// 认证信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Attestation()
        {
            return View(this.SetAttestationData());
        }

        /// <summary>
        /// 初始化认证信息
        /// </summary>
        /// <returns></returns>
        private MemberAttestationModel SetAttestationData()
        {
            var member = base.LoginUserinfo;
            MemberAttestationModel model = new MemberAttestationModel();
            model.DoctorTitleList = this.DoctorTitleService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.HospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.HospitalDepartmentList = this.HospitalDepartmentService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();

            model.DoctorTitleList.Insert(0, new SelectListItem() { Value = "0", Text = "-- 请选择 --", Selected = true });
            model.HospitalList.Insert(0, new SelectListItem() { Value = "0", Text = "-- 请选择 --", Selected = true });
            model.HospitalDepartmentList.Insert(0, new SelectListItem() { Value = "0", Text = "-- 请选择 --", Selected = true });

            //是否已经提交过了
            model.IsSuccess = this.MemberAttestationService.Count(m => m.Mark > 0 && m.Status != (int)MemberAttestationStatus.Cancel && m.MemberId == member.Id) > 0;

            if (model.IsSuccess)
            {
                var entity = this.MemberAttestationService.QueryEntity(m => m.Mark > 0 && m.Status != (int)MemberAttestationStatus.Cancel && m.MemberId == member.Id);
                if (entity != null && entity.Id > 0)
                {
                    model.Id = entity.Id;
                    model.AreaName = entity.AreaName;
                    model.DoctorTitleName = entity.DoctorTitleName;
                    model.HospitalDepartmentName = entity.HospitalDepartmentName;
                    model.HospitalName = entity.HospitalName;
                    model.Status = entity.Status;
                    model.StatusName = ((MemberAttestationStatus)model.Status).GetDescription();
                }
            }

            return model;
        }

        /// <summary>
        /// 提交认证信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Attestation(MemberAttestationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = base.LoginUserinfo;

                    //检查是否重复提交
                    if (this.MemberAttestationService.Count(m => m.Mark > 0 && m.Status != (int)MemberAttestationStatus.Cancel && m.MemberId == member.Id) > 0)
                        throw new Exception("您已经提交过，请勿重复提交");

                    var city = this.AreaService.QueryEntity(model.AreaId);
                    if (city == null || city.Id <= 0) throw new Exception("选择的城市不存在，请刷新重试");

                    var hospital = this.HospitalService.QueryEntity(model.HospitalId);
                    if (hospital == null || hospital.Id <= 0) throw new Exception("选择的医院不存在，请刷新重试");

                    var department = this.HospitalDepartmentService.QueryEntity(model.HospitalDepartmentId);
                    if (department == null || department.Id <= 0) throw new Exception("选择的所属科室不存在，请刷新重试");

                    var title = this.DoctorTitleService.QueryEntity(model.DoctorTitleId);
                    if (title == null || title.Id <= 0) throw new Exception("选择的医生职称不存在，请刷新重试");

                    MemberAttestation entity = new MemberAttestation();
                    entity.AreaId = city.Id;
                    entity.AreaName = city.Name;
                    entity.DoctorTitleId = title.Id;
                    entity.DoctorTitleName = title.Name;
                    entity.HospitalDepartmentId = department.Id;
                    entity.HospitalDepartmentName = department.Name;
                    entity.HospitalId = hospital.Id;
                    entity.HospitalName = hospital.Name;
                    entity.Status = (int)MemberAttestationStatus.WaitCheck;
                    entity.MemberId = member.Id;
                    entity.MemberName = member.Name;

                    this.MemberAttestationService.Insert(entity);

                    string logContent = "会员提交认证申请，申请人：" + base.LoginUserinfo.Name;
                    base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                    base.SuccessNotification("提交认证申请成功，请等待审核");

                    return this.RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    base.ErrorNotification(ex.Message);
                }
            }
            return View(this.SetAttestationData());
        }
        #endregion

        #region 邀请会员

        /// <summary>
        /// 邀请会员
        /// </summary>
        /// <returns></returns>
        public ActionResult Invite()
        {
            CenterInviteModel model = new CenterInviteModel();
            var member = this.MemberService.QueryEntity(base.LoginUserinfo.Id);

            if (member.Type != (int)MemberType.Authentication)
            {
                //this.ViewBag.ErrorMessage = "";
                base.ErrorNotification("只有认证用户才能邀请");
                return this.RedirectToAction("Index");
            }

            if (member != null && member.Id > 0 && member.Mark > 0)
            {
                model.InviteCode = member.InviteCode;
                model.InviteUrl = WebSettingService.GetWebUrl() + "/Home/Regist?code=" + model.InviteCode;
            }

            return this.View(model);
        }

        /// <summary>
        /// 邀请记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult InviteList(int pageIndex = 1)
        {
            var member = this.MemberService.QueryEntity(base.LoginUserinfo.Id);
            if (member.Type != (int)MemberType.Authentication)
            {
                base.ErrorNotification("只有认证用户才能邀请");
                return this.RedirectToAction("Index");
            }

            if (member == null || member.Id < 0 && member.Mark <= 0 || string.IsNullOrWhiteSpace(member.InviteCode))
                return this.View();

            var data = this.MemberService.QueryByInviteCode(member.InviteCode)
                .Select(x => new CenterInviteListModel()
                {
                    MemberId = x.Id,
                    MemberLoginId = x.LoginId,
                    MemberName = x.Name,
                    InsertTime = x.InsertTime
                }).ToPagedList<CenterInviteListModel>(pageIndex, MvcPagerExtensions.PageSize);

            return this.View(data);
        }

        #endregion

        #region 我的收藏

        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <returns></returns>
        public ActionResult Collection()
        {
            return View();
        }

        /// <summary>
        /// 我的收藏 信息动态
        /// </summ 
        public ActionResult MeetingCollection(int pageIndex = 1)
        {
            try
            {
                var list = this.MeetingCollectService.Query(base.LoginUserinfo.Id);
                var data = list.Select(x => new CenterMeetingCollectionModel()
                {
                    Id = x.Id,
                    MeetingId = x.MeetingId,
                    InsertTime = x.InsertTime,
                    MeetingName = x.MeetingName
                }).ToPagedList<CenterMeetingCollectionModel>(pageIndex, MvcPagerExtensions.PageSize);

                List<long> meetingId = data.Select(m => m.MeetingId).ToList();
                var meetingList = this.MeetingService.Query(m => meetingId.Contains(m.Id) && m.Mark > 0);

                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.MeetingId).FirstOrDefault();
                    if (meetingTemp != null)
                        item.MeetingName = meetingTemp.Name;
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 我的收藏  信息动态  删除收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult DelelteMeetingCollection(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                var entity = this.MeetingCollectService.QueryEntity(id);
                if (entity == null || entity.Id <= 0 || entity.MemberId != this.LoginUserinfo.Id) return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在，请刷新网页重试"));

                this.MeetingCollectService.Delete(id);

                Meeting meetingEntity = this.MeetingService.QueryEntity(entity.MeetingId);
                meetingEntity.CollectCount -= 1;
                meetingEntity.CollectCount = meetingEntity.CollectCount > 0 ? meetingEntity.CollectCount : 0;
                this.MeetingService.Update(meetingEntity);

                base.InsetActionLog(ActionType.Delete, "【手动】删除信息动态收藏", entity.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！失败原因：" + ex.ToString()));
            }
        }

        /// <summary>
        /// 我的收藏 科研合作
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCollection(int pageIndex = 1)
        {
            try
            {
                var list = this.ResearchCollectService.Query(base.LoginUserinfo.Id);
                var data = list.Select(x => new CenterResearchCollectionModel()
                {
                    Id = x.Id,
                    ResearchId = x.ResearchId,
                    InsertTime = x.InsertTime,
                    ResearchName = x.ResearchName
                }).ToPagedList<CenterResearchCollectionModel>(pageIndex, MvcPagerExtensions.PageSize);

                List<long> meetingId = data.Select(m => m.ResearchId).ToList();
                var meetingList = this.ResearchService.Query(m => meetingId.Contains(m.Id) && m.Mark > 0);

                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.ResearchId).FirstOrDefault();
                    if (meetingTemp != null)
                        item.ResearchName = meetingTemp.Name;
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// 我的收藏  科研合作  删除收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult DelelteResearchCollection(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                var entity = this.ResearchCollectService.QueryEntity(id);
                if (entity == null || entity.Id <= 0 || entity.MemberId != this.LoginUserinfo.Id) return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在，请刷新网页重试"));

                this.ResearchCollectService.Delete(id);

                base.InsetActionLog(ActionType.Delete, "【手动】删除科研合作收藏", entity.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        #endregion

        #region 我的申请

        /// <summary>
        /// 我的申请  信息动态
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingApply(int pageIndex = 1)
        {
            try
            {
                var list = this.MeetingApplyService.Query(base.LoginUserinfo.Id);
                var data = list.Select(x => new CenterMeetingApplyModel()
                {
                    Id = x.Id,
                    MeetingId = x.MeetingId,
                    InsertTime = x.InsertTime,
                    MeetingName = x.MeetingName
                }).ToPagedList<CenterMeetingApplyModel>(pageIndex, MvcPagerExtensions.PageSize);

                List<long> meetingId = data.Select(m => m.MeetingId).ToList();
                var meetingList = this.MeetingService.Query(m => meetingId.Contains(m.Id) && m.Mark > 0);

                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.MeetingId).FirstOrDefault();
                    if (meetingTemp != null)
                        item.MeetingName = meetingTemp.Name;
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 我的申请  信息动态  取消报名申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult DelelteMeetingApply(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));

                var user = base.LoginUserinfo;
                this.MeetingApplyService.Cancel(user.Id, id, base.GetUserFullName());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        /// <summary>
        /// 我的申请 科研合作
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchApply(int pageIndex = 1)
        {
            try
            {
                var list = this.ResearchApplyService.Query(base.LoginUserinfo.Id);
                var data = list.Select(x => new CenterResearchApplyModel()
                {
                    Id = x.Id,
                    ResearchId = x.ResearchId,
                    InsertTime = x.InsertTime,
                    ResearchName = x.ResearchName,
                    Status = x.Status
                }).ToPagedList<CenterResearchApplyModel>(pageIndex, MvcPagerExtensions.PageSize);

                List<long> meetingId = data.Select(m => m.ResearchId).ToList();
                var meetingList = this.ResearchService.Query(m => meetingId.Contains(m.Id) && m.Mark > 0);

                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.ResearchId).FirstOrDefault();
                    if (meetingTemp != null)
                        item.ResearchName = meetingTemp.Name;

                    if (meetingTemp != null)
                    {
                        item.ResearchName = meetingTemp.Name;
                        if (meetingTemp.StartTime < DateTime.Now)
                            item.StatusName = "已结束";
                    }
                    else
                        item.StatusName = "已删除";

                    if (string.IsNullOrWhiteSpace(item.StatusName))
                        item.StatusName = ((ResearchApplyStatusEnum)item.Status).GetDescription();
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 我的申请  科研合作  取消报名申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult DelelteResearchApply(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));

                var user = base.LoginUserinfo;
                this.ResearchApplyService.Cancel(user.Id, id, base.GetUserFullName());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        #endregion

        #region 我发布的活动

        /// <summary>
        /// 我发布的活动  信息动态
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingList(int pageIndex = 1)
        {
            var user = base.LoginUserinfo;

            var list = this.MeetingService.Query(user.Id);
            var data = list.Select(x => new MeetingModel()
            {
                Id = x.Id,
                CoverImage = x.CoverImage,
                Name = x.Name,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                AreaName = x.AreaName,
                InsertTime = x.InsertTime,
                MemberName = x.MemberName,
                ApplyCount = x.ApplyCount,
                ViewCount = x.ViewCount,
                Status = x.Status
            }).ToPagedList<MeetingModel>(pageIndex, MvcPagerExtensions.PageSize);


            List<long> areaIdArray = data.Select(m => m.AreaId).ToList();
            List<long> memberIdArray = data.Select(m => m.MemberId).ToList();
            var areaList = this.AreaService.Query(m => areaIdArray.Contains(m.Id));

            foreach (var item in data)
            {
                var areaTemp = areaList.Where(m => m.Id == item.AreaId).FirstOrDefault();
                item.AreaName = (areaTemp == null || areaTemp.Id <= 0) ? item.AreaName : areaTemp.Name;

                switch ((MeetingStatusEnum)item.Status)
                {
                    case MeetingStatusEnum.Wait:
                        item.StatusName = "等待审核";
                        break;
                    case MeetingStatusEnum.Finish:
                        item.StatusName = (DateTime.Now > item.EndTime) ? "已结束" : "报名中";
                        break;
                    case MeetingStatusEnum.Stop:
                        item.StatusName = "已停用";
                        break;
                }
            }

            return this.View(data);
        }

        /// <summary>
        /// 我发布的活动  信息动态  管理发布的单个活动 基础信息页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MeetingDetail(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("会议不存在");

                MeetingModel model = this.MeetingService.QueryEntity(id).ToModel();
                if (model == null || model.Id <= 0) throw new Exception("会议不存在");

                model.TypeName = ((MeetingTypeEnum)model.Type).GetDescription();

                return this.View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("MeetingList");
            }
        }

        /// <summary>
        /// 我发布的活动  信息动态  管理发布的单个活动 报名申请页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MeetingDetailApplyList(long id, string searchKey = "", int status = 0, int pageIndex = 1)
        {
            try
            {
                if (id <= 0) throw new Exception("会议不存在");

                MeetingDetailApplyModel model = new MeetingDetailApplyModel();
                model.Meeting = this.MeetingService.QueryEntity(id).ToModel();
                if (model.Meeting == null || model.Meeting.Id <= 0) throw new Exception("会议不存在");

                model.StatusList = MeetingApplyStatusEnum.Cancel.ToSelectList(false, false).ToList();
                model.StatusList.Insert(0, new SelectListItem() { Text = "报名状态", Value = "0" });

                model.SearchKey = searchKey;
                model.Status = status;

                //分页数据
                var list = this.MeetingApplyService.QueryByMeetingId(model.Meeting.Id, searchKey, status);
                var data = list.Select(x => new MeetingApplyModel()
                {
                    Id = x.Id,
                    MeetingId = x.MeetingId,
                    InsertTime = x.InsertTime,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    PayAmount = x.PayAmount,
                    ReceivedAmount = x.ReceivedAmount,
                    Status = x.Status
                }).ToPagedList<MeetingApplyModel>(pageIndex, MvcPagerExtensions.PageSize);

                foreach (var item in data)
                {
                    item.StatusName = ((MeetingApplyStatusEnum)item.Status).GetDescription();
                }

                model.ApplyPageList = data;

                return this.View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("MeetingList");
            }
        }

        /// <summary>
        /// 我发布的活动  信息动态  管理发布的单个活动 报名申请详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MeetingDetailApplyDetail(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("数据不存在");

                MeetingApplyModel model = new MeetingApplyModel();
                model = this.MeetingApplyService.QueryEntity(id).ToModel();
                if (model == null || model.Id <= 0) throw new Exception("数据不存在");
                model.StatusName = ((MeetingApplyStatusEnum)model.Status).GetDescription();

                return View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

            }

            return this.View(new MeetingApplyModel());
        }

        /// <summary>
        /// 我发布的活动  信息动态  管理发布的单个活动 评论页面
        /// </summary>
        /// <param name="id">信息动态的id</param>
        /// <param name="pageIndex">分页</param>
        /// <returns></returns>
        public ActionResult MeetingDetailCommentList(long id, int pageIndex = 1)
        {
            try
            {
                if (id <= 0) throw new Exception("会议不存在");

                MeetingDetailCommentModel model = new MeetingDetailCommentModel();
                model.Meeting = this.MeetingService.QueryEntity(id).ToModel();
                if (model.Meeting == null || model.Meeting.Id <= 0) throw new Exception("会议不存在");

                return this.View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("MeetingList");
            }
        }


        /// <summary>
        /// 我发布的活动  科研合作
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchList(int pageIndex = 1)
        {
            var user = base.LoginUserinfo;

            var list = this.ResearchService.Query(user.Id);
            var data = list.Select(x => new ResearchModel()
            {
                Id = x.Id,
                CoverImage = x.CoverImage,
                Name = x.Name,
                StartTime = x.StartTime,
                AreaName = x.AreaName,
                InsertTime = x.InsertTime,
                MemberName = x.MemberName,
                ApplyCount = x.ApplyCount,
                ViewCount = x.ViewCount,
                Status = x.Status
            }).ToPagedList<ResearchModel>(pageIndex, MvcPagerExtensions.PageSize);


            List<long> areaIdArray = data.Select(m => m.AreaId).ToList();
            List<long> memberIdArray = data.Select(m => m.MemberId).ToList();
            var areaList = this.AreaService.Query(m => areaIdArray.Contains(m.Id));

            foreach (var item in data)
            {
                var areaTemp = areaList.Where(m => m.Id == item.AreaId).FirstOrDefault();
                item.AreaName = (areaTemp == null || areaTemp.Id <= 0) ? item.AreaName : areaTemp.Name;

                switch ((ResearchStatusEnum)item.Status)
                {
                    case ResearchStatusEnum.Wait:
                        item.StatusName = "等待审核";
                        break;
                    case ResearchStatusEnum.Finish:
                        item.StatusName = "报名中";
                        break;
                    case ResearchStatusEnum.Stop:
                        item.StatusName = "已停用";
                        break;
                }
            }
            return this.View(data);
        }

        /// <summary>
        /// 我发布的活动  科研合作  管理发布的单个活动 基础信息页面
        /// </summary>
        /// <param name="id">科研合作的id</param>
        /// <returns></returns>
        public ActionResult ResearchDetail(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("科研不存在");

                ResearchModel model = this.ResearchService.QueryEntity(id).ToModel();
                if (model == null || model.Id <= 0) throw new Exception("科研不存在");

                var typeModel = this.ResearchTypeService.QueryEntity(model.ResearchTypeId);
                if (typeModel != null && typeModel.Id > 0)
                    model.ResearchTypeName = typeModel.Name;

                return this.View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("ResearchList");
            }
        }

        /// <summary>
        /// 我发布的活动  科研合作  管理发布的单个活动 报名申请页面
        /// </summary>
        /// <param name="id">科研合作的id</param>
        /// <returns></returns>
        public ActionResult ResearchDetailApplyList(long id, string searchKey = "", int status = 0, int pageIndex = 1)
        {
            try
            {
                if (id <= 0) throw new Exception("科研不存在");

                ResearchDetailApplyModel model = new ResearchDetailApplyModel();
                model.Research = this.ResearchService.QueryEntity(id).ToModel();
                if (model.Research == null || model.Research.Id <= 0) throw new Exception("科研不存在");

                model.StatusList = ResearchApplyStatusEnum.Cancel.ToSelectList(false, false).ToList();
                model.StatusList.Insert(0, new SelectListItem() { Text = "报名状态", Value = "0" });

                model.SearchKey = searchKey;
                model.Status = status;

                //分页数据
                var list = this.ResearchApplyService.QueryByMeetingId(model.Research.Id, searchKey, status);
                var data = list.Select(x => new ResearchApplyModel()
                {
                    Id = x.Id,
                    ResearchId = x.ResearchId,
                    InsertTime = x.InsertTime,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    Status = x.Status
                }).ToPagedList<ResearchApplyModel>(pageIndex, MvcPagerExtensions.PageSize);

                foreach (var item in data)
                {
                    item.StatusName = ((ResearchApplyStatusEnum)item.Status).GetDescription();
                }

                model.ApplyPageList = data;

                return this.View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("ResearchList");
            }
        }

        /// <summary>
        /// 我发布的活动  科研合作  管理发布的单个活动 报名申请详细页面
        /// </summary>
        /// <param name="id">科研合作的id</param>
        /// <returns></returns>
        public ActionResult ResearchDetailApplyDetail(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("数据不存在");

                ResearchApplyModel model = new ResearchApplyModel();
                model = this.ResearchApplyService.QueryEntity(id).ToModel();
                if (model == null || model.Id <= 0) throw new Exception("数据不存在");
                model.StatusName = ((ResearchApplyStatusEnum)model.Status).GetDescription();

                return View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

            }

            return this.View(new ResearchApplyModel());
        }


        /// <summary>
        /// 我发布的活动  科研合作  管理发布的单个活动 评论页面
        /// </summary>
        /// <param name="id">科研合作的id</param>
        /// <returns></returns>
        public ActionResult ResearchDetailCommentList(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("会议不存在");

                ResearchDetailCommentModel model = new ResearchDetailCommentModel();
                model.Research = this.ResearchService.QueryEntity(id).ToModel();
                if (model.Research == null || model.Research.Id <= 0) throw new Exception("会议不存在");

                return this.View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("ResearchList");
            }
        }


        #endregion

        #region 医学信息管理

        /// <summary>
        /// 医学数据管理
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult MedicineManage(int year = 0, int quarter = 0, string fileName = "", int status = 0, int pageIndex = 1)
        {
            CenterMedicineManageModel model = new CenterMedicineManageModel();
            int yearTemp = year == 0 ? DateTime.Now.Year : year;
            model.Year = year;
            model.Quarter = quarter;
            model.StatusList = MedicalDataStatusEnum.Effective.ToSelectList().ToList();
            model.YearList = new List<SelectListItem>() {
                 new SelectListItem() {  Text="选择年",Value="0"},
                 new SelectListItem() {  Text=(yearTemp-1)+"年",Value=(yearTemp-1).ToString()},
                 new SelectListItem() {  Text=yearTemp+"年",Value=(yearTemp).ToString()},
                 new SelectListItem() {  Text=(yearTemp+1)+"年",Value=(yearTemp+1).ToString()}
            };
            model.QuarterList = new List<SelectListItem>() {
                 new SelectListItem() {  Text="选择季度",Value="0"},
                 new SelectListItem() {  Text="上半年（1月 - 6月）",Value="6"},
                 new SelectListItem() {  Text="下半年（7月 - 12月）",Value="7"},
                 new SelectListItem() {  Text="全年",Value="5"}
            };

            var user = base.LoginUserinfo;
            var list = this.MedicalDataService.Query(user.Id, year, quarter, fileName);

            var data = list.Select(x => new CenterMedicineManageItemModel()
            {
                Id = x.Id,
                Year = x.Year,
                Quarter = x.Quarter,
                UploadMessage = x.UploadMessage,
                InsertTime = x.InsertTime,
                Status = x.Status
            }).ToPagedList<CenterMedicineManageItemModel>(pageIndex, MvcPagerExtensions.PageSize);

            model.StatusName = ((MedicalDataStatusEnum)model.Status).GetDescription();

            foreach (var item in data)
            {
                var uploadResult = item.UploadMessage.DeserializeObject<UploadMedicalResult>();
                if (uploadResult != null && uploadResult.BaseMessage != null)
                {
                    item.FileName = uploadResult.BaseMessage.FileName;
                    item.FileSize = uploadResult.BaseMessage.FileLength.ToString();
                }

                item.StatusName = ((MedicalDataStatusEnum)item.Status).GetDescription();
            }

            model.PageList = data;
            return this.View(model);
        }

        /// <summary>
        /// 医学数据管理    删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult DelelteMedicine(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                var entity = this.MedicalDataService.QueryEntity(id);

                if (entity == null || entity.Id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在，请刷新网页重试"));

                if (entity.MemberId != this.LoginUserinfo.Id) return this.Content(JsonHelper.GetBaseMessage(false, "您无权操作"));

                this.MedicalDataService.Delete(id);

                base.InsetActionLog(ActionType.Delete, "【手动】删除医学数据", entity.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        #endregion

        #region 医学类型管理

        /// <summary>
        /// 医学类型管理
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult MedicineCategoryManage(int year = 0, int quarter = 0, string fileName = "", int status = 0, int pageIndex = 1)
        {
            CenterMedicineManageModel model = new CenterMedicineManageModel();
            int yearTemp = year == 0 ? DateTime.Now.Year : year;
            model.Year = year;
            model.Quarter = quarter;
            model.StatusList = MedicalDataStatusEnum.Effective.ToSelectList().ToList();
            model.YearList = new List<SelectListItem>() {
                 new SelectListItem() {  Text="选择年",Value="0"},
                 new SelectListItem() {  Text=(yearTemp-1)+"年",Value=(yearTemp-1).ToString()},
                 new SelectListItem() {  Text=yearTemp+"年",Value=(yearTemp).ToString()},
                 new SelectListItem() {  Text=(yearTemp+1)+"年",Value=(yearTemp+1).ToString()}
            };
            model.QuarterList = new List<SelectListItem>() {
                 new SelectListItem() {  Text="选择季度",Value="0"},
                 new SelectListItem() {  Text="上半年（1月 - 6月）",Value="6"},
                 new SelectListItem() {  Text="下半年（7月 - 12月）",Value="7"},
                 new SelectListItem() {  Text="全年",Value="5"}
            };

            var user = base.LoginUserinfo;
            var list = this.MedicalDataService.Query(user.Id, year, quarter, fileName);

            var data = list.Select(x => new CenterMedicineManageItemModel()
            {
                Id = x.Id,
                Year = x.Year,
                Quarter = x.Quarter,
                UploadMessage = x.UploadMessage,
                InsertTime = x.InsertTime,
                Status = x.Status
            }).ToPagedList<CenterMedicineManageItemModel>(pageIndex, MvcPagerExtensions.PageSize);

            model.StatusName = ((MedicalDataStatusEnum)model.Status).GetDescription();

            foreach (var item in data)
            {
                var uploadResult = item.UploadMessage.DeserializeObject<UploadMedicalResult>();
                if (uploadResult != null && uploadResult.BaseMessage != null)
                {
                    item.FileName = uploadResult.BaseMessage.FileName;
                    item.FileSize = uploadResult.BaseMessage.FileLength.ToString();
                }

                item.StatusName = ((MedicalDataStatusEnum)item.Status).GetDescription();
            }

            model.PageList = data;
            return this.View(model);
        }

        /// <summary>
        /// 医学类型管理    删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult DeleteMedicineCategory(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                var entity = this.MedicalDataService.QueryEntity(id);

                if (entity == null || entity.Id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在，请刷新网页重试"));

                if (entity.MemberId != this.LoginUserinfo.Id) return this.Content(JsonHelper.GetBaseMessage(false, "您无权操作"));

                this.MedicalDataService.Delete(id);

                base.InsetActionLog(ActionType.Delete, "【手动】删除医学数据", entity.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CreateMedicineCategory(long id)
        {
            try
            {
                if (id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                var entity = this.MedicalDataService.QueryEntity(id);

                if (entity == null || entity.Id <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在，请刷新网页重试"));

                if (entity.MemberId != this.LoginUserinfo.Id) return this.Content(JsonHelper.GetBaseMessage(false, "您无权操作"));

                this.MedicalDataService.Delete(id);

                base.InsetActionLog(ActionType.Delete, "【手动】删除医学数据", entity.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        #endregion

        #region 项目数据管理

        /// <summary>
        /// 项目数据管理
        /// </summary>
        /// <param name="hospital"></param>
        /// <param name="fileName"></param>
        /// <param name="project"></param>
        /// <param name="year"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ViewResult ProjectDataManage(string hospital, string fileName, long project = 0, int year = 0, int status = 0, int pageIndex = 1)
        {
            Member user = MemberService.QueryEntity(LoginUserinfo.Id);

            ProjectDataManageManageModel model = new ProjectDataManageManageModel();
            int nowYear = DateTime.Now.Year;
            model.Year = year;
            model.FileName = fileName ?? "";
            model.Project = project;
            model.Hospital = hospital ?? "";
            model.Status = status;
            model.StatusList = MedicalDataStatusEnum.Effective.ToSelectList(true, false).ToList();
            model.StatusList.Insert(0, new SelectListItem() { Value = "0", Text = "全部状态" });
            model.YearList = new List<SelectListItem>() {
                 new SelectListItem() {  Text="选择年",Value="0"},
                 new SelectListItem() {  Text=(nowYear-1)+"年",Value=(nowYear-1).ToString()},
                 new SelectListItem() {  Text=nowYear+"年",Value=(nowYear).ToString()},
                 new SelectListItem() {  Text=(nowYear+1)+"年",Value=(nowYear+1).ToString()}
            };
            model.ProjectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "全部项目", Value = "0", Selected = project == 0 },
            };

            List<long> projectItems = new List<long>();
            try
            {
                if (user.ProjectItem != null && !string.IsNullOrWhiteSpace(user.ProjectItem))
                {
                    projectItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<long>>(user.ProjectItem);
                }
            }
            catch (Exception)
            {
                projectItems = new List<long>();
                projectItems.Add(0);
            }

            //Peng 2024-09-23
            //var projectlist = this.medicalDataProjectService.Query(m => m.Mark > 0 && projectItems.Contains(m.Id)).OrderBy(x => x.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            var projectlist = this.medicalDataProjectService.Query(m => m.Mark > 0 && projectItems.Contains(m.Id));
            if (projectlist != null && projectlist.Count > 0)
            {
                var _projectlist= this.medicalDataProjectService.Query(m => m.Mark > 0 && projectItems.Contains(m.Id)).OrderBy(x => x.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
                foreach (var item in _projectlist)
                {
                    model.ProjectList.Add(item);
                }
            }

            var list = this.MedicalDataService.Query(projectItems, hospital, fileName, project, year, status);
            if(list != null && list.Count() > 0)
            {
                model.PageList = list.Select(x => new ProjectDataManageManageItemModel()
                {
                    Id = x.Id,
                    FileName = "",
                    FileSize = "",
                    Hospital = x.HospitalName ?? "",
                    InsertTime = x.InsertTime,
                    UploadMessage = x.UploadMessage,
                    Year = x.Year,
                    Status = x.Status,
                    Quarter = x.Quarter,
                    Project = x.ProjectType
                }).OrderByDescending(r => r.InsertTime).ToPagedList(pageIndex, MvcPagerExtensions.PageSize);

                foreach (var item in model.PageList)
                {
                    item.ProjectName = this.MedicalDataService.GetProejctName(item.Project);
                    item.Time = this.MedicalDataService.GetDataQuarter(item.Year, item.Quarter);
                    item.StatusName = ((MedicalDataStatusEnum)item.Status).GetDescription();

                    var uploadResult = item.UploadMessage.DeserializeObject<UploadMedicalResult>();
                    if (uploadResult != null && uploadResult.BaseMessage != null)
                    {
                        item.FileName = uploadResult.BaseMessage.FileName;
                        item.FileSize = uploadResult.BaseMessage.FileLength.ToString();
                    }
                }
            }
            
            return View(model);
        }

        #region 批量下载
        [HttpPost]
        public ActionResult ProjectDataDownload(List<long> ids, MedicalDataDownloadEnum downloadEvent)
        {
            string logContent = null;
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    string file = MedicalDataService.Download(ids, downloadEvent);
                    if (!string.IsNullOrWhiteSpace(file))
                    {
                        return File(file, "application/x-zip-compressed", $"{Guid.NewGuid().ToString("N")}.zip");
                    }
                    else
                    {
                        logContent = "没有可用文件.";
                    }
                }
                else
                {
                    logContent = "请选择需要下载的数据.";
                }
            }
            catch (Exception ex)
            {
                logContent = "文件下载失败. ";
                base.InsetActionLog(ActionType.Export, logContent, ex.SerializeObject());
            }

            return Content(logContent);
        }

        [HttpPost]
        public ActionResult ProjectDataFullDownload(MedicalDataDownloadEnum downloadEvent)
        {
            string logContent = null;
            try
            {
                List<long> projectTypes = new List<long>();
                try
                {
                    if (!string.IsNullOrWhiteSpace(LoginUserinfo.ProjectItem) && LoginUserinfo.ProjectItem.StartsWith("[") && LoginUserinfo.ProjectItem.EndsWith("]") && !LoginUserinfo.ProjectItem.Equals("[]"))
                    {
                        projectTypes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<long>>(LoginUserinfo.ProjectItem);
                    }
                }
                catch (Exception)
                {
                    projectTypes = new List<long>();
                }

                string file = MedicalDataService.DownloadByProjectType(projectTypes, downloadEvent);
                if (!string.IsNullOrWhiteSpace(file))
                {
                    return File(file, "application/x-zip-compressed", $"{Guid.NewGuid().ToString("N")}.zip");
                }
                else
                {
                    logContent = "没有可用文件.";
                }
            }
            catch (Exception ex)
            {
                logContent = "文件下载失败. ";
                base.InsetActionLog(ActionType.Export, logContent, ex.SerializeObject());
            }

            return Content(logContent);
        }
        #endregion

        #endregion

        #region 我的兑换记录


        /// <summary>
        /// 我的兑换记录  订单列表数据
        /// </summ 
        public ActionResult OrderList(int pageIndex = 1, string sn = "", int status = 0)
        {
            try
            {
                CenterOrderListModel returnModel = new CenterOrderListModel();
                returnModel.SN = string.IsNullOrWhiteSpace(sn) ? "" : sn.Trim();
                returnModel.OrderStatusList = OrderStatusEnum.Cancel.ToSelectList(false, true).ToList();

                var list = this.OrderService.Query(base.LoginUserinfo.Id, status, sn);
                var data = list.Select(x => new OrderModel()
                {
                    Id = x.Id,
                    MemberId = x.MemberId,
                    InsertTime = x.InsertTime,
                    SN = x.SN,
                    Amount = x.Amount,
                    ReceivedAmount = x.ReceivedAmount,
                    Status = x.Status
                }).ToPagedList<OrderModel>(pageIndex, MvcPagerExtensions.PageSize);

                List<long> orderId = data.Select(m => m.Id).ToList();
                var orderItemList = this.OrderItemService.Query(m => orderId.Contains(m.OrderId) && m.Mark > 0);

                //设置订单的明细数据
                foreach (var item in data)
                {
                    item.StatusName = ((OrderStatusEnum)item.Status).GetDescription();
                    var itemList = orderItemList.Where(m => m.OrderId == item.Id);
                    item.OrderItemList = itemList == null ? new List<OrderItemModel>() : itemList.Select(x => x.ToModel()).ToList();
                }
                returnModel.PageList = data;

                return View(returnModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 订单详细页面
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(string sn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sn))
                    throw new Exception("订单号不能为空");

                var member = base.LoginUserinfo;
                var order = this.OrderService.QueryEntityBySn(sn);

                if (order == null || order.Id <= 0 || order.MemberId != member.Id)
                    throw new Exception("订单不存在");

                OrderDetailModel model = new OrderDetailModel();
                model.OrderModel = order.ToModel();
                model.OrderModel.StatusName = ((OrderStatusEnum)order.Status).GetDescription();
                model.OrderItemList = this.OrderItemService.Query(m => m.OrderId == order.Id && m.Mark > 0).Select(x => x.ToModel()).ToList();
                model.OrderModel.OrderProductCount = model.OrderItemList.Count();
                model.OrderModel.OrderProductItemCount = model.OrderItemList.Where(m => m.ProductId > 0).Sum(m => m.Count);
                model.AddressModel = this.OrderAddressService.QueryEntity(m => m.OrderId == m.OrderId && m.Mark > 0);

                return View(model);
            }
            catch (Exception ex)
            {
                return View(new OrderDetailModel());
            }
        }


        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="sn">订单号</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult OrderConfirm(string sn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sn)) throw new Exception("订单号不能为空！");

                var entity = this.OrderService.QueryEntityBySn(sn);
                if (entity == null || entity.Id <= 0 || entity.MemberId != base.LoginUserinfo.Id)
                    throw new Exception("订单不存在！");

                if (this.OrderService.Receive(entity.Id, "", ActionSource.Web, base.LoginUserinfo))
                    return this.Content(JsonHelper.GetBaseMessage(true, ""));

                return this.Content(JsonHelper.GetBaseMessage(false, "确认收货失败"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }


        #endregion

        #region 收货地址管理
        public ActionResult AddressList()
        {
            var member = base.LoginUserinfo;
            var addressList = this.MemberAddressService.Query(m => m.Mark > 0 && m.MemberId == member.Id).Select(x => x.ToModel()).ToList();

            return this.View(addressList);
        }

        /// <summary>
        /// 新增收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult AddressAdd()
        {
            MemberAddressModel model = new MemberAddressModel();
            return View(model);
        }

        /// <summary>
        /// 新增收货地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddressAdd(MemberAddressModel model)
        {
            model.Id = CommonHelper.GuidToLongID;
            model.ZipPostalCode = "";
            model.Email = string.IsNullOrWhiteSpace(model.Email) ? "" : model.Email;

            if (!string.IsNullOrWhiteSpace(model.Email) && !CommonHelper.IsValidEmail(model.Email))
                return this.Content("邮箱格式错误");
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                entity.MemberId = base.LoginUserinfo.Id;
                entity.Remark = "";
                entity.Area = this.AreaService.GetFullName(entity.DistrictsId);


                this.MemberAddressService.Insert(entity, base.LoginUserinfo, ActionSource.Web);

                string logContent = "新增收货地址成功，收货人：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification("新增收货地址成功");

                return this.Content("true");
            }

            return this.Content("");
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddressDelete(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("数据不能为空！");

                var entity = this.MemberAddressService.QueryEntity(id);
                if (entity == null || entity.Id <= 0 || entity.MemberId != base.LoginUserinfo.Id)
                    throw new Exception("数据不存在！");

                entity.IsMain = false;
                this.MemberAddressService.Delete(entity);

                string logContent = "【手动】会员删除收货地址，收货地址id：" + entity.Id;
                base.InsetActionLog(ActionType.Delete, logContent, entity.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        /// <summary>
        /// 编辑收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult AddressEdit(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("数据不能为空");

                var memberAddress = this.MemberAddressService.QueryEntity(id);
                if (memberAddress == null || memberAddress.Id <= 0 || memberAddress.MemberId != base.LoginUserinfo.Id || memberAddress.Mark <= 0)
                    throw new Exception("数据不存在");

                return this.View(memberAddress.ToModel());
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);
            }

            return this.View(new MemberAddressModel());
        }


        /// <summary>
        /// 编辑收货地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddressEdit(MemberAddressModel model)
        {
            model.Email = string.IsNullOrWhiteSpace(model.Email) ? "" : model.Email;

            if (!string.IsNullOrWhiteSpace(model.Email) && !CommonHelper.IsValidEmail(model.Email))
                return this.Content("邮箱格式错误");

            if (ModelState.IsValid)
            {
                var entity = this.MemberAddressService.QueryEntity(model.Id);
                if (entity == null || entity.Id <= 0 || entity.MemberId != base.LoginUserinfo.Id || entity.Mark <= 0)
                    return this.Content("编辑的收货地址不存在");

                entity.Name = model.Name;
                entity.Phone = model.Phone;
                entity.Tel = model.Tel;
                entity.Address = model.Address;
                entity.Email = model.Email;
                entity.Remark = "";
                entity.Area = this.AreaService.GetFullName(model.DistrictsId).TrimStart(' ');
                entity.IsMain = model.IsMain;
                entity.ProvinceId = model.ProvinceId;
                entity.CityId = model.CityId;
                entity.DistrictsId = model.DistrictsId;

                this.MemberAddressService.Update(entity, base.LoginUserinfo, ActionSource.Web);

                string logContent = "修改收货地址成功，收货人：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification("修改收货地址成功");

                return this.Content("true");
            }

            return this.Content("");
        }



        #endregion

        #region 积分明细

        /// <summary>
        /// 用户积分明细
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult IntegralList(int pageIndex = 1)
        {
            var list = this.MemberIntegralLogService.Query(base.LoginUserinfo.Id).ToPagedList(pageIndex, MvcPagerExtensions.PageSize); ;
            return this.View(list);
        }


        #endregion

        #region 建议反馈
        public ActionResult Feedback()
        {
            var user = base.LoginUserinfo;
            FeedbackModel model = new FeedbackModel
            {
                Mobile = user.Phone,
                Email = user.Email
            };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Feedback(FeedbackModel model)
        {
            ModelState.Remove("Id");
            Regex regex = new Regex(ConfigHelper.GetConfigString("regex.mobile"));
            if (!regex.IsMatch(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "请输入正确的手机号码");
            }

            if (ModelState.IsValid)
            {
                var user = base.LoginUserinfo;

                Feedback entity = new Feedback
                {
                    Content = string.IsNullOrWhiteSpace(model.Content) ? "" : model.Content,
                    Describe = Newtonsoft.Json.JsonConvert.SerializeObject(new { Mobile = model.Mobile, Email = model.Email }),
                    Name = model.Name,
                    MemberId = user.Id,
                    MemberName = user.Name,
                    Status = false
                };

                this.FeedbackService.Insert(entity);

                string logContent = "用户提交建议反馈成功，反馈标题：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification("提交成功，感谢您的反馈");

                // 发送邮箱
                FeedbackEmail(entity, user, model.Mobile, model.Email);
                return this.RedirectToAction("Index");
            }

            return this.View();
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

            string noticeEmails = SettingService.QueryValue<string>("web.notice.email");
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
            EngineContext.Current.Resolve<IMessageEmailService>().Insert(emails);
        }

        #endregion

        public ActionResult _Menu()
        {
            return this.View(base.LoginUserinfo);
        }

    }
}