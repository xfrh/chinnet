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
using ManageSystem.Core.DynamicLinq;
using ManageSystem.Admin.Validators.Medicine;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Admin.Controllers
{

    public class MedicineController : AdminBaseController
    {
        private readonly IHospitalService HospitalService;
        private readonly IBacteriaTypeService BacteriaTypeService;
        private readonly ISpecimenService SpecimenService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly IMedicalDataService MedicalDataService;
        private readonly IMedicalDataProjectService medicalDataProjectService;
        private readonly IDoctorTitleService DoctorTitleService;
        private readonly IAreaService AreaService;
        private readonly IMedicalDataItemService MedicalDataItemService;
        private readonly IMemberService MemberService;
        private readonly IMedicalDataItemValidateService MedicalDataItemValidateService;
        private readonly IMedicalAntibioticService MedicalAntibioticService;
        private readonly IMedicalAntibioticRuleService MedicalAntibioticRuleService;
        private readonly IMedicalOrganismService MedicalOrganismService;
        private readonly IHospitalWardLocationService HospitalWardLocationService;
        private readonly IUserRoleService UserRoleService;
        private readonly IRoleService RoleService;


        public MedicineController(
            IHospitalService _hospitalService,
            IBacteriaTypeService _bacteriaTypeService,
             ISpecimenService _specimenService,
              IHospitalDepartmentService _hospitalDepartmentService,
            IMedicalDataService _medicalDataService,
             IDoctorTitleService _doctorTitleService,
           IAreaService _areaService,
           IMedicalDataItemService _medicalDataItemService,
           IMemberService _memberService,
           IMedicalDataItemValidateService _medicalDataItemValidateService,
             IMedicalAntibioticService _medicalAntibioticService,
             IMedicalAntibioticRuleService _medicalAntibioticRuleService,
             IHospitalWardLocationService _hospitalWardLocationService,
               IMedicalOrganismService _medicalOrganismService,
             IMedicalDataProjectService _medicalDataProjectService,
             IUserRoleService _userRoleService, IRoleService _RoleService
        )
        {
            this.HospitalService = _hospitalService;
            this.BacteriaTypeService = _bacteriaTypeService;
            this.SpecimenService = _specimenService;
            this.HospitalDepartmentService = _hospitalDepartmentService;
            this.MedicalDataService = _medicalDataService;
            this.DoctorTitleService = _doctorTitleService;
            this.AreaService = _areaService;
            this.MedicalDataItemService = _medicalDataItemService;
            this.MemberService = _memberService;
            this.MedicalDataItemValidateService = _medicalDataItemValidateService;
            this.MedicalAntibioticService = _medicalAntibioticService;
            this.MedicalAntibioticRuleService = _medicalAntibioticRuleService;
            this.MedicalOrganismService = _medicalOrganismService;
            this.HospitalWardLocationService = _hospitalWardLocationService;
            this.medicalDataProjectService = _medicalDataProjectService;
            UserRoleService = _userRoleService;
            RoleService = _RoleService;
        }

        #region 医院

        private bool HospitalEditRole = false; //编辑权限
        private bool HospitalViewRole = false;//查看权限
        private bool HospitalSetAdminRole = false;//设置管理员

        /// <summary>
        /// 医院 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalList()
        {
            HospitalModel model = new HospitalModel();
            model.StateList = base.GetEnabledSelectList();

            return View(model);
        }

        /// <summary>
        /// 医院 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HospitalList(DataSourceRequest command, HospitalModel model)
        {
            //检查权限
            this.HospitalEditRole = UserinfoExtensions.CheckFunction("/Medicine/HospitalEdit");
            this.HospitalViewRole = UserinfoExtensions.CheckFunction("/Medicine/HospitalView");
            this.HospitalSetAdminRole = UserinfoExtensions.CheckFunction("/Medicine/HospitalSetAdmin");

            if (model.Name == null) { model.Name = string.Empty; }

            //获得数据
            var list = this.HospitalService.QueryPage(model.Name, model.StateValue, command.Page - 1, command.PageSize);
            var data = list.Select(x =>
            {
                return new HospitalModel()
                {
                    IdString = x.Id.ToString(),
                    Name = x.Name,
                    Sort = x.Sort,
                    Address = x.Address,
                    ContactsTel = x.ContactsTel,
                    ContactsUser = x.ContactsUser,
                    StateValue = x.State ? "启用" : "禁用",
                    ActionHtml = this.GetHospitalActionHtml(x),
                    MemberId = x.MemberId,
                    ProvinceName = x.ProvinceName,
                    InsertTimestring = x.InsertTime.ToString(),
                    Code = x.Code
                };
            }).ToList();

            //存在医院未设置联系人，原代码处理未处理该情况，故修改
            var tmpMemberIds = list.Select(m => m.MemberId);
            if (tmpMemberIds != null)
            {
                var memberIds = tmpMemberIds.ToList();
                var memberList = this.MemberService.Query(m => memberIds.Contains(m.Id) && m.Mark > 0);
                if (memberList != null)
                {
                    foreach (var item in data)
                    {
                        var memberTemp = memberList.Where(m => m.Id == item.MemberId);
                        if (memberTemp != null && memberTemp.FirstOrDefault() != null && memberTemp.FirstOrDefault().Id > 0)
                            item.MemberName = memberTemp.FirstOrDefault().Name;
                    }
                }
            }


            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 获取页面的权限按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetHospitalActionHtml(Hospital model)
        {
            StringBuilder sb = new StringBuilder();

            if (this.HospitalViewRole)
                sb.Append("<a href=\"/Medicine/HospitalView?id=" + model.Id + "\" >查看</a>&nbsp;&nbsp;");

            if (this.HospitalEditRole)
                sb.Append("<a href=\"/Medicine/HospitalEdit?id=" + model.Id + "\" >编辑</a>&nbsp;&nbsp;");

            if (this.HospitalSetAdminRole)
                sb.Append("<a href=\"javascript:SetAdmin('" + model.Id + "')\" >设置管理员</a>&nbsp;&nbsp;");

            return sb.ToString();
        }

        /// <summary>
        /// 医院 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public HospitalModel SetHospitalCreateData()
        {
            HospitalModel model = new HospitalModel();
            model.State = true;
            model.ProvinceList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();    //区域列表（顶级，省份）

            return model;
        }

        /// <summary>
        /// 医院 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalCreate()
        {
            return View(this.SetHospitalCreateData());

        }

        /// <summary>
        /// 医院 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalCreate(HospitalModel model)
        {
            model.Content = "";

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.ContactsUser))
                    model.ContactsUser = "";
                if (string.IsNullOrWhiteSpace(model.ContactsTel))
                    model.ContactsTel = "";

                var entity = model.ToEntity();

                entity.ProvinceName = "";
                if (model.ProvinceId > 0)
                {
                    var area = this.AreaService.QueryEntity(model.ProvinceId);
                    if (area != null && area.Id > 0)
                        entity.ProvinceName = area.Name;
                }

                this.HospitalService.Insert(entity);

                string logContent = "添加医院，医院名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("HospitalList");
            }

            // return View(this.SetHospitalCreateData());

            return this.RedirectToAction("HospitalCreate");

        }


        /// <summary>
        /// 医院 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public HospitalModel SetHospitalEditData(long id)
        {
            var entity = this.HospitalService.QueryEntity(id);

            var model = entity.ToModel();
            model.ProvinceList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();    //区域列表（顶级，省份）

            return model;
        }

        /// <summary>
        /// 医院 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalEdit(long id)
        {
            return this.View(this.SetHospitalEditData(id));
        }

        /// <summary>
        /// 医院 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult HospitalEdit(HospitalModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.HospitalService.QueryEntity(model.Id);

                if (string.IsNullOrWhiteSpace(model.ContactsUser))
                    model.ContactsUser = "";
                if (string.IsNullOrWhiteSpace(model.ContactsTel))
                    model.ContactsTel = "";

                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                entity.ProvinceName = "";
                if (model.ProvinceId > 0)
                {
                    var area = this.AreaService.QueryEntity(model.ProvinceId);
                    if (area != null && area.Id > 0)
                        entity.ProvinceName = area.Name;
                }

                this.HospitalService.Update(entity);

                string logContent = "修改医院，医院名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("HospitalList");
            }

            return this.View(this.SetHospitalEditData(model.Id));
        }


        /// <summary>
        /// 医院 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalView(long id)
        {
            return this.View(this.SetHospitalEditData(id));
        }


        /// <summary>
        /// 医院 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("HospitalList");
            }

            this.HospitalService.Delete(selectedIds);

            string logContent = "【手动】删除医院，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("HospitalList");
        }

        /// <summary>
        /// 设置医院管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HospitalSetAdmin(long id)
        {
            HostpitalSetAdminModel model = new HostpitalSetAdminModel();

            var hostital = this.HospitalService.QueryEntity(id);
            if (hostital != null && hostital.Id > 0)
            {
                model.MmeberList = this.MemberService.Query(m => m.HospitalId == hostital.Id && m.Mark > 0).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
                model.MmeberList.Insert(0, new SelectListItem() { Value = "0", Text = "-- 请选择 --", Selected = true });

                model.HostpitalName = hostital.Name;
                model.Id = hostital.Id;
            }

            if (hostital.MemberId > 0)
            {
                var member = this.MemberService.QueryEntity(hostital.MemberId);
                if (member != null && member.Id > 0)
                {
                    model.MemberId = member.Id;
                    model.MemberName = member.Name;
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// 设置医院管理员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult HospitalSetAdmin(HostpitalSetAdminModel model)
        {
            if (model.Id <= 0 || model.MemberId <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));

            try
            {
                var hostital = this.HospitalService.QueryEntity(model.Id);
                hostital.MemberId = model.MemberId;
                this.HospitalService.Update(hostital);

                base.InsetActionLog(ActionType.Export, "设置医院管理员，医院名称：" + hostital.Name, hostital.SerializeObject());

                return this.Content(JsonHelper.GetBaseMessage(true, "设置管理员成功"));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "设置管理员成功失败，失败原因：" + ex.Message));
            }
        }

        #endregion

        #region 细菌类型

        /// <summary>
        /// 细菌类型 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BacteriaTypeList()
        {
            BacteriaTypeModel model = new BacteriaTypeModel();
            model.StateList = base.GetEnabledSelectList();

            return View(model);
        }

        /// <summary>
        /// 细菌类型 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BacteriaTypeList(DataSourceRequest command, BacteriaTypeModel model)
        {
            //获得数据
            var list = this.BacteriaTypeService.QueryPage(model.Name, model.StateValue, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.Name,
                        Sort = x.Sort,
                        Status = x.Status ? "启用" : "禁用",
                        Describe = x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 细菌类型 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public BacteriaTypeModel SetBacteriaTypeCreateData()
        {
            BacteriaTypeModel model = new BacteriaTypeModel();
            model.Status = true;
            return model;
        }

        /// <summary>
        /// 细菌类型 创建
        /// </summary>
        /// <returns></returns>

        public ActionResult BacteriaTypeCreate()
        {
            return View(this.SetBacteriaTypeCreateData());

        }

        /// <summary>
        /// 细菌类型 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BacteriaTypeCreate(BacteriaTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.BacteriaTypeService.Insert(entity);

                string logContent = "添加细菌类型，类型名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("BacteriaTypeCreate");
            }

            return View(this.SetBacteriaTypeCreateData());
        }


        /// <summary>
        /// 细菌类型 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public BacteriaTypeModel SetBacteriaTypeEditData(long id)
        {
            var entity = this.BacteriaTypeService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 细菌类型 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult BacteriaTypeEdit(long id)
        {
            return this.View(this.SetBacteriaTypeEditData(id));
        }

        /// <summary>
        /// 细菌类型 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BacteriaTypeEdit(BacteriaTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.BacteriaTypeService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.BacteriaTypeService.Update(entity);

                string logContent = "修改细菌类型，类型名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("BacteriaTypeList");
            }
            return this.View(this.SetBacteriaTypeEditData(model.Id));
        }


        /// <summary>
        /// 细菌类型 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult BacteriaTypeView(long id)
        {
            return this.View(this.SetBacteriaTypeEditData(id));
        }


        /// <summary>
        /// 细菌类型 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BacteriaTypeDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("BacteriaTypeList");
            }

            this.BacteriaTypeService.Delete(selectedIds);

            string logContent = "【手动】删除细菌类型，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("BacteriaTypeList");
        }


        #endregion

        #region 标本

        /// <summary>
        /// 标本 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecimenList()
        {
            SpecimenModel model = new SpecimenModel();
            model.StateList = base.GetEnabledSelectList();

            return View(model);
        }

        /// <summary>
        /// 标本 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SpecimenList(DataSourceRequest command, SpecimenModel model)
        {

            //获得数据
            var list = this.SpecimenService.QueryPage(model.Name, model.StateValue, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.Name,
                        Sort = x.Sort,
                        Status = x.Status ? "启用" : "禁用",
                        Describe = x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 标本 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public SpecimenModel SetSpecimenCreateData()
        {
            SpecimenModel model = new SpecimenModel();
            model.Status = true;
            return model;
        }

        /// <summary>
        /// 标本 创建
        /// </summary>
        /// <returns></returns>

        public ActionResult SpecimenCreate()
        {
            return View(this.SetSpecimenCreateData());
        }

        /// <summary>
        /// 标本 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SpecimenCreate(SpecimenModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.SpecimenService.Insert(entity);

                string logContent = "添加标本，标本名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("SpecimenCreate");
            }

            return View(this.SetSpecimenCreateData());
        }


        /// <summary>
        /// 标本 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public SpecimenModel SetSpecimenEditData(long id)
        {
            var entity = this.SpecimenService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 标本 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecimenEdit(long id)
        {
            return this.View(this.SetSpecimenEditData(id));
        }

        /// <summary>
        /// 标本 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SpecimenEdit(SpecimenModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.SpecimenService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.SpecimenService.Update(entity);

                string logContent = "修改标本，标本名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("SpecimenList");
            }
            return this.View(this.SetSpecimenEditData(model.Id));
        }


        /// <summary>
        /// 标本 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecimenView(long id)
        {
            return this.View(this.SetSpecimenEditData(id));
        }


        /// <summary>
        /// 标本 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SpecimenDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("SpecimenList");
            }

            this.SpecimenService.Delete(selectedIds);

            string logContent = "【手动】删除标本，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SpecimenList");
        }


        #endregion

        #region 医院科室

        /// <summary>
        /// 医院科室 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalDepartmentList()
        {
            HospitalDepartmentModel model = new HospitalDepartmentModel();
            model.StateList = base.GetEnabledSelectList();

            return View(model);

        }

        /// <summary>
        /// 医院科室 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HospitalDepartmentList(DataSourceRequest command, HospitalDepartmentModel model)
        {
            //获得数据
            var list = this.HospitalDepartmentService.QueryPage(model.Name, model.StateValue, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.Name,
                        Sort = x.Sort,
                        Status = x.Status ? "启用" : "禁用",
                        Describe = x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 医院科室 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public HospitalDepartmentModel SetHospitalDepartmentCreateData()
        {
            HospitalDepartmentModel model = new HospitalDepartmentModel();
            model.Status = true;
            return model;
        }

        /// <summary>
        /// 医院科室 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalDepartmentCreate()
        {
            return View(this.SetHospitalDepartmentCreateData());

        }

        /// <summary>
        /// 医院科室 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult HospitalDepartmentCreate(HospitalDepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.HospitalDepartmentService.Insert(entity);

                string logContent = "添加医院科室，科室名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("HospitalDepartmentCreate");
            }

            return View(this.SetHospitalDepartmentCreateData());
        }


        /// <summary>
        /// 医院科室 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public HospitalDepartmentModel SetHospitalDepartmentEditData(long id)
        {
            var entity = this.HospitalDepartmentService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 医院科室 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalDepartmentEdit(long id)
        {
            return this.View(this.SetHospitalDepartmentEditData(id));
        }

        /// <summary>
        /// 医院科室 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult HospitalDepartmentEdit(HospitalDepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.HospitalDepartmentService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.HospitalDepartmentService.Update(entity);

                string logContent = "修改医院科室，科室名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("HospitalDepartmentList");
            }
            return this.View(this.SetHospitalDepartmentEditData(model.Id));
        }


        /// <summary>
        /// 医院科室 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalDepartmentView(long id)
        {
            return this.View(this.SetHospitalDepartmentEditData(id));
        }


        /// <summary>
        /// 医院科室 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalDepartmentDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("HospitalDepartmentList");
            }

            this.HospitalDepartmentService.Delete(selectedIds);

            string logContent = "【手动】删除医院科室，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("HospitalDepartmentList");
        }


        #endregion

        #region 医学数据
        /// <summary>
        /// 医学数据 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalDataList()
        {
            MedicalDataModel model = new MedicalDataModel();
            model.HospitalList = this.HospitalService.Query(m => m.State && m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.AreaList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();    //区域列表（顶级，省份）

            model.HospitalList.Insert(0, new SelectListItem() { Text = "全部医院", Value = "" });
            model.AreaList.Insert(0, new SelectListItem() { Text = "全部地区", Value = "" });

            model.YearList = new List<SelectListItem>();
            int yearTemp = DateTime.Now.Year;
            for (int i = yearTemp; i >= 2010; i--)
            {
                model.YearList.Add(new SelectListItem() { Text = "" + i, Value = i.ToString() });
            }
            model.YearList.Insert(0, new SelectListItem() { Text = "全部年份", Value = "" });
            model.QuarterList = new List<SelectListItem>() {
                    new SelectListItem() {  Text="请选择",Value="0"},
                 //new SelectListItem() {  Text="第一季度（1月 - 3月）",Value="1"},
                 //new SelectListItem() {  Text="第二季度（4月 - 6月）",Value="2"},
                 //new SelectListItem() {  Text="第三季度（7月 - 9月）",Value="3"},
                 //new SelectListItem() {  Text="第四季度（10月 - 12月）",Value="4"},
                 new SelectListItem() {  Text="上半年（1月 - 6月）",Value="6"},
                 new SelectListItem() {  Text="下半年（7月 - 12月）",Value="7"},
                 new SelectListItem() {  Text="全年",Value="5"}
            };

            model.ProjectTypeItems = this.medicalDataProjectService.Query(m => m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.ProjectTypeItems.Insert(0, new SelectListItem() { Text = "全部", Value = "0" });
            return View(model);
        }

        /// <summary>
        /// 医学数据 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MedicalDataList(DataSourceRequest command, MedicalDataModel model)
        {
            Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(base.LoginUserinfo.Id.ToString())));

            //获得数据
            var list = this.MedicalDataService.QueryPage(model.HospitalName, model.AreaId, model.HospitalId, model.ProjectType, model.Year, model.Quarter, command.Page - 1, command.PageSize, string.IsNullOrEmpty(role.SatelliteId) ? "" : base.LoginUserinfo.Describe);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        SN = x.SN,
                        FileName = x.FileName,
                        Year = x.Year.ToString(),
                        Size = this.MedicalDataListSize(x.UploadMessage),
                        MemberName = x.MemberName,
                        Quarter = this.MedicalDataService.GetDataQuarter(x.Year, x.Quarter),
                        HospitalName = x.HospitalName,
                        BacteriaTypeName = x.BacteriaTypeName,
                        SpecimenName = x.SpecimenName,
                        HospitalDepartmentName = x.HospitalDepartmentName,
                        AreName = x.AreName,
                        UploadFilePath = x.UploadFilePath,
                        // DisposeFilePath = x.DisposeFilePath,
                        DisposeFilePath = x.Id.ToString(),
                        DisposeFileName = string.IsNullOrWhiteSpace(x.DisposeFilePath) ? "未生成" : "点击下载",
                        LoginId = MemberService.QueryEntity(x.MemberId) != null ? MemberService.QueryEntity(x.MemberId).LoginId : "卫星网"

                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 医学数据 获取数据
        /// </summary>
        /// <param name="uploadMessage"></param>
        /// <returns></returns>
        private string MedicalDataListSize(string uploadMessage)
        {
            try
            {
                var uploadResult = uploadMessage.DeserializeObject<UploadMedicalResult>();
                if (uploadResult != null && uploadResult.BaseMessage != null)
                {
                    return uploadResult.BaseMessage.FileLength;
                }

                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// 医学数据 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalDataModel SetMedicalDataCreateData()
        {
            MedicalDataModel model = new MedicalDataModel();

            return model;
        }

        /// <summary>
        /// 医学数据 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalDataCreate()
        {
            return View(this.SetMedicalDataCreateData());

        }

        /// <summary>
        /// 医学数据 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalDataCreate(MedicalDataModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MedicalDataService.Insert(entity);

                string logContent = "添加医学数据 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalDataCreate");
            }

            return View(this.SetMedicalDataCreateData());
        }


        /// <summary>
        /// 医学数据 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalDataModel SetMedicalDataEditData(long id)
        {
            var entity = this.MedicalDataService.QueryEntity(m => m.Id == id && m.Mark > 0);
            var model = entity.ToModel();
            model.UploadMessageEntity = entity.UploadMessage.DeserializeObject<UploadMessageModel>();
            model.ValidateList = this.MedicalDataItemValidateService.Query(m => m.MedicalDataId == entity.Id && m.Mark > 0).OrderBy(m => m.Sort).ToList();

            if (model.ValidateList != null && model.ValidateList.Any())
            {
                model.ValidateErrorCount = model.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Error).Count();
                model.ValidateWarningCount = model.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Warning).Count();
            }

            return model;
        }

        /// <summary>
        /// 医学数据 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalDataEdit(long id)
        {
            return this.View(this.SetMedicalDataEditData(id));
        }

        /// <summary>
        /// 医学数据 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalDataEdit(MedicalDataModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MedicalDataService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.MedicalDataService.Update(entity);

                string logContent = "修改医学数据 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalDataList");
            }
            return this.View(this.SetMedicalDataEditData(model.Id));
        }

        /// <summary>
        /// 医学数据 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalDataView(long id)
        {
            var entity = this.MedicalDataService.QueryEntity(m => m.Id == id && m.Mark > 0);
            ViewData["ExceptionMessage"] = entity.ExceptionMessage;
            return this.View(this.SetMedicalDataEditData(id));
        }

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
        [CheckRole(true, false)]
        public ActionResult DownloadNewData(long id)
        {
            //Text = "上半年（1月 - 6月）",Value = "6"},
            // Text="下半年（7月 - 12月）",Value="7"},
            // Text="全年",Value="5"}

            try
            {
                var entity = this.MedicalDataService.QueryEntity(id);
                var memberentity = this.MemberService.QueryEntity(entity.MemberId);
                string filePath = this.MedicalDataService.DownloadNewData(id);
                string path = this.Server.MapPath(filePath);
                //string ex = Path.GetExtension(path);
                //string name = "data-adjust-" + id + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + ex;
                string name = System.IO.Path.GetFileName(filePath);
                if (entity.ProjectType == 1)
                {
                    if (!string.IsNullOrEmpty(memberentity.OpenId))
                    {
                        //string qu = "";
                        //switch (entity.Quarter)
                        //{
                        //    case 6:
                        //        qu = "16";
                        //        break;
                        //    case 7:
                        //        qu = "712";
                        //        break;
                        //    case 5:
                        //        qu = "112";
                        //        break;
                        //}
                        name = "w" + entity.Year.ToString().Replace("20", "") + "c" + (int.Parse(memberentity.OpenId.Replace("CHINET", "")).ToString()) + ".dbf";
                    }

                }
                return File(path, "application/octet-stream", Url.Encode(name));
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex);
                return this.RedirectToAction("MedicalDataView", new { id = id.ToString() });
            }
        }

        /// <summary>
        /// 医学数据 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MedicalDataDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MedicalDataList");
            }

            this.MedicalDataService.Delete(selectedIds);

            string logContent = "【手动】删除医学数据，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MedicalDataList");
        }


        /// <summary>
        /// 获取验证的分页数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataId">>对应的医学数据Id</param>
        /// <param name="level">0：表示全部  1：提示  2：警告  3：错误</param>
        /// <returns></returns>
        [CheckRole(true, false)]
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
        [CheckRole(true, false)]
        public JsonResult GetDataList(DataSourceRequest command, long dataId)
        {
            var IntegraIItemList = this.MedicalDataItemService.QueryPage(dataId, command.Page - 1, command.PageSize);
            return Json(new { total = IntegraIItemList.TotalPages, rows = IntegraIItemList });
        }

        /// <summary>
        /// 下载原始文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MedicalDataDownload1(List<long> ids, MedicalDataDownloadEnum downloadEvent)
        {
            string logContent = null;
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    string file = MedicalDataService.Download(ids, downloadEvent);
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
            return RedirectToAction("MedicalDataList");
        }

        /// <summary>
        /// 下载容错文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MedicalDataDownload2(List<long> ids, MedicalDataDownloadEnum downloadEvent)
        {
            string logContent = null;
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    string file = MedicalDataService.Download(ids, downloadEvent);
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
            return RedirectToAction("MedicalDataList");
        }

        [HttpGet, CheckRole(true, false)]
        public ActionResult MedicalDataToWord(long id)
        {
            try
            {
                string fileName = "";
                string path = new MedicalDataWordServiceExtensions().CreateWord(id, ref fileName);
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new Exception("Word文件生成失败！");
                }
                return File(path, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return Content(@"<div style=""width: 80%; margin: 0 auto; text-align: center; margin-top: 10%;"">
    <h2 style="""">Word文件生成失败！</h2>
    <div style=""color: #999;"">
        <lable id=""secLable"" style=""font-size: 1.5rem; margin: 0 0.5rem; font-weight: bold; color: #fb6e52;"">5</lable>秒后自动返回
    </div>
</div>
<script>var t = 4, timer; function countdown() {if (t >= 0) { document.getElementById('secLable').innerText = t--; timer = setTimeout('countdown()', 1000);} else { clearTimeout(timer); location.href = '" + Url.Action("MedicalDataView", new { id = id }).ToString() + "'; } } timer = setTimeout('countdown()', 1000);</script>");
            }
        }
        #endregion

        #region 医生职称

        /// <summary>
        /// 医生职称 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DoctorTitleList()
        {
            DoctorTitleModel model = new DoctorTitleModel();
            model.StateList = base.GetEnabledSelectList();

            return View(model);
        }

        /// <summary>
        /// 医生职称 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoctorTitleList(DataSourceRequest command, DoctorTitleModel model)
        {

            //获得数据
            var list = this.DoctorTitleService.QueryPage(model.Name, model.StateValue, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.Name,
                        Sort = x.Sort,
                        Status = x.Status ? "启用" : "禁用",
                        Describe = x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 医生职称 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public DoctorTitleModel SetDoctorTitleCreateData()
        {
            DoctorTitleModel model = new DoctorTitleModel();
            model.Status = true;
            return model;
        }

        /// <summary>
        /// 医生职称 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult DoctorTitleCreate()
        {
            return View(this.SetDoctorTitleCreateData());

        }

        /// <summary>
        /// 医生职称 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DoctorTitleCreate(DoctorTitleModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.DoctorTitleService.Insert(entity);

                string logContent = "添加医生职称，职称名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("DoctorTitleCreate");
            }

            return View(this.SetDoctorTitleCreateData());
        }


        /// <summary>
        /// 医生职称 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public DoctorTitleModel SetDoctorTitleEditData(long id)
        {
            var entity = this.DoctorTitleService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 医生职称 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult DoctorTitleEdit(long id)
        {
            return this.View(this.SetDoctorTitleEditData(id));
        }

        /// <summary>
        /// 医生职称 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DoctorTitleEdit(DoctorTitleModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.DoctorTitleService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.DoctorTitleService.Update(entity);

                string logContent = "修改医生职称，职称名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("DoctorTitleList");
            }
            return this.View(this.SetDoctorTitleEditData(model.Id));
        }


        /// <summary>
        /// 医生职称 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult DoctorTitleView(long id)
        {
            return this.View(this.SetDoctorTitleEditData(id));
        }


        /// <summary>
        /// 医生职称 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorTitleDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("DoctorTitleList");
            }

            this.DoctorTitleService.Delete(selectedIds);

            string logContent = "【手动】删除医生职称，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("DoctorTitleList");
        }


        #endregion

        #region 医学数据抗生素

        /// <summary>
        /// 医学数据抗生素 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticList()
        {
            return View();
        }

        /// <summary>
        /// 医学数据抗生素 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MedicalAntibioticList(DataSourceRequest command, MedicalAntibioticModel model)
        {

            //获得数据
            var list = this.MedicalAntibioticService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 医学数据抗生素 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalAntibioticModel SetMedicalAntibioticCreateData()
        {
            MedicalAntibioticModel model = new MedicalAntibioticModel();

            return model;
        }

        /// <summary>
        /// 医学数据抗生素 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticCreate()
        {
            return View(this.SetMedicalAntibioticCreateData());

        }

        /// <summary>
        /// 医学数据抗生素 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalAntibioticCreate(MedicalAntibioticModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MedicalAntibioticService.Insert(entity);

                string logContent = "添加医学数据抗生素 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalAntibioticCreate");
            }

            return View(this.SetMedicalAntibioticCreateData());
        }


        /// <summary>
        /// 医学数据抗生素 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalAntibioticModel SetMedicalAntibioticEditData(long id)
        {
            var entity = this.MedicalAntibioticService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 医学数据抗生素 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticEdit(long id)
        {
            return this.View(this.SetMedicalAntibioticEditData(id));
        }

        /// <summary>
        /// 医学数据抗生素 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalAntibioticEdit(MedicalAntibioticModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MedicalAntibioticService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.MedicalAntibioticService.Update(entity);

                string logContent = "修改医学数据抗生素 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalAntibioticEdit");
            }
            return this.View(this.SetMedicalAntibioticEditData(model.Id));
        }


        /// <summary>
        /// 医学数据抗生素 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticView(long id)
        {
            return this.View(this.SetMedicalAntibioticEditData(id));
        }


        /// <summary>
        /// 医学数据抗生素 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MedicalAntibioticDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MedicalAntibioticList");
            }

            this.MedicalAntibioticService.Delete(selectedIds);

            string logContent = "【手动】删除医学数据抗生素，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MedicalAntibioticList");
        }


        #endregion

        #region 医学数据 细菌与抗生素规则

        /// <summary>
        /// 医学数据 细菌与抗生素规则 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticRuleList()
        {
            return View();
        }

        /// <summary>
        /// 医学数据 细菌与抗生素规则 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MedicalAntibioticRuleList(DataSourceRequest command, MedicalAntibioticRuleModel model)
        {

            //获得数据
            var list = this.MedicalAntibioticRuleService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 医学数据 细菌与抗生素规则 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalAntibioticRuleModel SetMedicalAntibioticRuleCreateData()
        {
            MedicalAntibioticRuleModel model = new MedicalAntibioticRuleModel();

            return model;
        }

        /// <summary>
        /// 医学数据 细菌与抗生素规则 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticRuleCreate()
        {
            return View(this.SetMedicalAntibioticRuleCreateData());

        }

        /// <summary>
        /// 医学数据 细菌与抗生素规则 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalAntibioticRuleCreate(MedicalAntibioticRuleModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MedicalAntibioticRuleService.Insert(entity);

                string logContent = "添加医学数据 细菌与抗生素规则 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalAntibioticRuleCreate");
            }

            return View(this.SetMedicalAntibioticRuleCreateData());
        }


        /// <summary>
        /// 医学数据 细菌与抗生素规则 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalAntibioticRuleModel SetMedicalAntibioticRuleEditData(long id)
        {
            var entity = this.MedicalAntibioticRuleService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 医学数据 细菌与抗生素规则 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticRuleEdit(long id)
        {
            return this.View(this.SetMedicalAntibioticRuleEditData(id));
        }

        /// <summary>
        /// 医学数据 细菌与抗生素规则 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalAntibioticRuleEdit(MedicalAntibioticRuleModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MedicalAntibioticRuleService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.MedicalAntibioticRuleService.Update(entity);

                string logContent = "修改医学数据 细菌与抗生素规则 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalAntibioticRuleEdit");
            }
            return this.View(this.SetMedicalAntibioticRuleEditData(model.Id));
        }


        /// <summary>
        /// 医学数据 细菌与抗生素规则 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalAntibioticRuleView(long id)
        {
            return this.View(this.SetMedicalAntibioticRuleEditData(id));
        }


        /// <summary>
        /// 医学数据 细菌与抗生素规则 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MedicalAntibioticRuleDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MedicalAntibioticRuleList");
            }

            this.MedicalAntibioticRuleService.Delete(selectedIds);

            string logContent = "【手动】删除医学数据 细菌与抗生素规则，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MedicalAntibioticRuleList");
        }


        #endregion

        #region 医学数据细菌

        /// <summary>
        /// 医学数据细菌 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalOrganismList()
        {
            return View();
        }

        /// <summary>
        /// 医学数据细菌 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MedicalOrganismList(DataSourceRequest command, MedicalOrganismModel model)
        {

            //获得数据
            var list = this.MedicalOrganismService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x => x.ToModel()),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 医学数据细菌 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalOrganismModel SetMedicalOrganismCreateData()
        {
            MedicalOrganismModel model = new MedicalOrganismModel();

            return model;
        }

        /// <summary>
        /// 医学数据细菌 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalOrganismCreate()
        {
            return View(this.SetMedicalOrganismCreateData());

        }

        /// <summary>
        /// 医学数据细菌 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalOrganismCreate(MedicalOrganismModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MedicalOrganismService.Insert(entity);

                string logContent = "添加医学数据细菌 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalOrganismCreate");
            }

            return View(this.SetMedicalOrganismCreateData());
        }


        /// <summary>
        /// 医学数据细菌 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MedicalOrganismModel SetMedicalOrganismEditData(long id)
        {
            var entity = this.MedicalOrganismService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 医学数据细菌 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalOrganismEdit(long id)
        {
            return this.View(this.SetMedicalOrganismEditData(id));
        }

        /// <summary>
        /// 医学数据细菌 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MedicalOrganismEdit(MedicalOrganismModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MedicalOrganismService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.MedicalOrganismService.Update(entity);

                string logContent = "修改医学数据细菌 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MedicalOrganismEdit");
            }
            return this.View(this.SetMedicalOrganismEditData(model.Id));
        }


        /// <summary>
        /// 医学数据细菌 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalOrganismView(long id)
        {
            return this.View(this.SetMedicalOrganismEditData(id));
        }


        /// <summary>
        /// 医学数据细菌 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MedicalOrganismDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MedicalOrganismList");
            }

            this.MedicalOrganismService.Delete(selectedIds);

            string logContent = "【手动】删除医学数据细菌，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MedicalOrganismList");
        }


        #endregion

        #region 修改了科室的配置数据，重新更新上传记录的值，请勿随意删除和调用

        /// <summary>
        /// 修改了科室的配置数据
        /// </summary>
        /// <param name="id">上传数据Id</param>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ContentResult UpdateLocation(long id)
        {
            try
            {
                var result = this.HospitalWardLocationService.UpdateItem(id);
                return this.Content(result);
            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 修改了科室的配置数据
        /// </summary>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ContentResult UpdateDisposeFilePath2()
        {
            try
            {
                var result = this.MedicalDataService.UpdateDisposeFilePath3();
                return this.Content("");
            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);
            }
        }

        [CheckRole(false, false)]
        public ContentResult UpdateDisposeFilePath4(int type = 3)
        {
            try
            {
                var result = this.MedicalDataService.UpdateDisposeFilePath4(type);
                return this.Content(result);
            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);
            }
        }

        #region 科室配置

        #region 列表
        public ActionResult HospitalWardLocationList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult HospitalWardLocationList(DataSourceRequest command, HospitalModel model)
        {
            var list = HospitalWardLocationService.QueryPage(name: model.Name, command.Page - 1, command.PageSize);
            var data = list.Select(item => new
            {
                HospitalId = item.HospitalId.ToString(),
                item.Name,
                item.WardCount,
                DateTime = item.LastDateTime.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 新增医院科室配置
        /// <summary>
        /// 新增医院科室配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CheckRole(false, false)]
        public ActionResult HospitalWardLocationCreate()
        {
            ViewData["HospitalList"] = HospitalService.Query(r => r.State && r.Mark > 0).OrderBy(r => r.Sort)
                  .ThenByDescending(r => r.InsertTime)
                  .ThenByDescending(r => r.Id)
                  .Select(r => new SelectListItem
                  {
                      Value = r.Id.ToString(),
                      Text = string.IsNullOrWhiteSpace(r.Code) ? r.Name : $"{r.Name} (医院编码：{r.Code})"
                  });
            return View(new HospitalWardLocationModel());
        }

        /// <summary>
        /// 提交新增医院科室配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false, false)]
        public ActionResult HospitalWardLocationCreate(HospitalWardLocationModel model)
        {
            if (ModelState.IsValid)
            {
                model.Trim();

                Hospital hospital = HospitalService.QueryEntity(model.HospitalID);
                if (hospital == null || hospital.Id == 0 || hospital.Mark <= 0)
                {
                    ViewData["HospitalList"] = HospitalService.Query(r => r.State && r.Mark > 0).OrderBy(r => r.Sort)
                        .ThenByDescending(r => r.InsertTime)
                        .ThenByDescending(r => r.Id)
                        .Select(r => new SelectListItem
                        {
                            Value = r.Id.ToString(),
                            Text = string.IsNullOrWhiteSpace(r.Code) ? r.Name : $"{r.Name} (医院编码：{r.Code})"
                        });
                    ErrorNotification("您所输入或选择的医院不存在或已删除");
                    return View(model);
                }

                if (HospitalWardLocationService.Count(r => r.HospitalId == hospital.Id && r.Ward == model.Ward && r.Department_EN == model.Department && r.Location == model.Location && r.Location_Type == model.LocationType && r.Mark > 0) == 0)
                {
                    HospitalWardLocation entity = model.ToEntity();
                    entity.Id = CommonHelper.GuidToLongID;
                    entity.HospitalId = hospital.Id;
                    entity.Name = hospital.Name;
                    entity.Department_CN = entity.Ward;
                    entity.Sort = 1;
                    entity.InsertTime = DateTime.Now;
                    entity.UpdateTime = entity.InsertTime;
                    entity.DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0);
                    entity.Mark = 1;
                    entity.Version = 1;
                    entity.Describe = null;
                    HospitalWardLocationService.Insert(entity);
                    string logContent = $"{entity.Name} 科室配置添加成功！";
                    SuccessNotification(logContent);
                    ActionLogService.Insert(ActionType.Create, ActionSource.Admin, LoginUserinfo.Id, LoginUserinfo.Name, logContent, entity.SerializeObject());
                    return RedirectToAction("HospitalWardLocationList");
                }
                else
                {
                    base.ErrorNotification("所录入的科室配置信息已存在");
                }
            }
            else
            {
                string errorMessage = ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "提交失败";
                }
                base.ErrorNotification(errorMessage);
            }

            ViewData["HospitalList"] = HospitalService.Query(r => r.Mark > 0 && r.State).OrderBy(r => r.Sort)
                  .ThenByDescending(r => r.InsertTime)
                  .ThenByDescending(r => r.Id)
                  .Select(r => new SelectListItem
                  {
                      Value = r.Id.ToString(),
                      Text = string.IsNullOrWhiteSpace(r.Code) ? r.Name : $"{r.Name} (医院编码：{r.Code})"
                  });
            return View(model);
        }
        #endregion

        #region 科室配置详情
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">医院ID</param>
        /// <returns></returns>
        public ActionResult HospitalWardLocationDetail(long id)
        {
            Hospital hospital = HospitalService.QueryEntity(id);
            HospitalModel hospitalModel = new HospitalModel()
            {
                IdString = hospital.Id.ToString(),
                Name = hospital.Name,
                Sort = hospital.Sort,
                Address = hospital.Address,
                ContactsTel = hospital.ContactsTel,
                ContactsUser = hospital.ContactsUser,
                StateValue = hospital.State ? "启用" : "禁用",
                MemberId = hospital.MemberId,
                ProvinceName = hospital.ProvinceName
            };
            return View(hospitalModel);
        }

        [HttpPost]
        public JsonResult HospitalWardLocationDetail(long id, DataSourceRequest command)
        {
            var list = HospitalWardLocationService.QueryPage(command.Page - 1, command.PageSize, r => r.HospitalId == id && r.Mark > 0);

            var data = list.OrderByDescending(r => r.InsertTime).ThenByDescending(r => r.Sort).ThenByDescending(r => r.Id).Select(item => new
            {
                Id = item.Id.ToString(),
                item.Name,
                item.Ward,
                Department = item.Department_EN,
                item.Location,
                item.Location_Type,
                DateTime = item.InsertTime.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 按照医院删除科室配置
        /// <summary>
        /// 按照医院删除科室配置
        /// </summary>
        /// <param name="ids">医院id集合</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false, false)]
        public ActionResult HospitalWardLocationDelete(List<long> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                HospitalWardLocationService.Delete(item => ids.Contains(item.HospitalId) && item.Mark > 0);
                SuccessNotification($"成功删除所选医院的全部科室配置数据");
                try
                {
                    ActionLogService.Insert(ActionType.Delete, ActionSource.Admin, LoginUserinfo.Id, LoginUserinfo.Name, $"【手动】成功删除医院所有科室配置共{ids.Count}条", $"医院id集合：{ids.SerializeObject()}");
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ErrorNotification("删除失败，请选择您要删除的数据.");
            }
            return RedirectToAction("HospitalWardLocationList");
        }
        #endregion

        #region 上传医院科室配置
        public ViewResult HospitalWardLocation()
        {
            ViewData["HospitalList"] = HospitalService.Query(r => r.Mark > 0 && r.State).OrderBy(r => r.Sort)
                  .ThenByDescending(r => r.InsertTime)
                  .ThenByDescending(r => r.Id)
                  .Select(r => new SelectListItem
                  {
                      Value = r.Id.ToString(),
                      Text = string.IsNullOrWhiteSpace(r.Code) ? r.Name : $"{r.Name} (医院编码：{r.Code})"
                  });
            return View();
        }

        /// <summary>
        /// 搜索医院名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost, CheckRole(CheckRole = false)]
        public JsonResult SearchHospitalByWardLocation(string name)
        {
            var list = this.HospitalService.Query(m => m.Name.Contains(name)).OrderBy(m => m.Name).Take(10).Select(m => new
            {
                id = m.Id.ToString(),
                value = $"{m.Name} {(!string.IsNullOrWhiteSpace(m.Code) ? $" (医院编码：{m.Code})" : "")}"
            });

            return Json(new
            {
                query = name,
                suggestions = list
            });
        }

        /// <summary>
        /// 提交上传科室配置数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, CheckRole(CheckRole = false)]
        public JsonResult OnSubmit_HospitalWardLocation(long id, HttpPostedFileBase file = null)
        {
            #region 简单验证

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
                        // String.IsNullOrWhiteSpace(department_CN) &&
                        String.IsNullOrWhiteSpace(department) &&
                        String.IsNullOrWhiteSpace(location) &&
                        String.IsNullOrWhiteSpace(location_Type))
                    {
                        continue;
                    }

                    if (HospitalWardLocationService.Count(r => r.HospitalId == id && r.Ward == _ward && r.Department_EN == department && r.Location == location && r.Location_Type == location_Type && r.Mark > 0) == 0)
                    {
                        entities.Add(new Core.Domain.Medicine.HospitalWardLocation
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
            catch (Exception)
            {
                return Json(new
                {
                    status = false,
                    message = "读取文件失败，请使用指定导入模板文件上传"
                });
            }

            try
            {
                if (entities.Count > 0)
                {
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
        #endregion

        #region 新增
        /// <summary>
        /// 新增医院科室配置
        /// </summary>
        /// <param name="id">医院id</param>
        /// <returns></returns>
        [App_Start.CheckRole(false, false)]
        public ActionResult HWLItemCreate(long id)
        {
            Hospital hospital = HospitalService.QueryEntity(id);
            return View(new HospitalWardLocationModel { Id = CommonHelper.GuidToLongID, HospitalID = hospital.Id, Name = hospital.Name });
        }

        /// <summary>
        /// 提交新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(false, false)]
        public ActionResult HWLItemCreate(HospitalWardLocationModel model)
        {
            Hospital hospital = HospitalService.QueryEntity(model.HospitalID);

            if (ModelState.IsValid)
            {
                model.Trim();

                // 判断存在
                if (HospitalWardLocationService.Count(r => r.HospitalId == hospital.Id && r.Ward == model.Ward && r.Department_EN == model.Department && r.Location == model.Location && r.Location_Type == model.LocationType && r.Mark > 0) == 0)
                {
                    HospitalWardLocation entity = model.MapTo<HospitalWardLocationModel, HospitalWardLocation>();
                    entity.Id = CommonHelper.GuidToLongID;
                    entity.HospitalId = hospital.Id;
                    entity.Name = hospital.Name;
                    entity.Department_CN = entity.Ward;
                    entity.Sort = 1;
                    entity.InsertTime = DateTime.Now;
                    entity.UpdateTime = entity.InsertTime;
                    entity.DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0);
                    entity.Mark = 1;
                    entity.Version = 1;
                    entity.Describe = null;
                    HospitalWardLocationService.Insert(entity);
                    string logContent = $"{entity.Name} 科室配置添加成功！";
                    SuccessNotification(logContent);
                    ActionLogService.Insert(ActionType.Create, ActionSource.Admin, LoginUserinfo.Id, LoginUserinfo.Name, logContent, entity.SerializeObject());
                    return RedirectToAction("HospitalWardLocationDetail", new { id = entity.HospitalId });
                }
                else
                {
                    base.ErrorNotification("所录入的科室配置信息已存在");
                }
            }
            else
            {
                string errorMessage = ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "提交失败";
                }
                base.ErrorNotification(errorMessage);
            }

            model.Id = hospital.Id;
            model.Name = hospital.Name;
            return View(model);
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑科室配置信息
        /// </summary>
        /// <param name="id">科室配置id</param>
        /// <returns></returns>
        [App_Start.CheckRole(false, false)]
        public ActionResult HWLItemEdit(long id)
        {
            HospitalWardLocation entity = HospitalWardLocationService.QueryEntity(id);
            HospitalWardLocationModel model = entity.MapTo<HospitalWardLocation, HospitalWardLocationModel>();
            return View(model);
        }

        /// <summary>
        /// 提交编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(false, false)]
        public ActionResult HWLItemEdit(HospitalWardLocationModel model)
        {
            if (ModelState.IsValid)
            {
                model.Trim();
                HospitalWardLocation entity = HospitalWardLocationService.QueryEntity(model.Id);
                if (!string.IsNullOrEmpty(entity.Ward) && entity.Ward.Equals(model.Ward) &&
                    !string.IsNullOrEmpty(entity.Department_EN) && entity.Department_EN.Equals(model.Department) &&
                    !string.IsNullOrEmpty(entity.Location) && entity.Location.Equals(model.Location) &&
                    !string.IsNullOrEmpty(entity.Location_Type) && entity.Location_Type.Equals(model.LocationType))
                {
                    SuccessNotification("本次操作未作任何修改");
                    return RedirectToAction("HospitalWardLocationDetail", new { id = entity.HospitalId });
                }

                // 判断存在
                if (HospitalWardLocationService.Count(r => r.Id != entity.Id && r.HospitalId == entity.HospitalId && r.Ward == model.Ward && r.Department_EN == model.Department && r.Location == model.Location && r.Location_Type == model.LocationType && r.Mark > 0) == 0)
                {
                    entity.Ward = model.Ward;
                    entity.Department_EN = model.Department;
                    entity.Location = model.Location;
                    entity.Location_Type = model.LocationType;
                    entity.Department_CN = entity.Ward;
                    entity.UpdateTime = DateTime.Now;
                    entity.Mark = 2;
                    HospitalWardLocationService.Update(entity);
                    string logContent = $"{entity.Name} 科室配置修改成功！";
                    SuccessNotification(logContent);
                    ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, LoginUserinfo.Id, LoginUserinfo.Name, logContent, entity.SerializeObject());
                    return RedirectToAction("HospitalWardLocationDetail", new { id = entity.HospitalId });
                }
                else
                {
                    base.ErrorNotification("所录入的科室配置信息已存在");
                }
            }
            else
            {
                string errorMessage = ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "提交失败";
                }
                base.ErrorNotification(errorMessage);
            }

            return View(model);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除科室配置
        /// </summary>
        /// <param name="hid">医院id</param>
        /// <param name="ids">要删除的科室配置id集合</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(false, false)]
        public ActionResult HWLItemDelete(long id, List<long> ids)
        {
            if (id == 0)
            {
                ErrorNotification("删除失败，请求的参数不正确.");
            }
            else if (ids == null || ids.Count <= 0)
            {
                ErrorNotification("删除失败，请选择您要删除的科室配置.");
            }
            else
            {

                HospitalWardLocationService.Delete(item => ids.Contains(item.Id) && item.Mark > 0);

                try
                {
                    Hospital hospital = HospitalService.QueryEntity(id);
                    SuccessNotification($"成功删除{hospital.Name}的共{ids.Count}条科室配置数据");
                    ActionLogService.Insert(ActionType.Delete, ActionSource.Admin, LoginUserinfo.Id, LoginUserinfo.Name, $"【手动】成功删除{hospital.Name}医院科室配置共{ids.Count}条", ids.SerializeObject());
                }
                catch (Exception)
                {
                    SuccessNotification($"成功删除{ids.Count}条科室配置数据");
                }

            }
            return RedirectToAction("HospitalWardLocationDetail", new { id = id });
        }
        #endregion

        #endregion

        #region 项目上传情况
        public ActionResult ProjectSituationList()
        {
            MedicalDataModel model = new MedicalDataModel();
            model.YearList = new List<SelectListItem>();
            int yearTemp = DateTime.Now.Year;
            for (int i = yearTemp; i >= 2010; i--)
            {
                model.YearList.Add(new SelectListItem() { Text = "" + i, Value = i.ToString() });
            }
            model.QuarterList = new List<SelectListItem>() {
                new SelectListItem() {  Text="请选择",Value="0"},
                 //new SelectListItem() {  Text="第二季度（4月 - 6月）",Value="2"},
                 //new SelectListItem() {  Text="第三季度（7月 - 9月）",Value="3"},
                 //new SelectListItem() {  Text="第四季度（10月 - 12月）",Value="4"},
                  new SelectListItem() {  Text="全年",Value="5"},
                 new SelectListItem() {  Text="上半年（1月 - 6月）",Value="6"},
                 new SelectListItem() {  Text="下半年（7月 - 12月）",Value="7"}

            };
            model.ProjectTypeItems = this.medicalDataProjectService.Query(m => m.Mark > 0).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            return View(model);
        }
        /// <summary>
        /// 获取数据情况查询
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProjectSituationList(DataSourceRequest command, MedicalDataModel model)
        {
            if (model.Year <= 0)
            {
                model.Year = DateTime.Now.Year;
            }
            //if (model.Quarter <= 0)
            //{
            //    model.Quarter = 5;
            //}
            //获得数据
            if (model.Quarter > 0)
            {
                var list = this.MedicalDataService.QueryPage(model.ProjectType, model.Year, model.Quarter, command.Page - 1, command.PageSize);
                List<string> Idlist = new List<string>();
                foreach (var item in list)
                {
                    Idlist.Add(item.HospitalId.ToString());
                }
                var Hospitallist = this.HospitalService.QueryList("[" + model.ProjectType + "]", command.Page - 1, command.PageSize);

                var gridModel = new DataSourceResult
                {

                    Data = Hospitallist.Select(x =>
                     {
                         var data = MedicalDataService.QueryEntity(m => m.Mark > 0 && m.Display == true && m.ProjectType == model.ProjectType && m.Year == model.Year && m.Quarter == model.Quarter && m.HospitalId == x.Id);
                         return new
                         {
                             Id = data == null ? "0" : data.Id.ToString(),
                             FileName = data == null ? "" : data.FileName,
                             Year = model.Year,
                             MemberName = data == null ? "" : data.MemberName,
                             Quarter = this.MedicalDataService.GetDataQuarter(model.Year, model.Quarter),
                             HospitalName = x.Name,
                             UploadFilePath = data == null ? "" : data.UploadFilePath,
                             DisposeFilePath = data == null ? "" : data.DisposeFilePath,
                             Complete = data == null ? "" : (Idlist.Contains(x.Id.ToString()) && !string.IsNullOrEmpty(data.DisposeFilePath) ? "<i class='fa fa-check' aria-hidden='true' style='color:blue'></i>" : "重新生成"),
                             hangintheair = data == null ? "未上传" : (!Idlist.Contains(x.Id.ToString()) ? "未上传" : ""),
                             LoginId = data == null ? "" : (MemberService.QueryEntity(data.MemberId) != null ? MemberService.QueryEntity(data.MemberId).LoginId : ""),
                         };
                     }),
                    Total = Hospitallist.TotalCount
                };

                return new JsonResult { Data = gridModel };
            }
            else
            {
                var Hospitallist = this.HospitalService.QueryList("[" + model.ProjectType + "]");
                List<int> Quarter = new List<int>() { 6, 7, 5 };
                List<ProjectSituation> ProjectLIst = new List<ProjectSituation>();
                foreach (var item in Quarter)
                {
                    var list = this.MedicalDataService.QueryPage(model.ProjectType, model.Year, item);
                    List<string> Idlist = new List<string>();
                    foreach (var item1 in list)
                    {
                        Idlist.Add(item1.HospitalId.ToString());
                    }
                    foreach (var item2 in Hospitallist)
                    {
                        ProjectSituation situation = new ProjectSituation();
                        var data = MedicalDataService.QueryEntity(m => m.Mark > 0 && m.Display == true && m.ProjectType == model.ProjectType && m.Year == model.Year && m.Quarter == item && m.HospitalId == item2.Id);
                        situation.Id = data == null ? "0" : data.Id.ToString();
                        situation.FileName = data == null ? "" : data.FileName;
                        situation.Year = model.Year;
                        situation.MemberName = data == null ? "" : data.MemberName;
                        situation.Quarter = this.MedicalDataService.GetDataQuarter(model.Year, item);
                        situation.HospitalName = item2.Name;
                        situation.UploadFilePath = data == null ? "" : data.UploadFilePath;
                        situation.DisposeFilePath = data == null ? "" : data.DisposeFilePath;
                        situation.Complete = data == null ? "" : (Idlist.Contains(item2.Id.ToString()) && !string.IsNullOrEmpty(data.DisposeFilePath) ? "<i class='fa fa-check' aria-hidden='true' situati.onstyle='color:blue'></i>" : "重新生成");
                        situation.hangintheair = data == null ? "未上传" : (!Idlist.Contains(item2.Id.ToString()) ? "未上传" : "");
                        situation.LoginId = data == null ? "" : (MemberService.QueryEntity(data.MemberId) != null ? MemberService.QueryEntity(data.MemberId).LoginId : "");
                        ProjectLIst.Add(situation);
                    }
                }
                var aalist = ProjectLIst.AsQueryable().Skip((command.Page - 1) * command.PageSize).Take(command.PageSize).ToList();
                var gridModel = new DataSourceResult
                {

                    Data = aalist,
                    Total = ProjectLIst.Count
                };

                return new JsonResult { Data = gridModel };
            }
        }
        #endregion

    }
}

