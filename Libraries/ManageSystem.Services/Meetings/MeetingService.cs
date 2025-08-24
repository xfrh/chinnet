using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.Meetings
{
    /// <summary>
    /// 操作类 ，数据库表名：Meeting 
    /// </summary>
    public partial class MeetingService : BaseService<Meeting>, IMeetingService
    {

        public MeetingService(IRepository<Meeting> repository) : base(repository)
        {

        }

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
        public IQueryable<Meeting> Query(long typeId, long areaId, int day, int price)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0 && m.Status == (int)MeetingStatusEnum.Finish);

            if (typeId > 0)
                query = query.Where(m => m.MeetingTypeId == typeId);

            if (areaId > 0)
                query = query.Where(m => m.AreaProvinceId == areaId);

            if (day > 0)
            {
                DateTime endTime = DateTime.Now;
                switch (day)
                {
                    case 1:
                        //今天
                        DateTime startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
                        query = query.Where(m => m.StartTime >= startTime && m.StartTime <= endTime);

                        break;
                    case 2:
                        //近一周
                        startTime = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00"));
                        query = query.Where(m => m.StartTime >= startTime && m.StartTime <= endTime);

                        break;
                    case 3:
                        //近一月
                        startTime = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00"));
                        query = query.Where(m => m.StartTime >= startTime && m.StartTime <= endTime);
                        break;
                }
            }

            if (price > 0)
            {
                if (price == 1) //收费
                    query = query.Where(m => m.Type == (int)MeetingTypeEnum.Charge);

                else if (price == 2) //免费
                    query = query.Where(m => m.Type == (int)MeetingTypeEnum.Free);
            }

            query = query.OrderByDescending(m => m.StartTime).OrderByDescending(m => m.InsertTime);

            return query;

        }

        public List<Meeting> QueryHotMeeting()
        {
            var query = this._repository.Table.Where(m => m.Mark > 0 && m.StartTime <= DateTime.Now && m.EndTime >= DateTime.Now)
                                                            .OrderByDescending(m => m.ApplyCount).Take(10);
            return query.ToList();
        }

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
        public IPagedList<Meeting> QueryPage(string name, long meetingType, int type, string memberName, string contact, string startTime, string endTime, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (meetingType > 0)
                query = query.Where(m => m.MeetingTypeId == meetingType);

            if (type > 0)
                query = query.Where(m => m.Type == type);

            if (!string.IsNullOrWhiteSpace(memberName))
                query = query.Where(m => m.Author.Contains(memberName.Trim()));

            if (!string.IsNullOrWhiteSpace(contact))
                query = query.Where(m => m.Contact.Contains(contact.Trim()));

            if (!string.IsNullOrWhiteSpace(startTime) && DateHelper.IsDateTime(startTime))
            {
                DateTime startTimeTemp = DateTime.Parse(startTime);
                query = query.Where(m => m.StartTime >= startTimeTemp);
            }

            if (!string.IsNullOrWhiteSpace(endTime) && DateHelper.IsDateTime(endTime))
            {
                DateTime endTimeTemp = DateTime.Parse(endTime);
                query = query.Where(m => m.StartTime <= endTimeTemp);
            }


            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Meeting>(query, pageIndex, pageSize);
        }

        public IPagedList<Meeting> QueryPageSa(string name, string said,long meetingType, int type, string memberName, string contact, string startTime, string endTime, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (meetingType > 0)
                query = query.Where(m => m.MeetingTypeId == meetingType);

            if (type > 0)
                query = query.Where(m => m.Type == type);

            if (!string.IsNullOrWhiteSpace(memberName))
                query = query.Where(m => m.Author.Contains(memberName.Trim()));

            if (!string.IsNullOrWhiteSpace(contact))
                query = query.Where(m => m.Contact.Contains(contact.Trim()));

            if (!string.IsNullOrWhiteSpace(startTime) && DateHelper.IsDateTime(startTime))
            {
                DateTime startTimeTemp = DateTime.Parse(startTime);
                query = query.Where(m => m.StartTime >= startTimeTemp);
            }

            if (!string.IsNullOrWhiteSpace(endTime) && DateHelper.IsDateTime(endTime))
            {
                DateTime endTimeTemp = DateTime.Parse(endTime);
                query = query.Where(m => m.StartTime <= endTimeTemp);
            }

            query = query.Where(m => m.Describe.Contains(said));


            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Meeting>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 会员中心 分页查询  网页  
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        public IQueryable<Meeting> Query(long memberId)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (memberId > 0)
                query = query.Where(m => m.MemberId == memberId);

            query = query.OrderByDescending(m => m.InsertTime);

            return query;
        }


    }
}
