using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule
{
    /// <summary>
    /// 重评申请
    /// </summary>
    public partial class DataQuality_Reevaluation_Apply : BaseEntity
    {
        /// <summary>
        /// 关联id
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
        /// 关联id
        /// </summary>
        public long DataQuality_Score_Record_Id { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public double Evaluate_Score { get; set; }
        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 申请状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 处理者
        /// </summary>
        public long? Handler_Id { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? Handle_Time { get; set; }
        /// <summary>
        /// 处理说明
        /// </summary>
        public string Handle_Remark { get; set; }
        /// <summary>
        /// 重评是否完成<br />
        /// 0 = 申请重评且未评分<br />
        /// 1 = 申请重评且已完成评分<br />
        /// 其他值无效
        /// </summary>
        public int ActionStatus { get; set; }
    }
}
