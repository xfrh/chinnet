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
	/// 操作接口类 ，数据库表名：Meeting 
	/// </summary>
	public  partial interface IMeetingService : IBaseService<Meeting>
	{

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="name">会议主题</param>
        /// <param name="meetingType">所属分类</param>
        /// <param name="type">会议类型</param>
        /// <param name="memberName">发布人</param>
        /// <param name="contact">联系方式</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Meeting> QueryPage(string name,long meetingType , int type , string memberName ,string contact , string startTime, string endTime, int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<Meeting> QueryPageSa(string name, string said, long meetingType, int type, string memberName, string contact, string startTime, string endTime, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 分页查询  网页
        /// </summary>
        /// <param name="typeId">会议类型</param>
        /// <param name="areaId">所属区域（省份）</param>
        /// <param name="day">天查询条件</param>
        /// <param name="price">收费条件</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Meeting> Query(long typeId, long areaId, int day, int price);



        /// <summary>
        /// 会员中心 分页查询  网页  
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        IQueryable<Meeting> Query(long memberId);

        /// <summary>
        ///查询热门的会议，网页端
        /// </summary>
        /// <returns></returns>
        List<Meeting> QueryHotMeeting();

    }
}
