using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Researches;

namespace ManageSystem.Services.Researches
{
    /// <summary>
    /// 操作接口类 ，数据库表名：ResearchCollect 
    /// </summary>
    public partial interface IResearchCollectService : IBaseService<ResearchCollect>
	{
        bool Insert(long memberId, string memberName, long researchId, ref string errorMessage);

        /// <summary>
        /// 检查用户的搜藏状态
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <param name="researchId">科研id</param>
        /// <returns>true 已经搜藏  false  未搜藏</returns>
        bool MemberCollectStatus(long memberId,long researchId);


        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="meetingId">科研合作的id</param>
        /// <param name="meetingName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ResearchCollect> QueryPage(long researchId, string meetingName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 根据会员编号获取所对应的科研合作数据  （会员中心）
        /// </summary>
        /// <param name="memberId">会员编号</param>
        /// <returns></returns>
        IQueryable<ResearchCollect> Query(long memberId);

    }
}
