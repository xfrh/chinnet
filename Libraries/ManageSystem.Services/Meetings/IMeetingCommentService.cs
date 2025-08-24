using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Meetings;

namespace ManageSystem.Services.Meetings
{
	/// <summary>
	/// 操作接口类 ，数据库表名：MeetingComment 
	/// </summary>
	public  partial interface IMeetingCommentService : IBaseService<MeetingComment>
	{
        bool Insert( MeetingComment modelComment, ref string errorMessage);

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="meetingId">信息动态的id</param>
        /// <param name="meetingName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MeetingComment> QueryPage(long meetingId, string meetingName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 根据会员编号获取所对应的信息动态的评论数据  （会员中心）
        /// </summary>
        /// <param name="meetingId">信息动态编号</param>
        /// <returns></returns>
        IQueryable<MeetingComment> QueryByMeetingId(long meetingId);

    }
}
