using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.CRs
{
    public partial class CRItem: BaseEntity
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

        /// <summary>
        /// 标本来源
        /// </summary>
        public string Specimensource { get; set; }

    }
}
