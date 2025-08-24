using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine.Statistics
{
    /// <summary>
    /// 查询细菌敏感值统计报表
    /// </summary>
    public class MedicalByAntibioticResult
    {
        /// <summary>
        /// 字段名称，也就是抗生素对应的导入列名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 细菌Id
        /// </summary>
        public long OrganismId { get; set; }

        /// <summary>
        /// 细菌名称
        /// </summary>
        public string OrganismName { get; set; }

        /// <summary>
        /// 细菌编码
        /// </summary>
        public string OrganismCode { get; set; }

        /// <summary>
        /// 抗生素Id
        /// </summary>
        public long AntibioticId { get; set; }

        /// <summary>
        /// 抗生素名称
        /// </summary>
        public string AntibioticName { get; set; }

        /// <summary>
        /// 抗生素编码
        /// </summary>
        public string AntibioticCode { get; set; }

        /// <summary>
        /// 敏感数量 + 中介数量+耐药数量  = 总数量
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 敏感数量
        /// </summary>
        public int SensitiveCount { get; set; }

        /// <summary>
        /// 敏感数量所占比， DataCount/SensitiveCount=SensitiveRatio
        /// </summary>
        public Decimal SensitiveRatio { get; set; }

        /// <summary>
        /// 中介数量
        /// </summary>
        public int IntermediaryCount { get; set; }

        /// <summary>
        /// 中介数量所占比， DataCount/IntermediaryCount=IntermediaryRatio
        /// </summary>
        public Decimal IntermediaryRatio { get; set; }

        /// <summary>
        /// 耐药数量
        /// </summary>
        public int ResistanceCount { get; set; }

        /// <summary>
        /// 耐药数量 所占比， DataCount/ResistanceCount=ResistanceRatio
        /// </summary>
        public Decimal ResistanceRatio { get; set; }


    }
}
