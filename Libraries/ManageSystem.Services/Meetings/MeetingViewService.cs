using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Data;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Services.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Core;

namespace ManageSystem.Services.Meetings
{
    /// <summary>
    /// 操作类 ，数据库表名：MeetingView 
    /// </summary>
    public partial class MeetingViewService : BaseService<MeetingView>, IMeetingViewService
    {
        private readonly IMeetingService MeetingService;
        private readonly ISystemLogService SystemLogService;
        public MeetingViewService(IRepository<MeetingView> repository,
                     IMeetingService meetingService,
                     ISystemLogService systemLogService
            ) : base(repository)
        {
            this.MeetingService = meetingService;
            this.SystemLogService = systemLogService;
        }

        public bool Insert(long memberId, string memberName, long meetingId)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查信息动态数据
                    Meeting meetingEntity = this.MeetingService.QueryEntity(meetingId);
                    if (meetingEntity == null || meetingEntity.Id <= 0) throw new Exception("会议不存在");

                    //插入查看明细表
                    MeetingView model = new MeetingView()
                    {
                        Ip = HttpHelper.GetIp(),
                        BrowserName = HttpHelper.GetBrowserName(),
                        MeetingId = meetingEntity.Id,
                        MeetingName = meetingEntity.Name,
                        MemberName = memberName,
                        MemberId = memberId
                    };
                    base.Insert(model);

                    //修改会议的查看次数
                    meetingEntity.ViewCount += 1;
                    this.MeetingService.Update(meetingEntity);

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert("添加查看信息动态记录失败", ex.ToString(), Core.Domain.Log.SystemLogLevel.Error);
            }

            return false;

        }

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="meetingId">信息动态的id</param>
        /// <param name="meetingName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<MeetingView> QueryPage(long meetingId, string meetingName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (meetingId > 0)
                query = query.Where(m => m.MeetingId == meetingId);

            if (!string.IsNullOrWhiteSpace(meetingName))
            {
                List<long> ids = this.MeetingService.Query(m => m.Name.Contains(meetingName)).Select(m => m.Id).ToList();
              
                if (ids != null && ids.Any())
                {
                    query = query.Where(m => ids.Contains(m.MeetingId));
                }
                else
                {
                    query = query.Where(m => m.Id == 0);
                }

            }

            if (!string.IsNullOrWhiteSpace(memberName))
                query = query.Where(m => m.MemberName.Equals(memberName.Trim()));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<MeetingView>(query, pageIndex, pageSize);
        }
    }
}
