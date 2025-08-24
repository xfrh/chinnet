using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Admin.Extensions;
using ManageSystem.Framework.Controllers;
using ManageSystem.Framework.Kendoui;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Core;
using ManageSystem.Services.Medicine;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Core.Domain.Medicine;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Services.SystemSet;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ManageSystem.Admin.App_Start;
using ManageSystem.Services.Users;
using ManageSystem.Services.Members;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Messages;
using OfficeOpenXml.Style;
using System.Drawing;
using ManageSystem.Services.Survey;
using ManageSystem.Admin.Models.Survey;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Newtonsoft.Json;

namespace ManageSystem.Admin.Controllers
{

    public class SurveySurveyController : AdminBaseController
    {
        private readonly ISurveySurveyService surveySurveyService;
        private readonly ISystemLogService SystemLogService;
        private readonly ISurveyRecordService surveyRecordService;



        public SurveySurveyController(ISurveySurveyService _surveySurveyService, ISystemLogService _systemLogService, ISurveyRecordService _surveyRecordService)
        {
            this.surveySurveyService = _surveySurveyService;
            this.SystemLogService = _systemLogService;
            this.surveyRecordService = _surveyRecordService;
        }


        #region 问卷调查数据

        /// <summary>
        /// 问卷调查数据 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyDataList()
        {
            SurveySurveyModel model = new SurveySurveyModel();
            model.StateList = new List<SelectListItem>(){
                   new SelectListItem { Text = "启用", Value = "1" },
                   new SelectListItem { Text = "禁用", Value = "2" },
         };
            model.RoodList = new List<SelectListItem>(){
                   new SelectListItem { Text = "开放问卷", Value = "1" },
                   new SelectListItem { Text = "隐藏问卷", Value = "2" },
         };
            return View(model);
        }

        /// <summary>
        /// 问卷调查数据 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SurveyDataList(DataSourceRequest command, SurveySurveyModel model)
        {
            //获得数据
            var list = this.surveySurveyService.QueryPage(model.Name, 0, command.Page - 1, command.PageSize);
            string serviceEmail = ConfigHelper.GetConfigString("FileWebUrl");
            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.Name,
                        QR = x.QR,
                        OpenTime = x.StartTime.ToString("yyyy.MM.dd HH:mm") + " - " + x.EndTime.ToString("yyyy.MM.dd HH:mm"),
                        Rood = x.Rood == 1 ? "开放问卷" : x.Rood == 2 ? "隐私问卷" : "",
                        Remark = x.Remark,
                        Sort = x.Sort,
                        State = x.State == 1 ? "启用" : x.State == 2 ? "禁用" : "",
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="surveyId">调查报告ID</param>
        /// <param name="content29">根据医院查询</param>
        /// <param name="content30">根据姓名查询</param>
        /// <param name="content31">根据手机号查询</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SurveyDataViewList(DataSourceRequest command, long surveyId, string content29, string content30, string content31)
        {
            //获得数据
            var list = this.surveyRecordService.QueryPage(surveyId, content29, content30, content31, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Content29 = x.Content29,
                        Content30 = x.Content30,
                        Content31 = x.Content31,
                        MemberId = x.MemberId,
                        MemberName = x.MemberName,
                        MemberPhone = x.MemberPhone,
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        #region  新增
        public ActionResult SurveyCreate()
        {
            SurveySurveyModel model = new SurveySurveyModel();
            model.StartTime = DateTime.Now;
            model.EndTime = DateTime.Now;
            model.StateList = new List<SelectListItem>(){
                   new SelectListItem { Text = "启用", Value = "1" },
                   new SelectListItem { Text = "禁用", Value = "2" },
         };
            model.RoodList = new List<SelectListItem>(){
                   new SelectListItem { Text = "开放问卷", Value = "1" },
                   new SelectListItem { Text = "隐藏问卷", Value = "2" },
         };
            return View(model);
        }

        /// <summary>
        /// 问卷调查 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public ActionResult SurveyCreate(SurveySurveyModel model)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    base.ErrorNotification("请输入调查名称");
                    return this.RedirectToAction("SurveyCreate");
                }


                if (string.IsNullOrWhiteSpace(model.StartTime.ToString()))
                {
                    base.ErrorNotification("请输入开始时间");
                    return this.RedirectToAction("SurveyCreate");
                }
                if (string.IsNullOrWhiteSpace(model.EndTime.ToString()))
                {
                    base.ErrorNotification("请输入结束时间");
                    return this.RedirectToAction("SurveyCreate");
                }
                if (string.IsNullOrEmpty(model.Remark))
                {
                    model.Remark = "";
                }

                var entity = model.ToEntity();

                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                entity.Id = CommonHelper.GuidToLongID;

                entity.QR = $@"{ConfigHelper.GetConfigString("m.chinet.com").TrimEnd('/')}/survey?id={entity.Id}";
                string filePath = $"/Content/Survey/QrCode/{entity.Id}.png";
                if (GenerateQrCode(entity.QR, filePath))
                {
                    entity.QR = filePath;
                }
                else
                {
                    base.ErrorNotification("生成问卷二维码失败");
                    return this.RedirectToAction("SurveyCreate");
                }

                this.surveySurveyService.Insert(entity);

                string logContent = "添加问卷调查名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("SurveyDataList");
            }

            return this.View(this.SetSurveyEditData(model.Id));
        }

