using ManageSystem.Core;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Framework.Upload;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Tasks;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Medicine;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ManageSystem.Web.Controllers
{
    public class MedicineController : WebBaseController
    {
        private readonly IMemberAddressService memberAddressService;
        private readonly IHospitalService HospitalService;
        private readonly IBacteriaTypeService BacteriaTypeService;
        private readonly ISpecimenService SpecimenService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly IMedicalDataService MedicalDataService;
        private readonly IMedicalDataItemService MedicalDataItemService;
        private readonly IAreaService AreaService;
        private readonly IMemberService MemberService;
        private readonly IMedicalDataItemValidateService MedicalDataItemValidateService;
        private readonly IMedicalAntibioticResultService MedicalAntibioticResultService;
        private readonly IHospitalWardLocationService HospitalWardLocationService;
        private readonly IEncryptionService encryptionService;
        private readonly IMedicalDataProjectService medicalDataProjectService;
        private readonly ISatelliteService satelliteService;


        public MedicineController(
            IMemberAddressService _memberAddressService,
            IHospitalService _hospitalService,
            IBacteriaTypeService _bacteriaTypeService,
             ISpecimenService _specimenService,
            IHospitalDepartmentService _hospitalDepartmentService,
             IMedicalDataService _medicalDataService,
            IAreaService _areaService,
             IMedicalDataItemService _medicalDataItemService,
            IMedicalDataItemValidateService _medicalDataItemValidateService,
            IMedicalAntibioticResultService _medicalAntibioticResultService,
            IMemberService _memberService,
            IHospitalWardLocationService _hospitalWardLocationService,
            IEncryptionService _encryptionService,
            IMedicalDataProjectService _medicalDataProjectService,
            ISatelliteService _satelliteService
        )
        {
            memberAddressService = _memberAddressService;
            this.HospitalService = _hospitalService;
            this.BacteriaTypeService = _bacteriaTypeService;
            this.SpecimenService = _specimenService;
            this.HospitalDepartmentService = _hospitalDepartmentService;
            this.MedicalDataService = _medicalDataService;
            this.AreaService = _areaService;
            this.MedicalDataItemService = _medicalDataItemService;
            this.MedicalDataItemValidateService = _medicalDataItemValidateService;
            this.MedicalAntibioticResultService = _medicalAntibioticResultService;
            this.MemberService = _memberService;
            this.HospitalWardLocationService = _hospitalWardLocationService;
            encryptionService = _encryptionService;
            this.medicalDataProjectService = _medicalDataProjectService;
            satelliteService = _satelliteService;
        }

        #region 首页

        /// <summary>
        /// 医学信息首页
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ActionResult Index(IndexSearchModel searchModel)
        {
            return this.Redirect("/");

            return View(this.SetIndexData(searchModel));
        }

        /// <summary>
        /// 查询医学首页的数据
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private IndexModel SetIndexData(IndexSearchModel searchModel)
        {
            IndexModel model = new IndexModel();
            model.SearchModel = searchModel ?? new IndexSearchModel();

            //   model.SearchHospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).ToList();
            //     model.SearchAreaList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //区域列表（顶级，省份）
            //     model.SearchDepartmentList = this.HospitalDepartmentService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).ToList(); //科室
            //    model.SearchSpecimenList = this.SpecimenService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).ToList(); //标本
            //    model.SearchBacteriaTypeList = this.BacteriaTypeService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).ToList(); //细菌类型

            //分页数据
            //model.PageList = this.MedicalDataService.Query(searchModel.Area, searchModel.Hospital, searchModel.Specimen, searchModel.Bacteria, searchModel.Department).ToPagedList(searchModel.PageIndex, 10); //获取查询数据

            ////填充菌株数量
            //List<long> dataIdList = model.PageList.Select(m => m.Id).ToList();
            //var dataItemList = this.MedicalDataItemService.Query(m => dataIdList.Contains(m.MedicalDataId));

            //foreach (var item in model.PageList)
            //{
            //    var memberTemp = dataItemList.Where(m => m.MedicalDataId == item.Id).Count();
            //    item.AreaId = memberTemp; //用区域id暂用作 菌株数量
            //}

            return model;
        }

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <returns></returns>
        public ContentResult GetArea()
        {
            var data = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //区域列表（顶级，省份）
            if (data == null || !data.Any())
                return this.Content("");

            var list = data.Select(x =>
            {
                return new
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                };
            });

            return this.Content(list.SerializeObject());
        }

        /// <summary>
        /// 获取医院数据
        /// </summary>
        /// <returns></returns>
        public ContentResult GetHospital()
        {
            var data = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).ToList();
            if (data == null || !data.Any())
                return this.Content("");

            var list = data.Select(x =>
            {
                return new
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                };
            });

            return this.Content(list.SerializeObject());
        }

        /// <summary>
        /// 获取药敏实验方法
        /// </summary>
        /// <returns></returns>
        public ContentResult GetBacteria()
        {
            var data = this.BacteriaTypeService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).ToList(); //细菌类型
            if (data == null || !data.Any())
                return this.Content("");

            var list = data.Select(x =>
            {
                return new
                {
                    Id = x.Id.ToString(),
                    Name = x.Name
                };
            });

            return this.Content(list.SerializeObject());
        }

        /// <summary>
        /// 获取验证的分页数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataId">>对应的医学数据Id</param>
        /// <param name="level">0：表示全部  1：提示  2：警告  3：错误</param>
        /// <returns></returns>
        public JsonResult GetMedicineList(DataSourceRequest command, IndexSearchModel searchModel)
        {
            //目前不再显示数据
            return Json("");

            var list = this.MedicalDataService.Query(searchModel.Area, searchModel.Hospital, searchModel.Specimen, searchModel.Bacteria, searchModel.Department, command.Page - 1, command.PageSize);

            if (list == null || !list.Any())
                return Json("");

            var data = list.Select(x =>
            {
                return new
                {
                    Id = x.Id.ToString(),
                    InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                    AreaId = x.AreaId.ToString(),
                    AreName = x.AreName,
                    SN = x.SN,
                    HospitalName = x.HospitalName
                };
            }).ToList();

            return Json(new { total = list.TotalPages, rows = data });
        }

        #endregion

        #region 上传数据

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckRole(true)]
        public ActionResult Upload(long id = 0)
        {
            var userinfo = base.LoginUserinfo;

            // 判断

            ViewBag.isFaultiness = base.LoginUserinfo.HospitalId <= 0 ? 1 : 0;
            this.ViewBag.IsTest = userinfo.IsTest;
            ViewBag.isUpload = !(string.IsNullOrWhiteSpace(userinfo.ProjectUploadItem) || "[]".Equals(userinfo.ProjectUploadItem));
            ViewBag.HospitalWard = HospitalWardLocationService.Query(userinfo.HospitalId).Count() <= 0 ? 1 : 0;
            ViewBag.Department = userinfo.Department == true ? 1 : 0;
            return View(this.SetUploadData(false));
        }


        /// <summary>
        /// 卫星网-上传数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckRole(false)]
        public ActionResult Upload1()
        {
            var userinfo = base.LoginSatelliteUserinfo;
            var sateliUser = satelliteService.QueryEntity(userinfo.SatelliteId);
            // 判断

            ViewBag.isFaultiness = long.Parse(sateliUser.PlaceOfWork) <= 0 ? 1 : 0;
            ViewBag.isUpload = true;
            ViewBag.HospitalWard = HospitalWardLocationService.Query(long.Parse(sateliUser.PlaceOfWork)).Count() <= 0 ? 1 : 0;
            ViewBag.Department = 0;
            ViewBag.Istest = false;
            return View(this.SetUploadData(true));
        }

        /// <summary>
        /// 设置上传数据页面的数据
        /// </summary>
        /// <returns></returns>
        private UploadMedicalDataModel SetUploadData(bool isSatellite)
        {
            int year = DateTime.Now.Year;


            UploadMedicalDataModel model = new UploadMedicalDataModel();
            model.YearList = new List<SelectListItem>() {
                 new SelectListItem() {  Text="请选择",Value="0"},
                 new SelectListItem() {  Text=(year-1)+"年",Value=(year-1).ToString()},
                 new SelectListItem() {  Text=year+"年",Value=(year).ToString()},
                 new SelectListItem() {  Text=(year+1)+"年",Value=(year+1).ToString()}
            };
            model.QuarterList = new List<SelectListItem>() {
                 new SelectListItem() {  Text="请选择",Value="0"},
                 new SelectListItem() {  Text="上半年（1月 - 6月）",Value="6"},
                 new SelectListItem() {  Text="下半年（7月 - 12月）",Value="7"},
                 new SelectListItem() {  Text="全年",Value="5"}
            };

            model.SpecimenList = new List<SelectListItem>();
            model.SpecimenList = this.SpecimenService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x =>
            {
                return new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                };
            }).ToList();

            model.SpecimenList.Insert(0, new SelectListItem()
            {
                Text = "请选择",
                Value = "0"
            });

            model.BacteriaTypeList = this.BacteriaTypeService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).ToList();

            Member memberEntity = new Member();
            if (!isSatellite)
            {
                var loginUser = base.LoginUserinfo;
                memberEntity = MemberService.QueryEntity(loginUser.Id);
            }

            if (isSatellite)
                model.Email = "";
            else
                model.Email = memberEntity.MedicineEmail ?? "";

            model.ProjectTypeList = new List<SelectListItem>();
            model.ProjectTypeList.Add(new SelectListItem() { Text = "请选择数据所属项目", Value = "0" });

            if (isSatellite)
            {
                model.ProjectTypeList = medicalDataProjectService.Query(m => m.Id == 6).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(memberEntity.ProjectUploadItem) && !"[]".Equals(memberEntity.ProjectUploadItem))
                {
                    try
                    {
                        List<long> projectUploadIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<long>>(memberEntity.ProjectUploadItem);
                        model.ProjectTypeList = this.medicalDataProjectService
                            .Query(m => m.Mark > 0 && projectUploadIds.Contains(m.Id))
                            .OrderBy(x => x.Sort)
                            .Select(x => new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.Id.ToString()
                            }).ToList();
                        model.ProjectTypeList.Insert(0, new SelectListItem { Text = "请选择数据所属项目", Value = "0" });
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            model.ProjectType = isSatellite ? 6 : memberEntity.DefaultProject;
            return model;
        }

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult Upload(UploadMedicalDataModel model)
        {
            try
            {
                Files.ForEach(f => f.InputStream.Dispose());
                Files.Clear();
                Log4Helper.Info("记录日志-开始时间(上传数据)：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

                model.SpecimenId = 0;
                ModelState.Remove("Id");

                #region //数据验证

                Hospital hospital = new Hospital();
                Area area = new Area();
                if (model.IsSatellite == 0)
                {
                    var loginUser = base.LoginUserinfo;
                    if (loginUser.HospitalId <= 0)
                        return this.Content(SpringJsonResult.Error("您未关联任何医院，无法上传数据"));

                    if (!ModelState.IsValid)
                        return this.Content(SpringJsonResult.Error("请填写相关数据"));

                    long hospitalId = loginUser.HospitalId;
                    if (model.HospitalId > 0 && loginUser.IsTest)
                        //如果是测试账号且前台选择医院，则这里的医院数据不就是当前登录用户。
                        hospitalId = model.HospitalId;

                    hospital = this.HospitalService.QueryEntity(hospitalId);
                    if (hospital == null || hospital.Id <= 0)
                        throw new Exception("医院不存在");

                    area = this.AreaService.QueryEntity(loginUser.ProvinceId);
                    if (area == null || area.Id <= 0)
                        throw new Exception("您所属区域不存在");
                }
                else
                {
                    var loginSatellite = base.LoginSatelliteUserinfo;
                    hospital = this.HospitalService.QueryEntity(long.Parse(satelliteService.QueryEntity(loginSatellite.SatelliteId).PlaceOfWork));
                    area = this.AreaService.QueryEntity(long.Parse(satelliteService.QueryEntity(loginSatellite.SatelliteId).Province));
                }

                string savePath = "";
                if (model.FilePathList.Count > 1)
                {
                    if (model.Quarter == 5 && !model.FileName.Contains("dbf"))
                    {
                        savePath = MergeExccel(model.FilePathList);
                        if (string.IsNullOrEmpty(savePath))
                            throw new Exception("合并Excel出错");
                    }
                }

                //数据验证
                Member user = new Member();
                MedicalData entity = new MedicalData();

                user = MemberService.QueryEntity(LoginUserinfo.Id);//获当前用户
                if(user == null || user.Id <= 0)
                {
                    throw new Exception("未获取到用户信息，上传失败");
                }

                #endregion

                #region //上传文件

                //填充基础数据
                entity.Id = CommonHelper.GuidToLongID;
                entity.Year = model.Year;
                entity.Quarter = model.Quarter;
                entity.SpecimenId = model.SpecimenId;
                entity.SpecimenName = ""; //该字段删除了，所以默认空
                entity.BacteriaTypeIds = model.BacteriaIds;
                entity.BacteriaTypeName = "";
                entity.HospitalId = hospital.Id;
                entity.HospitalName = hospital.Name;
                entity.HospitalDepartmentId = 0;
                entity.HospitalDepartmentName = "";
                entity.AreaId = area.Id;
                entity.AreName = area.Name;
                entity.MemberId = user.Id;
                entity.MemberName = user.Name;
                //entity.UploadFilePath = model.FilePath;
                entity.UploadFilePath = string.IsNullOrEmpty(savePath)?  model.FilePath:savePath;//如果选择为全年文件 存储的地址为合并的excel文件地址
                if (string.IsNullOrWhiteSpace(entity.UploadFilePath) && model.FilePathList != null && model.FilePathList.Count > 0)
                {
                    entity.UploadFilePath = model.FilePathList[0].Substring(model.FilePathList[0].IndexOf('/'));
                }
                entity.FileName = model.FileName;
                entity.DisposeFilePath = "";
                entity.ProjectType = model.ProjectType;
                entity.WordFile = "";
                entity.InsertTime = DateTime.Now;
                entity.Display = true;//默认显示出来，后面在数据分析的时候会重新检查一次，同时设置历史的不显示出来

                //产生对应的二维码图片
                string codeValue = base.SettingService.QueryValue<string>("web.web.url") + "/Code/Index?type=medical&data=&dataId=" + entity.Id;
                string codeFileNmae = "";
                string codePath = "/Content/Upload/MedicalCode/";
                TwoDimensionCode.CreateAndSaveCode(codeValue, this.Server.MapPath(codePath), ref codeFileNmae);
                if (string.IsNullOrWhiteSpace(codeFileNmae)) throw new Exception("产生二维码图片失败，请重试");
                entity.CodeFilePath = codePath + codeFileNmae;

                #region 填充基础数据

                UploadMedicalResult uploadModel = new UploadMedicalResult()
                {
                    BaseMessage = new UploadMedicalResultBase(),
                    MedicalEntity = entity,
                    ExcelItemList = new List<MedicalDataItem>(),
                    ValidateList = new List<MedicalDataItemValidate>(),
                    MemberEntity = user,
                    HospitalEntity = hospital,
                    ItemList = new List<MedicalDataItem>(),
                    ExcelFilePath = this.Server.MapPath(model.FilePath)
                };

                uploadModel.BaseMessage = new UploadMedicalResultBase()
                {
                    FileName = model.FileName,
                    FileLength = model.FileSize.ToString() + " KB",
                    BacteriaTypeName = entity.BacteriaTypeName,
                    SpecimenName = entity.SpecimenName,
                    UploadTime = model.InsertTime,
                    DataTime = this.MedicalDataService.GetDataQuarter(model.Year, model.Quarter),
                    MedicalDataId = entity.Id
                };

                #endregion

                //填充返回数据
                this.MedicalDataService.Insert2(uploadModel);

                #region //更新邮箱

                try
                {
                    if (model.Email != null && user.MedicineEmail != null && !model.Email.Equals(user.MedicineEmail))
                    {
                        //用户修改了邮箱地址，重新设置
                        var member = this.MemberService.QueryEntity(user.Id);
                        member.MedicineEmail = model.Email;
                        this.MemberService.Update(member);
                    }
                }
                catch { }

                #endregion

                #region //数据上传

                if (Request.Url.DnsSafeHost.Contains("chinets.com"))
                {
                    try
                    {
                        if (user.ProjectType == 2)
                        {
                            this.uploadToCHIENT(model, area, user);
                        }
                        else
                        {
                            this.uploadToCHIENT(model, area, user, Request.Url.Authority);
                        }
                    }
                    catch { }
                }

                #endregion

                string logContent = "上传医学信息";
                base.InsetActionLog(ActionType.Create, logContent, model.SerializeObject());
                return this.Content(SpringJsonResult.Success(entity.Id.ToString()));

                #endregion
            }
            catch (Exception ex)
            {
                Log4Helper.Error(ex.Message);
                this.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(SpringJsonResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 卫星网-上传数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult Upload1(UploadMedicalDataModel model)
        {
            try
            {
                Files.ForEach(f => f.InputStream.Dispose());
                Files.Clear();
                Log4Helper.Info("记录日志-开始时间(上传数据-卫星网)：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

                model.SpecimenId = 0;
                ModelState.Remove("Id");

                #region //数据验证

                Hospital hospital = new Hospital();
                Area area = new Area();
                if (model.IsSatellite == 0)
                {
                    var loginUser = base.LoginUserinfo;
                    if (loginUser.HospitalId <= 0)
                        return this.Content(SpringJsonResult.Error("您未关联任何医院，无法上传数据"));

                    if (!ModelState.IsValid)
                        return this.Content(SpringJsonResult.Error("请填写相关数据"));

                    long hospitalId = loginUser.HospitalId;
                    if (model.HospitalId > 0 && loginUser.IsTest)
                        //如果是测试账号且前台选择医院，则这里的医院数据不就是当前登录用户。
                        hospitalId = model.HospitalId;

                    hospital = this.HospitalService.QueryEntity(hospitalId);
                    if (hospital == null || hospital.Id <= 0)
                        throw new Exception("医院不存在");

                    area = this.AreaService.QueryEntity(loginUser.ProvinceId);
                    if (area == null || area.Id <= 0)
                        throw new Exception("您所属区域不存在");
                }
                else
                {
                    var loginSatellite = base.LoginSatelliteUserinfo;
                    hospital = this.HospitalService.QueryEntity(long.Parse(satelliteService.QueryEntity(loginSatellite.SatelliteId).PlaceOfWork));
                    area = this.AreaService.QueryEntity(long.Parse(satelliteService.QueryEntity(loginSatellite.SatelliteId).Province));
                }

                string savePath = "";
                if (model.Quarter == 5 && !model.FileName.Contains("dbf"))
                {
                    savePath = MergeExccel(model.FilePathList);
                    if (string.IsNullOrEmpty(savePath))
                        throw new Exception("合并Excel出错");
                }

                //数据验证
                MedicalData entity = new MedicalData();
                Member user = new Member();
                if (model.IsSatellite == 0)
                    user = MemberService.QueryEntity(LoginUserinfo.Id);

                #endregion

                #region //上传CHINET数据云

                entity.Id = CommonHelper.GuidToLongID;
                //填充基础数据
                entity.Year = model.Year;
                entity.Quarter = model.Quarter;
                entity.SpecimenId = model.SpecimenId;
                entity.SpecimenName = ""; //该字段删除了，所以默认空
                entity.BacteriaTypeIds = model.BacteriaIds;
                entity.BacteriaTypeName = "";
                entity.HospitalId = hospital.Id;
                entity.HospitalName = hospital.Name;
                entity.HospitalDepartmentId = 0;
                entity.HospitalDepartmentName = "";
                entity.AreaId = area.Id;
                entity.AreName = area.Name;
                entity.MemberId = model.IsSatellite == 1 ? satelliteService.QueryEntity(base.LoginSatelliteUserinfo.SatelliteId).Id : user.Id;
                entity.MemberName = model.IsSatellite == 1 ? base.LoginSatelliteUserinfo.UserName : user.Name;
                //entity.UploadFilePath = model.FilePath;
                entity.UploadFilePath = model.Quarter == 5 ? savePath : model.FilePath;//如果选择为全年文件 存储的地址为合并的excel文件地址
                entity.FileName = model.FileName;
                entity.DisposeFilePath = "";
                entity.ProjectType = model.ProjectType;
                entity.WordFile = "";
                entity.InsertTime = DateTime.Now;
                entity.Display = true;//默认显示出来，后面在数据分析的时候会重新检查一次，同时设置历史的不显示出来

                //产生对应的二维码图片
                string codeValue = base.SettingService.QueryValue<string>("web.web.url") + "/Code/Index?type=medical&data=&dataId=" + entity.Id;
                string codeFileNmae = "";
                string codePath = "/Content/Upload/MedicalCode/";
                TwoDimensionCode.CreateAndSaveCode(codeValue, this.Server.MapPath(codePath), ref codeFileNmae);
                if (string.IsNullOrWhiteSpace(codeFileNmae)) throw new Exception("产生二维码图片失败，请重试");
                entity.CodeFilePath = codePath + codeFileNmae;

                #region 填充基础数据

                UploadMedicalResult uploadModel = new UploadMedicalResult()
                {
                    BaseMessage = new UploadMedicalResultBase(),
                    MedicalEntity = entity,
                    ExcelItemList = new List<MedicalDataItem>(),
                    ValidateList = new List<MedicalDataItemValidate>(),
                    MemberEntity = model.IsSatellite == 1 ? null : user,
                    HospitalEntity = hospital,
                    ItemList = new List<MedicalDataItem>(),
                    ExcelFilePath = this.Server.MapPath(model.FilePath)
                };

                uploadModel.BaseMessage = new UploadMedicalResultBase()
                {
                    FileName = model.FileName,
                    FileLength = model.FileSize.ToString() + " KB",
                    BacteriaTypeName = entity.BacteriaTypeName,
                    SpecimenName = entity.SpecimenName,
                    UploadTime = model.InsertTime,
                    DataTime = this.MedicalDataService.GetDataQuarter(model.Year, model.Quarter),
                    MedicalDataId = entity.Id
                };

                #endregion

                //填充返回数据
                this.MedicalDataService.Insert2(uploadModel);

                if (!model.Email.Equals(user.MedicineEmail))
                {
                    //用户修改了邮箱地址，重新设置
                    var member = this.MemberService.QueryEntity(user.Id);
                    member.MedicineEmail = model.Email;
                    this.MemberService.Update(member);
                }

                if (Request.Url.DnsSafeHost.Contains("chinets.com"))
                {
                    try
                    {
                        this.uploadToCHIENT(model, area, user, Request.Url.Authority);//上传至多中心
                    }
                    catch { }
                }
                
                string logContent = "上传医学信息";
                base.InsetActionLog(ActionType.Create, logContent, model.SerializeObject());
                return this.Content(SpringJsonResult.Success(entity.Id.ToString()));

                #endregion
            }
            catch (Exception ex)
            {
                Log4Helper.Error(ex.Message);
                this.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(SpringJsonResult.Error(ex.Message));
            }
        }

        /// <summary>
        /// 2025.05.12 Gerry 手动上传至多中心
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TestUpload(long id= 0)
        {
            if (id > 0)
            {
                var dataModel = MedicalDataService.Query(id);
                if (dataModel != null && dataModel.Id > 0)
                {
                    var memberModel=MemberService.QueryEntity(dataModel.MemberId);
                    Area area = this.AreaService.QueryEntity(memberModel.ProvinceId);
                    if (area == null || area.Id <= 0)
                        throw new Exception("您所属区域不存在");

                    UploadMedicalDataModel model = new UploadMedicalDataModel()
                    {
                        ReportNumber = "0",
                        FileName = dataModel.FileName,
                        FilePath = dataModel.UploadFilePath,
                        Year = dataModel.Year,
                        Quarter = dataModel.Quarter,
                        Email=memberModel.Email,
                        ProjectType = dataModel.ProjectType,
                    };

                    this.uploadToCHIENT(model, area, memberModel);
                }
            }
            return RedirectToAction("AntibioticDrugFast", "Data");
        }

        /// <summary>
        /// 2025.02.14 上传至多中心（仅传文件地址）
        /// </summary>
        private void uploadToCHIENT(UploadMedicalDataModel model, Area area, Member memberModel)
        {
            try
            {
                //当前登录用户
                var loginUser = base.LoginUserinfo;

                //多个文件会合并成为一个文
                var FilePath = model.FilePath;
                var FileName = model.FileName;
                FilePath = Server.MapPath(FilePath);

                if (model.FilePathList!=null && model.FilePathList.Count > 0)
                {
                    FilePath = Server.MapPath(model.FilePathList[0].Substring(model.FilePathList[0].IndexOf('/')));
                }
                if (!string.IsNullOrWhiteSpace(FilePath) && System.IO.File.Exists(FilePath))
                {
                    using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                    {
                        #region //非空处理

                        if (model.ReportNumber==null|| string.IsNullOrWhiteSpace(model.ReportNumber))
                        {
                            model.ReportNumber = "0";
                        }
                        if (model.ReportType == null) { model.ReportType = string.Empty; }
                        if (model.ReportRemark == null) { model.ReportRemark = string.Empty; }

                        #endregion

                        #region  //form参数

                        System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent();
                        form.Add(new StringContent(memberModel.HospitalId.ToString()), "hospitalId");
                        form.Add(new StringContent(memberModel.HospitalDepartmentId.ToString()), "hospitalDepartmentId");
                        form.Add(new StringContent(model.Year.ToString()), "year");
                        form.Add(new StringContent(model.Quarter.ToString()), "quarter");
                        if (model.Email != null && !string.IsNullOrWhiteSpace(model.Email))
                        {
                            form.Add(new StringContent(model.Email), "notifyEmail");
                        }
                        form.Add(new StringContent(model.ProjectType.ToString()), "projectId");
                        form.Add(new StringContent(area.Id.ToString()), "areaId");
                        form.Add(new StringContent(memberModel.Id.ToString()), "uploadMemberId");
                        //if (model.ReportNumber != null)
                        form.Add(new StringContent(model.ReportNumber), "reportNumber");//报告数量
                        //if (model.ReportType != null)
                        form.Add(new StringContent(model.ReportType), "reportType");//报告形式
                        //if (model.ReportRemark != null)
                        form.Add(new StringContent(model.ReportRemark), "reportRemark");//其他报告形式

                        //多研究中心
                        if (memberModel != null && memberModel.Id > 0)
                            form.Add(new StringContent(memberModel.ProjectType.ToString()), "projectType");
                        else
                            form.Add(new StringContent("0"), "projectType");
                        form.Add(new StringContent(FilePath), "originFilePath");//文件地址

                        #endregion

                        #region //发送请求

                        var dnsSafeHost = System.Configuration.ConfigurationManager.AppSettings["HuayaoChinetsURL"].ToString();
                        var apiUrl = $"{dnsSafeHost}/api/medical/file/upload";//多中心

                        var response = client.PostAsync(apiUrl.Trim(), form).Result;
                        response.EnsureSuccessStatusCode();

                        var resultModel = JsonHelper.JSONToObject<TestResult>(response.Content.ReadAsStringAsync().Result);
                        if (resultModel != null && resultModel.code == 200)
                        {
                            //成功记录日志
                            base.InsetActionLog(ActionType.Create, "成功上传至多中心", string.Format("[{0}]{1}", model.FileName, model.FilePath));
                        }
                        else
                        {
                            //失败记录日志
                            base.InsetActionLog(ActionType.Create, "成功上传至多中心失败:" + resultModel.code, string.Format("[{0}]{1}", model.FileName, model.FilePath));
                        }

                        #endregion
                    }
                }
                else
                {
                    //记录日志文件
                }
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 2024.01.10-上传至多中心
        /// </summary>
        private void uploadToCHIENT(UploadMedicalDataModel model, Area area, Member memberModel,string dnsSafeHost= "")
        {
            try
            {
                //当前登录用户
                var loginUser = base.LoginUserinfo;

                //多个文件会合并成为一个文
                var FilePath = model.FilePath;
                var FileName = model.FileName;
                FilePath = Server.MapPath(FilePath);

                if (model.FilePathList.Count > 0)
                {
                    FilePath = Server.MapPath(model.FilePathList[0].Substring(model.FilePathList[0].IndexOf('/')));
                }

                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                    {
                        

                        System.Net.Http.MultipartFormDataContent form = new System.Net.Http.MultipartFormDataContent();
                        
                        //非空处理
                        if (model.ReportNumber == null || string.IsNullOrWhiteSpace(model.ReportNumber))
                        {
                            model.ReportNumber = "0";
                        }
                        if (model.ReportType == null) { model.ReportType = string.Empty; }
                        if (model.ReportRemark == null) { model.ReportRemark = string.Empty; }

                        #region  //其他参数

                        //基础参数
                        form.Add(new StringContent(loginUser.HospitalId.ToString()), "hospitalId");
                        form.Add(new StringContent(loginUser.HospitalDepartmentId.ToString()), "hospitalDepartmentId");
                        form.Add(new StringContent(model.Year.ToString()), "year"); 
                        form.Add(new StringContent(model.Quarter.ToString()), "quarter");
                        if (model.Email != null && !string.IsNullOrWhiteSpace(model.Email))
                        {
                            form.Add(new StringContent(model.Email), "notifyEmail");
                        }
                        form.Add(new StringContent(model.ProjectType.ToString()), "projectId");
                        form.Add(new StringContent(area.Id.ToString()), "areaId"); 
                        form.Add(new StringContent(loginUser.Id.ToString()), "uploadMemberId");

                        //if (model.ReportNumber != null)
                        form.Add(new StringContent(model.ReportNumber), "reportNumber");//报告数量
                        //if (model.ReportType != null)
                        form.Add(new StringContent(model.ReportType), "reportType");//报告形式
                        //if (model.ReportRemark != null)
                        form.Add(new StringContent(model.ReportRemark), "reportRemark");//其他报告形式

                        //多研究中心/X项目
                        if (memberModel!=null&& memberModel.Id > 0)
                            form.Add(new StringContent(memberModel.ProjectType.ToString()), "projectType");
                        else
                            form.Add(new StringContent("0"), "projectType"); 
                        form.Add(new StringContent(FilePath), "originFilePath");//文件地址

                        #endregion

                        #region // 发送 POST 请求到第三方接口

                        dnsSafeHost = System.Configuration.ConfigurationManager.AppSettings["HuayaoChinetsURL"].ToString();
                        var apiUrl = $"{dnsSafeHost}/api/medical/file/upload";//多中心

                        var response = client.PostAsync(apiUrl.Trim(), form).Result;
                        response.EnsureSuccessStatusCode();

                        var resultModel = JsonHelper.JSONToObject<TestResult>(response.Content.ReadAsStringAsync().Result);
                        if (resultModel != null && resultModel.code == 200)
                        {
                            //成功记录日志
                            base.InsetActionLog(ActionType.Create, "成功上传至多中心", model.SerializeObject());
                            base.InsetActionLog(ActionType.Create, "成功上传至多中心", string.Format("[{0}]{1}", model.FileName, model.FilePath));
                        }
                        else
                        {
                            //失败记录日志
                            base.InsetActionLog(ActionType.Create, "N上成功上传至多中心失败:" + resultModel.code, string.Format("[{0}]{1}", model.FileName, model.FilePath));
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex){
                this.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());

                throw new Exception(ex.Message);
            }
        }

        #region 暂时不用

        static List<FileModel> Files = new List<FileModel>();

        public struct FileModel
        {
            public Stream InputStream { get; set; }
            public string Extension { get; set; }

            public string FilePath { get; set; }

            public HttpFileCollectionBase HttpFile { get; set; }

            public long MaxLength { get; set; }

            public string FileName { get; set; }

            public string NewFileName { get; set; }
            public UploadTypeEnum Type { get; set; }

            public string GetFullPath { get; set; }
        }


        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="projectType">选择的项目类型，不同的类型放不同的文件夹</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult UploadFile(long projectType)
        {
            try
            {
                if (projectType == 0)
                    throw new Exception("请选择所属项目");

                string path = this.medicalDataProjectService.QueryEntity(projectType).Path ?? "Other";

                if (base.LoginUserinfo == null||base.LoginUserinfo.Id<=0)
                {
                    var _tmpErrorModel = new UploadResult() { IsSuccess = false, ErrorMessage = "请登录后再上传数据" };
                    return this.Content(_tmpErrorModel.SerializeObject());
                }

                var file = this.HttpContext.Request.Files[0];
                if (file == null || file.ContentLength <= 0)
                    throw new Exception("请选择文件");
                string newFileName = "";
                if (projectType != 6)
                {
                    var hospitalEntity = this.HospitalService.QueryEntity(base.LoginUserinfo == null ? long.Parse(satelliteService.QueryEntity(base.LoginSatelliteUserinfo.SatelliteId).PlaceOfWork) : base.LoginUserinfo.HospitalId);
                    if (hospitalEntity == null || hospitalEntity.Id <= 0)
                        throw new Exception("医院不存在");

                    //文件名称规范：原始文件名称_年月日_医院名称
                    newFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + hospitalEntity.Name;

                    // 文件命名规范：原始文件名称_医院的名称.后缀
                    newFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{hospitalEntity.Name}";
                }
                else
                {
                    newFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_卫星网";
                }

                UploadParameter par = new UploadParameter();
                UploadResult model = new UploadifyUpload().Upload(new UploadParameter()
                {
                    Extension = "xlsx,xls,dbf",
                    FilePath = $"/Content/Upload/MedicalData/{path}/{DateTime.Now.ToString("yyyyMMddHHmmss")}/",
                    HttpFile = this.HttpContext.Request.Files,
                    MaxLength = 70 * 1024, // 50M // 2020-01-17修改为70M
                    NewFileName = newFileName,
                    Type = UploadTypeEnum.Excel
                });
                if (model.IsSuccess)
                {
                    return this.Content(model.SerializeObject());
                }
                else
                {
                    throw new Exception("上传文件失败，原由：" + model.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                var errorModel = new UploadResult() { IsSuccess = false, ErrorMessage = ex.Message };
                return this.Content(errorModel.SerializeObject());
            }
        }

        #region //合并Excel-已注销

        //public static void MergeExcel(List<FileModel> files, string destinationFile)
        //{
        //    XSSFWorkbook product = new XSSFWorkbook();
        //    var newdestinationFile = @"C:\Users\df\Desktop\" + destinationFile;

        //    int itemIndex = 1;
        //    foreach (var item in files)
        //    {
        //        //stream.Write(byteArray, 0, (int)byteArray.Length);
        //        //HSSFWorkbook book1 = new HSSFWorkbook(fs);
        //        if (item.InputStream.Length == 0)
        //            continue;
        //        XSSFWorkbook book1 = new XSSFWorkbook(item.InputStream);
        //        for (int i = 0; i < book1.NumberOfSheets; i++)
        //        {
        //            XSSFSheet sheet1 = book1.GetSheetAt(i) as XSSFSheet;
        //            //avoid sheet has same name
        //            //string reportName = strFile.ReportTitleName;
        //            //if (string.IsNullOrEmpty(reportName))
        //            //{
        //            //    reportName = "";
        //            //}
        //            //else
        //            //{
        //            //    reportName = reportName.Replace("/", "").Replace("\\", "").Replace("[", "").Replace("]", "");
        //            //}
        //            sheet1.CopyTo(product, "_Sheet0" + itemIndex, true, true);
        //            itemIndex++;
        //        }
        //    }
        //    FileStream fs = new FileStream(newdestinationFile, FileMode.Create, FileAccess.Write);
        //    using (fs)
        //    {
        //        product.Write(fs);
        //    }
        //}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        private UploadResult CreateFlie(FileModel fileModel)
        {
            return new UploadifyUpload().Upload(new UploadParameter()
            {
                Extension = fileModel.Extension,
                FilePath = fileModel.FilePath,
                HttpFile = fileModel.HttpFile,
                MaxLength = fileModel.MaxLength,
                NewFileName = fileModel.FileName,
                Type = fileModel.Type
            });
        }
        #endregion

        #region 新的上传

        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="projectType">选择的项目类型，不同的类型放不同的文件夹</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile1(long projectType)
        {
            try
            {
                if (projectType == 0)
                {
                    throw new Exception("请选择所属项目");
                }
                string path = this.medicalDataProjectService.QueryEntity(projectType).Path ?? "Other";
                HttpPostedFileBase file = Request.Files.Get("myfile");
                if (file == null || file.ContentLength <= 0)
                {
                    throw new Exception("请选择文件");
                }
                if (file.ContentLength > (70 * 1024))
                {
                    throw new Exception("文件大小不能超过70M！");
                }
                var hospitalEntity = this.HospitalService.QueryEntity(base.LoginUserinfo.HospitalId);
                if (hospitalEntity == null || hospitalEntity.Id <= 0)
                {
                    throw new Exception("医院不存在");
                }
                //文件名称规范：原始文件名称_年月日_医院名称
                string extension = Path.GetExtension(file.FileName);
                string newFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + hospitalEntity.Name + extension;
                path = $"/Content/Upload/MedicalData/{path}/{DateTime.Now.ToString("yyyyMMddHHmm")}/";
                var newSavepath = SaveFilepath(path, newFileName);
                if (!string.IsNullOrWhiteSpace(extension))
                {
                    //文件后缀检查
                    //string extension = UploadHelper.GetExtensionName(file.FileName);
                    if (string.IsNullOrWhiteSpace(extension))
                    {

                    }
                }
                if (!Directory.Exists(Path.GetDirectoryName(newSavepath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newSavepath));
                }
                file.SaveAs(newSavepath);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var errorModel = new UploadResult() { IsSuccess = false, ErrorMessage = ex.Message };
                Console.WriteLine(errorModel.ErrorMessage);
                return this.Content(errorModel.SerializeObject());
            }
        }

        /// <summary>
        /// 保存路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SaveFilepath(string path, string fileName)
        {
            return new UploadFile().Savepath(path, fileName);
        }

        #endregion

        /// <summary>
        /// 上传数据结果页面
        /// </summary>
        /// <param name="dataId">MedicalDataId</param>
        /// <returns></returns>
        public ActionResult Result(long dataId)
        {
            try
            {
                var model = this.MedicalDataService.QueryEntity(dataId);
                if (model == null || model.Id <= 0 || string.IsNullOrWhiteSpace(model.UploadMessage)) throw new Exception("数据错误");

                return this.RedirectToAction("Detail", "Medicine", new { id = model.Id });

                UploadMedicalResult data = model.UploadMessage.DeserializeObject<UploadMedicalResult>();
                if (data == null) throw new Exception("数据错误");

                return this.View(data);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);
            }

            return this.View();
        }

        /// <summary>
        /// 根据医院名称查询医院数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ContentResult QueryHospital(string query)
        {
            var list = this.HospitalService.Query(m => m.Name.Contains(query)).OrderBy(m => m.Name).Take(10).Select(m =>
              {
                  return new
                  {
                      text = m.Name + (string.IsNullOrWhiteSpace(m.Code) ? "" : "（" + m.Code + "）"),
                      value = m.Id.ToString(),
                  };
              }).ToList();

            return this.Content(list.SerializeObject());
        }


        #endregion

        #region 详细页面

        /// <summary>
        /// 查看详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(long id)
        {
            try
            {
                var user = base.LoginUserinfo;

                var entity = this.MedicalDataService.QueryEntity(m => m.Id == id && m.Mark > 0);
                if (entity == null)
                    throw new Exception("数据不存在");

                //查看权限，只有自己和对应的项目管理员才能看到
                if (entity.MemberId != user.Id && !user.ProjectItem.Contains(entity.ProjectType.ToString()))
                    throw new Exception("您无权查看该数据");

                var model = entity.ToModel();
                model.UploadMessageEntity = entity.UploadMessage.DeserializeObject<UploadMessageModel>();
                
                try
                {
                    model.ValidateList = this.MedicalDataItemValidateService.Query(m => m.Mark > 0 && m.MedicalDataId == entity.Id).OrderBy(m => m.Sort).ToList();
                    model.ProjectTypeName = medicalDataProjectService.QueryEntity(model.ProjectType).Name;
                    if (model.ValidateList != null && model.ValidateList.Any())
                    {
                        model.ValidateErrorCount = model.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Error).Count();
                        model.ValidateWarningCount = model.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Warning).Count();
                    }
                }
                catch { }
                
                return this.View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
                base.ErrorNotification(ex.Message);
                return this.RedirectToAction("Index", "Medicine");
            }
        }

        /// <summary>
        /// 下载上传的原始数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DownloadOriginalData(long id)
        {
            try
            {
                string filePath = this.MedicalDataService.DownloadOriginalData(id);
                string path = this.Server.MapPath(filePath);
                //string ex = Path.GetExtension(path);
                //string name = "data-" + id + ex;
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
        /// 下载经过容错处理的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DownloadNewData(long id)
        {
            try
            {
                string filePath = this.MedicalDataService.DownloadNewData(id);
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
        /// 获取验证的分页数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataId">>对应的医学数据Id</param>
        /// <param name="level">0：表示全部  1：提示  2：警告  3：错误</param>
        /// <returns></returns>
        public JsonResult GetValidateList(DataSourceRequest command, long dataId, int level)
        {
            var IntegraIItemList = this.MedicalDataItemValidateService.QueryPage(dataId, level, command.Page - 1, command.PageSize);
            var newItemList = IntegraIItemList.Select(m => new
            {
                UploadRowIndex = m.UploadRowIndex,
                Level = m.Level,
                ErrorName = m.ErrorName,
                Content = m.Content,
                KeyName = m.KeyName,
                LevelName = ((MedicalDataItemValidateLevelEnum)m.Level).GetDescription()
            });

            return Json(new { total = IntegraIItemList.TotalPages, rows = newItemList });
        }

        /// <summary>
        /// 获取详细的分页数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataId">>对应的医学数据Id</param>
        /// <returns></returns>
        public JsonResult GetDataList(DataSourceRequest command, long dataId)
        {
            var IntegraIItemList = this.MedicalDataItemService.QueryPage(dataId, command.Page - 1, command.PageSize);

            return Json(new { total = IntegraIItemList.TotalPages, rows = IntegraIItemList });
        }

        /// <summary>
        /// 获取数据的word文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult DownloadWord(long id)
        {
            try
            {
                string fileName = "";
                string path = new MedicalDataWordServiceExtensions().CreateWord(id, ref fileName);
                new MedicalDataWordServiceExtensions().WordToPDF(Server.MapPath(Request.ApplicationPath + path));
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new Exception("Word文件生成失败！");
                }
                return File(path.Replace(".docx", ".pdf"), "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Content(@"<div style=""width: 80%; margin: 0 auto; text-align: center; margin-top: 10%;"">
    <h2 style="""">Word文件生成失败！</h2>
    <div style=""color: #999;"">
        <lable id=""secLable"" style=""font-size: 1.5rem; margin: 0 0.5rem; font-weight: bold; color: #fb6e52;"">5</lable>秒后自动返回
    </div>
</div>
<script>var t = 4, timer; function countdown() {if (t >= 0) { document.getElementById('secLable').innerText = t--; timer = setTimeout('countdown()', 1000);} else { clearTimeout(timer); location.href = '" + Url.Action("Detail", new { id = id }).ToString() + "'; } } timer = setTimeout('countdown()', 1000);</script>");
            }
        }
        #endregion

        /// <summary>
        /// 根据Id检查上传数据的状态
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        public ContentResult CheckStatus(long id)
        {
            if (id <= 0) return this.Content(SpringJsonResult.Error("数据不能为空"));

            try
            {
                if (this.MedicalDataService.CheckStatus(id))
                    return this.Content(SpringJsonResult.Success("分析成功"));
                else
                {
                    // 判断是否发生异常
                    var entity = this.MedicalDataService.QueryEntity(id);

                    if (entity != null && entity.Mark > 0 && !string.IsNullOrEmpty(entity.ExceptionMessage))
                    {
                        return Content(SpringJsonResult.Error(entity.ExceptionMessage));
                    }

                    if (entity != null && entity.Mark > 0 && !string.IsNullOrEmpty(entity.ExceptionStackTrace))
                    {
                        return Content(SpringJsonResult.Error("本次上传失败，请根据错误提示处理好数据文件后重新上传，感谢您的支持。"));
                    }
                }
                return this.Content(SpringJsonResult.Error(""));
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                return Content(SpringJsonResult.Error("本次上传失败，请根据错误提示处理好数据文件后重新上传，感谢您的支持。"));
            }

        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Data1()
        {
            return View();
        }

        public ActionResult Data2()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        #region 数据导入 - 未公开
        public ActionResult ImportHospitalWardLocation(string token, string filePath)
        {
            if (string.IsNullOrWhiteSpace(token) || !"9102nekot".Equals(token))
            {
                return Content("需要指定正确token确保已对待上传的数据进行处理");
            }
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Content("没有文件信息");
            }
            System.IO.FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                throw new Exception("未能读取到数据文件，文件不存在。");
            }

            DataTable table = null;
            switch (fileInfo.Extension.ToUpperInvariant())
            {
                case ".XLS":
                    table = Core.Utility.Excel.ImportDataTable.ExcelToDataTable(fileInfo.FullName, true);
                    break;
                case ".XLSX":
                    table = EPPlusHelper.WorksheetToTable(fileInfo.FullName);
                    break;
                default:
                    throw new Exception("文件格式不支持。");
            }

            if (table == null || table.Rows.Count <= 0) return Content("未能读取excel");
            DateTime now = DateTime.Now;
            int sort = 1;
            foreach (DataRow dataRow in table.Rows)
            {
                long _hospitalId = 0;
                long.TryParse(dataRow["HospitalId"].ToString(), out _hospitalId);
                if (_hospitalId <= 0) { continue; }
                string _ward = dataRow["Ward"].ToString();
                string department_EN = dataRow["Department_EN"].ToString();
                string location = dataRow["Location"].ToString();
                string location_Type = dataRow["Location_Type"].ToString();
                if (HospitalWardLocationService.Count(r => r.HospitalId == _hospitalId && r.Ward == _ward && r.Department_EN == department_EN && r.Location == location && r.Location_Type == location_Type && r.Mark == 1) == 0)
                {
                    HospitalWardLocationService.Insert(new HospitalWardLocation
                    {
                        Department_CN = dataRow["Department_CN"].ToString(),
                        Department_EN = department_EN,
                        HospitalId = _hospitalId,
                        Id = CommonHelper.GuidToLongID,
                        InsertTime = now,
                        Location = location,
                        Location_Type = location_Type,
                        Name = dataRow["Name"].ToString(),
                        Sort = sort++,
                        Mark = 1,
                        Version = 1,
                        Ward = _ward
                    });
                }
            }

            return Content($"完成! 导入{sort}条");
        }

        public ActionResult GetImportToken()
        {
            long timestamp = DateTime.Now.ToTimestamp();
            string nonce = Guid.NewGuid().ToString("N").Substring(0, 10);
            string token = ManageSystem.Core.Utility.DESEncrypt.Encrypt($"{nonce}&{timestamp + 300}&{LoginUserinfo.LoginId}", LoginUserinfo.LoginId);
            return Json(new
            {
                nonce,
                timestamp = timestamp.ToString(),
                token
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportMemberAndHospital(long timestamp, string nonce, string token, string filePath)
        {
            #region 验证
            if (timestamp <= 0 || string.IsNullOrWhiteSpace(token))
            {
                return Content("token不能为空");
            }

            long _timestamp = DateTime.Now.ToTimestamp();
            if (_timestamp > timestamp + 300)
            {
                return Content("token已过期");
            }

            string _token = ManageSystem.Core.Utility.DESEncrypt.Encrypt($"{nonce}&{timestamp + 300}&{LoginUserinfo.LoginId}", LoginUserinfo.LoginId);

            if (!_token.Equals(token))
            {
                return Content("token验证失败");
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Content("没有文件信息");
            }

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return Content("未能读取到数据文件，文件不存在。");
            }
            #endregion

            DataTable table = null;
            switch (fileInfo.Extension.ToUpperInvariant())
            {
                case ".XLS":
                    table = Core.Utility.Excel.ImportDataTable.ExcelToDataTable(fileInfo.FullName, true);
                    break;
                case ".XLSX":
                    table = EPPlusHelper.WorksheetToTable(fileInfo.FullName);
                    break;
                default:
                    return Content("文件格式不支持。");
            }

            if (table == null || table.Rows.Count <= 0) return Content("未能读取excel");
            DateTime now = DateTime.Now;

            DataTable errorTable = table.Clone();
            errorTable.Columns.Add("失败说明", typeof(string));
            int success = 0, error = 0;
            int sort = AreaService.MaxSort() + 1;
            foreach (DataRow dataRow in table.Rows)
            {
                try
                {
                    string _hospitalName = dataRow["医院名称"].ToString();
                    if (string.IsNullOrWhiteSpace(_hospitalName))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【医院名称】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    _hospitalName = _hospitalName.Trim();

                    string name = dataRow["接收人姓名"].ToString().Trim();
                    string phone = dataRow["接收人电话"].ToString().Trim();

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【接收人姓名】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(phone))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【接收人电话】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    long province_id = 0;
                    long city_Id = 0;
                    long region_id = 0;

                    long.TryParse(dataRow["省"].ToString().Trim(), out province_id);
                    long.TryParse(dataRow["市"].ToString().Trim(), out city_Id);
                    long.TryParse(dataRow["区"].ToString().Trim(), out region_id);

                    if (province_id <= 0)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【省】数据转换为long类型失败，值：{dataRow["省"].ToString().Trim()}";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    if (city_Id <= 0)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【市】数据转换为long类型失败，值：{dataRow["市"].ToString().Trim()}";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    if (region_id <= 0)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【区】数据转换为long类型失败，值：{dataRow["区"].ToString().Trim()}";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    string shshq = dataRow["省市区"].ToString().Trim();
                    string address = dataRow["地址"].ToString().Trim();
                    string loginId = dataRow["账号"].ToString().Trim();
                    string pwd = dataRow["密码"].ToString().Trim();

                    if (string.IsNullOrWhiteSpace(loginId))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【账号】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(pwd))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【密码】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    if (pwd.Length <= 5)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【密码】过去简单，至少需要6位";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    long _hospitalId = 0;
                    if (HospitalService.Count(r => r.Name == _hospitalName && r.Mark > 0) > 0)
                    {
                        _hospitalId = (HospitalService.QueryEntity(r => r.Name == _hospitalName && r.Mark > 0)?.Id) ?? 0;
                    }

                    if (_hospitalId <= 0)
                    {
                        // 新增医院信息
                        Hospital hospital = new Hospital
                        {
                            Id = CommonHelper.GuidToLongID,
                            Name = _hospitalName,
                            Sort = sort++,
                            State = true,
                            Address = null,
                            Content = null,
                            ContactsUser = null,
                            ContactsTel = null,
                            Describe = null,
                            InsertTime = now,
                            UpdateTime = now,
                            DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                            Version = 1,
                            Mark = 1,
                            MemberId = 0,
                            Code = null,
                            ProvinceId = province_id,
                            ProvinceName = AreaService.QueryEntity(province_id)?.Name,
                            IsTeam = false
                        };

                        HospitalService.Insert(hospital);
                        _hospitalId = hospital.Id;
                    }

                    if (MemberService.Count(r => r.LoginId == loginId && r.Mark > 0) > 0)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【账号】{loginId}已经存在";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    Member member = new Core.Domain.Members.Member
                    {
                        Id = CommonHelper.GuidToLongID,
                        Name = name,
                        NickName = name,
                        OpenId = null,
                        Phone = phone,
                        Email = null,
                        Birthday = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                        Status = (int)MemberStatus.Normal,
                        Sex = (int)MemberSex.Unknown,
                        Type = (int)MemberType.Doctor,
                        Describe = null,
                        HeadImage = null,
                        LoginId = loginId,
                        Password = pwd,
                        InsertTime = now,
                        UpdateTime = now,
                        DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                        Version = 1,
                        Mark = 1,
                        AreaId = province_id,
                        HospitalId = _hospitalId,
                        DoctorTitleId = 0,
                        InputInviteCode = null,
                        InviteCode = MemberExtensions.GetInviteCode(),
                        HospitalDepartmentId = 0,
                        IntegralAmount = 0,
                        OtherHospital = null,
                        MedicineEmail = null,
                        ProjectItem = "[]",
                        IsTest = false,
                        IsSettingPassword = true,
                        ProvinceId = province_id,
                        CityId = city_Id,
                        DistrictsId = region_id,
                        Area = shshq,
                        Address = address,
                        DefaultProject = 0
                    };
                    MemberAddress memberAddress = new MemberAddress
                    {
                        Id = CommonHelper.GuidToLongID,
                        MemberId = member.Id,
                        Name = member.Name,
                        Email = member.Email,
                        Address = member.Address,
                        Phone = member.Phone,
                        ZipPostalCode = null,
                        Tel = member.Phone,
                        IsMain = true,
                        ProvinceId = member.ProvinceId,
                        CityId = member.CityId,
                        DistrictsId = member.DistrictsId,
                        Remark = null,
                        InsertTime = now,
                        UpdateTime = now,
                        DeleteTime = member.DeleteTime,
                        Version = 1,
                        Mark = 1,
                        Describe = null,
                        Area = member.Area
                    };
                    MemberService.Insert(member);
                    memberAddressService.Insert(memberAddress);
                    success++;
                }
                catch (Exception ex)
                {
                    var newRow = errorTable.NewRow();
                    foreach (DataColumn item in table.Columns)
                    {
                        newRow[item.Caption] = dataRow[item];
                    }
                    newRow["失败说明"] = ex.Message;
                    errorTable.Rows.Add(newRow);
                    error++;
                }
            }

            if (error > 0)
            {
                string excelName = $"{Guid.NewGuid().ToString("N")}.xlsx";
                string excelPath = Server.MapPath($"/Content/{excelName}");
                System.IO.FileInfo newFile = new System.IO.FileInfo(excelPath);
                if (!newFile.Exists)
                {
                    System.IO.File.Create(excelPath).Close();

                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        int rowIndex = 2;

                        //创建ExcelWorkSheet对象，这个对象就是面对表的，是工作簿中单个表
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                        foreach (DataColumn item in errorTable.Columns)
                        {
                            worksheet.Cells[1, errorTable.Columns.IndexOf(item) + 1].Value = item.Caption;
                        }

                        foreach (DataRow row in errorTable.Rows)
                        {
                            foreach (DataColumn col in errorTable.Columns)
                            {
                                worksheet.Cells[rowIndex, errorTable.Columns.IndexOf(col) + 1].Value = row[col].ToString();
                            }
                            rowIndex++;
                        }

                        excelPackage.SaveAs(newFile);
                    }
                }

                return Content($@"<section style=""width: 75%; margin: 0 auto; padding: 25px;"">
<h4>导入完成<h4>
<p style=""color: #1ab394; line-height: 1.5rem;"">成功：{success}条</p>
<p style=""color: #ed5565; line-height: 1.5rem;"">失败：{error}条</p>
<a href=""/Content/{excelName}"">点击下载上传失败的数据</a>
</section>");

            }
            else
            {
                return Content($@"<section style=""width: 75%; margin: 0 auto; padding: 25px;"">
<h4>导入完成<h4>
<p style=""color: #1ab394; line-height: 1.5rem;"">成功：{success}条</p>
<p style=""color: #ed5565; line-height: 1.5rem;"">失败：{error}条</p>
</section>");
            }
        }

        public ActionResult ImportMemberAndHospitalHENAN(long timestamp, string nonce, string token, string filePath)
        {
            #region 验证
            if (timestamp <= 0 || string.IsNullOrWhiteSpace(token))
            {
                return Content("token不能为空");
            }

            long _timestamp = DateTime.Now.ToTimestamp();
            if (_timestamp > timestamp + 300)
            {
                return Content("token已过期");
            }

            string _token = ManageSystem.Core.Utility.DESEncrypt.Encrypt($"{nonce}&{timestamp + 300}&{LoginUserinfo.LoginId}", LoginUserinfo.LoginId);

            if (!_token.Equals(token))
            {
                return Content("token验证失败");
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Content("没有文件信息");
            }

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return Content("未能读取到数据文件，文件不存在。");
            }
            #endregion

            DataTable table = null;
            switch (fileInfo.Extension.ToUpperInvariant())
            {
                case ".XLS":
                    table = Core.Utility.Excel.ImportDataTable.ExcelToDataTable(fileInfo.FullName, true);
                    break;
                case ".XLSX":
                    table = EPPlusHelper.WorksheetToTable(fileInfo.FullName);
                    break;
                default:
                    return Content("文件格式不支持。");
            }

            if (table == null || table.Rows.Count <= 0) return Content("未能读取excel");
            DateTime now = DateTime.Now;

            DataTable errorTable = table.Clone();
            errorTable.Columns.Add("失败说明", typeof(string));
            int success = 0, error = 0;
            int sort = AreaService.MaxSort() + 1;
            foreach (DataRow dataRow in table.Rows)
            {
                try
                {
                    string _hospitalName = dataRow["医院名称"].ToString();
                    if (string.IsNullOrWhiteSpace(_hospitalName))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【医院名称】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    _hospitalName = _hospitalName.Trim();

                    string name = dataRow["姓名"].ToString().Trim();
                    string phone = dataRow["手机号"].ToString().Trim();
                    string email = dataRow["邮箱"].ToString().Trim().Replace(" ", "");
                    int defaultProject = 0;
                    int.TryParse(dataRow["所属项目"].ToString().Trim(), out defaultProject);

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【姓名】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    //if (string.IsNullOrWhiteSpace(phone))
                    //{
                    //    var newRow = errorTable.NewRow();
                    //    foreach (DataColumn item in table.Columns)
                    //    {
                    //        newRow[item.Caption] = dataRow[item];
                    //    }
                    //    newRow["失败说明"] = $"【手机号】不能为空";
                    //    errorTable.Rows.Add(newRow);
                    //    error++;
                    //    continue;
                    //}

                    long province_id = 0;
                    long city_Id = 0;
                    long region_id = 0;

                    long.TryParse(dataRow["省"].ToString().Trim(), out province_id);
                    long.TryParse(dataRow["市"].ToString().Trim(), out city_Id);
                    long.TryParse(dataRow["区"].ToString().Trim(), out region_id);

                    if (province_id <= 0)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【省】数据转换为long类型失败，值：{dataRow["省"].ToString().Trim()}";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    if (city_Id <= 0)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【市】数据转换为long类型失败，值：{dataRow["市"].ToString().Trim()}";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    //if (region_id <= 0)
                    //{
                    //    var newRow = errorTable.NewRow();
                    //    foreach (DataColumn item in table.Columns)
                    //    {
                    //        newRow[item.Caption] = dataRow[item];
                    //    }
                    //    newRow["失败说明"] = $"【区】数据转换为long类型失败，值：{dataRow["区"].ToString().Trim()}";
                    //    errorTable.Rows.Add(newRow);
                    //    error++;
                    //    continue;
                    //}

                    string shshq = dataRow["省市区"].ToString().Trim();
                    string loginId = dataRow["账号"].ToString().Trim();
                    string pwd = dataRow["密码"].ToString().Trim();

                    if (string.IsNullOrWhiteSpace(loginId))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【账号】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(pwd))
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【密码】不能为空";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }
                    if (pwd.Length <= 5)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【密码】过去简单，至少需要6位";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    long _hospitalId = 0;
                    if (HospitalService.Count(r => r.Name == _hospitalName && r.Mark > 0) > 0)
                    {
                        _hospitalId = (HospitalService.QueryEntity(r => r.Name == _hospitalName && r.Mark > 0)?.Id) ?? 0;
                    }

                    if (_hospitalId <= 0)
                    {
                        // 新增医院信息
                        Hospital hospital = new Hospital
                        {
                            Id = CommonHelper.GuidToLongID,
                            Name = _hospitalName,
                            Sort = sort++,
                            State = true,
                            Address = null,
                            Content = null,
                            ContactsUser = null,
                            ContactsTel = null,
                            Describe = null,
                            InsertTime = now,
                            UpdateTime = now,
                            DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                            Version = 1,
                            Mark = 1,
                            MemberId = 0,
                            Code = null,
                            ProvinceId = province_id,
                            ProvinceName = AreaService.QueryEntity(province_id)?.Name,
                            IsTeam = false
                        };

                        HospitalService.Insert(hospital);
                        _hospitalId = hospital.Id;
                    }

                    if (MemberService.Count(r => r.LoginId == loginId && r.Mark > 0) > 0)
                    {
                        var newRow = errorTable.NewRow();
                        foreach (DataColumn item in table.Columns)
                        {
                            newRow[item.Caption] = dataRow[item];
                        }
                        newRow["失败说明"] = $"【账号】{loginId}已经存在";
                        errorTable.Rows.Add(newRow);
                        error++;
                        continue;
                    }

                    Member member = new Core.Domain.Members.Member
                    {
                        Id = CommonHelper.GuidToLongID,
                        Name = name,
                        NickName = name,
                        OpenId = loginId,
                        Phone = phone,
                        Email = email,
                        Birthday = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                        Status = (int)MemberStatus.Normal,
                        Sex = (int)MemberSex.Unknown,
                        Type = (int)MemberType.Doctor,
                        Describe = null,
                        HeadImage = null,
                        LoginId = loginId,
                        Password = pwd,
                        InsertTime = now,
                        UpdateTime = now,
                        DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                        Version = 1,
                        Mark = 1,
                        AreaId = province_id,
                        HospitalId = _hospitalId,
                        DoctorTitleId = 0,
                        InputInviteCode = null,
                        InviteCode = MemberExtensions.GetInviteCode(),
                        HospitalDepartmentId = 0,
                        IntegralAmount = 0,
                        OtherHospital = null,
                        MedicineEmail = null,
                        ProjectItem = "[]",
                        IsTest = false,
                        IsSettingPassword = true,
                        ProvinceId = province_id,
                        CityId = city_Id,
                        DistrictsId = region_id,
                        Area = shshq,
                        Address = null,
                        DefaultProject = defaultProject
                    };
                    MemberService.Insert(member);
                    success++;
                }
                catch (Exception ex)
                {
                    var newRow = errorTable.NewRow();
                    foreach (DataColumn item in table.Columns)
                    {
                        newRow[item.Caption] = dataRow[item];
                    }
                    newRow["失败说明"] = ex.Message;
                    errorTable.Rows.Add(newRow);
                    error++;
                }
            }

            if (error > 0)
            {
                string excelName = $"{Guid.NewGuid().ToString("N")}.xlsx";
                string excelPath = Server.MapPath($"/Content/{excelName}");
                System.IO.FileInfo newFile = new System.IO.FileInfo(excelPath);
                if (!newFile.Exists)
                {
                    System.IO.File.Create(excelPath).Close();

                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        int rowIndex = 2;

                        //创建ExcelWorkSheet对象，这个对象就是面对表的，是工作簿中单个表
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                        foreach (DataColumn item in errorTable.Columns)
                        {
                            worksheet.Cells[1, errorTable.Columns.IndexOf(item) + 1].Value = item.Caption;
                        }

                        foreach (DataRow row in errorTable.Rows)
                        {
                            foreach (DataColumn col in errorTable.Columns)
                            {
                                worksheet.Cells[rowIndex, errorTable.Columns.IndexOf(col) + 1].Value = row[col].ToString();
                            }
                            rowIndex++;
                        }

                        excelPackage.SaveAs(newFile);
                    }
                }

                return Content($@"<section style=""width: 75%; margin: 0 auto; padding: 25px;"">
<h4>导入完成<h4>
<p style=""color: #1ab394; line-height: 1.5rem;"">成功：{success}条</p>
<p style=""color: #ed5565; line-height: 1.5rem;"">失败：{error}条</p>
<a href=""/Content/{excelName}"">点击下载上传失败的数据</a>
</section>");

            }
            else
            {
                return Content($@"<section style=""width: 75%; margin: 0 auto; padding: 25px;"">
<h4>导入完成<h4>
<p style=""color: #1ab394; line-height: 1.5rem;"">成功：{success}条</p>
<p style=""color: #ed5565; line-height: 1.5rem;"">失败：{error}条</p>
</section>");
            }
        }
        #endregion

        #region CR用户发送短信通知 -- 未公开
        public ActionResult SendSmsToken(string mobile)
        {
            if (!mobile.Equals(LoginUserinfo.LoginId))
            {
                return Content("非法访问");
            }

            long timestamp = DateTime.Now.ToTimestamp();
            string nonce = Guid.NewGuid().ToString("N").Substring(0, 10);
            string token = ManageSystem.Core.Utility.DESEncrypt.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                timestamp = timestamp,
                nonce = nonce,
                user = LoginUserinfo.LoginId,
                type = "SMS"
            }), LoginUserinfo.LoginId);
            return Json(new
            {
                timestamp = timestamp.ToString(),
                nonce,
                token
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CR_SendSMS(long timestamp, string nonce, string token)
        {
            long _timestamp = DateTime.Now.ToTimestamp();
            if (_timestamp > timestamp + 300)
            {
                return Json(new
                {
                    status = false,
                    message = "token已过期"
                }, JsonRequestBehavior.AllowGet);
            }

            string _token = ManageSystem.Core.Utility.DESEncrypt.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                timestamp = timestamp,
                nonce = nonce,
                user = LoginUserinfo.LoginId,
                type = "SMS"
            }), LoginUserinfo.LoginId);

            if (!_token.Equals(token))
            {
                return Json(new
                {
                    status = false,
                    message = "token验证失败"
                }, JsonRequestBehavior.AllowGet);
            }
            List<Member> members = new List<Member>();
            //List<Member> members = MemberService.Query(r => r.LoginId.StartsWith("CRFW") && r.Mark > 0 && System.Data.Entity.SqlServer.SqlFunctions.DateDiff("ss", "2019-08-27 12:00:00.000", r.InsertTime) > 0);
            //members = MemberService.Query(r => r.Phone == "13761992263" && r.Mark > 0);
            string key = Services.Configuration.WebSettingService.GetWebSMS();

            int success = 0;
            int fail = 0;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("用户ID", typeof(string));
            dataTable.Columns.Add("姓名", typeof(string));
            dataTable.Columns.Add("手机号", typeof(string));
            dataTable.Columns.Add("账号", typeof(string));
            dataTable.Columns.Add("密码", typeof(string));
            dataTable.Columns.Add("失败原因", typeof(string));
            foreach (Member item in members)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    byte[] b = wc.DownloadData($"http://v.juhe.cn/sms/send?mobile={item.Phone}&tpl_id=210471&tpl_value={HttpUtility.UrlEncode($"#account#={item.LoginId}&#password#={encryptionService.DecryptText(item.Password)}")}&key={key}");
                    string s = Encoding.GetEncoding("utf-8").GetString(b);
                    SmsApiResult result = s.DeserializeObject<SmsApiResult>();
                    if (result.error_code == 0)
                    {
                        ++success;
                    }
                    else
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["用户ID"] = item.Id.ToString();
                        dr["姓名"] = item.Name;
                        dr["手机号"] = item.Phone;
                        dr["账号"] = item.LoginId;
                        dr["密码"] = encryptionService.DecryptText(item.Password);
                        dr["失败原因"] = result.reason;
                        dataTable.Rows.Add(dr);
                        ++fail;
                    }
                }
            }

            if (dataTable.Rows.Count > 0)
            {
                string excelName = $"{Guid.NewGuid().ToString("N")}.xlsx";
                string excelPath = Server.MapPath($"/Content/{excelName}");
                System.IO.FileInfo newFile = new System.IO.FileInfo(excelPath);
                if (!newFile.Exists)
                {
                    System.IO.File.Create(excelPath).Close();

                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        int rowIndex = 2;

                        //创建ExcelWorkSheet对象，这个对象就是面对表的，是工作簿中单个表
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                        foreach (DataColumn item in dataTable.Columns)
                        {
                            worksheet.Cells[1, dataTable.Columns.IndexOf(item) + 1].Value = item.Caption;
                        }

                        foreach (DataRow row in dataTable.Rows)
                        {
                            foreach (DataColumn col in dataTable.Columns)
                            {
                                worksheet.Cells[rowIndex, dataTable.Columns.IndexOf(col) + 1].Value = row[col].ToString();
                            }
                            rowIndex++;
                        }

                        excelPackage.SaveAs(newFile);
                    }
                }

                return Content($@"<section style=""width: 75%; margin: 0 auto; padding: 25px;"">
<h4>结果：<h4>
<p style=""color: #1ab394; line-height: 1.5rem;"">成功：{success}条</p>
<p style=""color: #ed5565; line-height: 1.5rem;"">失败：{fail}条</p>
<a href=""/Content/{excelName}"">点击下载发送失败的数据</a>
</section>");
            }
            else
            {
                return Json(new
                {
                    status = true,
                    message = $"成功发送短信{success}条",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CR_SendSMSByMobile(long timestamp, string nonce, string token, string mobile)
        {
            long _timestamp = DateTime.Now.ToTimestamp();
            if (_timestamp > timestamp + 300)
            {
                return Json(new
                {
                    status = false,
                    message = "token已过期"
                }, JsonRequestBehavior.AllowGet);
            }

            string _token = ManageSystem.Core.Utility.DESEncrypt.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                timestamp = timestamp,
                nonce = nonce,
                user = LoginUserinfo.LoginId,
                type = "SMS"
            }), LoginUserinfo.LoginId);

            if (!_token.Equals(token))
            {
                return Json(new
                {
                    status = false,
                    message = "token验证失败"
                }, JsonRequestBehavior.AllowGet);
            }
            mobile = mobile.Trim().Replace(" ", "");

            List<Member> members = MemberService.Query(r => r.LoginId.StartsWith("CRFW") && r.Mark > 0 && r.Phone == mobile && System.Data.Entity.SqlServer.SqlFunctions.DateDiff("ss", "2019-08-27 12:00:00.000", r.InsertTime) > 0);

            string key = Services.Configuration.WebSettingService.GetWebSMS();

            int success = 0;
            int fail = 0;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("用户ID", typeof(string));
            dataTable.Columns.Add("姓名", typeof(string));
            dataTable.Columns.Add("手机号", typeof(string));
            dataTable.Columns.Add("账号", typeof(string));
            dataTable.Columns.Add("密码", typeof(string));
            dataTable.Columns.Add("失败原因", typeof(string));

            foreach (Member item in members)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    byte[] b = wc.DownloadData($"http://v.juhe.cn/sms/send?mobile={item.Phone}&tpl_id=210471&tpl_value={HttpUtility.UrlEncode($"#account#={item.LoginId}&#password#={encryptionService.DecryptText(item.Password)}")}&key={key}");
                    string s = Encoding.GetEncoding("utf-8").GetString(b);
                    SmsApiResult result = s.DeserializeObject<SmsApiResult>();
                    if (result.error_code == 0)
                    {
                        ++success;
                    }
                    else
                    {
                        DataRow dr = dataTable.NewRow();
                        dr["用户ID"] = item.Id.ToString();
                        dr["姓名"] = item.Name;
                        dr["手机号"] = item.Phone;
                        dr["账号"] = item.LoginId;
                        dr["密码"] = encryptionService.DecryptText(item.Password);
                        dr["失败原因"] = result.reason;
                        dataTable.Rows.Add(dr);
                        ++fail;
                    }
                }
            }

            if (dataTable.Rows.Count > 0)
            {
                string excelName = $"{Guid.NewGuid().ToString("N")}.xlsx";
                string excelPath = Server.MapPath($"/Content/{excelName}");
                System.IO.FileInfo newFile = new System.IO.FileInfo(excelPath);
                if (!newFile.Exists)
                {
                    System.IO.File.Create(excelPath).Close();

                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        int rowIndex = 2;

                        //创建ExcelWorkSheet对象，这个对象就是面对表的，是工作簿中单个表
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                        foreach (DataColumn item in dataTable.Columns)
                        {
                            worksheet.Cells[1, dataTable.Columns.IndexOf(item) + 1].Value = item.Caption;
                        }

                        foreach (DataRow row in dataTable.Rows)
                        {
                            foreach (DataColumn col in dataTable.Columns)
                            {
                                worksheet.Cells[rowIndex, dataTable.Columns.IndexOf(col) + 1].Value = row[col].ToString();
                            }
                            rowIndex++;
                        }

                        excelPackage.SaveAs(newFile);
                    }
                }

                return Content($@"<section style=""width: 75%; margin: 0 auto; padding: 25px;"">
<h4>结果：<h4>
<p style=""color: #1ab394; line-height: 1.5rem;"">成功：{success}条</p>
<p style=""color: #ed5565; line-height: 1.5rem;"">失败：{fail}条</p>
<a href=""/Content/{excelName}"">点击下载发送失败的数据</a>
</section>");
            }
            else
            {
                return Json(new
                {
                    status = true,
                    message = $"成功发送短信{success}条",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ContentResult GetDistrictList(long parentId)
        {
            var data = this.AreaService.QueryByParentId(parentId).SerializeObject();
            return this.Content(data);
        }
        [CheckRole(false)]
        public ContentResult UpdateArea(long parentId)
        {
            try
            {
                var entity = this.MemberService.QueryEntity(LoginUserinfo.Id);
                entity.ProvinceId = parentId;
                this.MemberService.Update(entity);
            }
            catch (Exception ex)
            {

                Log4Helper.Error(ex.Message);
                this.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
                return this.Content(SpringJsonResult.Error(ex.Message));
            }
            return this.Content(JsonHelper.GetBaseMessage(true, "绑定成功"));
        }

        /// <summary>
        /// 合并excel文件
        /// </summary>
        /// <returns></returns>
        public string MergeExccel(List<string> filePathList)
        {
            try
            {
                string savePath = $"/Content/Upload/MedicalData/Chinet/{DateTime.Now.ToString("yyyyMMddHHmmss")}/" + "MergeFiles.xlsx";
                if (filePathList.Count == 1)
                {
                    #region //单个文件保存

                    //System.Web.HttpContext.Current.Server.MapPath(outPath)
                    Workbook newbook = new Workbook();
                    newbook.Version = ExcelVersion.Version2013;

                    //删除文档中的工作表（新创建的文档默认包含3张工作表）
                    newbook.Worksheets.Clear();

                    //创建一个临时的workbook，用于加载需要合并的Excel文档
                    Workbook tempbook = new Workbook();

                    //遍历数组
                    for (int i = 0; i < filePathList.Count; i++)
                    {
                        ;
                        //载入Excel文档
                        tempbook.LoadFromFile(this.Server.MapPath(filePathList[i].Split('^')[1]));

                        //使用AddCopy方法，将文档中的所有工作表添加到新的workbook
                        foreach (Worksheet sheet in tempbook.Worksheets)
                        {
                            newbook.Worksheets.AddCopy(sheet);
                        }
                    }

                    //保存文档
                    newbook.SaveToFile(savePath, ExcelVersion.Version2013);
                    if (!System.IO.File.Exists(Server.MapPath(savePath)))
                    {
                        newbook.SaveToFile(Server.MapPath(savePath), ExcelVersion.Version2013);
                        if (!System.IO.File.Exists(Server.MapPath(savePath)))
                        {
                            Log4Helper.Error("文件保存成功：" + savePath);
                        }
                    }

                    #endregion
                }
                else
                {
                    #region //多个文件合并

                    List<Workbook> workbookList = new List<Workbook>();

                    Workbook workbook1 = new Workbook();
                    //加载第一个Excel文件
                    workbook1.LoadFromFile(this.Server.MapPath(filePathList[0].Split('^')[1]));

                    for (int i = 1; i < filePathList.Count; i++)
                    {
                        Workbook workbook = new Workbook();
                        //加载第一个Excel文件
                        workbook.LoadFromFile(this.Server.MapPath(filePathList[i].Split('^')[1]));
                        workbookList.Add(workbook);
                    }

                    //获取第一个文件的第一张工作表
                    Worksheet sheet1 = workbook1.Worksheets[0];

                    for (int i = 0; i < workbookList.Count; i++)
                    {
                        Worksheet worksheet = workbookList[i].Worksheets[0];
                        DataTable dataTable = worksheet.ExportDataTable();
                        //sheet1.InsertDataTable(dataTable, false, sheet1.LastRow + 1, 1);

                        //获取第一个表结构
                        DataTable tmpdt = sheet1.ExportDataTable().Clone();

                        //组装表结构
                        foreach (DataColumn dc in dataTable.Columns)
                        {
                            if (!tmpdt.Columns.Contains(dc.ColumnName))
                            {
                                tmpdt.Columns.Add(dc.ColumnName);
                            }
                        }
                        //填充数据
                        foreach(DataRow dr in dataTable.Rows)
                        {
                            var ndr = tmpdt.NewRow();
                            foreach (DataColumn dc in dataTable.Columns)
                            {
                                ndr[dc.ColumnName] = dr[dc.ColumnName];
                            }
                            tmpdt.Rows.Add(ndr);
                        }
                        sheet1.InsertDataTable(tmpdt, false, sheet1.LastRow + 1, 1);
                    }
                    //保存文件
                    workbook1.SaveToFile(savePath, ExcelVersion.Version2013);

                    //Peng-2023-01-16
                    if (!System.IO.File.Exists(Server.MapPath(savePath)))
                    {
                        Log4Helper.Error("未找到合并的文件，再次保存");
                        workbook1.SaveToFile(Server.MapPath(savePath), ExcelVersion.Version2013);
                        if (!System.IO.File.Exists(Server.MapPath(savePath)))
                        {
                            Log4Helper.Error("文件合并保存成功：" + savePath);
                        }
                    }

                    #endregion
                }
                return savePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log4Helper.Error(ex.Message);
                return "";
            }

        }
    }

    public class TestResult
    {
        /// <summary>
        /// 0成功
        /// </summary>
        public int code { get; set; }
        public string message { get; set; }
    }
}