using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Services.Log;
using ManageSystem.Data;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Core;

namespace ManageSystem.Services.Researches
{
    /// <summary>
    /// 操作类 ，数据库表名：ResearchCollect 
    /// </summary>
    public partial class ResearchCollectService :  BaseService<ResearchCollect>, IResearchCollectService
    {

        private readonly IResearchService ResearchService;
        private readonly ISystemLogService SystemLogService;
        public ResearchCollectService(IRepository<ResearchCollect> repository,
                     IResearchService researchService,
                     ISystemLogService systemLogService
            ) : base(repository)
        {
            this.ResearchService = researchService;
            this.SystemLogService = systemLogService;
        }


        public bool Insert(long memberId, string memberName, long researchId, ref string errorMessage)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查信息动态数据
                    Research researchEntity = this.ResearchService.QueryEntity(researchId);
                    if (researchEntity == null || researchEntity.Id <= 0)
                    {
                        errorMessage = "会议不存在";
                        return false;
                    }

                    //重复收藏
                    if (this.Count(m => m.ResearchId == researchId && m.MemberId == memberId) > 0)
                    {
                        errorMessage = "已经收藏过了";
                        return false;
                    }


                    //插入查看明细表
                    ResearchCollect model = new ResearchCollect()
                    {
                        Ip = HttpHelper.GetIp(),
                        BrowserName = HttpHelper.GetBrowserName(),
                        ResearchId = researchEntity.Id,
                        ResearchName = researchEntity.Name,
                        MemberName = memberName,
                        MemberId = memberId
                    };
                    base.Insert(model);

                    //修改会议的查看次数
                    researchEntity.CollectCount += 1;
                    this.ResearchService.Update(researchEntity);

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert("添加收藏科研合作记录失败", ex.ToString(), Core.Domain.Log.SystemLogLevel.Error);
            }

            errorMessage = "收藏失败，请重试";
            return false;

        }


        /// <summary>
        /// 检查用户的搜藏状态
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <param name="researchId">科研id</param>
        /// <returns>true 已经搜藏  false  未搜藏</returns>
        public bool MemberCollectStatus(long memberId, long researchId)
        {
            if (memberId <= 0) return false;
            return (this.Count(m => m.MemberId == memberId &&  m.ResearchId == researchId && m.Mark > 0) > 0);
        }


        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="meetingId">科研合作的id</param>
        /// <param name="meetingName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ResearchCollect> QueryPage(long researchId, string researchName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue)
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
                    query = query.Where(m => m.Id ==0);
                }
            }

            if (!string.IsNullOrWhiteSpace(memberName))
                query = query.Where(m => m.MemberName.Equals(memberName.Trim()));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<ResearchCollect>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// 根据会员编号获取所对应的科研合作数据  （会员中心）
        /// </summary>
        /// <param name="memberId">会员编号</param>>
        /// <returns></returns>
        public IQueryable<ResearchCollect> Query(long memberId)
        {
            if (memberId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId).OrderByDescending(m => m.InsertTime);

            return query;
        }
    }
}
