using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Data;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Services.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Core;

namespace ManageSystem.Services.Researches
{
    /// <summary>
    /// 操作类 ，数据库表名：ResearchView 
    /// </summary>
    public partial class ResearchViewService : BaseService<ResearchView>, IResearchViewService
    {
        private readonly IResearchService ResearchService;
        private readonly ISystemLogService SystemLogService;
        public ResearchViewService(IRepository<ResearchView> repository,
                     IResearchService researchService,
                     ISystemLogService systemLogService
            ) : base(repository)
        {
            this.ResearchService = researchService;
            this.SystemLogService = systemLogService;
        }

        public bool Insert(long memberId, string memberName, long researchId)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查信息动态数据
                    Research meetingEntity = this.ResearchService.QueryEntity(researchId);
                    if (meetingEntity == null || meetingEntity.Id <= 0) throw new Exception("会议不存在");

                    //插入查看明细表
                    ResearchView model = new ResearchView()
                    {
                        Ip = HttpHelper.GetIp(),
                        BrowserName = HttpHelper.GetBrowserName(),
                        ResearchId = meetingEntity.Id,
                        ResearchName = meetingEntity.Name,
                        MemberName = memberName,
                        MemberId = memberId
                    };
                    base.Insert(model);

                    //修改会议的查看次数
                    meetingEntity.ViewCount += 1;
                    this.ResearchService.Update(meetingEntity);

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert("添加查看科研合作记录失败", ex.ToString(), Core.Domain.Log.SystemLogLevel.Error);
            }

            return false;

        }

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="researchId">科研的id</param>
        /// <param name="researchName">科研名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ResearchView> QueryPage(long researchId, string researchName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue)
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

            return new PagedList<ResearchView>(query, pageIndex, pageSize);
        }
    }
}
