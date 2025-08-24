using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Core.Domain.Medicine.Statistics;
using ManageSystem.Services.Medicine.Statistics;
using ManageSystem.Web.Models.Members;
using ManageSystem.Services.Members;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Meetings;
using ManageSystem.Services.Researches;
using ManageSystem.Web.App_Start;

namespace ManageSystem.Web.Controllers
{
    public class MainController : WebBaseController
    {
        private readonly IHospitalService _hospitalService;
        private readonly IMeetingService _meetingService;
        private readonly IResearchService _researchService;

        public MainController(
            IHospitalService hospitalService,
              IMeetingService meetingService,
              IResearchService researchService
            )
        {
            this._hospitalService = hospitalService;
            this._meetingService = meetingService;
            this._researchService = researchService;
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Index()
        {
            var member = base.LoginUserinfo;
            ViewBag.Name = member.Name;
            ViewBag.Img = MemberExtensions.GetHeadImage(member.HeadImage);

            return View();
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult MapIndex()
        {
            var member = base.LoginUserinfo;
            ViewBag.Name = member.Name;
            ViewBag.Img = MemberExtensions.GetHeadImage(member.HeadImage);

            return View();
        }

        /// <summary>
        /// 设置相关报表的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowData()
        {
            this.GetAreaData();
            this.GetTenGerm();
            this.GetMRSASensitivityDrug();
            this.GetGramNegativeBacteria();

            //设置统计数据
            this.ViewBag.HospitalCount = this._hospitalService.Count(m => m.Mark > 0 && m.State == true); //医院总数量
            this.ViewBag.MeetingCount = this._meetingService.Count(m => m.Mark > 0 && m.Status != 1); //信息动态总数量 ，会议状态，1：待审核  2：已审核   3：禁用, 这个状态有问题，貌似是不需要审核
            this.ViewBag.ResearchCount = this._researchService.Count(m => m.Mark > 0 && m.Status == 3); //科研合作总数量 ，状态  1：待审核  2：已取消  3：通过审核   4：拒绝审核

            return View();
        }

        public void GetAreaData()
        {
            try
            {
                var data = new MedicalStatisticsService().GetByAreaData();

                string value = "";

                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        if (item.Name.Equals("内蒙古自治区")) item.Name = "内蒙古";
                        if (item.Name.Equals("广西壮族自治区")) item.Name = "广西";
                        if (item.Name.Equals("宁夏回族自治区")) item.Name = "宁夏";
                        if (item.Name.Equals("新疆维吾尔自治区")) item.Name = "新疆";
                        if (item.Name.Equals("香港特别行政区")) item.Name = "香港";
                        if (item.Name.Equals("澳门特别行政区")) item.Name = "澳门";
                        if (item.Name.Equals("西藏自治区")) item.Name = "西藏";

                        if (item.Name.Contains("省"))
                            item.Name = item.Name.Replace("省", "");

                        value += " { name: '" + item.Name + "', value: " + item.MedicalCount.ToString() + " },";
                    }
                }

                if (!string.IsNullOrWhiteSpace(value))
                    value = value.Trim(',');

                this.ViewBag.AreaData = value;
                this.ViewBag.AreaDataCount = data.OrderByDescending(m => m.MedicalCount).FirstOrDefault().MedicalCount + 10;
            }
            catch (Exception ex)
            {

            }
          
        }

        /// <summary>
        /// 医学数据统计  分离细菌中排名前十位的细菌
        /// </summary>
        /// <returns></returns>
        public void GetTenGerm()
        {
            try
            {

                var data = new MedicalStatisticsService().GetByOrganism();

                string text = "";
                string value = "";

                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        text += string.Format("'{0}',", item.Name);
                        value += item.Ratio + ",";
                    }
                }

                if (!string.IsNullOrWhiteSpace(text))
                    text = text.Trim(',');

                if (!string.IsNullOrWhiteSpace(value))
                    value = value.Trim(',');

                this.ViewBag.TenGermPageText = text;
                this.ViewBag.TenGermPageValue = value;
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 医学数据统计 -- MRSA对抗菌药物的耐药率
        /// MRSA 表示的是金黄色葡萄球菌(Staphylococcus aureus)
        /// </summary>
        /// <param name="provinceId">省份id，需要查询的省份</param>
        /// <returns></returns>
        public void GetMRSASensitivityDrug(long provinceId = 0)
        {
            List<MedicalByAntibioticResult> data = new MedicalStatisticsService().GetByMRSASensitivityDrug(provinceId);

            string text = "";
            string sensitiveValue = ""; //敏感
            string intermediaryValue = ""; //中介
            string resistanceValue = ""; //耐药

            if (data != null && data.Any())
            {
                data = data.OrderBy(m => m.ResistanceCount).ToList();
                foreach (var item in data)
                {
                    text += string.Format("'{0}',", item.AntibioticName);
                    sensitiveValue += item.SensitiveRatio + ",";
                    intermediaryValue += item.IntermediaryRatio + ",";
                    resistanceValue += item.ResistanceRatio + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
                text = text.Trim(',');

            if (!string.IsNullOrWhiteSpace(sensitiveValue))
                sensitiveValue = sensitiveValue.Trim(',');

            if (!string.IsNullOrWhiteSpace(intermediaryValue))
                intermediaryValue = intermediaryValue.Trim(',');

            if (!string.IsNullOrWhiteSpace(resistanceValue))
                resistanceValue = resistanceValue.Trim(',');

            this.ViewBag.MRSAPageText = text;
            this.ViewBag.MRSAPageSensitiveValue = sensitiveValue;
            this.ViewBag.MRSAPageIntermediaryValue = intermediaryValue;
            this.ViewBag.MRSAPageResistanceValue = resistanceValue;
        }


        /// <summary>
        /// 革兰阴性菌菌种分布
        /// </summary>
        /// <returns></returns>
        public void GetGramNegativeBacteria()
        {
            try
            {

                var data = new MedicalStatisticsService().GetByGramNegativeBacteria();

                string text = "";
                string value = "";

                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        text += string.Format("'{0}',", item.Name);
                        value += "{" + string.Format("value: {0}, name: '{1}'", item.DataCount, item.Name) + "},";
                    }
                }

                if (!string.IsNullOrWhiteSpace(text))
                    text = text.Trim(',');

                if (!string.IsNullOrWhiteSpace(value))
                    value = value.Trim(',');

                this.ViewBag.GramNegativePageText = text;
                this.ViewBag.GramNegativePageValue = value;
            }
            catch (Exception)
            {

            }
        }
    }
}