using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.MIC;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class MICapplyController : WebBaseController
    {

        private readonly IMemberService MemberService;
        private readonly IAreaService AreaService;
        private readonly IHospitalService HospitalService;
        private readonly IMICPermissionapplication MICPermissionapplication;
        private readonly IDoctorTitleService DoctorTitleService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        public MICapplyController(
            IMemberService _MemberService,
            IAreaService _AreaService,
            IHospitalService _HospitalService,
            IMICPermissionapplication _MICPermissionapplication,
            IDoctorTitleService _DoctorTitleService,
            IHospitalDepartmentService _HospitalDepartmentService
            )
        {
            MemberService = _MemberService;
            AreaService = _AreaService;
            HospitalService = _HospitalService;
            MICPermissionapplication = _MICPermissionapplication;
            DoctorTitleService = _DoctorTitleService;
            HospitalDepartmentService = _HospitalDepartmentService;
        }
        // GET: MICapply
        public ActionResult Index()
        {
            //获取当前登录用户名
            var member = base.LoginUserinfo;
            //根据用户ID获取用户信息
            var userinfo = this.MemberService.QueryEntity(member.Id);
            List<SelectListItem> provinceList = this.AreaService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            List<SelectListItem> hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == userinfo.HospitalId }; }).ToList();
            List<SelectListItem> hospitalDepartmentList = this.HospitalDepartmentService.GetHospitalDepartments(userinfo.HospitalId).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.HospitalId }).ToList();
            List<SelectListItem> doctorTitleList = this.DoctorTitleService.QueryByParentId(0).Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = r.Id == userinfo.ProvinceId }).ToList();
            provinceList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            hospitalDepartmentList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            doctorTitleList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            MICIndexMemberModel model = new MICIndexMemberModel()
            {
                IsCheckEmail = userinfo.IsCheckEmail,
                Email = userinfo.Email,
                Id = userinfo.Id,
                Name = userinfo.Name,
                Phone = userinfo.Phone,
                HospitalId = userinfo.HospitalId,
                HospitalList = hospitalList,
                HospitalName = HospitalService.QueryEntity(userinfo.HospitalId)?.Name,
                ProvinceList = provinceList,
                ProjectUploadItem = userinfo.ProjectUploadItem,
                HospitalDepartmentList = hospitalDepartmentList,
                DoctorTitleList = doctorTitleList,
                ProvinceId = userinfo.ProvinceId,
                DoctorTitleId = userinfo.DoctorTitleId,
                ProvinceName = AreaService.QueryEntity(userinfo.ProvinceId)?.Name
            };
            ViewBag.HospitalId = userinfo.HospitalId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MICIndexMemberModel model)
        {
            //获取登录用户名
            var member = base.LoginUserinfo;
            //根据用户ID获取实体数据
            var entity = this.MemberService.QueryEntity(member.Id);

            if (string.IsNullOrEmpty(model.Name))
                ModelState.AddModelError("Name", "姓名不能为空！");
            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "电子邮箱不能为空！");
            if (string.IsNullOrEmpty(model.Phone))
                ModelState.AddModelError("Phone", "电话不能为空！");
            if (string.IsNullOrEmpty(model.ProvinceName))
                ModelState.AddModelError("ProvinceName", "所属省份不能为空！");
            if (string.IsNullOrEmpty(model.HospitalName))
                ModelState.AddModelError("HospitalName", "所属医院不能为空！");
            if (string.IsNullOrEmpty(model.HospitalDepartmentName))
                ModelState.AddModelError("HospitalDepartmentName", "科室名称不能为空！");
            if (string.IsNullOrEmpty(model.DoctorTitleName))
                ModelState.AddModelError("DoctorTitleName", "职位不能为空！");

            if (model.Name.Length > 5)
                ModelState.AddModelError("Name", "请输入真实姓名！");

            if (!IsHasChinese(model.Name))
                ModelState.AddModelError("Name", "请输入真实姓名！");



            if (!string.IsNullOrWhiteSpace(member.Phone))
            {
                if (MemberService.Count(r => r.Id != member.Id && (r.Phone == member.Phone || r.LoginId == member.Phone) && r.Mark > 0) > 0)
                {
                    ModelState.AddModelError("Phone", "此手机号已被他人使用");
                }
            }
            if (ModelState.IsValid)
            {
                entity.Name = member.Name;
                entity.Phone = member.Phone;
                if (entity.Email != member.Email)
                    entity.IsCheckEmail = false;
                entity.Email = member.Email;

                MICPermissionapplication micentity = new MICPermissionapplication();
                micentity.MID = entity.Id;
                micentity.Name = model.Name;
                micentity.Phone = model.Phone;
                micentity.Email = model.Email;
                micentity.CompanyName = model.HospitalName;
                micentity.Provincesandcities = model.ProvinceName;
                micentity.Department = model.HospitalDepartmentName;
                micentity.Position = model.DoctorTitleName;
                micentity.Applicationtime = DateTime.Now;
                if (micentity.Applicationstate != 0 || micentity.Applicationstate != 1)
                {
                    micentity.Applicationstate = -1;
                }
                var list = this.MICPermissionapplication.Query().Where(x => x.MID == micentity.MID);

                //账号验证 是否已经提交过mic申请
                if (list.Count() > 0)
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list.Count(x => x.Applicationstate == 0) > 0)
                        {
                            base.ErrorNotification("已经提交审批，请勿重复提交");
                            return this.RedirectToAction("Index");
                        }
                        else if (list.Count(x => x.Applicationstate == -1) > 0)
                        {
                            base.ErrorNotification("已经提交审批，请勿重复提交");
                            return this.RedirectToAction("Index");
                        }
                    }
                }

                this.MICPermissionapplication.Insert(micentity);
                this.HttpContext.Session["LoginUserinfoSession"] = "";
                base.InsetActionLog(ActionType.Create, "添加申请MIC访问", entity.SerializeObject());
                base.SuccessNotification("申请成功,等待管理员审批!");
                return this.RedirectToAction("Index", "MICapply");
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
                return this.RedirectToAction("Index");
            }

        }


        private bool IsHasChinese(string chinese)
        {
            foreach (var item in chinese)
            {
                if (IsChinese(item))
                    return true;
            }
            return false;
        }

        private bool IsChinese(char chinese)
        {
            if (chinese >= 0x4e00 && chinese <= 0x9fbb)
                return true;
            return false;
        }
    }
}