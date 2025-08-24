using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MedicalData 
    /// </summary>
    public partial interface IMedicalDataProjectService : IBaseService<MedicalDataProject>
    {

        /// <summary>
        /// 分页查询数据 后台
        /// </summary>
        IPagedList<MedicalDataProject> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue);

        string GetProjectItemName(string projectStr);
    }
}
