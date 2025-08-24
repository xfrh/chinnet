using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Data;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using ManageSystem.Services.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Core;

namespace ManageSystem.Services.Researches
{
    /// <summary>
    /// 操作类 ，数据库表名：ResearchComment 
    /// </summary>
    public partial class ResearchCommentService : BaseService<ResearchComment>, IResearchCommentService
    {

        private readonly IResearchService ResearchService;
        private readonly ISystemLogService SystemLogService;
        public ResearchCommentService(IRepository<ResearchComment> repository,
                     IResearchService researchService,
                     ISystemLogService systemLogService
            ) : base(repository)
        {
            this.ResearchService = researchService;
            this.SystemLogService = systemLogService;
        }

        public bool Insert(ResearchComment modelComment, ref string errorMessage)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查信息动态数据
                    Research researchEntity = this.ResearchService.QueryEntity(modelComment.ResearchId);
                    if (researchEntity == null || researchEntity.Id <= 0)
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

                    if (modelComment.MemberId == researchEntity.MemberId)
                        modelComment.Type = (int)ResearchCommentTypeEnum.Author;

                    //插入查看明细表
                    ResearchComment model = new ResearchComment()
                    {
                        Ip = HttpHelper.GetIp(),
                        BrowserName = HttpHelper.GetBrowserName(),
                        ResearchId = researchEntity.Id,
                        ResearchName = researchEntity.Name,
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
                    researchEntity.CommentCount += 1;
                    this.ResearchService.Update(researchEntity);

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert("添加科研合作评论记录失败", ex.ToString(), Core.Domain.Log.SystemLogLevel.Error);
            }

            errorMessage = "评论失败，请重试";
            return false;

        }

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="researchId">科研合作的id</param>
        /// <param name="researchName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ResearchComment> QueryPage(long researchId, string researchName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (researchId > 0)
                query = query.Where(m => m.ResearchId == researchId);

            if (!string.IsNullOrWhiteSpace(researchName))
            {
                List<long> ids = this.ResearchService.Query(m => m.Name.Contains(researchName)).Select(m => m.Id).ToList();
                if (ids != null && ids.Any())
                {
                    query = query.Where(m => ids.Contains(m.ResearchId));
                }
                else
                {
                    query = query.Where(m => m.Id == 0);
                }
            }

            if (!string.IsNullOrWhiteSpace(memberName))
                query = query.Where(m => m.MemberName.Equals(memberName.Trim()));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<ResearchComment>(query, pageIndex, pageSize);
        }


    }
}
