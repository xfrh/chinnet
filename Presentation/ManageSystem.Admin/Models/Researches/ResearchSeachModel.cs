using ManageSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Researches
{
    /// <summary>
    /// 科研合作的查询数据实体
    /// </summary>
    public class ResearchSeachModel
    {

        /// <summary>
        /// 开始时间（用于列表页面的查询）
        /// <summary>
        [HtmlDisplayAttribute("科研时间", "科研开始时间")]
        public string StartTime { get; set; }


        /// <summary>
        /// 结束时间（用于列表页面的查询）
        /// <summary>
        [HtmlDisplayAttribute("结束时间", "科研结束时间")]
        public string EndTime { get; set; }

        /// <summary>
        /// 会议类型id
        /// <summary>
        [HtmlDisplayAttribute("会议类型", "会议类型")]
        public long ResearchTypeId { get; set; }


        /// <summary>
        /// 所有会议类型
        /// </summary>
        public IList<SelectListItem> ResearchTypeList { get; set; }

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
        /// 帐号状态枚举列表
        /// </summary>
        [HtmlDisplayAttribute("活动类型", "活动类型")]
        public IList<SelectListItem> TypeEnumList { get; set; }
    }
}