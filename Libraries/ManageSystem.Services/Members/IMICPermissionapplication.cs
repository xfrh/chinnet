using ManageSystem.Core;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.MIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Members
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MICPermissionapplication 
    /// </summary>
    public partial interface IMICPermissionapplication : IBaseService<MICPermissionapplication>
    {
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="companyName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MICPermissionapplication> QueryPage(string name, string phone, string companyName, int pageIndex, int pageSize);


        /// <summary>
        /// 根据Mid获取一个实体对象
        /// </summary>
        /// <param name="mid">需要</param>
        /// <returns></returns>
       ///MICPermissionapplication QueryEntityMID(long mid);
    }
}
