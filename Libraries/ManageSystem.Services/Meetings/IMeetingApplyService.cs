using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Services.Meetings
{
	/// <summary>
	/// 操作接口类 ，数据库表名：MeetingApply 
	/// </summary>
	public  partial interface IMeetingApplyService : IBaseService<MeetingApply>
	{
        /// <summary>
        /// 用户申请参与会议
        /// </summary>
        /// <param name="meetingId">会议id</param>
        /// <param name="userName">用户姓名</param>
        /// <param name="userPhone">用户手机</param>
        /// <param name="userEmail">用户邮箱</param>
        /// <param name="member">当前登录用户</param>
        /// <returns></returns>
        MeetingApply Insert(long meetingId, string userName, string userPhone, string userEmail, Member member);

        /// <summary>
        /// 检查用户的申请状态
        /// </summary>
        /// <param name="meetingId">会议id</param>
        /// <param name="researchId">科研id</param>
        /// <returns>true 已经申请  false  未申请</returns>
        bool MemberApplyStatus(long memberId, long meetingId);

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="meetingId">信息动态的id</param>
        /// <param name="name">报名人姓名</param>
        /// <param name="phone">报名人电话</param>
        /// <param name="email">报名人邮箱</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MeetingApply> QueryPage(long meetingId, string meetingName, string name, string phone, string email,  int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 根据会员编号获取所对应的信息动态数据  （会员中心）
        /// </summary>
        /// <param name="memberId">会员编号</param>
        /// <returns></returns>
        IQueryable<MeetingApply> Query(long memberId);

        /// <summary>
        /// 根据会员编号获取所对应的信息动态数据  （会员中心）
        /// </summary>
        /// <param name="meetingId">信息动态编号</param>
        /// <param name="searchKey">查询关键字</param>
        /// <param name="status">报名申请状态</param>
        /// <returns></returns>
        IQueryable<MeetingApply> QueryByMeetingId(long meetingId,String searchKey,int status);


        /// <summary>
        /// 用户取消申请
        /// </summary>
        /// <param name="memberId">会员编号</param>
        /// <param name="meetingApplyId">信息动态申请的Id</param>
        /// <param name="memberName">操作人姓名，格式：姓名（登录帐号）</param>
        /// <returns></returns>
        bool Cancel(long memberId, long meetingApplyId,string memberName);

    }
}
