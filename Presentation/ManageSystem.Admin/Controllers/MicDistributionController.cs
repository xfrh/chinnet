using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models.MicdataDistribution;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.MicdataDistribution;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.MicdataDistribution;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace ManageSystem.Admin.Controllers
{
    public class MicDistributionController : AdminBaseController
    {
        private readonly IddDocumentService  ddDocuments;
        private readonly IddYearService ddYears;
        private readonly IddGermService ddGerms;
        private readonly IddAntibioticService ddAntibiotics;
        private readonly IddDocumentItemService ddDocumentItems;
        public MicDistributionController(IddDocumentService _ddDocuments, IddYearService _ddYears, IddGermService _ddGerms, IddAntibioticService _ddAntibiotics,IddDocumentItemService _ddDocumentItems)
        {
            ddDocuments = _ddDocuments;
            ddYears = _ddYears;
            ddGerms = _ddGerms;
            ddAntibiotics = _ddAntibiotics;
            ddDocumentItems = _ddDocumentItems;
        }
        // GET: MicDistribution
        #region 数据管理
        public ActionResult MicDistributionlist()
        {
            ddDocumentModel model = new ddDocumentModel();
            model.YearList= this.ddYears.GetDdYears().OrderByDescending(m => m.sortid).Select(x => { return new SelectListItem() { Text = x.title, Value = x.year_id.ToString() }; }).ToList();
            model.YearList.Insert(0, new SelectListItem() { Text = "全部年份", Value ="" });
            return View(model);
        }

        /// <summary>
        /// 数据列表方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MicDistributionlist(DataSourceRequest command, ddDocument model)
        {
            IPagedList<ddDocument> list = ddDocuments.GetddDocument(model.title, model.year_id,command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    select new
                                    {
                                        document_id = x.document_id.ToString(),
                                        hospital_id = x.hospital_id,
                                        category_id = x.category_id,
                                        year = ddYears.GetDdYear(x.year_id),
                                        germ_id = x.germ_id,
                                        antibiotic_id = x.antibiotic_id,
                                        title = x.title,
                                        file_path = x.file_path,
                                        datacount = x.datacount,
                                        created = x.created.ToString(),
                                        username= ddDocuments.GetUsers(x.created_by)
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult DocumentCreate()
        {
            ddDocumentModel model = new ddDocumentModel();
            model.YearList = this.ddYears.GetDdYears().OrderByDescending(m => m.sortid).Select(x => { return new SelectListItem() { Text = x.title, Value = x.year_id.ToString() }; }).ToList();
            model.YearList.Insert(0, new SelectListItem() { Text = "请选择", Value = "" });
            return View(model);
        }

        [HttpPost, CheckRole(CheckRole = false)]
        public JsonResult OnSubmit_DocumentCreate(string id, HttpPostedFileBase file = null)
        {
            #region 简单验证          
            if (id=="")
            {
                return Json(new
                {
                    status = false,
                    message = "请选择年份"
                });
            }
            ddYear year = ddYears.QueryEntity(long.Parse(id));
            if (year == null || year.year_id <= 0 || year.isvalid != true)
            {
                return Json(new
                {
                    status = false,
                    message = "您所选择的年份不可用"
                });
            }

            if (file == null || file.ContentLength <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "请选择上传文件"
                });
            }
            string fileExt = System.IO.Path.GetExtension(file.FileName).ToUpperInvariant();
            List<string> support = new List<string> { ".xls", ".xlsx", ".XLS", ".XLSX", "xls", "xlsx", "XLS", "XLSX" };
            if (!support.Contains(fileExt))
            {
                return Json(new
                {
                    status = false,
                    message = "选择的上传文件格式不支持，仅支持.xls或.xlsx"
                });
            }
            #endregion

            try
            {
                          
                DataTable dt = new DataTable();
                var v = file.ContentLength;
                var v2 = file.InputStream;
                var v3 = file.InputStream.Length;
                Stream streamfile = file.InputStream;
                #region 读取文件数据
                if (fileExt.Equals(".XLSX"))
                {
                    XSSFWorkbook hssfworkbook = new XSSFWorkbook(streamfile);
                    ISheet sheet = hssfworkbook.GetSheetAt(0);
                    IEnumerator rows = sheet.GetRowEnumerator();
                    //IEnumerator rowss = sheet
                    IRow firstRow = sheet.GetRow(0);//第一行
                    for (int i = 0; i < firstRow.Cells.Count; i++)
                    {
                        dt.Columns.Add(firstRow.Cells[i].StringCellValue);
                    }
                    while (rows.MoveNext())
                    {
                        XSSFRow row = (XSSFRow)rows.Current;
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            ICell cell = row.GetCell(i);                           
                            if (cell == null)
                            {
                                dr[i] = null;
                            }
                            else
                            {
                                dr[i] = cell.ToString();
                            }
                        }
                        dt.Rows.Add(dr);
                    }

                    //ExcelPackage package = new ExcelPackage(file.InputStream);
                    //ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//选定 指定页
                    //dt = EPPlusHelper.WorksheetToTable(worksheet);
                }
                else if (fileExt.Equals(".XLS"))
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(streamfile);
                    ISheet sheet = hssfworkbook.GetSheetAt(0);
                    IEnumerator rows = sheet.GetRowEnumerator();
                    IRow firstRow = sheet.GetRow(0);//第一行
                    for (int i = 0; i < firstRow.Cells.Count; i++)
                    {
                        dt.Columns.Add(firstRow.Cells[i].StringCellValue);
                    }
                    while (rows.MoveNext())
                    {
                        HSSFRow row = (HSSFRow)rows.Current;
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < row.LastCellNum; i++)
                        {
                            ICell cell = row.GetCell(i);
                            if (cell == null)
                            {
                                dr[i] = null;
                            }
                            else
                            {
                                dr[i] = cell.ToString();
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "选择的文件不支持，仅支持.xls或.xlsx"
                    });
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    return Json(new
                    {
                        status = false,
                        message = "请使用指定的“标准模板.xlsx”上传数据"
                    });
                }
                #endregion
                string pahe = Path.Combine(Server.MapPath("/Content/Upload/MicDistrbution/"));
                //判断路径是否存在
                if (!Directory.Exists(pahe))
                {
                    Directory.CreateDirectory(pahe);
                }
                ddDocument model = new ddDocument();
                model.document_id= CommonHelper.GuidToLongID;
                model.hospital_id = null;
                model.year_id = long.Parse(id);
                model.title = file.FileName;
                model.file_path = "/Content/Upload/MicDistrbution/" + file.FileName;
                model.isvalid = true;
                model.created = DateTime.Now;
                model.created_by = LoginUserinfo.Id;
                model.datacount =dt.Rows.Count-1;
                model.antibiotic_id = null;
                model.germ_id = null;
                model.isvalid = true;
                model.sortid = 0;                
                string  title = "";
                string content = "";
                ddDocumentItem entity = new ddDocumentItem();
                entity.document_id = model.document_id;
                entity.isvalid = true;
                int result1 = 0;
                int result2 = 0;
                ///循环得到表头
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (model.header == null)
                    {
                        model.header = dt.Columns[i].ColumnName;
                    }
                    else
                    {
                        model.header +=","+ dt.Columns[i].ColumnName;
                    }
                    if (i >= 7)
                    {
                        title += dt.Columns[i].ColumnName + ',';
                    }
                }               
                //循环查询抗生素
                string[] Antibiotic = title.Split(',');
                for (int i = 0; i < Antibiotic.Length-1; i++)
                {
                    string biotica = Antibiotic[i].ToString().Replace("_NE", "").Replace("_NM", "").Replace("_ND10", "").Replace("_ND30", "").Replace("_ND20", "").Replace("_ND40", "");
                    if (ddAntibiotics.QueryEntityCode(biotica) != null)
                    {
                        ddAntibiotics.UpdateddAntibioticMark(biotica);
                    }
                    else
                    {
                        return Json(new
                        {
                            status = false,
                            type="抗生素",
                            message = "抗生素：" + Antibiotic[i].ToString() + "不存在，是否前去添加？"
                        });
                    }
                }              
                //循环添加数据
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Rows[i].ItemArray.Length; j++)
                    {
                        entity.patinet_id = dt.Rows[i].ItemArray[0].ToString();
                        entity.ward = dt.Rows[i].ItemArray[1].ToString();
                        entity.specimem = dt.Rows[i].ItemArray[2].ToString();
                        entity.specimem_date = dt.Rows[i].ItemArray[3].ToString();
                        entity.Specimen_Type = dt.Rows[i].ItemArray[4].ToString();
                        entity.organism = dt.Rows[i].ItemArray[5].ToString();
                        if (entity.organism == "")
                        {
                            continue;
                        }
                        entity.organism_Type = dt.Rows[i].ItemArray[6].ToString();
                        if (model.organism == null)
                        {
                           model.organism = dt.Rows[i].ItemArray[5].ToString();
                        }
                        else if (!model.organism.Contains(dt.Rows[i].ItemArray[5].ToString()))
                        {
                            model.organism += "," + dt.Rows[i].ItemArray[5].ToString();
                        }                      
                        if (j >= 7)
                        {
                            string value = dt.Rows[i].ItemArray[j].ToString().Replace("<=.", "0.").Replace("<=", "").Replace("<.", "0.").Replace("<", "").Replace(">=.", "0.").Replace(">=", "").Replace(">.", "0.").Replace(">", "");
                           
                            if (Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$"))
                            {
                                if (!dt.Rows[i].ItemArray[j].ToString().Contains("<") && !dt.Rows[i].ItemArray[j].ToString().Contains(">"))
                                {
                                    content += dt.Rows[i].ItemArray[j].ToString().Replace(".", "0.") + ",";
                                }
                                else
                                {
                                    content += dt.Rows[i].ItemArray[j].ToString() + ",";
                                }
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = false,
                                    type = "数据值",
                                    message = "文件中第【" + i + "】行第【" + j + "】列数据【" + dt.Rows[i].ItemArray[j].ToString() + "】格式不正确，请更正后重新上传。"
                                }) ;
                            }                           
                        }
                    }
                    entity.antibiotics = title;
                    entity.antibiotics_value = content;
                    result2 = ddDocumentItems.ddDocumentItemCreate(entity);
                    content = "";
                }
                string[] organism = model.organism.Split(',');
                for (int i = 0; i < organism.Length; i++)
                {
                    if (ddGerms.QueryEntityCode(organism[i].ToString()) != null)
                    {
                        ddGerms.UpdateddGermMark(organism[i].ToString());
                    }
                    else
                    {
                        ddDocumentItems.deleteDocumentItem(organism[i].ToString());
                        return Json(new
                        {
                            status = false,
                            type = "细菌",
                            message = "细菌：" + organism[i].ToString() + "不存在，是否前去添加"
                        });
                    }
                }
                result1 = ddDocuments.ddDocumentCreate(model);
                file.SaveAs(Path.Combine(pahe, file.FileName));
                if (result1 > 0 && result2 > 0)
                {
                    return Json(new
                    {
                        status = true,
                        message = $"文件上传成功"
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "文件上传异常"
                    });
                }

            }
            catch (Exception)
            {
                throw;
                //return Json(new
                //{
                //    status = false,
                //    message = "读取文件失败，请使用指定导入模板文件上传"
                //});
            }           
        }

        /// <summary>
        /// 医学数据 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocumentDataDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MedicalDataList");
            }

            this.ddDocuments.deleteDocument(selectedIds);

            string logContent = "【手动】删除医学数据，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MicDistributionlist");
        }
        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DocumentDataView(long Id)
        {
            var entity = this.ddDocuments.Document(Id);
            ViewBag.year = ddYears.GetDdYear(entity.year_id);
            ddDocumentModel model = new ddDocumentModel();
            model.title = entity.title;
            model.file_path = entity.file_path;
            model.created = entity.created;
            model.datacount = entity.datacount.ToString();
            model.header = entity.header;
            model.document_id = entity.document_id.ToString();
            return this.View(model);
        }
        /// <summary>
        /// 获取详细的分页数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataId">>对应的医学数据Id</param>
        /// <returns></returns>
        [CheckRole(true, false)]
        public JsonResult GetDataList(DataSourceRequest command, long dataId)
        {
            var IntegraIItemList = this.ddDocumentItems.QueryPage(dataId, command.Page - 1, command.PageSize);
            return Json(new { total = IntegraIItemList.TotalPages, rows = IntegraIItemList });
        }
        ///// <summary>
        ///// 详情页数据编辑
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //public ActionResult DocumentDataEdit(long Id)
        //{
        //    var entity = this.ddDocuments.Document(Id);
        //    ViewBag.year = ddYears.GetDdYear(entity.year_id);
        //    ddDocumentModel model = new ddDocumentModel();
        //    model.title = entity.title;
        //    model.file_path = entity.file_path;
        //    model.created = entity.created;
        //    model.datacount = entity.datacount.ToString();
        //    model.header = entity.header;
        //    model.document_id = entity.document_id.ToString();
        //    return this.View(model);
        //}
        #endregion

        #region 细菌管理
        /// <summary>
        /// 细菌列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ddGermList()
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
        public JsonResult ddGermList(DataSourceRequest command, ddGerm model)
        {
            IPagedList<ddGerm> list = ddGerms.GetddGerms(model.title,model.code, command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    select new
                                    {
                                        germ_id = x.germ_id.ToString(),
                                        group_id = x.group_id,
                                        mark = x.mark,
                                        title = x.title,
                                        title_en = x.title_en,
                                        code = x.code,
                                        sortid = x.sortid,
                                        isvalid = x.isvalid,
                                        created = x.created.ToString(),
                                        username = ddDocuments.GetUsers(x.created_by)
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };
        }

        /// <summary>
        /// 细菌添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GermCreate()
        {
            return View();
        }
        /// <summary>
        /// 细菌 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GermCreate(ddGermModel model)
        {
            try
            {         
                ddGerm entity = new ddGerm();
                entity.group_id = CommonHelper.GuidToLongID;
                entity.mark = model.mark;
                entity.title = model.title;
                entity.title_en = model.title_en;
                entity.code = model.code;
                entity.sortid = model.sortid;
                entity.isvalid = model.isvalid;
                entity.isdefault = model.isdefault;
                entity.created = DateTime.Now;
                entity.created_by = base.LoginUserinfo.Id;

                if (ddGerms.GetSort(entity.sortid) != null)
                {
                    ddGerms.UpdateSort(entity.sortid);

                    if (this.ddGerms.insertGerms(entity) > 0)
                    {
                        string logContent = "添加细菌，细菌名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "添加细菌，细菌名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
                else
                {
                    if (this.ddGerms.insertGerms(entity) > 0)
                    {
                        string logContent = "添加细菌，细菌名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "添加细菌，细菌名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return this.RedirectToAction("GermCreate");
        }

        /// <summary>
        /// 细菌编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateEdit(long id)
        {
            ddGermModel model = new ddGermModel();
            var entity = this.ddGerms.QueryEntity(id);
            model.germ_id = entity.germ_id;
            model.group_id = entity.group_id;
            model.mark = entity.mark;
            model.title = entity.title;
            model.title_en = entity.title_en;
            model.code = entity.code;
            model.created = entity.created;
            model.created_by = entity.created_by;
            model.isvalid = entity.isvalid;
            model.isdefault = entity.isdefault;
            model.sortid = entity.sortid;

            return View(model);
        }

        /// <summary>
        /// 细菌 编辑 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(ddGermModel model)
        {
            try
            {
                ddGerm entity = new ddGerm();
                entity.germ_id = model.germ_id;
                entity.group_id = model.group_id;
                entity.mark = model.mark;
                entity.title = model.title;
                entity.title_en = model.title_en;
                entity.code = model.code;
                entity.sortid = model.sortid;
                entity.isvalid = model.isvalid;
                entity.isdefault = model.isdefault;
                entity.created = DateTime.Now;
                entity.created_by = base.LoginUserinfo.Id;

                if (ddGerms.GetSort(entity.sortid) != null)
                {
                    ddGerms.UpdateSort(entity.sortid);

                    if (this.ddGerms.UpdateddGerm(entity) > 0)
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
                else
                {
                    if (this.ddGerms.UpdateddGerm(entity) > 0)
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return this.RedirectToAction("ddGermList");
        }

        ///// <summary>
        ///// 项目数据 删除
        ///// </summary>
        ///// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CREDataDelete(string selectedIds)
        //{
        //    var user = this.LoginUserinfo;
        //    if (string.IsNullOrWhiteSpace(selectedIds))
        //    {
        //        base.ErrorNotification("请选择需要删除的数据");
        //        return this.RedirectToAction("ddGermList");
        //    }

        //    this.creBackstageService.Delete(selectedIds);

        //    string logContent = "删除CRE项目数据，删除的Cre_id集合:" + selectedIds.ToString();
        //    ActionLogService.Insert(ActionType.Delete, ActionSource.Admin, user.Id, user.Name, "删除CRE数据", logContent);
        //    base.SuccessNotification(logContent);

        //    return this.RedirectToAction("ddGermList");
        //}

        #endregion

        #region 抗生素管理
        /// <summary>
        /// 抗生素列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ddAntibioticList()
        {
            return View();
        }
        /// <summary>
        /// 抗生素列表方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ddAntibioticList(DataSourceRequest command, ddAntibiotic model)
        {
            IPagedList<ddAntibiotic> list = ddAntibiotics.Antibioticslist(model.title, model.code, command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    select new
                                    {
                                        antibiotic__id = x.antibiotic__id.ToString(),
                                        group_id = x.group_id,
                                        mark = x.mark,
                                        title = x.title,
                                        title_en = x.title_en,
                                        code = x.code,
                                        sortid = x.sortid,
                                        isvalid = x.isvalid,
                                        created = x.created.ToString(),
                                        username = ddDocuments.GetUsers(x.created_by)
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };
        }

        /// <summary>
        /// 抗生素添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AntibioticCreate()
        {
            return View();
        }
        /// <summary>
        /// 抗生素 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AntibioticCreate(ddAntibioticModel model)
        {
            try
            {
                ddAntibiotic entity = new ddAntibiotic();
                entity.group_id = model.group_id;
                entity.mark = model.mark;
                entity.title = model.title;
                entity.title_en = model.title_en;
                entity.code = model.code;
                entity.sortid = model.sortid;
                entity.isvalid = model.isvalid;
                entity.isdefault = model.isdefault;
                entity.created = DateTime.Now;
                entity.created_by = base.LoginUserinfo.Id;

                if (ddAntibiotics.GetSort(entity.sortid) != null)
                {
                    ddAntibiotics.UpdateSort(entity.sortid);

                    if (this.ddAntibiotics.insertAntibiotic(entity) > 0)
                    {
                        string logContent = "添加抗生素，抗生素名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "添加抗生素，抗生素名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
                else
                {
                    if (this.ddAntibiotics.insertAntibiotic(entity) > 0)
                    {
                        string logContent = "添加抗生素，抗生素名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "添加抗生素，抗生素名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return this.RedirectToAction("AntibioticCreate");
        }
        /// <summary>
        /// 抗生素编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AntibioticEdit(long id)
        {
            ddAntibioticModel model = new ddAntibioticModel();
            var entity = this.ddAntibiotics.QueryEntity(id);
            model.antibiotic__id = entity.antibiotic__id;
            model.group_id = entity.group_id;
            model.mark = entity.mark;
            model.title = entity.title;
            model.title_en = entity.title_en;
            model.code = entity.code;
            model.created = entity.created;
            model.created_by = entity.created_by;
            model.isvalid = entity.isvalid;
            model.isdefault = entity.isdefault;
            model.sortid = entity.sortid;

            return View(model);
        }

        /// <summary>
        /// 抗生素 编辑 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AntibioticEdit(ddAntibioticModel model)
        {
            try
            {
                ddAntibiotic entity = new ddAntibiotic();
                entity.antibiotic__id = model.antibiotic__id;
                entity.group_id = model.group_id;
                entity.mark = model.mark;
                entity.title = model.title;
                entity.title_en = model.title_en;
                entity.code = model.code;
                entity.sortid = model.sortid;
                entity.isvalid = model.isvalid;
                entity.isdefault = model.isdefault;
                entity.created = DateTime.Now;
                entity.created_by = base.LoginUserinfo.Id;

                if (ddAntibiotics.GetSort(entity.sortid) != null)
                {
                    ddAntibiotics.UpdateSort(entity.sortid);

                    if (this.ddAntibiotics.UpdateAntibiotic(entity) > 0)
                    {
                        string logContent = "修改抗生素，抗生素名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "修改抗生素，抗生素名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
                else
                {
                    if (this.ddAntibiotics.UpdateAntibiotic(entity) > 0)
                    {
                        string logContent = "修改抗生素，抗生素名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "修改抗生素，抗生素名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return this.RedirectToAction("ddAntibioticList");
        }
        #endregion

        #region 年份管理
        /// <summary>
        /// 抗生素列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ddYearList()
        {
            return View();
        }
        /// <summary>
        /// 抗生素列表方法
        /// </summary>
        /// <param name="command"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ddYearList(DataSourceRequest command, ddYear model)
        {
            IPagedList<ddYear> list = ddYears.GetddYears(model.title, command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    select new
                                    {
                                        year_id = x.year_id.ToString(),
                                        title = x.title,                                   
                                        sortid = x.sortid,
                                        isvalid = x.isvalid==true?"是":"否",
                                        created = x.created.ToString(),
                                        username = ddDocuments.GetUsers(x.created_by)
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };
        }

        /// <summary>
        /// 抗生素添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult YearCreate()
        {
            return View();
        }
        /// <summary>
        /// 抗生素 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YearCreate(ddYearModel model)
        {
            try
            {
                ddYear entity = new ddYear();
                entity.title = model.title;
                entity.sortid = model.sortid;
                entity.isvalid = model.isvalid;
                entity.created = DateTime.Now;
                entity.created_by = base.LoginUserinfo.Id;

                if (ddYears.GetSort(entity.sortid) != null)
                {
                    ddYears.UpdateSort(entity.sortid);

                    if (this.ddYears.insertYears(entity) > 0)
                    {
                        string logContent = "添加年份，年份名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "添加年份，年份名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
                else
                {

                    if (this.ddYears.insertYears(entity) > 0)
                    {
                        string logContent = "添加年份，年份名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "添加年份，年份名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return this.RedirectToAction("YearCreate");
        }
        /// <summary>
        /// 抗生素编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult YearEdit(long id)
        {
            ddYearModel model = new ddYearModel();
            var entity = this.ddYears.QueryEntity(id);
            model.year_id = entity.year_id;        
            model.title = entity.title;          
            model.created = entity.created;
            model.created_by = entity.created_by;
            model.isvalid = entity.isvalid;
            model.sortid = entity.sortid;

            return View(model);
        }

        /// <summary>
        /// 抗生素 编辑 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YearEdit(ddYearModel model)
        {
            try
            {
                ddYear entity = new ddYear();
                entity.year_id = model.year_id;
                entity.title = model.title;
                entity.sortid = model.sortid;
                entity.isvalid = model.isvalid;
                entity.created = DateTime.Now;
                entity.created_by = base.LoginUserinfo.Id;

                if (ddYears.GetSort(entity.sortid) != null)
                {
                    if (entity.sortid != ddYears.GetSort(entity.sortid).sortid)
                    {
                        ddYears.UpdateSort(entity.sortid);
                    }

                    if (this.ddYears.UpdateddYears(entity) > 0)
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
                else
                {
                    if (this.ddYears.UpdateddYears(entity) > 0)
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                        base.SuccessNotification(logContent);
                    }
                    else
                    {
                        string logContent = "修改细菌，细菌名称：" + model.title;
                        base.ErrorNotification(logContent);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return this.RedirectToAction("ddYearList");
        }
        #endregion

    }
}