using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Data;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using ManageSystem.Services.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Core;

namespace ManageSystem.Services.Meetings
{
    /// <summary>
    /// 操作类 ，数据库表名：MeetingComment 
    /// </summary>
    public partial class MeetingCommentService : BaseService<MeetingComment>, IMeetingCommentService
    {

        private readonly IMeetingService MeetingService;
        private readonly ISystemLogService SystemLogService;
        public MeetingCommentService(IRepository<MeetingComment> repository,
                     IMeetingService meetingService,
                     ISystemLogService systemLogService
            ) : base(repository)
        {
            this.MeetingService = meetingService;
            this.SystemLogService = systemLogService;
        }

        public bool Insert(MeetingComment modelComment, ref string errorMessage)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查信息动态数据
                    Meeting meetingEntity = this.MeetingService.QueryEntity(modelComment.MeetingId);
                    if (meetingEntity == null || meetingEntity.Id <= 0)
                    {
                        errorMessage = "会议不存在";
                        return false;
                    }

                    //上级评论
                    int level = 0;
                    if (modelComment.ParentId > 0)
                    {
                        var parentComment = this.QueryEntity(modelComment.ParentId);
                        if (parentComment != null && parentComment.Id > 0)
                        {
                            level = parentComment.Level + 1;
                            parentComment.CommentCount += 1;
                            this.Update(parentComment);
                        }
                    }

                    if (modelComment.MemberId == meetingEntity.MemberId)
                        modelComment.Type = (int)MeetingCommentTypeEnum.Author;

                    //插入查看明细表
                    MeetingComment model = new MeetingComment()
                    {
                        Ip = HttpHelper.GetIp(),
                        BrowserName = HttpHelper.GetBrowserName(),
                        MeetingId = meetingEntity.Id,
                        MeetingName = meetingEntity.Name,
                        MemberName = modelComment.MemberName,
                        MemberId = modelComment.MemberId,
                        ParentId = modelComment.ParentId,
                        Content = modelComment.Content,
                        Type = modelComment.Type,
                        CommentCount = 0,
                       Level= level
                    };
                    base.Insert(model);

                    //修改会议的评论次数
                    meetingEntity.CommentCount += 1;
                    this.MeetingService.Update(meetingEntity);

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert("添加信息动态评论记录失败", ex.ToString(), Core.Domain.Log.SystemLogLevel.Error);
            }

            errorMessage = "评论失败，请重试";
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
        public IPagedList<MeetingComment> QueryPage(long meetingId, string meetingName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue)
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

            return new PagedList<MeetingComment>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// 根据会员编号获取所对应的信息动态的评论数据  （会员中心）
        /// </summary>
        /// <param name="meetingId">信息动态编号</param>
        /// <returns></returns>
        public IQueryable<MeetingComment> QueryByMeetingId(long meetingId)
        {
            if (meetingId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.ParentId==0 && m.MeetingId == meetingId);

            query = query.OrderByDescending(m => m.InsertTime);

            return query;
        }

    }
}
