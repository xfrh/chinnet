using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Datas
{
    /// <summary>
    /// 实体类 ，数据库表名：DataAntibioticDrugFast 
    /// </summary>
    public partial class DataAntibioticDrugFast : BaseEntity
	{
        /// <summary>
        /// 抗生素Id
        /// <summary>
        public long AntibioticId { get; set; }

        /// <summary>
        /// 抗生素名称
        /// <summary>
        public String AntibioticName { get; set; }

        /// <summary>
        /// 抗生素编码
        /// <summary>
        public String AntibioticCode { get; set; }

        /// <summary>
        /// 细菌Id
        /// <summary>
        public long GermId { get; set; }

        /// <summary>
        /// 细菌名称
        /// <summary>
        public String GermName { get; set; }

        /// <summary>
        /// 细菌编码
        /// <summary>
        public String GermCode { get; set; }

        /// <summary>
        /// 耐药性
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public int Sort { get; set; }

    }
}
