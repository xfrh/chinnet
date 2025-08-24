using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule
{
    /// <summary>
    /// 评分记录
    /// </summary>
    public partial class DataQuality_Score_Record : BaseEntity
    {
        /// <summary>
        /// 关联ID
        /// </summary>
        public long DataQuality_Score_Id { get; set; }
        /// <summary>
        /// 关联id
        /// </summary>
        public long DataQuality_Hospital_Id { get; set; }
        /// <summary>
        /// 关联id
        /// </summary>
        public long DataQuality_Jury_Id { get; set; }
        /// <summary>
        /// 总评分
        /// </summary>
        public double Evaluate_Score { get; set; }
        /// <summary>
        /// 评语
        /// </summary>
        public string Evaluate_Remark { get; set; }
        /// <summary>
        /// 评价时间
        /// </summary>
        public DateTime Evaluate_Time { get; set; }
        /// <summary>
        /// 请求一致性验证参数
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 请求一致性验证参数
        /// </summary>
        public string Timestamp { get; set; }
        /// <summary>
        /// 请求一致性验证参数
        /// </summary>
        public string Nonce { get; set; }
    }
}
