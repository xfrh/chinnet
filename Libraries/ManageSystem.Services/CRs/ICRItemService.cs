using ManageSystem.Core;
using ManageSystem.Core.Domain.CRs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.CRs
{
    public partial interface ICRItemService : IBaseService<CRItem>
    {
         /// <summary>
        /// 分页查询数据 后台
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="areaId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="projectType"></param>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<CRItem> QueryPage(long ProjectId, int pageIndex = 0, int pageSize = int.MaxValue);

    /// <summary>
    /// 根据CR项目Id删除数据
    /// </summary>
    /// <param name="projectId">CR项目Id</param>
    void DeleteByProject(long projectId);
}
}
