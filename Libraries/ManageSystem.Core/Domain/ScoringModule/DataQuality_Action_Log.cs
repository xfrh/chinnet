using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule.Log
{
    /// <summary>
    /// 评分操作日志
    /// </summary>
    public class DataQuality_Action_Log : BaseEntity
    {
        /// <summary>
        /// 关联id
        /// </summary>
        public long DataQuality_Score_Id { get; set; }
        /// <summary>
        /// 操作类型<br />
        /// 新增<br />
        /// 编辑<br />
        /// 删除<br />
        /// 评分
        /// </summary>
        public string Action_Type { get; set; }
        /// <summary>
        /// 操作员关联id
        /// </summary>
        public long Operator_Id { get; set; }
        /// <summary>
        /// 操作日志
        /// </summary>
        public string Action_Log { get; set; }
        /// <summary>
        /// 操作日志详细
        /// </summary>
        public string Action_Log_Detail { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Action_Time { get; set; }
        /// <summary>
        /// 平台<br />
        /// 见枚举：ManageSystem.Core.Domain.Log.ActionSource
        /// </summary>
        public int Platform { get; set; }
    }
}
