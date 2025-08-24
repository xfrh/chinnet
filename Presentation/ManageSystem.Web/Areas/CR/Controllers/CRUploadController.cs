using ManageSystem.Core.Domain.CRs;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Framework.Upload;
using ManageSystem.Services.CRs;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Project;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Newtonsoft.Json;

namespace ManageSystem.Web.Areas.CR.Controllers
{
    public class CRUploadController : WebBaseController
    {
        // GET: CRs/CRUpload
        private readonly ICRService ProjectService;
        private readonly ICRItemService CRItemService;
        private readonly ICRLogService CRLogService;
        private readonly IMemberService MemberService;
        private readonly IHospitalService hospitalService;
        private readonly IAreaService areaService;

        public CRUploadController(ICRService _projectService, IMemberService _memberService,
            IHospitalService _hospitalService, IAreaService _areaService,
            ICRItemService _CRItemService, ICRLogService _CRLogService)
        {
            this.ProjectService = _projectService;
            this.MemberService = _memberService;
            this.hospitalService = _hospitalService;
            this.areaService = _areaService;
            this.CRItemService = _CRItemService;
            this.CRLogService = _CRLogService;
        }

        [CheckRole(false)]
        // GET: CR/Upload
        /// <summary>
        /// CR复敏管理 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var member = base.LoginUserinfo;
            if (!member.SpecialManager.Contains("cr"))
            {
                base.TransferError("只有替加环素用户才能使用此功能");
                    return new EmptyResult();
            }
            var entity = this.ProjectService.QueryEntityByMember(member.Id);
            if (entity == null || entity.Id <= 0) //用户还未提交过数据，跳转到新增的页面
                return this.RedirectToAction("Add", "CRUpload");

            //用户提交过数据了，默认跳转到上传数据的页面
            return this.RedirectToAction("Upload", "CRUpload");
        }

        /// <summary>
        /// CR复敏数据管理   暂时废弃掉，不要列表页面了
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index2(int pageIndex = 1)
        {
            CenterProjectModel model = new CenterProjectModel();

            var member = base.LoginUserinfo;
            if (!member.SpecialManager.Contains("cr"))
            {
                base.TransferError("只有替加环素用户才能使用此功能");
                return new EmptyResult();
            }

            var list = this.ProjectService.Query(member.Id);

            var data = list.Select(x => new CenterProjectItemModel()
            {
                Id = x.Id,
                InsertTime = x.InsertTime,
                HospitalName = x.HospitalName,
                HospitalGrade = x.HospitalGrade,
                Name = x.Name,
                Phone = x.Phone,
                FileName = x.FileName,
                EmailStatus = x.EmailStatus,
                UploadFilePath = x.UploadFilePath
            }).ToPagedList<CenterProjectItemModel>(pageIndex, MvcPagerExtensions.PageSize);

            model.PageList = data;
            return this.View(model);
        }

        /// <summary>
        /// CR复敏数据管理    删除  暂时废弃掉了，不能删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult Delete(long id)
        {
            return this.Content("");
            try
            {
                if (id <= 0)
                    return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));

                var entity = this.ProjectService.QueryEntity(id);
                if (entity == null || entity.Id <= 0)
                    return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在，请刷新网页重试"));

                if (entity.MemberId != this.LoginUserinfo.Id)
                    return this.Content(JsonHelper.GetBaseMessage(false, "您无权操作"));

                this.ProjectService.Delete(id);
                base.InsetActionLog(ActionType.Delete, "【手动】删除CR复敏数据", entity.SerializeObject());

                var user = this.LoginUserinfo;
                this.CRLogService.Insert(ActionType.Delete, ActionSource.Web, id, user.Id, user.Name, "删除CR复敏数据", "删除CR复敏数据");