        #endregion

        #region 编辑 
        /// <summary>
        /// 编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public SurveySurveyModel SetSurveyEditData(long id)
        {
            var entity = this.surveySurveyService.QueryEntity(id);

            var model = entity.ToModel();
            model.StateList = new List<SelectListItem>(){
                   new SelectListItem { Text = "启用", Value = "1" },
                   new SelectListItem { Text = "禁用", Value = "2" },
         };
            model.RoodList = new List<SelectListItem>(){
                   new SelectListItem { Text = "开放问卷", Value = "1" },
                   new SelectListItem { Text = "隐藏问卷", Value = "2" },
         };
            return model;
        }


        /// <summary>
        /// 问卷调查 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyEdit(long id)
        {
            return this.View(this.SetSurveyEditData(id));
        }

        /// <summary>
        /// 问卷调查 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public ActionResult SurveyEdit(SurveySurveyModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.surveySurveyService.QueryEntity(model.Id);

                if (entity == null || entity.Id <= 0)
                {
                    base.ErrorNotification("数据不存在，请重新打开");
                    return this.RedirectToAction("SurveyEdit/" + model.Id);
                }

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    base.ErrorNotification("请输入调查名称");
                    return this.RedirectToAction("SurveyEdit/" + model.Id);
                }

                //if (string.IsNullOrWhiteSpace(model.QR))
                //{
                //    base.ErrorNotification("请输入问卷二维码");
                //    return this.RedirectToAction("SurveyEdit/" + model.Id);
                //}

                if (model.State == 0)
                {
                    base.ErrorNotification("请选择状态");
                    return this.RedirectToAction("SurveyEdit/" + model.Id);
                }
                if (model.Rood == "0")
                {
                    base.ErrorNotification("请选择状态");
                    return this.RedirectToAction("SurveyEdit/" + model.Rood);
                }

                if (string.IsNullOrWhiteSpace(model.StartTime.ToString()))
                {
                    base.ErrorNotification("请输入开始时间");
                    return this.RedirectToAction("SurveyEdit/" + model.Id);
                }


                if (string.IsNullOrWhiteSpace(model.EndTime.ToString()))
                {
                    base.ErrorNotification("请输入结束时间");
                    return this.RedirectToAction("SurveyEdit/" + model.Id);
                }

