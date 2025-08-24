using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Meetings
{
    /// <summary>
    /// 会员中心 信息动态收藏
    /// </summary>
    [Serializable]
    public  class CenterMeetingApplyModel
    {
        /// <summary>
        /// 收藏的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 会议id
        /// </summary>
        public long MeetingId { get; set; }

        /// <summary>
        /// 会议名称
        /// </summary>
        public string MeetingName { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime InsertTime { get; set; }

    }
}