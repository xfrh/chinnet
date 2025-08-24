using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Members
{
    /// <summary>
    /// 会员邀请的记录
    /// </summary>
    public class InviteModel
    {
    }

    /// <summary>
    ///  会员中心，邀请会员的列表数据
    /// </summary>
    public class CenterInviteListModel
    {
        /// <summary>
        /// 被邀请的会员id
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 被邀请的会员帐号
        /// </summary>
        public string MemberLoginId { get; set; }

        /// <summary>
        /// 被邀请的会员姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 注册的时间
        /// </summary>
        public DateTime InsertTime { get; set; }

    }



    /// <summary>
    /// 会员中心，邀请会员
    /// </summary>
    public class CenterInviteModel
    {
        /// <summary>
        /// 用户的邀请码
        /// </summary>
        public string InviteCode { get; set; }

        /// <summary>
        /// 给用户产生的邀请地址
        /// </summary>
        public string InviteUrl { get; set; }


    }

}