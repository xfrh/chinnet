using ManageSystem.Admin.Models.Organism;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Organism;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Organism;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;
using Newtonsoft.Json;

namespace ManageSystem.Admin.Controllers
{
    /// <summary>
    /// 细菌分布图表
    /// </summary>
    public class OrganismChartController : AdminBaseController
    {
        //增删改代码才可参考   ArticlesController  的文章分类功能
        private readonly IMedicalOrganismService medicalOrganismService;
        private readonly IMedicalOrganismTypeService medicalOrganismTypeService;
        private readonly IBacteriaDetailedDataService bacteriaDetailedDataService;
        public OrganismChartController(
                IMedicalOrganismService _medicalOrganismService,
                IMedicalOrganismTypeService _medicalOrganismTypeService,
        IBacteriaDetailedDataService _bacteriaDetailedDataService
            )
        {
            medicalOrganismService = _medicalOrganismService;
            medicalOrganismTypeService = _medicalOrganismTypeService;
            bacteriaDetailedDataService = _bacteriaDetailedDataService;
        }

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>

        public ActionResult ChartList()
        {
            BacteriaModel model = this.SetBacteriaModel();

            return View(model);
        }

        /// <summary>
        /// 设置 ChartList 试图模型
        /// </summary>
        /// <returns></returns>
        private BacteriaModel SetBacteriaModel()
        {
            BacteriaModel model = new BacteriaModel();

            model.MedicalOrganism = this.medicalOrganismService.Query(m=>m.Mark>0).OrderBy(c => c.OrganismTypeId2).Select(c =>
            {
                return new SelectListItem { Text = c.Name, Value = c.Id.ToString() };
            }).ToList();
            model.MedicalOrganismTypeList = this.medicalOrganismTypeService.Query(m => m.Mark > 0 && m.ParentId > 0).Select(c =>
            {
                return new SelectListItem { Text = c.Name, Value = c.Id.ToString() };
            }).ToList();

            model.MedicalOrganism = new List<SelectListItem>();
            model.MedicalOrganismTypeList.Insert(0, new SelectListItem { Text = "选择分类", Value = "0" });

            model.MedicalOrganismId = 0;
            model.MedicalOrganism.Insert(0, new SelectListItem() { Text = "全部细菌", Value = "" });

            return model;
        }

