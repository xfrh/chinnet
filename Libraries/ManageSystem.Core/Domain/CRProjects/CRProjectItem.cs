using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.CRProjects
{
    /// <summary>
    /// 实体类 ，数据库表名：MedicalDataItem 
    /// </summary>
    public partial class CRProjectItem : BaseEntity
    {
        /// <summary>
        /// 编号
        /// <summary>
        private String number = "";
        /// <summary>
        /// 编号
        /// <summary>
        public String Number { set { number = value; } get { return number; } }

        /// <summary>
        /// 主表管理
        /// <summary>
        private long projectId = 0L;
        /// <summary>
        /// 主表管理
        /// <summary>
        public long ProjectId { set { projectId = value; } get { return projectId; } }

        /// <summary>
        /// 细菌名称
        /// <summary>
        private String name = "";
        /// <summary>
        /// 细菌名称
        /// <summary>
        public String Name { set { name = value; } get { return name; } }

        /// <summary>
        /// 亚胺培南MIC值(ug/ml)
        /// </summary>
        public string MIC_Imipenem { get; set; } = null;

        /// <summary>
        /// 原始亚胺培南抑菌圈直径数值(mm)
        /// <summary>
        public string ImineNumber { get; set; } = null;

        /// <summary>
        /// 替加环素纸片法抑菌圈直径数值(mm)
        /// <summary>
        public string TegacyclineNumber { get; set; }

        /// <summary>
        /// 替加环素纸片法复敏抑菌圈直径数值(mm)
        /// <summary>
        public string BacteriostasisNumber { get; set; }

        /// <summary>
        /// 肉汤法复核抑菌数值(ug/ml)
        /// <summary>
        public string RecheckNumber { get; set; }

    }
}
