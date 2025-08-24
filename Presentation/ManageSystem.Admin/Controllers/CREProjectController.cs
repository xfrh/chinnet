using ManageSystem.Core;
using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.CRE;
using ManageSystem.Services.Log;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class CREProjectController : AdminBaseController
    {
        private readonly ICreBackstageService  creBackstageService;

        private readonly ISystemLogService SystemLogService;

        public CREProjectController(ICreBackstageService _creBackstageService,ISystemLogService _systemLogService)
        {
            creBackstageService = _creBackstageService;
            SystemLogService = _systemLogService;
        }
        // GET: CREProject
        /// <summary>
        /// 数据列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult CREProjectList()
        {
            return View();
        }
        /// <summary>
        /// 数据列表方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CREProjectList(DataSourceRequest command, Toconfigure model)
        {
            IPagedList<Toconfigure> list = creBackstageService.GetToconfigures(model.Hospital, model.Data_year, model.Name, command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    select new
                                    {
                                        Data_id = x.Data_id,
                                        Cre_id=x.Cre_id,
                                        Hospital = x.Hospital,
                                        Name = x.Name,
                                        Data_year = x.Data_year,
                                        Data_season=x.Data_season,
                                        Germ=x.Germ,
                                        Germ_detected=x.Germ_detected,
                                        Cr_detected = x.Cr_detected,
                                        Carbapenemase_kpc=x.Carbapenemase_kpc,
                                        Carbapenemase_ndm=x.Carbapenemase_ndm,
                                        Created = x.Created.ToString(),
                                        Modified = x.Modified.ToString(),
                                        Created_by=x.Created_by                                      
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };
        }
        /// <summary>
        /// 数据编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CREProjectEdit(long? id)
        {
            var model = GetToconfigure(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult CREProjectEdit(Toconfigure toconfigure)
        {
            var user = this.LoginUserinfo;
            toconfigure.Created = DateTime.Now;
            toconfigure.Modified = DateTime.Now;
            toconfigure.Allow_report_display = 1;
            toconfigure.Isvalid = 1;
            toconfigure.Isaudited = 1;
            toconfigure.Audited_by = 999;
            toconfigure.Audited_time = DateTime.Now;
            int resul = creBackstageService.UpdateCre_data(toconfigure);
            if (resul > 0)
            {
               
                    string logContent = "修改数据成功，数据Cre_id:" + toconfigure.Cre_id;
                    ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, user.Id, user.Name, "修改CRE数据", logContent);
                    base.SuccessNotification(logContent);              
                return this.RedirectToAction("CREProjectList");              
            }         
            return this.View(GetToconfigure(toconfigure.Cre_id));
        }
        /// <summary>
        /// 数据详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CREProjectdetails(long? id)
        {
            var model = GetToconfigure(id);
            return View(model);
        }

        public Toconfigure GetToconfigure(long? id)
        {
            var model = creBackstageService.GetToconfigure(id);
            model.PercentageList = creBackstageService.GetData_Percentages().Select(x => { return new SelectListItem() { Text = x.Percentages, Value = x.Percentages }; }).ToList();           
            model.BacteriafenlList=creBackstageService.Getgerm_types().Select(x => { return new SelectListItem() { Text = x.Germ_name, Value = x.Germ_name };}).ToList();
            model.PercentageList.Insert(0, new SelectListItem() { Text = "请选择", Value = "" });          
            return model;
        }


        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Exportexcel(List<long> ids)
        {
            try
            {
                ExcelPackage ep = new ExcelPackage();
                string path = this.creBackstageService.Export(ids, ep);
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
        /// 项目数据 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CREDataDelete(string selectedIds)
        {
            var user = this.LoginUserinfo;
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("CREProjectList");
            }

            this.creBackstageService.Delete(selectedIds);

            string logContent = "【手动】删除CRE项目数据，删除的Cre_id集合:" + selectedIds.ToString();
            ActionLogService.Insert(ActionType.Delete, ActionSource.Admin, user.Id, user.Name, "删除CRE数据", logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("CREProjectList");
        }


        public ActionResult BacteriaList()
        {
            return View();
        }
        /// <summary>
        /// 细菌列表方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BacteriaList(DataSourceRequest command, Cre_germ_types model)
        {
            IPagedList<Cre_germ_types> list = creBackstageService.GetBacteriaList(model.Germ_name,command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    //where x.State==true
                                    select new
                                    {
                                        Germ_id = x.Germ_id,
                                        Ger_code = x.Ger_code,
                                        Germ_upload_name = x.Germ_upload_name,
                                        Germ_name = x.Germ_name,
                                        Cr_name = x.Cr_name,
                                        Sortid = x.Sortid,
                                        Allow_data_upload = x.Allow_data_upload ? "是" : "否",
                                        Allow_report_display = x.Allow_report_display ? "是" : "否",
                                        allow_edit = x.allow_edit ? "是" : "否",
                                        InsrtTime = x.InsrtTime.ToString()                                       
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult CreateBacteria()
        {
            return View(SetCreateBacteria());
        }

        [HttpPost]
        public ActionResult CreateBacteria(Cre_germ_types germ_Types)
        {
            germ_Types.InsrtTime = DateTime.Now;
            germ_Types.UpdateTime = null;
            germ_Types.Force_audit = 0;
                int result= creBackstageService.CreateBacteria(germ_Types);
            if (result > 0)
            {
                string logContent = "新增细菌成功，细菌名称： " + germ_Types.Germ_name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);
            }
            return View(SetCreateBacteria());
        }

        private Cre_germ_types SetCreateBacteria()
        {
            return new Cre_germ_types
            {              
                Sortid = creBackstageService.GetMaxsort()+1,
                //UrlName = "《点击下载》",
                Allow_data_upload = true,
                Allow_report_display = false,
                allow_edit=true
            };
        }
        /// <summary>
        /// 根据id删除细菌
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CREBacteriaDelete(string selectedIds)
        {
            var user = this.LoginUserinfo;
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("BacteriaList");
            }

            this.creBackstageService.DeleteBacteria(selectedIds);

            string logContent = "【手动】删除细菌，删除的id集合:" + selectedIds.ToString();
            ActionLogService.Insert(ActionType.Delete, ActionSource.Admin, user.Id, user.Name, "删除CRE细菌", logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("BacteriaList");
        }

        /// <summary>
        /// 细菌编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BacteriaEdit(long id)
        {
            var model = SetCre_germ_typesEditData(id);
            return View(model);
        }

        /// <summary>
        /// 细菌编辑方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BacteriaEdit(Cre_germ_types germ_Types)
        {
            germ_Types.UpdateTime = DateTime.Now;            
            var user = this.LoginUserinfo;
            int resul = 0;
            int Sort = SetCre_germ_typesEditData(germ_Types.Germ_id).Sortid;
            if (germ_Types.Sortid != Sort && creBackstageService.GetSort(germ_Types.Sortid) != null)
            {
                creBackstageService.UpdateSort(germ_Types.Sortid);
                resul = creBackstageService.UpdateBacteria(germ_Types);
            }
            else
            {
                resul = creBackstageService.UpdateBacteria(germ_Types);
            }
            if (resul > 0)
            {
                string logContent = "修改数据成功，数据Cre_id:" + germ_Types.Germ_id;
                ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, user.Id, user.Name, "修改CRE细菌", logContent);
                base.SuccessNotification(logContent);
                return this.RedirectToAction("BacteriaList");
            }
            return View(SetCre_germ_typesEditData(germ_Types.Germ_id));
        }
        /// <summary>
        /// 数据详情/编辑返填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Cre_germ_types SetCre_germ_typesEditData(long id)
        {
            var entity = creBackstageService.GetCre_germ_types(id);
            if (entity == null)
            {
                return SetCreateBacteria();
            }
            return entity;


        }
        /// <summary>
        /// 细菌详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Bacteriadetails(long id)
        {
            var model = SetCre_germ_typesEditData(id);
            return View(model);
        }
    }
}