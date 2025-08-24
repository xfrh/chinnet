using ManageSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Meetings
{
    /// <summary>
    /// 会议管理的查询数据实体
    /// </summary>
    public class MeetingSeachModel
    {

        /// <summary>
        /// 开始时间（用于列表页面的查询）
        /// <summary>
        [HtmlDisplayAttribute("开始时间", "关注时间的开始时间")]
        public string StartTime { get; set; }


        /// <summary>
        /// 结束时间（用于列表页面的查询）
        /// <summary>
        [HtmlDisplayAttribute("结束时间", "关注时间的结束时间")]
        public string EndTime { get; set; }

        /// <summary>
        /// 会议类型id
        /// <summary>
        [HtmlDisplayAttribute("会议类型", "会议类型")]
        public long MeetingTypeId { get; set; }


        /// <summary>
        /// 所有会议类型
        /// </summary>
        public IList<SelectListItem> MeetingTypeList { get; set; }

        /// <summary>
        /// 会议主题
        /// <summary>
        [HtmlDisplayAttribute("会议主题", "会议主题", true)]
        public String Name { get; set; }


        /// <summary>
        /// 发布用户的姓名
        /// <summary>
        [HtmlDisplayAttribute("发布人", "发布人")]
        public String MemberName { get; set; }

        /// <summary>
        /// 联系方式
        /// <summary>
        [HtmlDisplayAttribute("联系方式", "联系方式")]
        public String Contact { get; set; }

        /// <summary>
        /// 活动类型，1：收费活动  2：免费活动
        /// <summary>
        [HtmlDisplayAttribute("活动类型", "活动类型")]
        public Int32 Type { get; set; }

        /// <summary>
        /// 帐号状态枚举列表
        /// </summary>
        [HtmlDisplayAttribute("活动类型", "活动类型")]
        public IList<SelectListItem> TypeEnumList { get; set; }
    }
}