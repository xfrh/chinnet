using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Ctr
{
    public partial class ClinicalTrialReport : BaseEntity
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

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        public string HospitalName { get; set; }

        /// <summary>
        /// 在药敏板中位置
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 最高浓度
        /// </summary>
        public string HighstPotency { get; set; }
    }
}
