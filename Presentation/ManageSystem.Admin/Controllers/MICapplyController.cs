using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Models;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.MIC;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class MICapplyController : AdminBaseController
    {

        private readonly IMICPermissionapplication MICPermissionapplication;
        private readonly IMemberService MemberService;




        public MICapplyController(IMICPermissionapplication _MICPermissionapplication, IMemberService _MemberService)
        {
            MICPermissionapplication = _MICPermissionapplication;
            MemberService = _MemberService;

        }
        // GET: MICapply
        public ActionResult MICIndex()
        {
            //Models.MICPermissionapplication model =new Models.MICPermissionapplication();
            //MICapplyforaccess model = new MICapplyforaccess();
            //var model = this.MICPermissionapplication.Query();
            //model.CompanyNameList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == userinfo.HospitalId }; }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult MICIndex1(DataSourceRequest command, Models.MICPermissionapplication model)
        {
            //获得数据
            var list = this.MICPermissionapplication.QueryPage(model.Name, model.Phone, model.CompanyName, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        MID = x.MID,
                        Name = x.Name,
                        Phone = x.Phone,
                        CompanyName = x.CompanyName,
                        Department = x.Department,
                        Provincesandcities = x.Provincesandcities,
                        Position = x.Position,
                        Email = x.Email,
                        Applicationtime = x.Applicationtime.ToString("G"),
                        Applicationstate = x.Applicationstate,
                        PassAction = x.Applicationstate != -1 ? "" : "<a href=\"javascript:Agree('" + x.MID.ToString() + "')\">通过</a>",
                        RejectAction = x.Applicationstate != -1 ? "" : "<a href=\"javascript:Reject('" + x.MID.ToString() + "')\">驳回</a>"
                    };
                }),
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 同意后将对应用户的MIC访问权限开启
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Agree(string Smid)
        {
            long MID = Convert.ToInt64(Smid);
            //根据用户ID获取实体数据
            var entity = this.MemberService.QueryEntity(MID);
            entity.MICjurisdiction = 1;
            this.MemberService.Update(entity);
            var modelentity = this.MICPermissionapplication.QueryEntity(m => m.MID.Equals(MID));
            modelentity.Applicationstate = 0;
            this.MICPermissionapplication.Update(modelentity);
            SendCheckEmail(entity.Email, entity.Name, true);
            base.SuccessNotification("已同意该用户的MIC访问权限!");
            return RedirectToAction("MICIndex", "MICapply");
        }

        /// <summary>
        /// 设置医院管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckRole(false, false)]
        public ActionResult MICRejectView(string Smid)
        {
            long id = long.Parse(Smid);
            var model = this.MICPermissionapplication.QueryEntity(m => m.MID.Equals(id));

            Models.MICPermissionapplication mICPermissionapplication = new Models.MICPermissionapplication()
            {
                Id = id,
                Smid = model.MID.ToString()
            };

            return this.View(mICPermissionapplication);
        }

        /// <summary>
        /// 驳回后
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reject(string Smid, string Reason)
        {
            long MID = Convert.ToInt64(Smid);
            //根据用户ID获取实体数据
            var entity = this.MemberService.QueryEntity(MID);
            entity.MICjurisdiction = 1;
            this.MemberService.Update(entity);
            var modelentity = this.MICPermissionapplication.QueryEntity(m => m.MID.Equals(MID));
            modelentity.Applicationstate = 1;
            this.MICPermissionapplication.Update(modelentity);
            SendCheckEmail(entity.Email, entity.Name, false, Reason);
            base.SuccessNotification("已驳回该用户的MIC访问权限!");
            return RedirectToAction("MICIndex", "MICapply");
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

                Member member = this.MemberService.QueryEntity(base.LoginUserinfo.Id);


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