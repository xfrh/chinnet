using ManageSystem.Core.Domain.Ctr;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Upload;
using ManageSystem.Services.Ctr;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class ClinicalTrialReportController : WebBaseController
    {
        private readonly IMemberService MemberService;
        private readonly IHospitalService HospitalService;
        private readonly IClinicalTrialReportService ClinicalTrialReportService;

        public ClinicalTrialReportController(
            IMemberService _MemberService,
            IHospitalService _HospitalService,
            IClinicalTrialReportService _ClinicalTrialReportService
            )
        {
            MemberService = _MemberService;
            HospitalService = _HospitalService;
            ClinicalTrialReportService = _ClinicalTrialReportService;
        }


        [CheckRole(false)]
        public ActionResult Index()
        {
            ////获取当前登录用户名
            //var member = base.LoginUserinfo;
            ////根据用户ID获取用户信息
            //var userinfo = this.MemberService.QueryEntity(member.Id);
            List<SelectListItem> hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ClinicalTrialReportModel model = new ClinicalTrialReportModel()
            {
                Id = new long(),
                HospitalList = hospitalList
            };

            //ViewBag.HospitalId = userinfo.HospitalId;
            return View(model);
        }

        /// <summary>
        /// 药敏结果图上传
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult UploadFile()
        {
            try
            {
                UploadParameter par = new UploadParameter();
                UploadResult model = new UploadifyUpload().Upload(new UploadParameter()
                {
                    Extension = "",
                    FilePath = $"/Content/Upload/ClinicalTrialsImage/{DateTime.Now.ToString("yyyyMMddHHmmss")}/",
                    HttpFile = this.HttpContext.Request.Files,
                    MaxLength = 70 * 1024, // 50M // 2020-01-17修改为70M
                    NewFileName = this.HttpContext.Request.Files[0].FileName.Split('.')[0].Trim(),
                    Type = UploadTypeEnum.Image
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
                throw new Exception("上传文件失败，原由：" + ex.ToString());
            }

        }

        /// <summary>
        /// 卫星网申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ActionResult ClinicalTrialReportApply(ClinicalTrialReport model)
        {
            if (string.IsNullOrEmpty(model.ProjectName))
                ModelState.AddModelError("ProjectName", "请填写项目名称");

            if (string.IsNullOrEmpty(model.HospitalName))
                ModelState.AddModelError("HospitalName", "请填写医院名称");

            //if (model.HospitalId == 0)
            //    ModelState.AddModelError("HospitalId", "请选择医院名称");

            if (string.IsNullOrEmpty(model.BacterialName))
                ModelState.AddModelError("BacterialName", "细菌名称不能为空");

            //if (string.IsNullOrEmpty(model.SpecimenType))
            //    ModelState.AddModelError("SpecimenType", "标本类型不能为空");
            if (string.IsNullOrEmpty(model.Position))
                ModelState.AddModelError("Position", "在药敏板中位置不能为空");

            if (string.IsNullOrEmpty(model.HighstPotency))
                ModelState.AddModelError("HighstPotency", "最高浓度不能为空");

            if (string.IsNullOrEmpty(model.DrugName))
                ModelState.AddModelError("DrugName", "药物名称不能为空");

            //if (string.IsNullOrEmpty(model.Reader))
            //    ModelState.AddModelError("Reader", "阅读者不能为空");
            if (string.IsNullOrEmpty(model.MicValue))
                ModelState.AddModelError("MicValue", "Mic值不能为空");

            //if (string.IsNullOrEmpty(model.Reviewer))
            //    ModelState.AddModelError("Reviewer", "复核者不能为空");

            //if (string.IsNullOrEmpty(model.ReviewMicValue))
            //    ModelState.AddModelError("ReviewMicValue", "复核Mic值不能为空");

            if (string.IsNullOrEmpty(model.PicturePath))
                ModelState.AddModelError("PicturePath", "药敏结果不能为空");

            string[] files = { "JPEG", "TIFF", "RAW", "BMP", "GIF", "PNG", "JPG", "jpeg", "tiff", "raw", "bmp", "gif", "png", "jpg" };
            string pictureFileName = model.PicturePath.Split('.')[1];
            if (!files.Contains(pictureFileName))
                ModelState.AddModelError("PicturePath", "药敏结果文件格式不正确");

            if (string.IsNullOrEmpty(model.ExperimentTime))
                ModelState.AddModelError("ExperimentTime", "实验时间不能为空");

            if (string.IsNullOrEmpty(model.StrainNo))
                ModelState.AddModelError("StrainNo", "菌株编号不能为空");

            ClinicalTrialReport entity = new ClinicalTrialReport();
            entity.Id = DateTime.Now.Ticks;
            entity.BacterialName = model.BacterialName;
            entity.HospitalId = model.HospitalId;
            entity.SpecimenType = model.SpecimenType;
            entity.DrugName = model.DrugName;
            entity.Reader = model.Reader;
            entity.MicValue = model.MicValue;
            entity.Reviewer = model.Reviewer;
            entity.ReviewMicValue = model.ReviewMicValue;
            entity.PicturePath = model.PicturePath;
            entity.ExperimentTime = model.ExperimentTime;
            entity.StrainNo = model.StrainNo;
            entity.ProjectName = model.ProjectName;
            entity.HospitalName = model.HospitalName;
            entity.Position = model.Position;
            entity.HighstPotency = model.HighstPotency;
            this.ClinicalTrialReportService.Insert(entity);
            this.HttpContext.Session["LoginUserinfoSession"] = "";
            base.InsetActionLog(ActionType.Create, "添加临床实验上报", entity.SerializeObject());
            base.SuccessNotification("上报成功!");
            return this.RedirectToAction("Index");
        }

        [CheckRole(true)]
        public ActionResult OnlineAudit(string ProjectName = "0", int pageIndex = 1)
        {
            //获取当前登录用户名
            var member = base.LoginUserinfo;
            //根据用户ID获取用户信息
            var userinfo = this.MemberService.QueryEntity(member.Id);

            ClinicalTrialReportManageModel model = new ClinicalTrialReportManageModel();

            var user = base.LoginUserinfo;
            //  var list = this.ClinicalTrialReportService.Query().Where(x=>x.HospitalId == userinfo.HospitalId);
            var list = this.ClinicalTrialReportService.Query();

            List<string> strList = list.Select(x => x.ProjectName).Distinct().ToList();
            model.ProjectList = list.Select(x => { return new SelectListItem() { Text = x.ProjectName, Value = x.ProjectName }; }).ToList();
            model.ProjectList.Insert(0, new SelectListItem { Value = "0", Text = "--全部--" });

            if (ProjectName != "0")
                list = list.Where(x => x.ProjectName == ProjectName).ToList();

            var data = list.Select(x => new ClinicalTrialReportModel()
            {
                Id = x.Id,
                HospitalName = x.HospitalId == 0 ? x.HospitalName : this.HospitalService.QueryEntity(x.HospitalId).Name,
                StrainNo = x.StrainNo,
                BacterialName = x.BacterialName,
                SpecimenType = x.SpecimenType,
                ExperimentTime = x.ExperimentTime,
                DrugName = x.DrugName,
                MicValue = x.MicValue,
                Reader = x.Reader,
                ReviewMicValue = x.ReviewMicValue,
                Reviewer = x.Reviewer,
                PicturePath = x.PicturePath,
                HighstPotency = x.HighstPotency,
                Position = x.Position
            }).ToPagedList(pageIndex, MvcPagerExtensions.PageSize);


            model.PageList = data;
            return this.View(model);
        }

        [HttpPost]
        public ContentResult UpdateMIC(string id, string value)
        {
            try
            {
                var entity = this.ClinicalTrialReportService.QueryEntity(long.Parse(id));
                entity.ReviewMicValue = value;
                this.ClinicalTrialReportService.Update(entity);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("更新复核MIC值失败，原由：" + ex.ToString());
            }


        }

        [HttpPost]
        [CheckRole(false)]
        public ContentResult ReturnPictureFileList()
        {
            try
            {
                var list = this.ClinicalTrialReportService.Query().Select(x => x.PicturePath + "," + x.Id.ToString());
                return this.Content(list.SerializeObject());
            }
            catch (Exception ex)
            {
                throw new Exception("获取所有图片链接失败，原由：" + ex.ToString());
            }
        }

    }
}