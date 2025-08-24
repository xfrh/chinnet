using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Chart;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Models;
using ManageSystem.Web.Models.Datas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class SatelliteController : WebBaseController
    {
        private readonly IMemberService MemberService;
        private readonly IAreaService AreaService;
        private readonly IHospitalService HospitalService;
        private readonly IDoctorTitleService DoctorTitleService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly ISatelliteService SatelliteService;
        private readonly IDataSegmentService DataSegmentService;
        private readonly IBarChartService BarChartService;
        private readonly IBarChartWithItemDataService BarChartWithItemDataService;

        public SatelliteController(
            IMemberService _MemberService,
            IAreaService _AreaService,
            IHospitalService _HospitalService,
            IDoctorTitleService _DoctorTitleService,
            IHospitalDepartmentService _HospitalDepartmentService,
            ISatelliteService _SatelliteService,
            IDataSegmentService _DataSegmentService,
            IBarChartService _BarChartService,
            IBarChartWithItemDataService _BarChartWithItemDataService
            )
        {
            MemberService = _MemberService;
            AreaService = _AreaService;
            HospitalService = _HospitalService;
            DoctorTitleService = _DoctorTitleService;
            HospitalDepartmentService = _HospitalDepartmentService;
            SatelliteService = _SatelliteService;
            DataSegmentService = _DataSegmentService;
            BarChartService = _BarChartService;
            BarChartWithItemDataService = _BarChartWithItemDataService;
        }
        [CheckRole(true)]
        public ActionResult Index1()
        {
            //获取当前登录用户名
            var member = base.LoginUserinfo;
            //根据用户ID获取用户信息
            var userinfo = this.MemberService.QueryEntity(member.Id);
            List<SelectListItem> provinceList = this.AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            List<SelectListItem> hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == userinfo.HospitalId }; }).ToList();
            List<SelectListItem> hospitalDepartmentList = this.HospitalDepartmentService.GetHospitalDepartments(userinfo.HospitalId).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.HospitalId }).ToList();
            List<SelectListItem> doctorTitleList = this.DoctorTitleService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            List<SelectListItem> cityList = new List<SelectListItem>();
            cityList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalDepartmentList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            doctorTitleList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            SatelliteModel model = new SatelliteModel()
            {
                Id = new long(),
                HospitalList = hospitalList,
                DoctorTitleList = doctorTitleList,
                ProvinceList = provinceList,
                HospitalDepartmentList = hospitalDepartmentList,
                CityList = cityList,
                ApplyUser = userinfo.Id.ToString()
            };

            ViewBag.HospitalId = userinfo.HospitalId;
            return View(model);
        }

        /// <summary>
        /// 卫星网申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckRole(false)]
        public ActionResult Index1(SatelliteModel model)
        {
            if (string.IsNullOrEmpty(model.SatelliteName))
                ModelState.AddModelError("SatelliteName", "卫星网名称不能为空");
            else
            {
                if (model.SatelliteName.Length > 13)
                    ModelState.AddModelError("SatelliteName", "卫星网名称过长，请小于13个汉字");
            }

            if (string.IsNullOrEmpty(model.ChargeName))
                ModelState.AddModelError("ChargeName", "负责人姓名不能为空");
            if (string.IsNullOrEmpty(model.ChargePhoneNumber))
                ModelState.AddModelError("ChargePhoneNumber", "负责人电话不能为空");
            //if (string.IsNullOrEmpty(model.Office))
            //    ModelState.AddModelError("Office", "科室不能为空");
            if (string.IsNullOrEmpty(model.PlaceOfWork))
                ModelState.AddModelError("PlaceOfWork", "工作单位不能为空");

            if (string.IsNullOrEmpty(model.Province) || string.IsNullOrEmpty(model.City))
                ModelState.AddModelError("Province", "省市不能为空");

            if (string.IsNullOrEmpty(model.ItemName))
                ModelState.AddModelError("ChargeName", "联系人姓名不能为空");
            if (string.IsNullOrEmpty(model.ItemPhoneNumber))
                ModelState.AddModelError("ChargePhoneNumber", "联系人电话不能为空");

            if (string.IsNullOrEmpty(model.RealmName))
                ModelState.AddModelError("RealmName", "卫星网域名不能为空");
            else
            {
                if (!Regex.IsMatch(model.RealmName, "^[A-Za-z0-9]+$"))
                    ModelState.AddModelError("RealmName", "卫星网域名格式不正确");
            }


            if (ModelState.IsValid)
            {
                Satellite entity = new Satellite();
                entity.Id = DateTime.Now.Ticks;
                entity.SatelliteName = model.SatelliteName;
                entity.ChargeName = model.ChargeName;
                entity.ChargeEmail = model.ChargeEmail;
                entity.ChargePhoneNumber = model.ChargePhoneNumber;
                entity.Office = model.Office;
                entity.ItemName = model.ItemName;
                entity.ItemEmail = model.ItemEmail;
                entity.ItemPhoneNumber = model.ItemPhoneNumber;
                entity.PlaceOfWork = model.PlaceOfWork;
                entity.UnitCount = model.UnitCount;
                entity.Profession = model.Profession;
                entity.Province = model.Province;
                entity.City = model.City;
                entity.Job = model.Job;
                entity.RealmName = "";
                entity.ApplyUser = model.ApplyUser;
                entity.ChargeWechatNumber = model.ChargeWechatNumber;
                entity.RealmName = model.RealmName;
                this.SatelliteService.Insert(entity);
                this.HttpContext.Session["LoginUserinfoSession"] = "";
                base.InsetActionLog(ActionType.Create, "添加卫星网申请", entity.SerializeObject());
                base.SuccessNotification("申请成功,等待管理员审批!");
                return this.RedirectToAction("Index");
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

            //获取当前登录用户名
            var member = base.LoginUserinfo;
            //根据用户ID获取用户信息
            var userinfo = this.MemberService.QueryEntity(member.Id);
            List<SelectListItem> provinceList = this.AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            List<SelectListItem> hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == userinfo.HospitalId }; }).ToList();
            List<SelectListItem> hospitalDepartmentList = this.HospitalDepartmentService.GetHospitalDepartments(userinfo.HospitalId).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.HospitalId }).ToList();
            List<SelectListItem> doctorTitleList = this.DoctorTitleService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            List<SelectListItem> cityList = new List<SelectListItem>();
            cityList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalDepartmentList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            doctorTitleList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            SatelliteModel model1 = new SatelliteModel()
            {
                Id = new long(),
                HospitalList = hospitalList,
                DoctorTitleList = doctorTitleList,
                ProvinceList = provinceList,
                HospitalDepartmentList = hospitalDepartmentList,
                CityList = cityList,
                ApplyUser = userinfo.Id.ToString()
            };

            ViewBag.HospitalId = userinfo.HospitalId;
            return View("Index1", model1);
            //return this.RedirectToAction("Index");
        }



        /// <summary>
        /// 省市联动菜单方法
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult GetCityList(long provinceId)
        {
            var data = this.AreaService.QueryByParentId(provinceId).SerializeObject();

            return this.Content(data);
        }

        /// <summary>
        /// 医院科室联动菜单方法
        /// </summary>
        /// <param name="hspitalId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult GetDepartemnt(long hspitalId)
        {
            var data = this.HospitalDepartmentService.GetHospitalDepartments(hspitalId).SerializeObject();

            return this.Content(data);
        }

        [HttpGet]
        [CheckRole(false)]
        public ActionResult MapLogin()
        {
            //try
            //{
            //    //获取当前登录用户名
            //    var member = base.LoginUserinfo;
            //    //根据用户ID获取用户信息
            //    var userinfo = this.MemberService.QueryEntity(member.Id);

            //    if (SatelliteService.QueryApplyUser(userinfo.Id.ToString()) > 0)
            //    {
            //        List<SelectListItem> provinceList = this.AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            //        List<SelectListItem> cityList = new List<SelectListItem>();
            //        cityList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            //        provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            //        SatelliteModel model = new SatelliteModel()
            //        {
            //            Id = new long(),
            //            ProvinceList = provinceList,
            //            CityList = cityList
            //        };

            //        return View(model);

            //    }
            //    else
            //        return RedirectToAction("Index", "Satellite");
            //}
            //catch (Exception)
            //{
            List<SelectListItem> provinceList = this.AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToList();
            List<SelectListItem> cityList = new List<SelectListItem>();
            cityList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            SatelliteModel model = new SatelliteModel()
            {
                Id = new long(),
                ProvinceList = provinceList,
                CityList = cityList
            };

            return View(model);
            // }

        }

        [HttpPost]
        [CheckRole(false)]
        public ContentResult ReturnChinaJson()
        {
            string jsonfile = System.Web.HttpContext.Current.Server.MapPath($"\\Content\\Scripts\\china.json");

            string json = System.IO.File.ReadAllText(jsonfile, System.Text.Encoding.UTF8);

            return this.Content(json);
        }

        /// <summary>
        /// 一级域名判断
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult ReturnSatelliteView(string id, string pageIndex)
        {
            //string path = "";
            switch (id)
            {
                case "0":
                    return Redirect("/Satellite/Index");
                case "Team":
                    return Redirect("/Team/Index");
                case "ECV":
                    return Redirect("/ECV/Index");
                case "Document":
                    pageIndex = string.IsNullOrEmpty(pageIndex) ? "0" : pageIndex;
                    return Redirect("/Document/Index?pageIndex=" + pageIndex);
                case "Meeting":
                    pageIndex = string.IsNullOrEmpty(pageIndex) ? "0" : pageIndex;
                    return Redirect("/Meeting/Index?pageIndex=" + pageIndex);
                case "Chinet":
                    return Redirect("/Chinet/Index");
                case "News":
                    return Redirect("/News/Index");
                case "Research":
                    return Redirect("/Research/Index");
                case "MICapply":
                    return Redirect("/MICapply/Index");
                case "Center":
                    return Redirect("/Center/Index1");
                case "Satellite":
                    return Redirect("/Satellite/Index1");
                case "ClinicalTrialReport":
                    return Redirect("/ClinicalTrialReport/Index");
                default:
                    if (this.SatelliteService.IsExist(id))
                    {
                        Satellite satellite = SatelliteService.Query().Single(x => x.RealmName == id);
                        List<BarChartDTO> barCharts = DataSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartDTO
                        {
                            Id = item.Id,
                            Name = item.Name,
                            DataItems = BarChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0 && r.SatelliteId == satellite.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new BarChartDataItemDTO
                            {
                                Id = m.Id,
                                Name = m.Name,
                                Default = m.Default
                            }).ToList()
                        }).ToList();
                        if (barCharts[0].DataItems.Count > 0 || barCharts[1].DataItems.Count > 0)
                            return View(barCharts);
                        else
                            return Redirect("/Satellite/SatelliteBuilding");
                    }
                    else
                        return Redirect("/Common/PageNotFound");

            }

        }

        [CheckRole(false)]
        public ActionResult SatelliteBuilding()
        {
            return View();
        }

        [CheckRole(false)]
        public ActionResult MapLoginOpenIndex(string id)
        {
            return View();
        }

        [HttpPost]
        [CheckRole(false)]
        public ContentResult GetLoginList()
        {
            List<Core.Domain.SystemSet.Area> areas = new List<Core.Domain.SystemSet.Area>();
            List<Satellite> satellites = this.SatelliteService.GetLoginList().Where(x => !string.IsNullOrEmpty(x.Describe) && x.RealmName != "&").ToList();

            foreach (Satellite satellite in satellites)
            {
                Core.Domain.SystemSet.Area area = this.AreaService.QueryEntity(long.Parse(satellite.Province));
                area.Describe = satellite.RealmName;
                areas.Add(area);
            }
            return Content(areas.SerializeObject());
        }

        /// <summary>
        /// 根据省市获取已经审批通过的卫星网
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">市ID</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult GetSatelliteListByPC(string provinceId, string cityId)
        {
            List<Satellite> resultList = this.SatelliteService.GetLoginList().Where(x => !string.IsNullOrEmpty(x.Describe) && x.Describe == "1").ToList();
            return Content(resultList.SerializeObject());
        }

        [CheckRole(false)]
        public string GetSatelliteTitie(string city)
        {
            if (string.IsNullOrEmpty(city))
                return null;
            var entty = this.SatelliteService.Query(x => !string.IsNullOrEmpty(x.Describe) && x.Describe == "1").Single(x => x.RealmName == city);
            string json = JsonConvert.SerializeObject(entty);
            return json;
        }

        [HttpPost]
        [CheckRole(false)]
        public string CheckRealmName(string Realmname)
        {
            if (Realmname.Contains(@"\") || Realmname.Contains("/"))
                return "1";
            try
            {
                var entty = this.SatelliteService.Query(x => !string.IsNullOrEmpty(x.Describe) && x.Describe == "1").Single(x => x.RealmName == Realmname);
                string json = JsonConvert.SerializeObject(entty);
                return json;
            }
            catch (Exception)
            {
                return "1";
            }

        }
    }
}