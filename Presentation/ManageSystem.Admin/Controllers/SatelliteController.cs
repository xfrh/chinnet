using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models.Satellite;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Log;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Teams;
using ManageSystem.Services.Users;
using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class SatelliteController : AdminBaseController
    {
        private readonly ISatelliteService SatelliteService;
        private readonly IAreaService AreaService;
        private readonly IHospitalService HospitalService;
        private readonly IDoctorTitleService DoctorTitleService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly ISatelliteUserService SatelliteUserService;
        private readonly ITeamService _teamService;
        private readonly IMemberService _memberService;

        private readonly IActionLogService actionLogService;
        private readonly IUserinfoService userinfoService;
        private readonly IRoleService roleService;
        private readonly IUserRoleService userRoleService;
        private readonly IEncryptionService encryptionService;
        private readonly IFunctionService functionService;

        public SatelliteController(ISatelliteService _SatelliteService, IAreaService _AreaService,
            IHospitalService _HospitalService,
            IDoctorTitleService _DoctorTitleService,
            IHospitalDepartmentService _HospitalDepartmentService,
            ISatelliteUserService _SatelliteUserService,
            ITeamService _TeamService,
            IMemberService _MemberSerivice,
              IActionLogService _actionLogService,
            IUserinfoService _userinfoService,
            IRoleService _roleService,
            IUserRoleService _userRoleService,
             IEncryptionService _encryptionService,
             IFunctionService _functionService)
        {
            SatelliteService = _SatelliteService;
            AreaService = _AreaService;
            HospitalService = _HospitalService;
            DoctorTitleService = _DoctorTitleService;
            HospitalDepartmentService = _HospitalDepartmentService;
            SatelliteUserService = _SatelliteUserService;
            _teamService = _TeamService;
            _memberService = _MemberSerivice;
            this.actionLogService = _actionLogService;
            this.userinfoService = _userinfoService;
            this.roleService = _roleService;
            this.userRoleService = _userRoleService;
            this.encryptionService = _encryptionService;
            this.functionService = _functionService;
        }

        public ActionResult SaIndex()
        {
            return View();
        }



        [HttpPost]
        public ActionResult SaIndex(DataSourceRequest command, Models.MICPermissionapplication model)
        {
            var user = base.LoginUserinfo;
            Role role = roleService.QueryEntity(long.Parse(userRoleService.GetRoleId(user.Id.ToString())));
            bool isSa = false;
            if (!string.IsNullOrEmpty(role.SatelliteId))
            {
                isSa = true;
            }

            //获得数据
            var list = isSa ? this.SatelliteService.QueryPageSa(model.Name, long.Parse(user.Describe), model.Phone, command.Page - 1, command.PageSize) : this.SatelliteService.QueryPage(model.Name, model.Phone, command.Page - 1, command.PageSize);
            var hospitalList = HospitalService.QueryList();
            var areaList = AreaService.Query();
            var departmentList = HospitalDepartmentService.Query();
            var doctitleList = DoctorTitleService.Query();
            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        ID = x.Id,
                        Name = x.ChargeName,
                        Phone = x.ChargePhoneNumber,
                        CompanyName = hospitalList.Single(p => p.Id == long.Parse(x.PlaceOfWork)).Name,
                        Department = x.Office == "0" ? "-" : departmentList.Single(p => p.Id == long.Parse(x.Office)).Name,
                        Provincesandcities = areaList.Single(p => p.Id == long.Parse(x.Province)).Name + "-" + (x.City == "0" ? "" : areaList.Single(p => p.Id == long.Parse(x.City)).Name),
                        Position = x.Profession == "0" ? "-" : doctitleList.Single(p => p.Id == long.Parse(x.Profession)).Name,
                        Email = x.ChargeEmail,
                        Applicationtime = x.InsertTime.ToString("G"),
                        PassAction = (x.RealmName != "&" && string.IsNullOrEmpty(x.Describe)) ? "<a href=\"javascript:Agree('" + x.Id.ToString() + "')\">通过</a>" : "",
                        RejectAction = (x.RealmName != "&" && string.IsNullOrEmpty(x.Describe)) ? "<a href=\"javascript:Reject('" + x.Id.ToString() + "')\">驳回</a>" : "",
                        EditInfo = (x.RealmName != "&" && x.Describe == "1") ? "<a href=\"/SatelliteUser/SatelliteUserList/" + x.Id + "\">编辑</a>" : "",
                        SetAdmin = (x.RealmName != "&" && x.Describe == "1") ? "<a href=\"javascript:ShowAdmin('" + x.Id.ToString() + "')\">设置管理员</a>" : "",
                    };
                }),
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }



        [HttpPost]
        public ActionResult Agree(long ID)
        {
            var modelentity = this.SatelliteService.QueryEntity(ID);
            //string name = this.AreaService.QueryEntity(long.Parse(modelentity.City)).Name;
            //modelentity.RealmName = GetPinyin(name.Substring(0, name.Length - 1));

            // modelentity.RealmName = GetPinyin(this.AreaService.QueryEntity(long.Parse(modelentity.Province)).ShortName.ToString()).ToLower();
            modelentity.Describe = "1";

            _teamService.CreateTeamClassfy(modelentity.SatelliteName);

            this.SatelliteService.CreateMeetingType(modelentity.SatelliteName);

            this.SatelliteService.CreateMedicalProjectType(modelentity.SatelliteName);

            this.SatelliteService.Update(modelentity);

            SendCheckEmail(modelentity.ChargeEmail, modelentity.ChargeName, true);
            base.SuccessNotification($"已同意ID为{modelentity.Id}的卫星网申请!");
            return RedirectToAction("SaIndex", "Satellite");
        }




        private static string GetPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }


        [HttpPost]
        public ActionResult Reject(string ID, string Reason)
        {
            long id = long.Parse(ID);
            var modelentity = this.SatelliteService.QueryEntity(id);
            modelentity.RealmName = "&";
            this.SatelliteService.Update(modelentity);
            SendCheckEmail(modelentity.ChargeEmail, modelentity.ChargeName, false, Reason);
            base.SuccessNotification($"已驳回ID为{modelentity.Id}的卫星网申请!");
            return RedirectToAction("SaIndex", "Satellite");
        }

        /// <summary>
        /// 设置医院管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckRole(false, false)]
        public ActionResult SalliteRejectView(string Smid)
        {
            long id = long.Parse(Smid);
            var model = this.SatelliteService.QueryEntity(m => m.Id.Equals(id));

            Models.Satellite.SatelliteModel satelliteModel = new SatelliteModel() {
            Id = model.Id
            };

            return this.View(satelliteModel);
        }

        [CheckRole(false, false)]
        public ActionResult SatelliteSetAdminView(string SaId)
        {
            long id = long.Parse(SaId);
            var model = this.SatelliteService.QueryEntity(m => m.Id.Equals(id));

            Models.Satellite.SatelliteModel satelliteModel = new SatelliteModel()
            {
                Id = model.Id
            };

            return this.View(satelliteModel);
        }

        [HttpPost]
        [CheckRole(false, false)]
        public JsonResult SetAdmin(string ID, string Account, string Password)
        {
            var modelentity = this.SatelliteService.QueryEntity(long.Parse(ID));

            UserinfoModel model = new UserinfoModel();
            model.LoginId = Account;
            model.Password = Password;
            model.Name = "卫星网管理员";
            model.Describe = modelentity.Id.ToString();
            model.State = 2;

            if (this.userinfoService.Count(m => m.LoginId.Equals(model.LoginId) && m.Mark > 0) > 0)
                this.ModelState.AddModelError("", "登录帐号已经存在");

            if (ModelState.IsValid)
            {
                model.Birthday = model.Birthday ?? model.Birthday.DefaultValue();

                var entity = model.ToEntity();
                entity.LastLoginDate = entity.LastLoginDate.DefaultValue();

                this.userinfoService.Insert(entity);
                this.userRoleService.Insert(entity, "5347493888906415245");

                string logContent = "添加卫星网管理员，用户登录帐号： " + entity.LoginId;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

            }
            return Json(new
            {
                status = true,
                message = $"设置管理员成功!"
            });
        }


        public JsonResult SendCheckEmail(string Email, string name, bool isPass, string reason = "")
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

                Member member = this._memberService.QueryEntity(base.LoginUserinfo.Id);


                long timestamp = DateHelper.UtcTimestamp;

                string validateUrl = "";
                string email_template_path = Server.MapPath("/Content/File/usercenter_email_validate_template.html");
                string html = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(email_template_path))
                {
                    html = sr.ReadToEnd();
                }
                html = html.Replace("$UserName$", member.Name)
                    .Replace("$IsPassOrNot$", isPass ? "您的卫星网申请已经通过！" : "您的卫星网申请已被驳回，驳回原因：" + reason);

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
                    Title = "卫星网通过驳回"
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
    }
}