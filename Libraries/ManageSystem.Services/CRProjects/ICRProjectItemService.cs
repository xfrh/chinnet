using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.CRProjects;

namespace ManageSystem.Services.CRProjects
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MedicalDataItem 
    /// </summary>
    public partial interface ICRProjectItemService : IBaseService<CRProjectItem>
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
        IPagedList<CRProjectItem> QueryPage(long ProjectId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 根据CR项目Id删除数据
        /// </summary>
        /// <param name="projectId">CR项目Id</param>
        void DeleteByProject(long projectId);

    }
}
