using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models;
using ManageSystem.Admin.Models.CRs;
using ManageSystem.Core.Domain.CRs;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.CRs;
using ManageSystem.Services.Log;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.SystemSet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class CrtigecyclineController : AdminBaseController
    {
        // GET: Crtigecycline
        private readonly ICRService CRService;
        private readonly ICRItemService CRItemService;
        private readonly ICRLogService CRLogService;
        private readonly IMemberService memberService;
        private readonly ISystemLogService SystemLogService;
        private readonly IAreaService areaService;

        public CrtigecyclineController(ICRService _crService, ICRItemService _crItemService,
            ICRLogService _crLogService, IMemberService _memberService,
             ISystemLogService _systemLogService,
             IAreaService _areaService
            )
        {
            this.CRService = _crService;
            this.CRItemService = _crItemService;
            this.CRLogService = _crLogService;
            this.memberService = _memberService;
            this.SystemLogService = _systemLogService;
            this.areaService = _areaService;
        }


        #region 项目数据

        /// <summary>
        /// 项目数据 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CRDataList()
        {
            CRDataModel model = new CRDataModel();

            return View(model);
        }

        /// <summary>
        /// 项目数据 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CRDataList(DataSourceRequest command, CRDataModel model)
        {
            //获得数据
            var list = this.CRService.QueryPage(model.HospitalName, model.HospitalId, command.Page - 1, command.PageSize);
            string serviceEmail = ConfigHelper.GetConfigString("FileWebUrl");
            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        HospitalName = x.HospitalName,
                        HospitalGrade = x.HospitalGrade,
                        Name = x.Name,
                        Phone = x.Phone,
                        Email = x.Email,
                        EmailStatus = x.EmailStatus == 1 ? "未发送" : x.EmailStatus == 2 ? "已发送" : x.EmailStatus == 3 ? "已确认" : "",
                        EmailTime = x.EmailTime,
                        MemberId = x.MemberId,
                        FileName = x.FileName,
                        MemberName = x.MemberName,
                        UploadFilePath = x.UploadFilePath,
                        UploadName = x.UploadFilePath != "" ? "下载文件" : "未上传",
                        HospitalTime = x.HospitalTime.ToString("yyyy-MM-dd HH:mm") == "1900-01-01 00:00" ? "" : x.HospitalTime.ToString("yyyy-MM-dd HH:mm"),
                        Year = x.Year,
                        Quarter = x.Quarter,
                        ActionHtml = x.EmailStatus == 1 ? "<a href=\"javascript:SetEmail('" + x.Id.ToString() + "')\">发送邮件</a>" : ""
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        #region 查看

        /// <summary>
        /// 项目数据 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult CRDataView(long id)
        {
            return this.View(this.SetCRDataEditData(id));
        }

        /// <summary>
        /// 项目详情数据 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CRDataViewList(DataSourceRequest command, long projectId)
        {
            //获得数据
            var list = this.CRItemService.QueryPage(projectId, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Number = x.Number,
                        Name = x.Name,
                        MIC_Imipenem = x.MIC_Imipenem,
                        ImineNumber = x.ImineNumber,
                        TegacyclineNumber = x.TegacyclineNumber,
                        BacteriostasisNumber = x.BacteriostasisNumber,
                        RecheckNumber = x.RecheckNumber,
                        Specimensource=x.Specimensource,
                        ProjectId = x.ProjectId,
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 项目详情日志 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CRDataLogViewList(DataSourceRequest command, CRDataItemModel model)
        {
            //获得数据
            var list = this.CRLogService.QueryProjectPage(model.ProjectId, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Content = x.Content,
                        UserinfoName = x.UserinfoName,
                        ProjectId = x.ProjectId,
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 项目数据 查看页面的数据
        /// </summary>
        /// <returns></returns>
        public CRDataModel SetCRDataEditData(long id)
        {
            var entity = this.CRService.QueryEntity(m => m.Id == id && m.Mark > 0);
            var model = entity.ToModel();
            var User = memberService.QueryEntity(model.MemberId);
            model.LoginId = User.LoginId;

            return model;
        }

        #endregion

        #region 编辑 
        /// <summary>
        /// 医院 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public CRDataModel SetCREditData(long id)
        {
            var entity = this.CRService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }


        /// <summary>
        /// 项目 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult CREdit(long id)
        {
            return this.View(this.SetCREditData(id));
        }

        /// <summary>
        /// 项目 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CREdit(CRDataModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.CRService.QueryEntity(model.Id);

                if (entity == null || entity.Id <= 0)
                {
                    base.ErrorNotification("数据不存在，请重新打开");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }

                if (string.IsNullOrWhiteSpace(model.HospitalName))
                {
                    base.ErrorNotification("请输入医院名称");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }

                if (string.IsNullOrWhiteSpace(model.HospitalGrade))
                {
                    base.ErrorNotification("请输入医院等级");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (model.DistrictsId <= 0)
                {
                    base.ErrorNotification("请选择省市区");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (string.IsNullOrWhiteSpace(model.CREDetectionRate))
                {
                    base.ErrorNotification("请输入上年CRE平均检出率");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (string.IsNullOrWhiteSpace(model.CREDrugRate))
                {
                    base.ErrorNotification("请输入上年CRAB平均检出率");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (string.IsNullOrWhiteSpace(model.MHSource))
                {
                    base.ErrorNotification("请输入MH平板来源厂家");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    base.ErrorNotification("请输入微生物联系人");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (string.IsNullOrEmpty(model.Phone))
                {
                    base.ErrorNotification("请输入联系电话");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (string.IsNullOrEmpty(model.Email))
                {
                    base.ErrorNotification("请输入邮箱地址");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                if (model.Email != null && model.Email.IndexOf("@") == -1)
                {
                    base.ErrorNotification("邮箱格式不正确");
                    return this.RedirectToAction("CREdit/" + model.Id);
                }
                model.Province = areaService.GetName(model.ProvinceId);
                model.City = areaService.GetName(model.CityId);
                model.Area = areaService.GetName(model.DistrictsId);               
                model.FileName = model.FileName == null ? "" : model.FileName;
                model.Address = model.Address == null ? "" : model.Address;
                model.DisposeFilePath = model.DisposeFilePath == null ? "" : model.DisposeFilePath;
                model.UploadFilePath = model.UploadFilePath == null ? "" : model.UploadFilePath;
                model.EmailTime = model.EmailTime == Convert.ToDateTime("0001/1/1 0:00:00") ? Convert.ToDateTime("1900/1/1 0:00:00") : model.EmailTime;
                model.HospitalTime = model.HospitalTime == Convert.ToDateTime("0001/1/1 0:00:00") ? Convert.ToDateTime("1900/1/1 0:00:00") : model.HospitalTime;
                model.Year = entity.Year;
                model.Quarter = entity.Quarter;

                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.CRService.Update(entity);

                string logContent = "修改替加环素项目，负责人名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("CRDataList");
            }

            return this.View(this.SetCREditData(model.Id));
        }

        #endregion

        /// <summary>
        /// 项目数据 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CRDataDelete(string selectedIds)
        {
            var user = this.LoginUserinfo;
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("CRDataList");
            }

            this.CRService.Delete(selectedIds);

            string logContent = "【手动】删除替加环素项目数据，删除的id集合:" + selectedIds;
            CRLogService.Insert(ActionType.Delete, ActionSource.Admin, 0, user.Id, user.Name, "删除数据", logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("CRDataList");
        }


        #endregion

        #region 下载
        /// <summary>
        /// 下载上传的原始数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ActionResult DownloadOriginalData(long id)
        {
            try
            {
                string filePath = this.CRService.DownloadOriginalData(id);
                string path = this.Server.MapPath(filePath);
                string ex = Path.GetExtension(path);
                string name = "data-" + id + ex;
                return File(path, "application/octet-stream", Url.Encode(name));
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex);
                return this.RedirectToAction("Detail", new { id = id.ToString() });
            }
        }

        /// <summary>
        /// 下载经过容错处理的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ActionResult DownloadNewData(long id)
        {
            try
            {
                string filePath = this.CRService.DownloadNewData(id);
                string path = this.Server.MapPath(filePath);
                //string ex = Path.GetExtension(path);
                //string name = "data-adjust-" + id + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + ex;
                string name = System.IO.Path.GetFileName(filePath);
                return File(path, "application/octet-stream", Url.Encode(name));
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex);
                return this.RedirectToAction("Detail", new { id = id.ToString() });
            }
        }


        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CRDownloadExcel(List<long> ids)
        {
            try
            {
                ExcelPackage ep = new ExcelPackage();
                string path = this.CRService.Export(ids, ep);
                if (string.IsNullOrWhiteSpace(path)) return this.Content("导出文件失败");

                var user = this.LoginUserinfo;
                this.ActionLogService.Insert(ActionType.Export, ActionSource.Admin, user.Id, user.Name, "导出Excel成功", "导出Excel成功");

                Response.Clear();
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                Response.AddHeader("content-disposition", "attachment;filename=" + path);
                Response.ContentType = "application/vnd.open";
                ep.SaveAs(Response.OutputStream);
                Response.Flush();
                Response.End();

                return this.Content("导出Excel成功");
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                return this.Content(ex.Message);
            }
        }

        /// <summary>
        /// 下载原始文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CRDataDownload1(List<long> ids, CRDownloadEnum downloadEvent)
        {
            string logContent = null;
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    string file = CRService.Download(ids, downloadEvent);
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

            base.ErrorNotification(logContent);
            return RedirectToAction("CRDataList");
        }
        /// <summary>
        /// 设置Excel的宽度
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="ws"></param>
        public void SheetColumn(int Columns, ExcelWorksheet ws)
        {
            if (Columns == 1)
            {
                ws.Column(1).Width = 18;
                ws.Column(2).Width = 18;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 10;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 10;
                ws.Column(7).Width = 25;
                ws.Column(8).Width = 25;
                ws.Column(9).Width = 14;
                ws.Column(10).Width = 20;
                ws.Column(11).Width = 12;
                ws.Column(12).Width = 12;
                ws.Column(13).Width = 20;
                ws.Column(14).Width = 10;
                ws.Column(15).Width = 18;
            }
            else if (Columns == 2)
            {
                ws.Column(1).Width = 18;
                ws.Column(2).Width = 10;
                ws.Column(3).Width = 17;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 27;
                ws.Column(6).Width = 30;
                ws.Column(7).Width = 18;
                ws.Column(8).Width = 17;
            }
        }
        #endregion

        #region 邮件
        /// <summary>
        /// 批量发送邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CRDataDownload2(string ids)
        {
            int count = 0;
            var entity = new CRData();
            string[] id = ids.Split(',');
            for (int i = 0; i < id.Length; i++)
            {
                entity = this.CRService.QueryEntity(long.Parse(id[i]));
                if (entity.EmailStr == "" || entity.EmailStr == null)
                {
                    entity.EmailStr = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
                    CRService.Update(entity);
                }

                StringBuilder s = new StringBuilder();
                string serviceWebUrl = ConfigHelper.GetConfigString("FileWebUrl");
                //s.Append("<div style='width:700px;'>");
                //s.Append("<div style='font-size:16px;font-weight: bold;text-indent:33px;'>");
                //s.Append("<div style='line-height: 50px;text-indent: 0;'>" + entity.Name + "&nbsp;&nbsp;您好,</div>");
                //s.Append("<div>感谢贵单位参与《微生物实验室细菌诊查及替加环素药敏操作规范培训工艺项目》，此次实验所需的物料已于近日派出，如您已收到，请点击下方按钮确认</div>");
                //s.Append("<div style='margin: 20px 0px;'><a style='background-color: #337ab7;color: white;border-radius: 4px;box-shadow: 2px 2px 2px rgba(0,0,0,0.2);cursor: pointer;padding: 10px 20px;height: 50px;line-height: 50px;text-decoration: none;' href=\"" + serviceWebUrl + "CRProject/Email?key=" + entity.EmailStr + "\" >确认实验物料已收到</a></div>");

                //s.Append("<div style='font-size:14px; '>");
                //s.Append("<div>如果此标签无法打开，请复制以下链接，并在浏览器中打开：</div>");
                //s.Append("<div><a  href='" + serviceWebUrl + "CRProject/Email?key=" + entity.EmailStr + "'>" + serviceWebUrl + "CRProject/Email/" + entity.EmailStr + "</a></div>");
                //s.Append("</div>");

                //s.Append("<div style='margin: 20px 0;text-indent: 0; '>感谢!</div>");
                //s.Append("<div style='margin: 20px 0;text-indent: 0; '>CHINET数据云</div>");
                //s.Append("</div>");
                //s.Append("</div>");

                s.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:700px;\">");
                s.Append("<tr style=\"font-size:16px;font-weight: bold;text-indent:33px;\"><td colspan=\"3\" style=\"font-size:16px;font-weight: bold;text-indent:33px;line-height: 50px;text-indent: 0;width:700px;\">" + entity.Name + "&nbsp;您好,</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"font-size:16px;font-weight: bold;text-indent:33px;line-height: 30px;width:700px;\">感谢贵公司参与《微生物实验室细菌诊查及替加环素药敏操作规范培训工艺项目》，此次实验所需的物料已于近日派出，如您已收到，请点击下方按钮确认</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 10px 0;text-indent: 0;font-weight: bold;\"></td></tr>");

                s.Append("<tr style=\"width: 33px;\"><td style=\"width: 33px;\"></td><td style=\"font-size:16px;font-weight:bold;width: 200px;text-align: center;background-color:#337ab7;color:white;border-radius:4px;text-decoration:none;padding: 10px 0;\"><a style=\"color:white;text-decoration:none;font-weight:bold;font-size:16px;border-radius:4px;\" href=\"" + serviceWebUrl + "CR/CRUpload/Email?key=" + entity.EmailStr + "\">确认实验物料已收到</a></td><td style=\"width: 467px;\"></td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 10px 0;text-indent: 0;font-weight: bold;\"></td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"font-size:14px;text-indent:33px;width:700px;\">如果此标签无法打开，请复制以下链接，并在浏览器中打开：</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"font-size:14px;text-indent:33px;width:700px;\"><a href=\"" + serviceWebUrl + "CR/CRUpload/Email?key=" + entity.EmailStr + "\" style=\"font-size:14px;text-indent:33px;\">" + serviceWebUrl + "CR/CRUpload/Email?key=" + entity.EmailStr + "</a></td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">感谢!</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">CHINET数据云</td></tr>");
                s.Append("</table>");

                if (entity != null && entity.Id > 0 && entity.EmailStatus == 1)
                {
                    MessageEmail email = new MessageEmail()
                    {
                        Content = s.ToString(),
                        Email = entity.Email,
                        MemberId = entity.MemberId,
                        MemberName = entity.MemberName,
                        Remark = "",
                        SceneType = "",
                        SendType = 1,
                        Source = "web",
                        Status = 1,//状态：1、待发送   2：已发送   3：失败
                        Title = "替加环素项目确认通知"
                    };

                    try
                    {
                        //发送邮件
                        string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
                        string servicePassword = ConfigHelper.GetConfigString("message.email.password");
                        int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
                        bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
                        string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
                        string displayname = ConfigHelper.GetConfigString("message.email.displayname");

                        bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { entity.Email }, email.Title, s.ToString());
                        if (result)
                        {
                            count++;
                            email.Status = 2;
                            //发送成功修改邮件状态为已发送
                            entity.EmailStatus = 2;
                            this.CRService.Update(entity);
                            //model.UploadResultLog += "，发送邮件成功";
                        }
                        else
                        {
                            email.Status = 3;
                            //model.UploadResultLog += "，发送邮件失败";
                        }
                        var user = this.LoginUserinfo;
                        CRLogService.Insert(ActionType.Email, ActionSource.Admin, entity.Id, user.Id, user.Name, "发送邮件成功" + count + "条，失败" + (id.Length - count) + "条", "发送邮件成功" + count + "条，失败" + (id.Length - count) + "条");
                    }
                    catch (Exception ex)
                    {
                        email.Remark = "发送邮件发生异常，异常信息：" + ex.Message + ",发送邮件成功" + count + "条，失败" + (id.Length - count) + "条";
                        return this.Content(JsonHelper.GetBaseMessage(false, "发送邮件失败"));
                    }

                    //保存发送邮件记录
                    EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);
                }
            }

            return this.Content(JsonHelper.GetBaseMessage(true, "发送邮件成功,发送邮件成功" + count + "条，失败" + (id.Length - count) + "条"));
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CRSetEmail(long id)
        {
            var entity = this.CRService.QueryEntity(id);
            if (entity != null && entity.Id > 0)
            {
                StringBuilder s = new StringBuilder();

                string serviceWebUrl = ConfigHelper.GetConfigString("FileWebUrl");

                if (entity.EmailStr == "" || entity.EmailStr == null)
                {
                    entity.EmailStr = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
                    CRService.Update(entity);
                }
                //s.Append("<div style='width:700px;'>");
                //s.Append("<div style='font-size:16px;font-weight: bold;text-indent:33px;'>");
                //s.Append("<div style='line-height: 50px;text-indent: 0;'>" + entity.Name + "&nbsp;&nbsp;您好,</div>");
                //s.Append("<div>感谢贵单位参与《微生物实验室细菌诊查及替加环素药敏操作规范培训工艺项目》，此次实验所需的物料已于近日派出，如您已收到，请点击下方按钮确认</div>");
                //s.Append("<div style='margin: 20px 0px;'><a style='background-color: #337ab7;color: white;border-radius: 4px;box-shadow: 2px 2px 2px rgba(0,0,0,0.2);cursor: pointer;padding: 10px 20px;height: 50px;line-height: 50px;text-decoration: none;' href=\"" + serviceWebUrl + "CRProject/Email?key=" + entity.EmailStr + "\" >确认实验物料已收到</a></div>");
                //s.Append("<div style='font-size:14px; '>");
                //s.Append("<div>如果此标签无法打开，请复制以下链接，并在浏览器中打开：</div>");
                //s.Append("<div><a  href='" + serviceWebUrl + "CRProject/Email?key=" + entity.EmailStr + "'>" + serviceWebUrl + "CRProject/Email/" + entity.EmailStr + "</a></div>");
                //s.Append("</div>");
                //s.Append("<div style='margin: 20px 0;text-indent: 0; '>感谢!</div>");
                //s.Append("<div style='margin: 20px 0;text-indent: 0; '>CHINET数据云</div>");
                //s.Append("</div>");
                //s.Append("</div>");



                s.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:700px;\">");
                s.Append("<tr style=\"font-size:16px;font-weight: bold;text-indent:33px;\"><td colspan=\"3\" style=\"font-size:16px;font-weight: bold;text-indent:33px;line-height: 50px;text-indent: 0;width:700px;\">" + entity.Name + "&nbsp;您好,</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"font-size:16px;font-weight: bold;text-indent:33px;line-height: 30px;width:700px;\">感谢贵公司参与《微生物实验室细菌诊查及替加环素药敏操作规范培训工艺项目》，此次实验所需的物料已于近日派出，如您已收到，请点击下方按钮确认</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 10px 0;text-indent: 0;font-weight: bold;\"></td></tr>");

                s.Append("<tr style=\"width: 33px;\"><td style=\"width: 33px;\"></td><td style=\"font-size:16px;font-weight:bold;width: 200px;text-align: center;background-color:#337ab7;color:white;border-radius:4px;text-decoration:none;padding: 10px 0;\"><a style=\"color:white;text-decoration:none;font-weight:bold;font-size:16px;border-radius:4px;\" href=\"" + serviceWebUrl + "CR/CRUpload/Email?key=" + entity.EmailStr + "\">确认实验物料已收到</a></td><td style=\"width: 467px;\"></td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 10px 0;text-indent: 0;font-weight: bold;\"></td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"font-size:14px;text-indent:33px;width:700px;\">如果此标签无法打开，请复制以下链接，并在浏览器中打开：</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"font-size:14px;text-indent:33px;width:700px;\"><a href=\"" + serviceWebUrl + "CR/CRUpload/Email?key=" + entity.EmailStr + "\" style=\"font-size:14px;text-indent:33px;\">" + serviceWebUrl + "CR/CRUpload/Email?key=" + entity.EmailStr + "</a></td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">感谢!</td></tr>");

                s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">CHINET数据云</td></tr>");
                s.Append("</table>");




                MessageEmail email = new MessageEmail()
                {
                    Content = s.ToString(),
                    Email = entity.Email,
                    MemberId = entity.MemberId,
                    MemberName = entity.MemberName,
                    Remark = "",
                    SceneType = "",
                    SendType = 1,
                    Source = "web",
                    Status = 1,//状态：1、待发送   2：已发送   3：失败
                    Title = "替加环素项目确认通知"
                };

                try
                {
                    //发送邮件
                    string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
                    string servicePassword = ConfigHelper.GetConfigString("message.email.password");
                    int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
                    bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
                    string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
                    string displayname = ConfigHelper.GetConfigString("message.email.displayname");

                    bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { entity.Email }, email.Title, s.ToString());
                    if (result)
                    {
                        email.Status = 2;
                        //发送成功修改邮件状态为已发送
                        entity.EmailStatus = 2;
                        this.CRService.Update(entity);
                        //model.UploadResultLog += "，发送邮件成功";
                    }
                    else
                    {
                        email.Status = 3;
                        //model.UploadResultLog += "，发送邮件失败";
                    }
                    var user = this.LoginUserinfo;
                    CRLogService.Insert(ActionType.Email, ActionSource.Admin, entity.Id, user.Id, user.Name, "发送邮件成功", "发送邮件成功");
                }
                catch (Exception ex)
                {
                    email.Remark = "发送邮件发生异常，异常信息：" + ex.Message;
                    return this.Content(JsonHelper.GetBaseMessage(false, "发送邮件失败"));
                }

                //保存发送邮件记录
                EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);
            }

            return this.Content(JsonHelper.GetBaseMessage(true, "发送邮件成功"));
            //return this.Content("");
        }


        #endregion
    }
}