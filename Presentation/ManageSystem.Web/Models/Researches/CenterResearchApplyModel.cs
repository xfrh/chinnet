using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Researches
{



    /// <summary>
    /// 会员中心 科研合作申请
    /// </summary>
    [Serializable]
    public class CenterResearchApplyModel
    {
        /// <summary>
        /// 收藏的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 科研合作收藏id
        /// </summary>
        public long ResearchId { get; set; }

        /// <summary>
        /// 科研合作收藏名称
        /// </summary>
        public string ResearchName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime InsertTime { get; set; }

    }
}