using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Meetings
{
    /// <summary>
    /// 会议首页查询条件
    /// </summary>
    public class IndexSearchModel
    {
        /// <summary>
        /// 会议分类
        /// </summary>
        public long Type { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public long Area { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 分页，页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页，每页数量
        /// </summary>
        public int PageSize { get; set; }

    }
}