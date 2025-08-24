using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models.Members;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Log;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class MembersController : AdminBaseController
    {
        private readonly IMemberService MemberService;
        private readonly IEncryptionService EncryptionService;
        private readonly IDoctorTitleService DoctorTitleService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly IHospitalService HospitalService;
        private readonly IAreaService AreaService;
        private readonly IMemberAddressService MemberAddressService;
        private readonly IMemberCartService MemberCartService;
        private readonly IMemberIntegralLogService MemberIntegralLogService;
        private readonly IMemberAttestationService MemberAttestationService;
        private readonly IFeedbackService FeedbackService;
        private readonly IMedicalDataService MedicalDataService;
        private readonly IMedicalDataProjectService medicalDataProjectService;
        private readonly IProjectHospitalService ProjectHospitalService;

        public MembersController(
            IActionLogService _actionLogService,
            IMemberService _memberService,
           IEncryptionService _encryptionService,
           IDoctorTitleService _doctorTitleService,
           IHospitalDepartmentService _hospitalDepartmentService,
           IHospitalService _hospitalService,
           IAreaService _areaService,
           IMemberAddressService _memberAddressService,
           IMemberCartService _memberCartService,
           IMemberIntegralLogService _memberIntegralLogService,
           IMemberAttestationService _memberAttestationService,
              IFeedbackService _feedbackService,
               IMedicalDataService _medicalDataService,
               IMedicalDataProjectService _medicalDataProjectService,
               IProjectHospitalService _projectHospitalService
        )
        {
            this.MemberService = _memberService;
            this.EncryptionService = _encryptionService;
            this.DoctorTitleService = _doctorTitleService;
            this.HospitalDepartmentService = _hospitalDepartmentService;
            this.HospitalService = _hospitalService;
            this.AreaService = _areaService;
            this.MemberAddressService = _memberAddressService;
            this.MemberCartService = _memberCartService;
            this.MemberIntegralLogService = _memberIntegralLogService;
            this.MemberAttestationService = _memberAttestationService;
            this.FeedbackService = _feedbackService;
            this.MedicalDataService = _medicalDataService;
            this.medicalDataProjectService = _medicalDataProjectService;
            this.ProjectHospitalService = _projectHospitalService;
        }

        #region 会员管理

        /// <summary>
        /// 会员管理 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberList()
        {
            MemberModel model = new MemberModel();
            model.StatusList = MemberStatus.Normal.ToSelectList().ToList();
            model.TypeList = MemberType.Director.ToSelectList().ToList();
            model.ProjectTypeList=MemberProjectType.XACDURO.ToSelectList().ToList();
            foreach (var item in model.TypeList)
            {
                item.Selected = false;
                if (item.Text.Equals("全部"))
                {
                    item.Value = "-1";
                    item.Selected = true;
                }
            }
            model.Type = -1;
            return View(model);
        }

        /// <summary>
        /// 会员管理 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <param name="hospital">医院关键字</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberList(DataSourceRequest command, MemberModel model, string hospital)
        {
            //获得数据
            //var list = this.MemberService.QueryPage(model.Name, model.NickName, model.LoginId, model.Phone, model.Status, model.Type, hospital, command.Page - 1, command.PageSize);
            var list = this.MemberService.QueryPage(model.ProjectType, model.Name, model.NickName, model.LoginId, model.Phone, model.Status, model.Type, hospital, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        LoginId = x.LoginId,
                        Name = x.Name,
                        Status = ((MemberStatus)x.Status).GetDescription(),
                        Type = ((MemberType)x.Type).GetDescription(),
                        NickName = x.NickName,
                        Phone = x.Phone,
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Sex = ((MemberSex)x.Sex).GetDescription(),
                        Hospital = this.HospitalService.QueryEntity(x.HospitalId)?.Name,
                        DoctorTitle = this.DoctorTitleService.QueryEntity(x.DoctorTitleId)?.Name,
                        ProjectItem = this.medicalDataProjectService.GetProjectItemName(x.ProjectUploadItem)
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 选择会员
        /// </summary>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ActionResult MemberSelect()
        {
            MemberModel model = new MemberModel();
            model.StatusList = MemberStatus.Normal.ToSelectList(false, true).ToList();

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Insert(0, new SelectListItem() { Text = "全部", Value = "-1", Selected = true });
            typeList.AddRange(MemberType.Director.ToSelectList(false, false).ToList());
            model.TypeList = typeList;

            return View(model);
        }


        /// <summary>
        /// 会员管理 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberModel SetMemberCreateData()
        {
            MemberModel model = new MemberModel();
            model.StatusList = MemberStatus.Normal.ToSelectList(true, false).ToList();
            model.TypeList = MemberType.Authentication.ToSelectList(true, false).ToList();
            model.SexList = MemberSex.Female.ToSelectList(true, false).ToList();
            model.ProjectTypeList=MemberProjectType.CHINET.ToSelectList(true,false).ToList();

            model.HospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.HospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            model.AreaList = this.AreaService.Query(m => m.Mark > 0 && m.ParentId == 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.DoctorTitleList = this.DoctorTitleService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.ProjectList = this.medicalDataProjectService.Query(m => m.Mark > 0).OrderBy(x => x.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = model.SelectProject.Contains(x.Id) }; });
            model.ProjectUploadList = this.medicalDataProjectService.Query(m => m.Mark > 0).OrderBy(x => x.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = model.SelectProjectUpload.Contains(x.Id) }; });
            model.HospitalDepartmentList = this.HospitalDepartmentService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();

            return model;
        }

        /// <summary>
        /// 会员管理 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCreate()
        {
            return View(this.SetMemberCreateData());

        }

        /// <summary>
        /// 会员管理 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberCreate(MemberModel model)
        {
            model.Birthday = DateHelper.DefaultValue();

            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符
            //  if (string.IsNullOrEmpty(model.LoginId) || !(new Regex(@"^[a-zA-Z][a-zA-Z0-9_]{3,15}$").IsMatch(model.LoginId)))
            if (string.IsNullOrEmpty(model.LoginId))
                this.ModelState.AddModelError("LoginId", "用户名格式不正确");

            //if ((this.MemberService.Query(m => m.LoginId.Equals(model.LoginId) && m.Mark > 0).Count()) > 0)
            //    this.ModelState.AddModelError("LoginId", "登录帐号已经存在，请检查更换");

            var tmpModel=this.MemberService.QueryModelByLoginId(model.LoginId);
            if(tmpModel!=null&& tmpModel.Id > 0 && tmpModel.Mark> 0)
            {
                this.ModelState.AddModelError("LoginId", "登录帐号已经存在，请检查更换");
            }

            //if ((this.MemberService.Query(m => m.Phone.Equals(model.Phone) && m.Mark > 0).Count()) > 0)
            //    this.ModelState.AddModelError("Phone", "手机号码已经存在，请检查更换");
            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Trim().Length < 5)
                this.ModelState.AddModelError("Password", "密码长度必须大于等于6位");

            //if (model.DistrictsId <= 0)
            //    this.ModelState.AddModelError("DistrictsId", "请选择省市区");

            model.Phone = (model.Phone ?? "").Trim();
            model.Email = (model.Email ?? "").Trim();
            model.MedicineEmail = (model.MedicineEmail ?? "").Trim();
            model.Describe = model.Describe ?? "";
            model.OpenId = model.LoginId;
            model.InviteCode = MemberExtensions.GetInviteCode();
            model.Address = model.Address ?? "";
            model.Area = model.Area ?? "";
            model.Phone = model.Phone ?? "";
            model.OnlineUpload = model.OnlineUpload;

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();

                //注册为0，默认为CHINET用户
                entity.ProjectType = model.ProjectType;

                entity.Password = EncryptionService.EncryptText(entity.Password);
                entity.ProjectItem = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectProject);
                entity.ProjectItem = !string.IsNullOrWhiteSpace(entity.ProjectItem) ? entity.ProjectItem : "[]";
                entity.DefaultProject = 0;
                entity.ProjectUploadItem = "[]";
                model.OnlineUpload = model._OnlineUpload ? 1 : 0;

                entity.OnlineUpload = model._OnlineUpload ? 1 : 0;
                if (model.SelectProjectUpload != null && model.SelectProjectUpload.Count() > 0)
                {
                    entity.ProjectUploadItem = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectProjectUpload);
                    entity.ProjectUploadItem = !string.IsNullOrWhiteSpace(entity.ProjectUploadItem) ? entity.ProjectUploadItem : "[]";
                    if (model.DefaultProject > 0 && model.SelectProjectUpload.Contains(model.DefaultProject))
                    {
                        entity.DefaultProject = model.DefaultProject;
                    }
                }
                //杜建中改动
                for (int i = 0; i < model.SelectSpecialManager.Count; i++)
                {
                    string manage = model.SelectSpecialManager[i];
                    if (manage == "cr")
                    {
                        List<string> Special = new List<string>();
                        Special.Add(manage);
                        entity.SpecialManager = Newtonsoft.Json.JsonConvert.SerializeObject(Special);
                        entity.SpecialManager = !string.IsNullOrWhiteSpace(entity.SpecialManager) ? entity.SpecialManager : "[]";
                    }
                    if (manage == "2")
                    {
                        entity.Crefillonline = manage.ToString();
                        entity.Crefillonline = !string.IsNullOrWhiteSpace(entity.Crefillonline) ? entity.Crefillonline : null;
                    }
                    if (manage == "3")
                    {
                        entity.Creformupload = manage.ToString();
                        entity.Creformupload = !string.IsNullOrWhiteSpace(entity.Creformupload) ? entity.Creformupload : null;
                    }

                }
                //entity.SpecialManager = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectSpecialManager);
                //entity.SpecialManager = !string.IsNullOrWhiteSpace(entity.SpecialManager) ? entity.SpecialManager : "[]";

                entity.Median = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectMedian);
                entity.Median = !string.IsNullOrWhiteSpace(entity.Median) ? entity.Median : "[]";
                this.MemberService.Insert(entity);

                string logContent = "添加会员，会员姓名：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, model.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MemberCreate");
            }

            return View(this.SetMemberCreateData());
        }

        /// <summary>
        /// 会员管理 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberModel SetMemberEditData(long id)
        {
            var entity = this.MemberService.QueryEntity(id);

            var model = entity.ToModel();
            model.StatusList = ((MemberStatus)model.Status).ToSelectList(true, false).ToList();
            model.TypeList = ((MemberType)model.Type).ToSelectList(true, false).ToList();
            model.SexList = ((MemberSex)model.Sex).ToSelectList(true, false).ToList();
            model.HospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name + "【" + x.Id + "】", Value = x.Id.ToString() }; }).ToList();
            model.HospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            model.AreaList = this.AreaService.Query(m => m.Mark > 0 && m.ParentId == 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.DoctorTitleList = this.DoctorTitleService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.HospitalDepartmentList = this.HospitalDepartmentService.Query(m => m.Mark > 0 && m.Status).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.ProjectList = this.medicalDataProjectService.Query(m => m.Mark > 0).OrderBy(x => x.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = model.SelectProject.Contains(x.Id) }; });
            model.ProjectUploadList = this.medicalDataProjectService.Query(m => m.Mark > 0).OrderBy(x => x.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = model.SelectProjectUpload.Contains(x.Id) }; });
            model._OnlineUpload = entity.OnlineUpload == 1 ? true : false;

            model.ProjectTypeList = ((MemberProjectType)model.ProjectType).ToSelectList(true, false).ToList();//项目类型

            try
            {
                model.SelectProject = string.IsNullOrWhiteSpace(entity.ProjectItem) ? new List<long>()
               : Newtonsoft.Json.JsonConvert.DeserializeObject<List<long>>(entity.ProjectItem);
            }
            catch (Exception)
            {
                model.SelectProject = new List<long>();
            }

            try
            {
                model.SelectProjectUpload = string.IsNullOrWhiteSpace(entity.ProjectUploadItem) ? new List<long>()
               : Newtonsoft.Json.JsonConvert.DeserializeObject<List<long>>(entity.ProjectUploadItem);
            }
            catch (Exception)
            {
                model.SelectProjectUpload = new List<long>();
            }

            try
            {
                //杜建中改动
                List<string> SpecialManager = new List<string>();
                SpecialManager = string.IsNullOrWhiteSpace(entity.SpecialManager) ? new List<string>() : Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(entity.SpecialManager);
                if (entity.Crefillonline != null)
                {
                    SpecialManager.Add(entity.Crefillonline);
                }
                if (entity.Creformupload != null)
                {
                    SpecialManager.Add(entity.Creformupload);
                }
                model.SelectSpecialManager = SpecialManager;
                //model.SelectSpecialManager = string.IsNullOrWhiteSpace(entity.SpecialManager) ? new List<int>() : Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(entity.SpecialManager);
            }
            catch (Exception)
            {
                model.SelectSpecialManager = new List<string>();
            }

            try
            {
                model.SelectMedian = string.IsNullOrWhiteSpace(entity.Median) ? new List<string>()
              : Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(entity.Median);
            }
            catch (Exception)
            {

                model.SelectMedian = new List<string>();
            }
            return model;
        }

        /// <summary>
        /// 会员管理 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberEdit(long id)
        {
            var model = this.SetMemberEditData(id);
            model.Password = "**********";

            return this.View(model);
        }

        /// <summary>
        /// 会员管理 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberEdit(MemberModel model)
        {
            //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
            //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
            //用户名长度为4～18个字符
            //  if (string.IsNullOrEmpty(model.LoginId) || !(new Regex(@"^[a-zA-Z][a-zA-Z0-9_]{3,15}$").IsMatch(model.LoginId)))
            if (string.IsNullOrEmpty(model.LoginId))
                this.ModelState.AddModelError("LoginId", "用户名格式不正确");

            //if ((this.MemberService.Query(m => m.LoginId.Equals(model.LoginId) && m.Mark > 0 && m.Id != model.Id).Count()) > 0)
            //    this.ModelState.AddModelError("LoginId", "登录帐号已经存在，请检查更换");

            var tmpModel = this.MemberService.QueryModelByLoginId(model.LoginId);
            if (tmpModel != null && tmpModel.Id>0 && tmpModel.Id!=model.Id)
            {
                this.ModelState.AddModelError("LoginId", "登录帐号已经存在，请检查更换");
            }

            //if ((this.MemberService.Query(m => m.Phone.Equals(model.Phone) && m.Mark > 0 && m.Id != model.Id).Count()) > 0)
            //    this.ModelState.AddModelError("Phone", "手机号码已经存在，请检查更换");

            model.Phone = (model.Phone ?? "").Trim();
            model.Email = (model.Email ?? "").Trim();
            model.MedicineEmail = (model.MedicineEmail ?? "").Trim();
            model.Describe = model.Describe ?? "";
            model.Address = model.Address ?? "";
            model.Area = model.Area ?? "";
            if (string.IsNullOrWhiteSpace(model.InviteCode))
                model.InviteCode = Guid.NewGuid().ToString("N").ToLower();

            // entity = model.ToEntity(entity);

            if (ModelState.IsValid)
            {
                var entity = this.MemberService.QueryEntity(model.Id);

                //2024-07-29 / 2025-02-14
                entity.ProjectType = model.ProjectType;//0数据云、1多研究中心、2X药

                entity.Phone = (model.Phone ?? "").Trim();
                entity.Email = (model.Email ?? "").Trim();
                entity.MedicineEmail = (model.MedicineEmail ?? "").Trim();
                entity.Describe = model.Describe ?? "";
                entity.Address = model.Address ?? "";
                entity.Area = model.Area ?? "";
                if (string.IsNullOrWhiteSpace(model.InviteCode))
                    entity.InviteCode = Guid.NewGuid().ToString("N").ToLower();

                base.SetDefaultValue(model, entity);

                if (!model.Password.Equals("**********"))
                {
                    model.Password = this.EncryptionService.EncryptText(model.Password);
                }
                else
                {
                    model.Password = entity.Password;
                }
                entity.Password = model.Password;
                model.Birthday = entity.Birthday;
                model.HeadImage = entity.HeadImage;
                model.OpenId = entity.OpenId;
                entity.ProjectItem = "[]";
                entity.OnlineUpload = model._OnlineUpload ? 1 : 0;
                model.OnlineUpload = model._OnlineUpload ? 1 : 0;
                if (model.SelectProject != null && model.SelectProject.Any())
                {
                    entity.ProjectItem = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectProject);
                    entity.ProjectItem = !string.IsNullOrWhiteSpace(entity.ProjectItem) ? entity.ProjectItem : "[]";
                }
                if (model.SelectProjectUpload != null && model.SelectProjectUpload.Any())
                {
                    entity.ProjectUploadItem = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectProjectUpload);
                    entity.ProjectUploadItem = !string.IsNullOrWhiteSpace(entity.ProjectUploadItem) ? entity.ProjectUploadItem : "[]";
                    if (model.DefaultProject > 0 && model.SelectProjectUpload.Contains(model.DefaultProject))
                    {
                        entity.DefaultProject = model.DefaultProject;
                    }
                }

                entity.SpecialManager = "[]";
                entity.Creformupload = null;
                entity.Crefillonline = null;
                if (model.SelectSpecialManager != null && model.SelectSpecialManager.Any())
                {
                    //杜建中改动
                    for (int i = 0; i < model.SelectSpecialManager.Count; i++)
                    {
                        string manage = model.SelectSpecialManager[i];
                        if (manage == "cr")
                        {
                            List<string> Special = new List<string>();
                            Special.Add(manage);
                            entity.SpecialManager = Newtonsoft.Json.JsonConvert.SerializeObject(Special);
                            entity.SpecialManager = !string.IsNullOrWhiteSpace(entity.SpecialManager) ? entity.SpecialManager : "[]";
                        }
                        if (manage == "2")
                        {
                            entity.Crefillonline = manage.ToString();
                            entity.Crefillonline = !string.IsNullOrWhiteSpace(entity.Crefillonline) ? entity.Crefillonline : null;

                        }
                        if (manage == "3")
                        {
                            entity.Creformupload = manage.ToString();
                            entity.Creformupload = !string.IsNullOrWhiteSpace(entity.Creformupload) ? entity.Creformupload : null;

                        }

                    }
                    //entity.SpecialManager = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectSpecialManager);
                    //entity.SpecialManager = !string.IsNullOrWhiteSpace(entity.SpecialManager) ? entity.SpecialManager : "[]";
                }
                entity.Median = "[]";
                if (model.SelectMedian != null && model.SelectMedian.Any())
                {
                    entity.Median = Newtonsoft.Json.JsonConvert.SerializeObject(model.SelectMedian);
                    entity.Median = !string.IsNullOrWhiteSpace(entity.Median) ? entity.Median : "[]";
                }

                entity.Type = model.Type;
                entity.Department = model.Department;

                this.MemberService.Update(entity);
                string logContent = "修改会员，会员姓名：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, model.SerializeObject());

                base.SuccessNotification(logContent);

                return this.RedirectToAction("MemberList");
            }
            return this.View(this.SetMemberEditData(model.Id));
        }

        /// <summary>
        /// 会员管理 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberView(long id)
        {
            var model = this.SetMemberEditData(id);
            model.Password = "**********";

            return this.View(model);
        }

        /// <summary>
        /// 会员管理 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MemberList");
            }

            this.MemberService.Delete(selectedIds);

            string logContent = "【手动】删除会员，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MemberList");
        }


        /// <summary>
        /// 导入会员
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberImport()
        {
            this.ViewBag.Result = "";
            return this.View();
        }

        /// <summary>
        /// 导入会员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberImport(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || string.IsNullOrWhiteSpace(excelFile.FileName))
            {
                base.ErrorNotification("请选择文件");
                return View();
            }

            string path = this.Server.MapPath("/Content/File/MemberImport/" + DateTime.Now.ToString("yyyyMMddHHmmsss") + Path.GetExtension(excelFile.FileName));

            excelFile.SaveAs(path);

            int successCount = 0;
            string result = this.MemberService.ImportExcel(path, ref successCount);

            result = "导入完成，本次共成功导入：" + successCount + "<br />" + result;

            this.ViewBag.Result = result;
            return View();
        }

        #endregion

        #region 会员设置

        /// <summary>
        /// 会员设置
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberSetting()
        {
            return View(this.SetMemberSettingData());
        }

        [NonAction]
        private MemberSettingModel SetMemberSettingData()
        {
            MemberSettingModel model = new MemberSettingModel();
            model.AuthenticationAmount = this.SettingService.QueryValue<decimal>("web.member.authenticationamount");  //会员认证金额
            model.MemberWeixinMessageAdmin = this.SettingService.QueryValue<string>("web.member.weixinmessage.admin");  //微信信息接收管理员微信会员Ids，多个使用逗号分割

            return model;

        }

        /// <summary>
        /// 保存网站配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberSetting(MemberSettingModel model)
        {
            //this.SettingService.UpdateValue("web.member.authenticationamount", model.AuthenticationAmount.ToString());//会员认证金额
            //this.SettingService.UpdateValue("web.member.weixinmessage.admin", model.MemberWeixinMessageAdmin);//微信信息接收管理员微信会员Ids，多个使用逗号分割

            string logContent = "修改会员设置完成";
            base.InsetActionLog(ActionType.Edit, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MemberSetting");
        }
        #endregion

        #region 收货地址

        /// <summary>
        /// 收货地址 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAddressList()
        {
            return View();
        }

        /// <summary>
        /// 收货地址 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberAddressList(DataSourceRequest command, MemberAddressModel model)
        {
            //获得数据
            var list = this.MemberAddressService.QueryPage(model.MemberLoginId, model.Name, model.Phone, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> memberIds = data.Select(m => m.MemberId).ToList();
            var memberList = this.MemberService.Query(m => memberIds.Contains(m.Id));

            if (memberList != null && memberList.Any())
            {
                foreach (var item in data)
                {
                    var meetingTemp = memberList.Where(m => m.Id == item.MemberId).FirstOrDefault();
                    if (meetingTemp != null && meetingTemp.Id > 0)
                        item.MemberLoginId = meetingTemp.LoginId;
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = data.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        MemberLoginId = x.MemberLoginId,
                        Name = x.Name,
                        Phone = x.Phone,
                        Tel = x.Tel,
                        Address = x.Area + " " + x.Address,
                        IsMain = x.IsMain ? "默认" : "",
                        Email = x.Email,
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 收货地址 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberAddressModel SetMemberAddressCreateData()
        {
            MemberAddressModel model = new MemberAddressModel();

            return model;
        }

        /// <summary>
        /// 收货地址 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAddressCreate()
        {
            return View(this.SetMemberAddressCreateData());

        }

        /// <summary>
        /// 收货地址 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberAddressCreate(MemberAddressModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var provinceEntity = this.AreaService.QueryEntity(model.ProvinceId);
                    if (provinceEntity == null || provinceEntity.Id <= 0)
                        throw new Exception("选择的省份不存在");

                    var cityEntity = this.AreaService.QueryEntity(model.CityId);
                    if (cityEntity == null || cityEntity.Id <= 0)
                        throw new Exception("选择的市不存在");

                    var districtsEntity = this.AreaService.QueryEntity(model.DistrictsId);
                    if (districtsEntity == null || districtsEntity.Id <= 0)
                        throw new Exception("选择的区不存在");

                    model.Email = string.IsNullOrWhiteSpace(model.Email) ? "" : model.Email;
                    model.Tel = string.IsNullOrWhiteSpace(model.Tel) ? "" : model.Tel;
                    model.ZipPostalCode = string.IsNullOrWhiteSpace(model.ZipPostalCode) ? "" : model.ZipPostalCode;
                    model.Remark = string.IsNullOrWhiteSpace(model.Remark) ? "" : model.Remark;

                    model.Area = provinceEntity.Name + " " + cityEntity.Name + " " + districtsEntity.Name;
                    model.Address = model.Address.Replace(" ", "");

                    var entity = model.ToEntity();
                    this.MemberAddressService.Insert(entity, base.LoginUserinfo, ActionSource.Admin);

                    base.SuccessNotification("添加会员收货地址成功");

                    return this.RedirectToAction("MemberAddressCreate");
                }
                catch (Exception ex)
                {
                    base.ErrorNotification(ex.Message);
                }
            }

            return View(this.SetMemberAddressCreateData());
        }

        /// <summary>
        /// 收货地址 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberAddressModel SetMemberAddressEditData(long id)
        {
            var entity = this.MemberAddressService.QueryEntity(id);

            var model = entity.ToModel();

            var member = this.MemberService.QueryEntity(m => m.Id == model.MemberId);
            model.MemberLoginId = member == null ? "" : member.LoginId;

            return model;
        }

        /// <summary>
        /// 收货地址 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAddressEdit(long id)
        {
            return this.View(this.SetMemberAddressEditData(id));
        }

        /// <summary>
        /// 收货地址 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberAddressEdit(MemberAddressModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var provinceEntity = this.AreaService.QueryEntity(model.ProvinceId);
                    if (provinceEntity == null || provinceEntity.Id <= 0)
                        throw new Exception("选择的省份不存在");

                    var cityEntity = this.AreaService.QueryEntity(model.CityId);
                    if (cityEntity == null || cityEntity.Id <= 0)
                        throw new Exception("选择的市不存在");

                    var districtsEntity = this.AreaService.QueryEntity(model.DistrictsId);
                    if (districtsEntity == null || districtsEntity.Id <= 0)
                        throw new Exception("选择的区不存在");

                    var entity = this.MemberAddressService.QueryEntity(model.Id);
                    model.Email = string.IsNullOrWhiteSpace(model.Email) ? "" : model.Email;
                    model.Tel = string.IsNullOrWhiteSpace(model.Tel) ? "" : model.Tel;
                    model.ZipPostalCode = string.IsNullOrWhiteSpace(model.ZipPostalCode) ? "" : model.ZipPostalCode;
                    model.Remark = string.IsNullOrWhiteSpace(model.Remark) ? "" : model.Remark;
                    model.Area = provinceEntity.Name + " " + cityEntity.Name + " " + districtsEntity.Name;
                    model.Address = model.Address.Replace(" ", "");

                    base.SetDefaultValue(model, entity);
                    entity = model.ToEntity(entity);

                    this.MemberAddressService.Update(entity, base.LoginUserinfo, ActionSource.Admin);

                    base.SuccessNotification("修改收货地址成功");

                    return this.RedirectToAction("MemberAddressList");
                }
                catch (Exception ex)
                {
                    base.ErrorNotification(ex.Message);
                }
            }
            return this.View(this.SetMemberAddressEditData(model.Id));
        }


        /// <summary>
        /// 收货地址 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAddressView(long id)
        {
            return this.View(this.SetMemberAddressEditData(id));
        }


        /// <summary>
        /// 收货地址 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberAddressDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MemberAddressList");
            }

            this.MemberAddressService.Delete(selectedIds);

            string logContent = "【手动】删除收货地址，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MemberAddressList");
        }


        #endregion

        #region 用户购物车

        /// <summary>
        /// 用户购物车 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCartList()
        {
            return View();
        }

        /// <summary>
        /// 用户购物车 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberCartList(DataSourceRequest command, MemberCartModel model)
        {

            //获得数据
            var list = this.MemberCartService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 用户购物车 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberCartModel SetMemberCartCreateData()
        {
            MemberCartModel model = new MemberCartModel();

            return model;
        }

        /// <summary>
        /// 用户购物车 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCartCreate()
        {
            return View(this.SetMemberCartCreateData());

        }

        /// <summary>
        /// 用户购物车 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberCartCreate(MemberCartModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MemberCartService.Insert(entity);

                string logContent = "添加用户购物车 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MemberCartCreate");
            }

            return View(this.SetMemberCartCreateData());
        }


        /// <summary>
        /// 用户购物车 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberCartModel SetMemberCartEditData(long id)
        {
            var entity = this.MemberCartService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 用户购物车 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCartEdit(long id)
        {
            return this.View(this.SetMemberCartEditData(id));
        }

        /// <summary>
        /// 用户购物车 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberCartEdit(MemberCartModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MemberCartService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.MemberCartService.Update(entity);

                string logContent = "修改用户购物车 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MemberCartEdit");
            }
            return this.View(this.SetMemberCartEditData(model.Id));
        }


        /// <summary>
        /// 用户购物车 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCartView(long id)
        {
            return this.View(this.SetMemberCartEditData(id));
        }


        /// <summary>
        /// 用户购物车 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberCartDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MemberCartList");
            }

            this.MemberCartService.Delete(selectedIds);

            string logContent = "【手动】删除用户购物车，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MemberCartList");
        }


        #endregion

        #region 用户积分使用记录

        /// <summary>
        /// 用户积分使用记录 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIntegralLogList()
        {
            return View();
        }

        /// <summary>
        /// 用户积分使用记录 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberIntegralLogList(DataSourceRequest command, MemberIntegralLogModel model)
        {
            //获得数据
            var list = this.MemberIntegralLogService.QueryPage(model.MemberLoginId, model.MemberName, model.Remark, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> memberIds = data.Select(m => m.MemberId).ToList();
            var memberList = this.MemberService.Query(m => memberIds.Contains(m.Id));

            if (memberList != null && memberList.Any())
            {
                foreach (var item in data)
                {
                    var meetingTemp = memberList.Where(m => m.Id == item.MemberId).FirstOrDefault();
                    if (meetingTemp != null && meetingTemp.Id > 0)
                    {
                        item.MemberLoginId = meetingTemp.LoginId;
                        item.MemberName = meetingTemp.Name;
                    }
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = data.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        MemberLoginId = x.MemberLoginId,
                        MemberName = x.MemberName,
                        Value = x.Value,
                        Source = x.Source,
                        Type = x.Type,
                        Remark = x.Remark,
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };

        }


        /// <summary>
        /// 用户积分使用记录 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberIntegralLogModel SetMemberIntegralLogCreateData()
        {
            MemberIntegralLogModel model = new MemberIntegralLogModel();

            model.TypeList = new List<SelectListItem>() {
                 new SelectListItem() { Value="赠送", Text="赠送"},
                 new SelectListItem() { Value="扣除",Text="扣除"}
            };
            return model;
        }

        /// <summary>
        /// 用户积分使用记录 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIntegralLogCreate()
        {
            return View(this.SetMemberIntegralLogCreateData());

        }

        /// <summary>
        /// 用户积分使用记录 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberIntegralLogCreate(MemberIntegralLogModel model)
        {
            if (model.Value == 0)
                ModelState.AddModelError("Value", "积分数量不能等于0");

            if (ModelState.IsValid)
            {
                var entity = new MemberIntegralLog();
                entity.Value = model.Value;
                entity.Remark = model.Remark;
                entity.Type = model.Type;
                entity.MemberId = model.MemberId;
                entity.Describe = string.IsNullOrWhiteSpace(model.Describe) ? "" : model.Describe;

                this.MemberIntegralLogService.Insert(entity, ActionSource.Admin, base.LoginUserinfo);

                base.SuccessNotification("添加用户积分记录成功");

                return this.RedirectToAction("MemberIntegralLogCreate");
            }

            return this.View(this.SetMemberIntegralLogCreateData());
        }


        /// <summary>
        /// 用户积分使用记录 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberIntegralLogModel SetMemberIntegralLogEditData(long id)
        {
            var entity = this.MemberIntegralLogService.QueryEntity(id);

            var model = entity.ToModel();

            var member = this.MemberService.QueryEntity(model.MemberId);
            if (member != null && member.Id > 0)
            {
                model.MemberLoginId = member.LoginId;
                model.MemberName = member.Name;
            }


            return model;
        }

        /// <summary>
        /// 用户积分使用记录 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIntegralLogEdit(long id)
        {
            return this.View(this.SetMemberIntegralLogEditData(id));
        }

        /// <summary>
        /// 用户积分使用记录 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberIntegralLogEdit(MemberIntegralLogModel model)
        {
            ModelState.Remove("MemberId");
            ModelState.Remove("Type");

            if (ModelState.IsValid)
            {
                var entity = new MemberIntegralLog();
                entity.Id = model.Id;
                entity.Value = model.Value;
                entity.Remark = model.Remark;
                entity.Describe = string.IsNullOrWhiteSpace(model.Describe) ? "" : model.Describe;

                this.MemberIntegralLogService.Update(entity, ActionSource.Admin, base.LoginUserinfo);

                base.SuccessNotification("修改用户积分明细成功");

                return this.RedirectToAction("MemberIntegralLogList");
            }
            return this.View(this.SetMemberIntegralLogEditData(model.Id));
        }


        /// <summary>
        /// 用户积分使用记录 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIntegralLogView(long id)
        {
            return this.View(this.SetMemberIntegralLogEditData(id));
        }


        /// <summary>
        /// 用户积分使用记录 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberIntegralLogDelete(string selectedIds)
        {
            //不开放删除功能
            return this.RedirectToAction("MemberIntegralLogList");

            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MemberIntegralLogList");
            }

            this.MemberIntegralLogService.Delete(selectedIds);

            string logContent = "删除用户积分使用记录，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MemberIntegralLogList");
        }


        #endregion

        #region 会认证申请信息

        private bool MemberAttestationEditRole = false; //编辑权限
        private bool MemberAttestationViewRole = false;//查看权限
        private bool MemberAttestationCheckRole = false;//审核权限

        /// <summary>
        /// 会认证申请信息 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAttestationList()
        {
            MemberAttestationModel model = new MemberAttestationModel();
            model.StatusList = MemberAttestationStatus.Finish.ToSelectList().ToList();

            return View(model);
        }

        /// <summary>
        /// 会认证申请信息 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MemberAttestationList(DataSourceRequest command, MemberAttestationModel model)
        {
            //检查权限
            this.MemberAttestationEditRole = UserinfoExtensions.CheckFunction("/Members/MemberAttestationEdit");
            this.MemberAttestationViewRole = UserinfoExtensions.CheckFunction("/Members/MemberAttestationView");
            this.MemberAttestationCheckRole = UserinfoExtensions.CheckFunction("/Members/MemberAttestationCheck");

            //获得数据
            var list = this.MemberAttestationService.QueryPage(model.MemberName, model.Status, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        MemberName = x.MemberName,
                        AreaName = x.AreaName,
                        Status = ((MemberAttestationStatus)x.Status).GetDescription(),
                        HospitalName = x.HospitalName,
                        HospitalDepartmentName = x.HospitalDepartmentName,
                        DoctorTitleName = x.DoctorTitleName,
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        ActionHtml = this.GetReferralActionHtml(x)
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 获取页面的权限按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetReferralActionHtml(MemberAttestation model)
        {
            StringBuilder sb = new StringBuilder();

            if (this.MemberAttestationViewRole)
                sb.Append("<a href=\"/Members/MemberAttestationView?id=" + model.Id + "\" >查看</a>&nbsp;&nbsp;");

            switch ((MemberAttestationStatus)model.Status)
            {
                case MemberAttestationStatus.WaitCheck:
                    if (this.MemberAttestationCheckRole)
                        sb.Append("<a href=\"javascript:MemberAttestationCheck('" + model.Id + "')\" >通过审核</a>&nbsp;&nbsp;");
                    break;
                case MemberAttestationStatus.Finish:
                    break;
                case MemberAttestationStatus.Cancel:
                    break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 会认证申请信息 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberAttestationModel SetMemberAttestationCreateData()
        {
            MemberAttestationModel model = new MemberAttestationModel();

            return model;
        }

        /// <summary>
        /// 会认证申请信息 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAttestationCreate()
        {
            return View(this.SetMemberAttestationCreateData());

        }

        /// <summary>
        /// 会认证申请信息 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberAttestationCreate(MemberAttestationModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MemberAttestationService.Insert(entity);

                string logContent = "添加会认证申请信息 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MemberAttestationCreate");
            }

            return View(this.SetMemberAttestationCreateData());
        }


        /// <summary>
        /// 会认证申请信息 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MemberAttestationModel SetMemberAttestationEditData(long id)
        {
            var entity = this.MemberAttestationService.QueryEntity(id);

            var model = entity.ToModel();
            model.StatusName = ((MemberAttestationStatus)model.Status).GetDescription();

            return model;
        }

        /// <summary>
        /// 会认证申请信息 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAttestationEdit(long id)
        {
            return this.View(this.SetMemberAttestationEditData(id));
        }

        /// <summary>
        /// 会认证申请信息 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MemberAttestationEdit(MemberAttestationModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MemberAttestationService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.MemberAttestationService.Update(entity);

                string logContent = "修改会认证申请信息 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MemberAttestationList");
            }
            return this.View(this.SetMemberAttestationEditData(model.Id));
        }


        /// <summary>
        /// 会认证申请信息 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberAttestationView(long id)
        {
            return this.View(this.SetMemberAttestationEditData(id));
        }


        /// <summary>
        /// 会认证申请信息 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberAttestationDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MemberAttestationList");
            }

            this.MemberAttestationService.Delete(selectedIds);

            string logContent = "【手动】删除会认证申请信息，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MemberAttestationList");
        }

        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult MemberAttestationCheck(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("数据错误");

                if (this.MemberAttestationService.FinishCheck(id, base.LoginUserinfo))
                    return this.Content(JsonHelper.GetBaseMessage(true, ""));

                return this.Content(JsonHelper.GetBaseMessage(false, "审核失败，请重试"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }

        }

        #endregion

        #region 建议反馈

        /// <summary>
        /// 建议反馈 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FeedbackList()
        {
            FeedbackModel model = new FeedbackModel();

            model.StatusList = new List<SelectListItem>() {
                 new SelectListItem() { Text="全部",Value="0" },
                 new SelectListItem() { Text="已查看",Value="1" },
                 new SelectListItem() { Text="未查看",Value="2" }
            };

            return View(model);
        }

        /// <summary>
        /// 建议反馈 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FeedbackList(DataSourceRequest command, FeedbackModel model)
        {
            try
            {
                //获得数据
                var list = this.FeedbackService.QueryPage(model.Name, model.StatusValue, command.Page - 1, command.PageSize);

                var gridModel = new DataSourceResult
                {
                    Data = list.Select(x =>
                    {
                        string mobile = "-", email = "-";
                        try
                        {
                            dynamic _desc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(x.Describe);
                            mobile = _desc.Mobile;
                            email = _desc.Email;
                        }
                        catch (Exception)
                        {

                        }

                        return new
                        {
                            Id = x.Id.ToString(),
                            MemberId = x.MemberId,
                            Name = x.Name,
                            MemberName = x.MemberName,
                            Mobile = mobile,
                            Email = email,
                            Status = x.Status ? "已查看" : "未查看",
                            Content = x.Content.CutString(100),
                            InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        };
                    }),
                    Total = list.TotalCount
                };


                return new JsonResult { Data = gridModel };
            }
            catch (Exception EX)
            {

                throw;
            }

        }


        /// <summary>
        /// 建议反馈 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public FeedbackModel SetFeedbackEditData(long id)
        {
            var entity = this.FeedbackService.QueryEntity(id);

            if (!entity.Status)
            {
                entity.Status = true;
                entity.UpdateTime = DateTime.Now;
                this.FeedbackService.Update(entity);

                base.InsetActionLog(ActionType.Edit, "查看建议反馈，反馈名称：" + entity.Name, entity.SerializeObject());
            }

            var model = entity.ToModel();

            return model;
        }


        /// <summary>
        /// 建议反馈 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult FeedbackView(long id)
        {
            return this.View(this.SetFeedbackEditData(id));
        }


        /// <summary>
        /// 建议反馈 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeedbackDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("FeedbackList");
            }

            this.FeedbackService.Delete(selectedIds);

            string logContent = "【手动】删除建议反馈，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("FeedbackList");
        }


        #endregion

        #region //多中心研究

        [CheckRole(false,false)]
        public ActionResult UploadHospital()
        {
            var fileName = $"/Content/Upload/testUpload.xlsx";
            if (!System.IO.File.Exists(Server.MapPath(fileName)))
            {
                Response.Write("文件不存在");
                Response.End();
            }

            //项目名称
            var projectType = Utility.ToInt(QueryString.Q("type"));
            if (projectType <= 0)
            {
                Response.Write("项目类型参数为空<br/>");
                Response.End();
            }

            var dataTable = ExcelHelper.Read(Server.MapPath(fileName)).Tables[0];
            if(dataTable!=null&& dataTable.Rows.Count > 0)
            {
                //所有省份
                var provinceList = this.AreaService.Query(t => t.ParentId == 0);
                var doctorTitleModel = DoctorTitleService.Query(t => t.Name == "主任医师").FirstOrDefault();
                var departmentModel = this.HospitalDepartmentService.Query(t => t.Name == "外科").FirstOrDefault();

                var rowIndex = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    rowIndex++;

                    #region //设置医院

                    var hospital = row["医院"].ToString().Trim();
                    var province = row["省份"].ToString().Trim();
                    var hospitalModel = this.HospitalService.QueryEntity(t => t.Name == hospital);
                    if(hospitalModel==null|| hospitalModel.Id <= 0)
                    {
                        var provinceModel = provinceList.Where(t => t.Name == province).FirstOrDefault();
                        if(provinceModel==null|| provinceModel.Id <= 0)
                        {
                            Response.Write($"省份《{province} {hospital}》不存在<br/>");
                            continue;//省份不存在
                        }

                        hospitalModel = new Core.Domain.Medicine.Hospital()
                        {
                            Id= CommonHelper.GuidToLongID,
                            Name= hospital,
                            Address= provinceModel.Name,
                            ProvinceId= provinceModel.Id,
                            ProvinceName= provinceModel.Name,
                            InsertTime= DateTime.Now,
                            UpdateTime= DateTime.Now,
                        };
                        this.HospitalService.Insert(hospitalModel);
                    }

                    var ProjectHospitalList = this.ProjectHospitalService.Query(t => t.HospitalId == hospitalModel.Id && t.ProjectType == projectType);
                    if (ProjectHospitalList != null && ProjectHospitalList.ToList().Count> 0)
                    {
                        continue;//存在则不创建帐号
                    }

                    #endregion

                    #region //创建帐号

                    //创建帐号
                    var tmpSecond = Convert.ToInt32(DateTime.Now.ToString("yy")) + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                    var LoginId = string.Format("{0}{1}{2}", "CRE", tmpSecond.ToString("000"), rowIndex.ToString("000"));
                    var originalPassword = Utility.GetCheckCode(6);//原始密码
                    var Password = EncryptionService.EncryptText(originalPassword);//加密后
                    Member entity = this.MemberService.Regist(loginId: LoginId, name: LoginId, phone: string.Empty, password: Password, password2: Password, source: 1);

                    //用户配置
                    entity.ProjectType = projectType;
                    entity.DoctorTitleId = doctorTitleModel.Id;
                    entity.HospitalId = hospitalModel.Id;
                    entity.HospitalDepartmentId = departmentModel.Id;
                    entity.AreaId = hospitalModel.ProvinceId;
                    entity.ProvinceId= hospitalModel.ProvinceId;
                    entity.Type = (Int32)MemberType.Doctor;
                    entity.Status = (Int32)MemberStatus.Normal;
                    entity.Sex = (Int32)MemberSex.Male;

                    //更新用户
                    this.MemberService.Update(entity);

                    //记录项目医院及用户
                    this.ProjectHospitalService.Insert(new ManageSystem.Core.Domain.Medicine.ProjectHospital()
                    {
                        Id = CommonHelper.GuidToLongID,
                        ProjectType = entity.ProjectType,
                        HospitalId = hospitalModel.Id,
                        HospitalTitle = hospitalModel.Name,
                        MemberId = entity.Id,
                        MemberLoginId = entity.LoginId,
                        MemberPassword = originalPassword,
                        InsertTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                    });

                    #endregion
                }
                Response.Write("导入成功");
                Response.End();
            }

            return RedirectToAction("MemberList", "Members");
        }

        #endregion

        #region 批量刷用户所属医院的数据和上传数据是否显示的数据，请勿随意删除和调用

        /// <summary>
        /// 批量刷用户所属医院的数据
        /// </summary>
        /// <returns></returns>
        [CheckRole(false, false)]
        public ActionResult UpdateMe()
        {
            //  this.MemberService.UpdateMe();
            return this.View();
        }

        /// <summary>
        /// 批量刷上传数据是否显示的数据
        /// </summary>
        /// <returns></returns>
        [CheckRole(false, false)]
        public ActionResult UpdateMe2()
        {
            //  this.MemberService.UpdateMe2();
            return this.View();
        }

        #endregion

    }
}
