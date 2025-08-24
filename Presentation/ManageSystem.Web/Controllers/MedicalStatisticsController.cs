using ManageSystem.Core.Domain.Medicine.Statistics;
using ManageSystem.Services.Medicine.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 
    /// 医学统计
    /// 
    /// </summary>
    public class MedicalStatisticsController : Controller
    {

        #region 数据统计报表

        /// <summary>
        /// 医学数据统计    按省份统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Area()
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
            
            this.ViewBag.PageValue = value;
            this.ViewBag.PageMaxValue = data.OrderByDescending(m => m.MedicalCount).FirstOrDefault().MedicalCount + 10;

            return View();
        }

        /// <summary>
        /// 医学数据统计    按年份统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Year()
        {
            var data = new MedicalStatisticsService().GetByYearData();

            string text = "";
            string value = "";

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    text += string.Format("'{0}',", item.Year);
                    value += item.MedicalCount + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
                text = text.Trim(',');

            if (!string.IsNullOrWhiteSpace(value))
                value = value.Trim(',');

            this.ViewBag.PageText = text;
            this.ViewBag.PageValue = value;

            return View();
        }

        /// <summary>
        /// 医学数据统计    按标本类型统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Specimen()
        {
            var data = new MedicalStatisticsService().GetBySpecimenData();

            string text = "";
            string value = "";

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    text += string.Format("'{0}',", item.SpecimenName);
                    value += item.MedicalCount + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
                text = text.Trim(',');

            if (!string.IsNullOrWhiteSpace(value))
                value = value.Trim(',');

            this.ViewBag.PageText = text;
            this.ViewBag.PageValue = value;

            return View();
        }

        /// <summary>
        /// 医学数据统计    按年份和季度统计
        /// </summary>
        /// <returns></returns>
        public ActionResult YearAndQuarter(int year = 0)
        {
            if (year <= 0) year = DateTime.Now.Year;

            var data = new MedicalStatisticsService().GetByYearAndQuarter(year);

            string text = "['第一季度（1月-3月）', '第二季度（4月-6月）', '第三季度（7月-9月）', '第四季度（10月-12月）']";
            string value = "";

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    string temp = "";
                    switch (item.Quarter)
                    {
                        case 1:
                            temp = "第一季度（1月-3月）";
                            break;
                        case 2:
                            temp = "第二季度（4月-6月）";
                            break;
                        case 3:
                            temp = "第三季度（7月-9月）";
                            break;
                        case 4:
                            temp = "第四季度（10月-12月）";
                            break;
                    }
                    value += "{ value:" + item.MedicalCount.ToString() + ", name: '" + temp + "' },";
                }
            }

            if (!string.IsNullOrWhiteSpace(value))
                value = value.Trim(',');
            else {
                value = "{value:0, name:'第一季度（1月-3月）'},{value:0, name:'第二季度（4月-6月）'},{value:0, name:'第三季度（7月-9月）'},{value:0, name:'第四季度（10月-12月）'}";
            }

            this.ViewBag.PageText = text;
            this.ViewBag.PageValue = value;

            return View();
        }

        /// <summary>
        /// 医学数据统计  分离细菌中排名前十位的细菌
        /// </summary>
        /// <returns></returns>
        public ActionResult TenGerm()
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

            this.ViewBag.PageText = text;
            this.ViewBag.PageValue = value;

            return View();
        }

        /// <summary>
        /// 革兰阴性菌菌种分布
        /// </summary>
        /// <returns></returns>
        public ActionResult GramNegativeBacteria()
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

            this.ViewBag.PageText = text;
            this.ViewBag.PageValue = value;

            return View();
        }

        /// <summary>
        /// 革兰阳性菌菌种分布
        /// </summary>
        /// <returns></returns>
        public ActionResult GramPositiveBacteria()
        {
            var data = new MedicalStatisticsService().GetByGramPositiveBacteria();

            string text = "";
            string value = "";

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    text += string.Format("'{0}',", item.Name);
                    value += "{"+string.Format("value: {0}, name: '{1}'",item.DataCount, item.Name)+ "},";
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
                text = text.Trim(',');

            if (!string.IsNullOrWhiteSpace(value))
                value = value.Trim(',');

            this.ViewBag.PageText = text;
            this.ViewBag.PageValue = value;

            return View();
        }

        /// <summary>
        /// 细菌在标本中占比
        /// </summary>
        /// <returns></returns>
        public ActionResult BacteriaInSpecimen()
        {
            var data = new MedicalStatisticsService().GetByBacteriaInSpecimen();

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

            this.ViewBag.PageText = text;
            this.ViewBag.PageValue = value;

            return View();
        }

        /// <summary>
        /// 医学数据统计 -- MRSA对抗菌药物的耐药率
        /// MRSA 表示的是金黄色葡萄球菌(Staphylococcus aureus)
        /// </summary>
        /// <param name="provinceId">省份id，需要查询的省份</param>
        /// <returns></returns>
        public ActionResult MRSASensitivityDrug(long provinceId = 0)
        {
            List< MedicalByAntibioticResult > data = new MedicalStatisticsService().GetByMRSASensitivityDrug(provinceId);

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

            this.ViewBag.PageText = text;
            this.ViewBag.PageSensitiveValue = sensitiveValue;
            this.ViewBag.PageIntermediaryValue = intermediaryValue;
            this.ViewBag.PageResistanceValue = resistanceValue;

            return this.View();
        }



        /// <summary>
        /// 上海
        /// 医学数据统计 -- MRSA对抗菌药物的耐药率
        /// MRSA 表示的是金黄色葡萄球菌(Staphylococcus aureus)
        /// </summary>
        /// <returns></returns>
        public ActionResult MRSASensitivityDrugSH()
        {
            List<MedicalByAntibioticResult> data = new MedicalStatisticsService().GetByMRSASensitivityDrug();

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

            this.ViewBag.PageText = text;
            this.ViewBag.PageSensitiveValue = sensitiveValue;
            this.ViewBag.PageIntermediaryValue = intermediaryValue;
            this.ViewBag.PageResistanceValue = resistanceValue;

            return this.View();
        }

        /// <summary>
        /// 医学数据统计 --  MRCNS对抗菌药物的耐药率
        ///MRCNS 表示的是溶血葡萄球菌(Staphylococcus haemolyticus)
        /// </summary>
        /// <returns></returns>
        public ActionResult MRCNSSensitivityDrug()
        {
            List<MedicalByAntibioticResult> data = new MedicalStatisticsService().GetByMRCNSSensitivityDrug();

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

            this.ViewBag.PageText = text;
            this.ViewBag.PageSensitiveValue = sensitiveValue;
            this.ViewBag.PageIntermediaryValue = intermediaryValue;
            this.ViewBag.PageResistanceValue = resistanceValue;

            return this.View();
        }

        /// <summary>
        /// 医学数据统计 --  MSSA对抗菌药物的耐药率
        /// MRCNS 表示的是  无
        /// </summary>
        /// <returns></returns>
        public ActionResult MSSASensitivityDrug()
        {
            List<MedicalByAntibioticResult> data = new MedicalStatisticsService().GetByMSSASensitivityDrug();

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

            this.ViewBag.PageText = text;
            this.ViewBag.PageSensitiveValue = sensitiveValue;
            this.ViewBag.PageIntermediaryValue = intermediaryValue;
            this.ViewBag.PageResistanceValue = resistanceValue;

            return this.View();
        }

        /// <summary>
        /// 医学数据统计 --  大肠埃希菌对抗耐药率
        /// </summary>
        /// <returns></returns>
        public ActionResult EscherichiaColiSensitivityDrug()
        {
            List<MedicalByAntibioticResult> data = new MedicalStatisticsService().GetByEscherichiaColiSensitivityDrug();

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

            this.ViewBag.PageText = text;
            this.ViewBag.PageSensitiveValue = sensitiveValue;
            this.ViewBag.PageIntermediaryValue = intermediaryValue;
            this.ViewBag.PageResistanceValue = resistanceValue;

            return this.View();
        }


        #endregion


        public ActionResult GetCount()
        {
            new MedicalStatisticsService().GetAntibioticResultCount();
            return this.View();
        }
      

    }
}