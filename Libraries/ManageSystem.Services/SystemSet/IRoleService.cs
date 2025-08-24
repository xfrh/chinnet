using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作接口类 ，数据库表名：Role 
	/// </summary>
	public  partial interface IRoleService : IBaseService<Role>
	{

        /// <summary>
        /// 根据指定用户的id获取该用户所拥有的角色
        /// </summary>
        /// <param name="userinfoId"></param>
        /// <returns></returns>
        List<Role> GetListByUserId(long userinfoId);

        List<Role> GetListBySatelliteUserId(long SatelliteUserId);

        IPagedList<Role> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
