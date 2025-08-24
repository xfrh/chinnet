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
	/// 操作接口类 ，数据库表名：MeetingView 
	/// </summary>
	public  partial interface IMeetingViewService : IBaseService<MeetingView>
	{
        bool Insert(long memberId, string memberName, long meetingId);

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="meetingId">信息动态的id</param>
        /// <param name="meetingName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MeetingView> QueryPage(long meetingId, string meetingName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
