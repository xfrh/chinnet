using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Services.Log;
using ManageSystem.Data;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Core;

namespace ManageSystem.Services.Meetings
{
    /// <summary>
    /// 操作类 ，数据库表名：MeetingCollect 
    /// </summary>
    public partial class MeetingCollectService : BaseService<MeetingCollect>, IMeetingCollectService
    {

        private readonly IMeetingService MeetingService;
        private readonly ISystemLogService SystemLogService;
        public MeetingCollectService(IRepository<MeetingCollect> repository,
                     IMeetingService meetingService,
                     ISystemLogService systemLogService
            ) : base(repository)
        {
            this.MeetingService = meetingService;
            this.SystemLogService = systemLogService;
        }


        public bool Insert(long memberId, string memberName, long meetingId, ref string errorMessage)
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
                    if (meetingEntity == null || meetingEntity.Id <= 0)
                    {
                        errorMessage = "会议不存在";
                        return false;
                    }

                    //重复收藏
                    if (this.Count(m => m.MeetingId == meetingId && m.MemberId == memberId && m.Mark > 0) > 0)
                    {
                        errorMessage = "已经收藏过了";
                        return false;
                    }


                    //插入查看明细表
                    MeetingCollect model = new MeetingCollect()
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
                    meetingEntity.CollectCount += 1;
                    this.MeetingService.Update(meetingEntity);

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert("添加收藏信息动态记录失败", ex.ToString(), Core.Domain.Log.SystemLogLevel.Error);
            }

            errorMessage = "收藏失败，请重试";
            return false;

        }

        public bool Cancel(long memberId, long meetingId, ref string errorMessage)
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
                    if (meetingEntity == null || meetingEntity.Id <= 0)
                    {
                        errorMessage = "会议不存在";
                        return false;
                    }

                    //重复收藏
                    if (this.Count(m => m.MeetingId == meetingId && m.MemberId == memberId && m.Mark > 0) == 0)
                    {
                        errorMessage = "您还没有收藏";
                        return false;
                    }
                    MeetingCollect entity = QueryEntity(r => r.MeetingId == meetingId && r.MemberId == memberId && r.Mark > 0);
                    if (entity != null)
                    {
                        Delete(entity);

                        //修改会议的查看次数
                        meetingEntity.CollectCount -= 1;
                        meetingEntity.CollectCount = meetingEntity.CollectCount > 0 ? meetingEntity.CollectCount : 0;
                        this.MeetingService.Update(meetingEntity);
                    }

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert("需要收藏信息动态记录失败", ex.ToString(), Core.Domain.Log.SystemLogLevel.Error);
            }

            errorMessage = "取消收藏失败，请重试";
            return false;
        }

        /// <summary>
        /// 检查用户的搜藏状态
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <param name="meetingId">会议id</param>
        /// <returns>true 已经搜藏  false  未搜藏</returns>
        public bool MemberCollectStatus(long memberId, long meetingId)
        {
            if (memberId <= 0) return false;
            return (this.Count(m => m.MemberId == memberId && m.MeetingId == meetingId && m.Mark > 0) > 0);
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
        public IPagedList<MeetingCollect> QueryPage(long meetingId, string meetingName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue)
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

            return new PagedList<MeetingCollect>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// 根据会员编号获取所对应的信息动态数据 （会员中心）
        /// </summary>
        /// <param name="memberId">会员编号</param>>
        /// <returns></returns>
        public IQueryable<MeetingCollect> Query(long memberId)
        {
            if (memberId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId).OrderByDescending(m => m.InsertTime);

            return query;
        }
    }
}
