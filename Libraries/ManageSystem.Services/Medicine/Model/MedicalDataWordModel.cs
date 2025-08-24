using Aspose.Words;
using ManageSystem.Core.Domain.Medicine.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine.Model
{
    /// <summary>
    ///  医学数据生成word报表的数据封装
    /// </summary>
    public class MedicalDataWordModel
    {
        /// <summary>
        /// 本次生成的医学数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 本次生成的医学数据的明细总数量，根据id查询
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 生成word的 文档对象
        /// </summary>
        public Document Doc { get; set; }

        /// <summary>
        /// 本次上传数据的耐药性分析结果。基础数据，在前面先赋值，后面涉及到耐药性计算的可以都直接使用
        /// </summary>
        public List<GetAntibioticResultModel> AntibioticResultList { get; set; }

    }

    /// <summary>
    ///  医学数据生成word报表的数据封装，葡萄球菌属对抗菌药物的耐药率和敏感率
    /// </summary>
    public class MedicalDataWordMRSAModel
    {

        /// <summary>
        /// MRSA Count
        /// </summary>
        public int MrsaCount { get; set; }

        /// <summary>
        /// MSSA Count
        /// </summary>
        public int MssaCount { get; set; }

        /// <summary>
        /// MRCNS Count
        /// </summary>
        public int MrcnsCount { get; set; }

        /// <summary>
        /// MSCNS Count
        /// </summary>
        public int MscnsCount { get; set; }

        public List<MedicalDataWordMRSAItemModel> List { get; set; }

    }

    /// <summary>
    ///  医学数据生成word报表的数据封装，葡萄球菌属对抗菌药物的耐药率和敏感率
    /// </summary>
    public class MedicalDataWordMRSAItemModel
    {

        public long OrganismId { get; set; }
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
        /// 抗生素数量
        /// </summary>
        public int AntibioticCount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        public int MRSAAntibioticCount { get; set; }

        public string MRSAR { get; set; }

        public string MRSAS { get; set; }
        public int MSSAAntibioticCount { get; set; }

        public string MSSAR { get; set; }

        public string MSSAS { get; set; }
        public int MRCNSAntibioticCount { get; set; }

        public string MRCNSR { get; set; }

        public string MRCNSS { get; set; }
        public int MSCNSAntibioticCount { get; set; }

        public string MSCNSR { get; set; }

        public string MSCNSS { get; set; }
    }
}
