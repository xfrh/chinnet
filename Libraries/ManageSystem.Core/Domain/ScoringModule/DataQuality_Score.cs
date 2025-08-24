using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using ManageSystem.Core.Domain.ScoringModule.Enum;

namespace ManageSystem.Core.Domain.ScoringModule
{
    /// <summary>
    /// 评分模块
    /// </summary>
    public partial class DataQuality_Score : BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public DataQuality_Score_Status Status { get; set; }
    }
}
