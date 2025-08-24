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
    /// 操作接口类 ，数据库表名：MeetingCollect 
    /// </summary>
    public partial interface IMeetingCollectService : IBaseService<MeetingCollect>
    {
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="memberName"></param>
        /// <param name="meetingId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool Insert(long memberId, string memberName, long meetingId, ref string errorMessage);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="meetingId"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool Cancel(long memberId, long meetingId, ref string errorMessage);
        /// <summary>
        /// 检查用户的搜藏状态
        /// </summary>
        /// <param name="meetingId">会议id</param>
        /// <param name="researchId">科研id</param>
        /// <returns>true 已经搜藏  false  未搜藏</returns>
        bool MemberCollectStatus(long memberId, long meetingId);


        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="meetingId">信息动态的id</param>
        /// <param name="meetingName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MeetingCollect> QueryPage(long meetingId, string meetingName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 根据会员编号获取所对应的信息动态数据  （会员中心）
        /// </summary>
        /// <param name="memberId">会员编号</param>
        /// <returns></returns>
        IQueryable<MeetingCollect> Query(long memberId);

    }
}