        /// <summary>
        ///  查询细菌列表
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChartList(DataSourceRequest command, BacteriaModel model)
        {
            //获得数据
            var list = this.bacteriaDetailedDataService.PageQuery(model.Name, model.MedicalOrganismId, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(c => new BacteriaDetailedData
                {
                    Id = c.Id,
                    Name = c.Name,
                    Ecoff = c.Ecoff,
                    Distributions = c.Distributions,
                    Observations = c.Observations,
                    V0002 = c.V0002.Contains(":") ? c.V0002.Split(':')[1] : c.V0002,
                    V0004 = c.V0004.Contains(":") ? c.V0004.Split(':')[1] : c.V0004,
                    V0008 = c.V0008.Contains(":") ? c.V0008.Split(':')[1] : c.V0008,
                    V0016 = c.V0016.Contains(":") ? c.V0016.Split(':')[1] : c.V0016,
                    V0032 = c.V0032.Contains(":") ? c.V0032.Split(':')[1] : c.V0032,
                    V0064 = c.V0064.Contains(":") ? c.V0064.Split(':')[1] : c.V0064,
                    V0125 = c.V0125.Contains(":") ? c.V0125.Split(':')[1] : c.V0125,
                    V0025 = c.V0025.Contains(":") ? c.V0025.Split(':')[1] : c.V0025,
                    V0005 = c.V0005.Contains(":") ? c.V0005.Split(':')[1] : c.V0005,
                    V1001 = c.V1001.Contains(":") ? c.V1001.Split(':')[1] : c.V1001,
                    V1002 = c.V1002.Contains(":") ? c.V1002.Split(':')[1] : c.V1002,
                    V1004 = c.V1004.Contains(":") ? c.V1004.Split(':')[1] : c.V1004,
                    V1008 = c.V1008.Contains(":") ? c.V1008.Split(':')[1] : c.V1008,
                    V1016 = c.V1016.Contains(":") ? c.V1016.Split(':')[1] : c.V1016,
                    V1032 = c.V1032.Contains(":") ? c.V1032.Split(':')[1] : c.V1032,
                    V1064 = c.V1064.Contains(":") ? c.V1064.Split(':')[1] : c.V1064,
                    V1128 = c.V1128.Contains(":") ? c.V1128.Split(':')[1] : c.V1128,
                    V1256 = c.V1256.Contains(":") ? c.V1256.Split(':')[1] : c.V1256,
                    V1512 = c.V1512.Contains(":") ? c.V1512.Split(':')[1] : c.V1512
                }),
                Total = list.TotalCount
            };

            return Json(gridModel, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChartImport()
        {
            return View(this.SetImportData());
        }

        /// <summary>
        /// 设置导入页面的数据
        /// </summary>
        /// <returns></returns>
        private BacteriaModel SetImportData()
        {
            BacteriaModel model = new BacteriaModel();
            model.MedicalOrganismTypeList = this.medicalOrganismTypeService.Query(m => m.Mark > 0 && m.ParentId > 0).Select(c =>
            {
                return new SelectListItem { Text = c.Name, Value = c.Id.ToString() };
            }).ToList();

            model.MedicalOrganism = new List<SelectListItem>();
            model.MedicalOrganismTypeList.Insert(0, new SelectListItem { Text = "选择分类", Value = "0" });
            model.MedicalOrganism.Insert(0, new SelectListItem { Text = "选择细菌", Value = "0" });
            model.MedicalOrganismTypeId = 0;

            return model;
        }

        /// <summary>
        /// 根据细菌的二级分类加载细菌数据，用于绑定下拉列表
        /// </summary>
        /// <param name="typeId">细菌二级分类id</param>
        /// <returns></returns>
        [CheckRoleAttribute(true,false)]
        public ContentResult GetMedicalOrganism(int typeId)
        {
            if (typeId <= 0)
                return this.Content("");

            var list = this.medicalOrganismService.Query(m=>m.Mark>0 && m.OrganismTypeId2 == typeId).Select(c =>
            {
                return new SelectListItem { Text = c.Name, Value = c.Id.ToString() };
            }).ToList();
            return this.Content(JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 导入数据  保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChartImport(BacteriaModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_temp" + Path.GetExtension(model.DataFiles.FileName);
                    var filePath = Server.MapPath(string.Format("~/{0}", "Content/Upload/Organism"));
                    model.DataFiles.SaveAs(Path.Combine(filePath, fileName));
                    string tempFilePath = filePath + "\\" + fileName;
                    DataTable dt = ImportDataTable.ExcelToDataTable(tempFilePath, true);

                    List<BacteriaDetailedData> insertData = new List<BacteriaDetailedData>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BacteriaDetailedData entity = new BacteriaDetailedData
                        {
                            OrganismTypeId = model.MedicalOrganismId,
                            Mark = 1,
                            Describe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 导入",
                            InsertTime = DateTime.Now,
                            Version = 1,
                            Name = dt.Rows[i]["Name"].ToString(),
                            V0002 = dt.Rows[i]["0.002"].ToString(),
                            V0004 = dt.Rows[i]["0.004"].ToString(),
                            V0008 = dt.Rows[i]["0.008"].ToString(),
                            V0016 = dt.Rows[i]["0.016"].ToString(),
                            V0032 = dt.Rows[i]["0.032"].ToString(),
                            V0064 = dt.Rows[i]["0.064"].ToString(),
                            V0125 = dt.Rows[i]["0.125"].ToString(),
                            V0025 = dt.Rows[i]["0.25"].ToString(),
                            V0005 = dt.Rows[i]["0.5"].ToString(),
                            V1001 = dt.Rows[i]["1"].ToString(),
                            V1002 = dt.Rows[i]["2"].ToString(),
                            V1004 = dt.Rows[i]["4"].ToString(),
                            V1008 = dt.Rows[i]["8"].ToString(),
                            V1016 = dt.Rows[i]["16"].ToString(),
                            V1032 = dt.Rows[i]["32"].ToString(),
                            V1064 = dt.Rows[i]["64"].ToString(),
                            V1128 = dt.Rows[i]["128"].ToString(),
                            V1256 = dt.Rows[i]["256"].ToString(),
                            V1512 = dt.Rows[i]["512"].ToString(),
                            Ecoff = dt.Rows[i]["ECOFF"].ToString(),
                            Distributions = dt.Rows[i]["Distributions"].ToString(),
                            Observations = dt.Rows[i]["Observations"].ToString()

                        };
                        //dt.Rows[i][""].ToString()
                        insertData.Add(entity);
                    }

                    if (insertData.Count > 0)
                    {
                        this.bacteriaDetailedDataService.Insert(insertData);
                    }

                    string logContent = "批量导入数据成功，总共导入 " + insertData.Count + "条";
                    base.InsetActionLog(ActionType.Import, logContent);
                    base.SuccessNotification(logContent);

                    this.DeleteTempExcelFile(tempFilePath);
                }
                catch (Exception ex)
                {
                    base.ErrorNotification("对不起，导入失败！原因：" + ex.Message);
                }
                finally
                {

                    this.RedirectToAction("ChartImport");
                }
            }

            long typeId = model.MedicalOrganismTypeId;
            HttpPostedFileBase file = model.DataFiles;
            model = this.SetImportData();

            return View(model);
        }

        /// <summary>
        /// 删除 零时EXCEL 文件
        /// </summary>
        /// <param name="path"></param>
        public void DeleteTempExcelFile(string path)
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                System.IO.File.Delete(path);
            }
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChartDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ArticleTypeList");
            }

            this.bacteriaDetailedDataService.Delete(selectedIds);

            string logContent = "删除细菌详细信息，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ChartList");


        }

        /// <summary>
        ///  编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ChartEdit(long id)
        {
            BacteriaDetailedDataModel m = SetEditBacteriaDetailedData(id);
            return View(m);
        }

        /// <summary>
        /// 设置编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private BacteriaDetailedDataModel SetEditBacteriaDetailedData(long id)
        {
            return this.bacteriaDetailedDataService.QueryEntity(id).ToModel();
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChartEdit(BacteriaDetailedDataModel model)
        {

            if (ModelState.IsValid)
            {
                var entity = this.bacteriaDetailedDataService.QueryEntity(model.Id);


                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.bacteriaDetailedDataService.Update(entity);

                string logContent = "修改细菌信息成功，细菌名称： " + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ChartList");
            }
            return this.View(this.SetEditBacteriaDetailedData(model.Id));
        }

    }

}