                if (string.IsNullOrWhiteSpace(model.Remark)) model.Remark = "";


                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                if (string.IsNullOrWhiteSpace(entity.QR))
                {
                    entity.QR = $@"{ConfigHelper.GetConfigString("m.chinet.com").Trim('/')}/survey?id={entity.Id}";
                    string filePath = $"/Content/Survey/QrCode/{entity.Id}.png";
                    if (GenerateQrCode(entity.QR, filePath))
                    {
                        entity.QR = filePath;
                    }
                    else
                    {
                        base.ErrorNotification("生成问卷二维码失败");
                        return this.RedirectToAction($"SurveyEdit/{model.Id}");
                    }
                }


                this.surveySurveyService.Update(entity);

                string logContent = "修改问卷调查，名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("SurveyEdit", new { id = entity.Id.ToString() });
            }

            return this.View(this.SetSurveyEditData(model.Id));
        }

        #endregion

        #region 查看
        /// <summary>
        /// 问卷调查数据 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyDetailView(long id)
        {
            return this.View(this.SetSurveyDetailData(id));
        }
        /// <summary>
        /// 问卷调查数据 查看页面的数据
        /// </summary>
        /// <returns></returns>
        public SurveyRecordModel SetSurveyDetailData(long id)
        {


            var entity = this.surveyRecordService.QueryEntity(m => m.Id == id && m.Mark > 0);
            var model = entity.ToModel();

            return model;
        }


        /// <summary>
        /// 问卷调查数据 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyDataView(long id)
        {
            if (id <= 0)
            {
                return this.RedirectToAction("SurveyDataList");
            }

            return this.View(this.SetSurveyDataEditData(id));
        }

        /// <summary>
        /// 问卷调查数据 查看页面的数据
        /// </summary>
        /// <returns></returns>
        public SurveySurveyModel SetSurveyDataEditData(long id)
        {


            var entity = this.surveySurveyService.QueryEntity(m => m.Id == id && m.Mark > 0);
            var model = entity.ToModel();
            model.StateList = new List<SelectListItem>(){
                   new SelectListItem { Text = "启用", Value = "1" },
                   new SelectListItem { Text = "禁用", Value = "2" },
         };
            model.RoodList = new List<SelectListItem>(){
                   new SelectListItem { Text = "开放问卷", Value = "1" },
                   new SelectListItem { Text = "隐藏问卷", Value = "2" },
         };

            return model;
        }

        #endregion

        #region 删除
        /// <summary>
        /// 问卷调查数据 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SurveyDelete(string selectedIds)
        {
            var user = this.LoginUserinfo;
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("SurveyDataList");
            }

            this.surveySurveyService.Delete(selectedIds);

            string logContent = "【手动】删除问卷调查数据，删除的id集合:" + selectedIds;
            //SurveyLogService.Insert(ActionType.Delete, ActionSource.Admin, 0, user.Id, user.Name, "删除数据", logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SurveyDataList");
        }
        #endregion

        #endregion

        #region 导出问卷       
        /// <summary>
        /// 供应商表 导出Excel文件
        /// </summary>
        /// <param name="parameter">查询条件json数据</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ActionResult SurveyExport(string parameter)
        {
            try
            {
                SurveyDeteilModel model = new SurveyDeteilModel();
                if (!string.IsNullOrWhiteSpace(parameter))
                    model = JsonConvert.DeserializeObject<SurveyDeteilModel>(parameter);

                ExcelPackage ep = new ExcelPackage();
                string path = this.surveySurveyService.Export(ep, model.SurveyId, model.Content29, model.Content30, model.Content31);
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

        #region 生成二维码
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="saveFilePath"></param>
        /// <param name="moduleSize"></param>
        /// <returns></returns>
        private bool GenerateQrCode(string content, string saveFilePath, int moduleSize = 9)
        {
            try
            {
                var encoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode = encoder.Encode(content);

                GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Two), Brushes.Black, Brushes.White);

                DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
                Bitmap map = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
                Graphics g = Graphics.FromImage(map);
                render.Draw(g, qrCode.Matrix);
                string savePath = Server.MapPath(saveFilePath);
                if (!System.IO.Directory.Exists(Path.GetDirectoryName(savePath)))
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                }
                map.Save(savePath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion
    }
}
