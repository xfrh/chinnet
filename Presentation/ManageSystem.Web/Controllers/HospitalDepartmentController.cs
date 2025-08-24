using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Medicine;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Medicine;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class HospitalDepartmentController : WebBaseController
    {

        private readonly IHospitalWardLocationService HospitalWardLocationService;

        private readonly IHospitalService HospitalService;
        public HospitalDepartmentController(IHospitalWardLocationService _hospitalWardLocationService, IHospitalService _hospitalService)
        {
            this.HospitalWardLocationService = _hospitalWardLocationService;
            this.HospitalService = _hospitalService;
        }
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 医院科室列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalDepartmentList(int pageIndex = 1)
        {
            HospitalWardLocationModel model = new HospitalWardLocationModel();
           

            var user = base.LoginUserinfo;
            if(user.HospitalId==0)
            {
                ViewBag.Hospitalid = user.HospitalId;
                return View();
            }
            var data = this.HospitalService.QueryEntity(user.HospitalId);
            ViewBag.HospitalName = data.Name;
            ViewBag.Department = user.Department == true ? 1 : 0;
            var list = this.HospitalWardLocationService.Query(user.HospitalId);

            model.PageList = list.Select(x => new HospitalWardLocationItemModel()
            {
                Id = x.Id,
                Name = x.Name,
                HospitalId = x.HospitalId,
                Department_CN = x.Department_CN,
                Department_EN = x.Department_EN,
                Location = x.Location,
                Location_Type = x.Location_Type,
                Ward = x.Ward,
                InsertTime = x.InsertTime
            }).ToPagedList<HospitalWardLocationItemModel>(pageIndex, MvcPagerExtensions.PageSize);
           
            return View(model);
        }

        public ActionResult CreateDepartment(long? Id)
        {
            var member = base.LoginUserinfo;
            var data = this.HospitalService.QueryEntity(member.HospitalId);          
            ViewBag.HospitalName = data.Name;
            ViewBag.Department = member.Department == true ? 1 : 0;

            if (Id!=null)
            {
                var hospital = this.HospitalWardLocationService.QueryEntity(Id);
                HospitalWardLocationItemModel model = new HospitalWardLocationItemModel()
                {
                    Location = hospital.Location,
                    Location_Type = hospital.Location_Type,
                    Department_EN = hospital.Department_EN,
                    Ward = hospital.Ward,
                    Name = data.Name
                };
                return View(model);
            }
            return View();

        }
        [HttpPost]
        public ContentResult CreateDepartment(HospitalWardLocation model)
        {
            try
            {
                var Ward = this.HospitalWardLocationService.QueryWard(model.HospitalId, model.Ward);
                if (Ward != null)
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "Ward已存在"));
                }
                var member = base.LoginUserinfo;
                model.HospitalId = member.HospitalId;
                model.InsertTime = DateTime.Now;             
                model.Id = CommonHelper.GuidToLongID;
                model.Sort = 1;
                model.UpdateTime = model.InsertTime;
                model.DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0);
                model.Mark = 1;
                model.Version = 1;
                model.Describe = null;
                model.Department_CN = model.Ward;
                HospitalWardLocationService.Insert(model);
                base.InsetActionLog(ActionType.Create, "添加科室");
                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                throw;
            }
          
        }
        /// <summary>
        /// 修改科室配置页面
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult UpdateDepartment(long Id)
        {
            var member = base.LoginUserinfo;
            var data = this.HospitalService.QueryEntity(member.HospitalId);
            ViewBag.HospitalName = data.Name;
            ViewBag.Department = member.Department == true ? 1 : 0;
            var hospital = this.HospitalWardLocationService.QueryEntity(Id);
                HospitalWardLocation model = new HospitalWardLocation()
                {
                    Location = hospital.Location,
                    Location_Type = hospital.Location_Type,
                    Department_EN = hospital.Department_EN,
                    Ward = hospital.Ward,
                    Name = hospital.Name
                };
                return View(model);
        }
        [HttpPost]
        public ActionResult UpdateDepartment(HospitalWardLocation hospital)
        {
            try
            {
                var Ward = this.HospitalWardLocationService.QueryWard(hospital.HospitalId, hospital.Ward);
                if (Ward != null)
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "Ward已存在"));
                }
                hospital.UpdateTime = DateTime.Now;
                hospital.Mark = 2;
                HospitalWardLocationService.UpdateDepartment(hospital);
                base.InsetActionLog(ActionType.Create, "修改科室","修改的科室id："+hospital.Id);
                base.SuccessNotification("保存成功");

                return this.RedirectToAction("UpdateDepartment");
            }
            catch (Exception)
            {
                base.SuccessNotification("操作失败，请重试");
                throw;
            }
          
        }
        /// <summary>
        /// 根据id删除科室
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>

        public ContentResult DeleteDepartment(string id)
        {
            id = id.TrimEnd(',');
            try
            {
                if (id=="") return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                   HospitalWardLocationService.Delete(id);
                base.InsetActionLog(ActionType.Delete, "【手动】删除科室配置", "数据id集合："+id);

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }


        public ActionResult DepartmentImport()
        {
            var member = base.LoginUserinfo;
            var data = this.HospitalService.QueryEntity(member.HospitalId);
            ViewBag.HospitalName = data.Name;
            ViewBag.HospitalId = data.Id;
            ViewBag.Department = member.Department == true ? 1 : 0;

            return View();
        }
        public JsonResult OnSubmit_HospitalWardLocation(long id)
        {
            #region 简单验证

            HttpPostedFileBase file = Request.Files["excelFile"];//file对应前端选择文件的name属性
            if (id <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "请输入或选择医院名称"
                });
            }
            Hospital hospital = HospitalService.QueryEntity(id);
            if (hospital == null || hospital.Id <= 0 || hospital.Mark <= 0 || !hospital.State)
            {
                return Json(new
                {
                    status = false,
                    message = "您所输入或选择医院名称不存在或已删除"
                });
            }

            if (file == null || file.ContentLength <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "请选择医院科室配置文件"
                });
            }
            string fileExt = System.IO.Path.GetExtension(file.FileName).ToUpperInvariant();
            List<string> support = new List<string> { ".xls", ".xlsx", ".XLS", ".XLSX", "xls", "xlsx", "XLS", "XLSX" };
            if (!support.Contains(fileExt))
            {
                return Json(new
                {
                    status = false,
                    message = "选择医院科室配置文件不支持，仅支持.xls或.xlsx"
                });
            }
            #endregion

            DateTime now = DateTime.Now;
            int sort = 1;
            List<HospitalWardLocation> entities = new List<HospitalWardLocation>();

            try
            {
                DataTable dt = null;
                var v = file.ContentLength;
                var v2 = file.InputStream;
                var v3 = file.InputStream.Length;
                #region 读取文件数据
                if (fileExt.Equals(".XLSX"))
                {
                    ExcelPackage package = new ExcelPackage(file.InputStream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//选定 指定页
                    dt = EPPlusHelper.WorksheetToTable(worksheet);
                }
                else if (fileExt.Equals(".XLS"))
                {
                    dt = Core.Utility.Excel.ImportDataTable.ExcelToDataTable(file.InputStream, Core.Utility.Excel.ExcelEnum.Excel2003, true);
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "选择医院科室配置文件不支持，仅支持.xls或.xlsx"
                    });
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    return Json(new
                    {
                        status = false,
                        message = "请使用指定的“医院科室配置标准模板.xlsx”上传数据"
                    });
                }
                #endregion

                foreach (DataRow dataRow in dt.Rows)
                {
                    string _ward = dataRow["Ward"].ToString().Trim();
                    //string department_CN = dataRow["Department_CN"].ToString().Trim();
                    string department = dataRow["Department"].ToString().Trim();
                    string location = dataRow["Location"].ToString().Trim();
                    string location_Type = dataRow["Location_Type"].ToString().Trim();

                    if (String.IsNullOrWhiteSpace(_ward) &&
                        //String.IsNullOrWhiteSpace(department_CN) &&
                        String.IsNullOrWhiteSpace(department) &&
                        String.IsNullOrWhiteSpace(location) &&
                        String.IsNullOrWhiteSpace(location_Type))
                    {
                        continue;
                    }

                    if (HospitalWardLocationService.Count(r => r.HospitalId == id && r.Ward == _ward && r.Department_EN == department && r.Location == location && r.Location_Type == location_Type && r.Mark > 0) == 0)
                    {
                        entities.Add(new HospitalWardLocation
                        {
                            Id = CommonHelper.GuidToLongID,
                            HospitalId = hospital.Id,
                            Name = hospital.Name,
                            Ward = _ward,
                            Department_CN = _ward,
                            Department_EN = department,
                            Location = location,
                            Location_Type = location_Type,
                            InsertTime = now,
                            Sort = sort++,
                            Mark = 1,
                            Version = 1
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = "读取文件失败，请使用指定导入模板文件上传。原因：" + ex.ToString()
                }); ;
            }

            try
            {
                if (entities.Count > 0)
                {

                    //HospitalWardLocationService.DeleDepartment(hospital.Id);
                    HospitalWardLocationService.Insert(entities);
                    InsetActionLog(ActionType.Import, $"导入{hospital.Name}科室配置", entities.SerializeObject());
                    return Json(new
                    {
                        status = true,
                        message = $"导入已完成，共成功导入{entities.Count()}条数据"
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = true,
                        message = $"操作已完成"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false,
                    message = "医院科室配置导入发生异常"
                });
            }
        }
    }
}