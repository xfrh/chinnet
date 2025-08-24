
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Meetings
{
    /// <summary>
    /// 会员中心 信息动态管理  会议的报名申请记录
    /// </summary>
    [Serializable]
    public  class MeetingDetailApplyModel
    {
        /// <summary>
        /// 搜索状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public String SearchKey { get; set; }

        /// <summary>
        /// 信息动态数据
        /// </summary>
        public MeetingModel Meeting{ get; set; }

        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public PagedList<MeetingApplyModel> ApplyPageList { get; set; }


    }
}