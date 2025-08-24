using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models
{
    public class ClinicalTrialReportModel : BaseEntityModel
    {
        /// <summary>
        /// 菌株编号
        /// </summary>
        public string StrainNo { get; set; }

        /// <summary>
        /// 细菌名称
        /// </summary>
        public string BacterialName { get; set; }

        /// <summary>
        /// 标本类型
        /// </summary>
        public string SpecimenType { get; set; }

        /// <summary>
        /// 实验时间
        /// </summary>
        public string ExperimentTime { get; set; }

        /// <summary>
        /// 药物名称
        /// </summary>
        public string DrugName { get; set; }

        /// <summary>
        /// MIC值
        /// </summary>
        public string MicValue { get; set; }

        /// <summary>
        /// 阅读者
        /// </summary>
        public string Reader { get; set; }

        /// <summary>
        /// 复核MIC值
        /// </summary>
        public string ReviewMicValue { get; set; }

        /// <summary>
        /// 复核者
        /// </summary>
        public string Reviewer { get; set; }

        /// <summary>
        /// 结果表路径
        /// </summary>
        public string PicturePath { get; set; }

        /// <summary>
        /// 所属医院ID
        /// </summary>
        public long HospitalId { get; set; }

        [HtmlDisplayAttribute("医院名称", "医院名称")]
        public IList<SelectListItem> HospitalList { get; set; }

        public string HospitalName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 在药敏板中位置
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 最高浓度
        /// </summary>
        public string HighstPotency { get; set; }
    }

    /// <summary>
    /// 会员中心，医学数据管理的数据封装
    /// </summary>
    [Serializable]
    public class ClinicalTrialReportManageModel
    {
        public IList<SelectListItem> ProjectList { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public PagedList<ClinicalTrialReportModel> PageList { get; set; }
    }
}