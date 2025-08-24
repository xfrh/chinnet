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
    /// 操作接口类 ，数据库表名：ResearchView 
    /// </summary>
    public partial interface IResearchViewService : IBaseService<ResearchView>
	{
        bool Insert(long memberId, string memberName, long researchId);

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="researchId">信息动态的id</param>
        /// <param name="researchName">会议名称</param>
        /// <param name="memberName">会员的名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ResearchView> QueryPage(long researchId, string researchName, string memberName, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