                return this.Content(JsonHelper.GetBaseMessage(true, "删除成功"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }


        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckRole(false)]
        public ActionResult Add()
        {
            var member = base.LoginUserinfo;

            var entity = this.ProjectService.QueryEntityByMember(member.Id);
            if (entity != null && entity.Id > 0)
                return this.RedirectToAction("Upload", "CRUpload");//用户提交过数据了，默认跳转到上传数据的页面

            CRData model = new CRData();

            //情况一：用户首次提交CR数据，直接读取用户相关的数据
            var hospital = this.hospitalService.QueryEntity(member.HospitalId) ?? new Hospital();
            model = new CRData()
            {
                MemberId = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                HospitalId = hospital.Id,
                HospitalName = hospital.Name,
                Address = member.Address,
                Area = member.Area,
                ProvinceId = member.ProvinceId,
                CityId = member.CityId,
                DistrictsId = member.DistrictsId
            };
            return this.View(model);
        }

        /// <summary>
        /// 添加  提交数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Add(CRData model)
        {
            #region 数据验证

            if (model == null)
                return this.Content(JsonHelper.GetBaseMessage(false, "数据错误"));
            var member = base.LoginUserinfo;
            if (member == null || member.Id <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "请登录"));
            if (string.IsNullOrWhiteSpace(model.HospitalName))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入医院名称"));
            if (string.IsNullOrWhiteSpace(model.HospitalGrade))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入医院等级"));
            if (model.DistrictsId <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "请选择省市区"));
            if (string.IsNullOrWhiteSpace(model.Address))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入详细地址"));
            //if (string.IsNullOrWhiteSpace(model.CREDetectionRate))
            //    return this.Content(JsonHelper.GetBaseMessage(false, "请输入上年CRE平均检出率"));
            //if (string.IsNullOrWhiteSpace(model.CREDrugRate))
            //    return this.Content(JsonHelper.GetBaseMessage(false, "请输入上年CRAB平均检出率"));
            if (string.IsNullOrWhiteSpace(model.MHSource))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入MH平板来源厂家"));
            if (string.IsNullOrEmpty(model.Name))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入微生物联系人"));
            if (string.IsNullOrEmpty(model.Phone))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入联系电话"));
            if (model.Email.IndexOf("@") == -1)
                return this.Content(JsonHelper.GetBaseMessage(false, "邮箱格式不正确"));

            if (!member.SpecialManager.Contains("cr"))
                return this.Content(JsonHelper.GetBaseMessage(false, "只有替加环素用户才能使用此功能"));

            //1个用户只能提交一次
            var entity = this.ProjectService.QueryEntityByMember(member.Id);
            if (entity != null && entity.Id > 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "您已提交过了，请勿重复提交"));

            #endregion

            #region  业务处理

            try
            {
                model.Id= CommonHelper.GuidToLongID;
                model.MemberId = member.Id;
                model.MemberName = member.Name;
                model.EmailStatus = 1;
                model.FileName = "";
                model.UploadFilePath = "";
                model.EmailTime = DateHelper.DefaultValue();
                model.HospitalTime = DateHelper.DefaultValue();
                model.EmailStr = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
                model.HospitalId = member.HospitalId;
                model.CREDetectionRate = null;
                model.CREDrugRate = null;
                model.is_new = true;
                Area Province = areaService.GetArea(model.ProvinceId).FirstOrDefault();
                model.Province = Province.ShortName;
                Area City = areaService.GetArea(model.CityId).FirstOrDefault();
                if (City.Name == "上海市" || City.Name == "天津市" || City.Name == "重庆市" || City.Name == "北京市")
                {
                    Area Dist = areaService.GetArea(model.DistrictsId).FirstOrDefault();
                    model.City = Dist.Name;
                }
                else
                {
                    model.City = City.Name;
                }
                this.ProjectService.Insert(model);
                if (model.Id > 0)
                {
                    this.CRLogService.Insert(ActionType.Create, ActionSource.Web, model.Id, member.Id, member.Name, "添加CR复敏数据", "添加CR复敏数据");
                    return this.Content(JsonHelper.GetBaseMessage(true, "提交成功"));
                }
                //if (ProjectService.AddCRData(model) > 0)
                //{
                //    this.CRLogService.Insert(ActionType.Create, ActionSource.Web, model.Id, member.Id, member.Name, "添加CR复敏数据", "添加CR复敏数据");
                //    return this.Content(JsonHelper.GetBaseMessage(true, "提交成功"));
                //}

                return this.Content(JsonHelper.GetBaseMessage(false, "提交失败，请重试"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
            #endregion
        }

        #endregion

        #region 编辑

        [CheckRole(false)]
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                var member = base.LoginUserinfo;
                var entity = this.ProjectService.QueryEntityByMember(member.Id);
                if (entity == null || entity.Id <= 0) //用户还未提交过数据，跳转到新增的页面
                    return this.RedirectToAction("Add", "CRUpload");

                //查看权限，只有自己查看自己的数据
                if (entity.MemberId != member.Id)
                    throw new Exception("您无权查看该数据");

                // 判断是否已经上传过数据
                ViewBag.uploaded = CRItemService.Count(r => r.ProjectId == entity.Id && r.Mark > 0) > 0;

                return this.View(entity);

            }
            catch (Exception ex)
            {
                base.TransferError(ex.Message);
                return new EmptyResult();
            }
        }

        /// <summary>
        /// 编辑   提交数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Edit(CRData model)
        {
            var member = base.LoginUserinfo;
            string provinceName = areaService.QueryEntity(model.ProvinceId) != null ? areaService.QueryEntity(model.ProvinceId).Name : "";
            string cityName = areaService.QueryEntity(model.CityId) != null ? areaService.QueryEntity(model.CityId).Name : "";
            string districtsName = areaService.QueryEntity(model.DistrictsId) != null ? areaService.QueryEntity(model.DistrictsId).Name : "";

            #region 数据验证

            var entity = this.ProjectService.QueryEntityByMember(member.Id);
            if (entity == null || entity.Id <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "数据不存在，请重新打开"));
            if (string.IsNullOrWhiteSpace(model.HospitalName))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入医院名称"));
            if (string.IsNullOrWhiteSpace(model.HospitalGrade))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入医院等级"));
            if (model.DistrictsId <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "请选择省市区"));
            //if (string.IsNullOrWhiteSpace(model.CREDetectionRate))
            //    return this.Content(JsonHelper.GetBaseMessage(false, "请输入上年CRE平均检出率"));
            //if (string.IsNullOrWhiteSpace(model.CREDrugRate))
            //    return this.Content(JsonHelper.GetBaseMessage(false, "请输入上年CRAB平均检出率"));
            if (string.IsNullOrWhiteSpace(model.MHSource))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入MH平板来源厂家"));
            if (string.IsNullOrEmpty(model.Name))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入微生物联系人"));
            if (string.IsNullOrEmpty(model.Phone))
                return this.Content(JsonHelper.GetBaseMessage(false, "请输入联系电话"));
            if (model.Email.IndexOf("@") == -1)
                return this.Content(JsonHelper.GetBaseMessage(false, "邮箱格式不正确"));

            if (!member.SpecialManager.Contains("cr"))
                return this.Content(JsonHelper.GetBaseMessage(false, "只有替加环素用户才能使用此功能"));

            #endregion

            #region 业务处理

            try
            {
                entity.HospitalName = model.HospitalName;
                entity.Name = model.Name;
                entity.Phone = model.Phone;
                entity.Address = model.Address;
                entity.Area = model.Area;
                entity.ProvinceId = model.ProvinceId;
                entity.DistrictsId = model.DistrictsId;
                entity.CityId = model.CityId;
                entity.MHSource = model.MHSource;
                entity.Email = model.Email;
                entity.HospitalGrade = model.HospitalGrade;
                //entity.is_new = true;
                Area Province = areaService.GetArea(model.ProvinceId).FirstOrDefault();
                entity.Province = Province.ShortName;
                Area City = areaService.GetArea(model.CityId).FirstOrDefault();
                if (City.Name == "上海市" || City.Name == "天津市" || City.Name == "重庆市" || City.Name == "北京市")
                {
                    Area Dist = areaService.GetArea(model.DistrictsId).FirstOrDefault();
                    entity.City = Dist.Name;
                }
                else
                {
                    entity.City = City.Name;
                }
                string result = this.ProjectService.Update(entity, member);
                if (!string.IsNullOrWhiteSpace(result) && result.Equals("SUCCESS"))
                    return this.Content(JsonHelper.GetBaseMessage(true, "修改成功"));

                return this.Content(JsonHelper.GetBaseMessage(false, result));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }

            #endregion
        }

        #endregion

        #region 查看

        /// <summary>
        /// 查看详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            try
            {

                var user = base.LoginUserinfo;
                if (!user.SpecialManager.Contains("cr"))
                {
                    base.TransferError("只有替加环素用户才能使用此功能");
                    return new EmptyResult();
                }


                var entity = this.ProjectService.Query().Where(x => x.MemberId == user.Id).FirstOrDefault();
                if (entity == null)
                    throw new Exception("数据不存在");

                //查看权限，只有自己和对应的项目管理员才能看到
                if (entity.MemberId != user.Id)
                    throw new Exception("您无权查看该数据");

                var model = entity.ToModel();

                model.FileName = user.LoginId;
                return this.View(model);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);
                return this.RedirectToAction("Add", "ProjectList");
            }
        }

        /// <summary>
        /// 获取验证的分页数据
        /// </summary>
        /// <param name="command">分页请求数据</param>
        /// <param name="projectId">>CR项目Id</param>
        /// <returns></returns>
        public JsonResult ItemList(DataSourceRequest command)
        {
            var user = base.LoginUserinfo;
            var entity = this.ProjectService.Query().Where(x => x.MemberId == user.Id).FirstOrDefault();
            if (entity == null)
                throw new Exception("数据不存在");
            var list = this.CRItemService.QueryPage(entity.Id, command.Page - 1, command.PageSize);
            var data = list.Select(m => new
            {
                Number = m.Number,
                Name = m.Name,
                MIC_Imipenem = m.MIC_Imipenem,
                ImineNumber = m.ImineNumber,
                TegacyclineNumber = m.TegacyclineNumber,
                BacteriostasisNumber = m.BacteriostasisNumber,
                RecheckNumber = m.RecheckNumber
            });

            return Json(new { total = list.TotalPages, rows = data });
        }

        /// <summary>
        ///查询指定项目的日志记录
        /// </summary>
        /// <param name="projectId">CR项目Id</param>
        /// <returns></returns>
        public JsonResult LogList()
        {
            var user = base.LoginUserinfo;
            var entity = this.ProjectService.Query().Where(x => x.MemberId == user.Id).FirstOrDefault();
            if (entity == null)
                return Json("数据不存在");

            var list = this.CRLogService.QueryByProject(entity.Id);
            var data = list.Select(m => new
            {
                InsertTime = m.InsertTime.GetNormalString("L"),
                UserinfoName = m.UserinfoName,
                Content = m.Content
            });

            return Json(data);
        }

        #endregion

        #region 上传数据
        [CheckRole(false)]
        /// <summary>
        /// 上传数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upload()
        {
            //return View();
            try
            {
                var member = base.LoginUserinfo;
                var entity = this.ProjectService.QueryEntityByMember(member.Id);
                if (entity == null || entity.Id <= 0) //用户还未提交过数据，跳转到新增的页面
                    return this.RedirectToAction("Add", "CRUpload");

                return this.View(entity);
            }
            catch (Exception ex)
            {
                base.TransferError(ex.Message);
                return new EmptyResult();
            }

        }

        /// <summary>
        /// 上传Excel文件，上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UploadFile()
        {
            try
            {
                var file = this.HttpContext.Request.Files[0];
                if (file == null || file.ContentLength <= 0)
                    throw new Exception("请选择文件");

                UploadResult model = new UploadifyUpload().Upload(new UploadParameter()
                {
                    Extension = "xlsx,xls",
                    FilePath = $"/Content/Upload/CR/",
                    HttpFile = this.HttpContext.Request.Files,
                    MaxLength = 50 * 1024, // 50M
                    NewFileName = file.FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Type = UploadTypeEnum.Excel
                });

                if (model.IsSuccess)
                    return this.Content(JsonHelper.GetBaseMessage(true, model.FilePath + "|" + model.FileName));

                return this.Content(JsonHelper.GetBaseMessage(false, model.ErrorMessage));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        /// <summary>
        ///  上传数据 提交
        /// </summary>
        /// <param name="filePath">上传成功的文件</param>
        /// <param name="fileName">上传成功的文件名称</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Upload(string Year, string Quarter, string CREDetectionRate, string CREDrugRate, string filePath, string fileName)
        {
            try
            {
                var member = base.LoginUserinfo;
                var entity = this.ProjectService.QueryEntityByMember(member.Id);
                if (entity == null || entity.Id <= 0) //用户还未提交过数据，跳转到新增的页面
                    return this.Content(JsonHelper.GetBaseMessage(false, "请先添加基本数据"));
                if (string.IsNullOrWhiteSpace(Year))
                    return this.Content(JsonHelper.GetBaseMessage(false, "年份不能为空"));
                if (string.IsNullOrWhiteSpace(Quarter))
                    return this.Content(JsonHelper.GetBaseMessage(false, "季度不能为空"));
                if (string.IsNullOrWhiteSpace(CREDetectionRate))
                    return this.Content(JsonHelper.GetBaseMessage(false, "CRE平均检出率不能为空"));
                if (string.IsNullOrWhiteSpace(CREDrugRate))
                    return this.Content(JsonHelper.GetBaseMessage(false, "CRAB平均检出率不能为空"));
                if (string.IsNullOrWhiteSpace(filePath))
                    return this.Content(JsonHelper.GetBaseMessage(false, "上传的文件不能为空"));

                string result = this.ProjectService.Upload(member.Id,Year,Quarter,CREDetectionRate,CREDrugRate,filePath, fileName, base.LoginUserinfo);
                if (!string.IsNullOrWhiteSpace(result) && result.Equals("SUCCESS"))
                    return this.Content(JsonHelper.GetBaseMessage(true, "上传成功"));

                return this.Content(JsonHelper.GetBaseMessage(false, result));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        #endregion

        #region 确认邮件

        /// <summary>
        /// 确认邮件
        /// </summary>
        /// <param name="key">邮件</param>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Email(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new Exception("数据不能为空");

                var entity = this.ProjectService.Query().Where(x => x.EmailStr.Equals(key)).FirstOrDefault();
                if (entity == null || entity.Id <= 0)
                    throw new Exception("数据不能为空");

                if (entity.EmailStatus == 1)
                    throw new Exception("只有发送过邮件的才能确认");

                //更新主表邮件状态
                entity.HospitalTime = DateTime.Now;
                entity.EmailStatus = 3;
                ProjectService.Update(entity);
                string message = "邮件确认成功";

                //添加CR日志
                this.CRLogService.Insert(ActionType.Email, ActionSource.Web, entity.Id, entity.MemberId, entity.MemberName, "邮件确认成功", "邮件确认成功");

                //添加操作日志
                this.ActionLogService.Insert(ActionType.Edit, ActionSource.Web, entity.MemberId, entity.MemberName, "确认CR邮件", entity.SerializeObject());

                this.ViewBag.Message = "邮件确认成功";
                return this.View();
            }
            catch (Exception ex)
            {
                base.TransferError(ex.Message);
                return new EmptyResult();
            }
        }

        #endregion

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetDatalist(string year)
        {
            var member = base.LoginUserinfo;
            var entity = ProjectService.GetDatalist(member.Id, year);
            return JsonConvert.SerializeObject(entity);
        }
    }
}