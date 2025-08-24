using ManageSystem.Core.Domain.Members;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Meetings
{
    /// <summary>
    /// 会议详细页面的实体封装
    /// </summary>
    public class MeetingDetailModel
    {
        /// <summary>
        /// 会议数据
        /// </summary>
        public MeetingModel MeetingEntity { get; set; }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public Member MemberEntity { get; set; }

        /// <summary>
        /// 申请用户列表
        /// </summary>
        public List<MemberModel> ApplyMemberList { get; set; }

        /// <summary>
        /// 是否已经报名了
        /// </summary>
        public bool IsApply { get; set; }

        /// <summary>
        /// 是否已经搜藏过了
        /// </summary>
        public bool IsCollect { get; set; }

    }

    /// <summary>
    /// 会议详细页面的用户申请数据
    /// </summary>
    public class ApplyMemberModel
    {
        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string MemberNickName { get; set; }

        /// <summary>
        /// 用户的id
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 用户的头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string TimeValue { get; set; }
    }
}