using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule
{
    /// <summary>
    /// 参与评分医院
    /// </summary>
    public partial class DataQuality_Hospital : BaseEntity
    {
        /// <summary>
        /// 关联ID<br />
        /// 关联评比基本信息
        /// </summary>
        public long DataQuality_Score_Id { get; set; }

        /// <summary>
        /// 医院ID<br />
        /// 参与医院的医院ID
        /// </summary>
        public long Hospital_Id { get; set; }

        /// <summary>
        /// 排序<br />
        /// 值越小越靠前<br />
        /// 值必须大于0
        /// </summary>
        public long Sort { get; set; }

        /// <summary>
        /// 参与说明
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 状态值<br />
        /// = 1 表示有效<br />
        /// = 0 表示暂存，评比未提交，也作无效处理<br />
        /// 其他值无效
        /// </summary>
        public int Status { get; set; }
    }
}
