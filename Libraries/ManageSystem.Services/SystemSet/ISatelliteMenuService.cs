using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Domain.Sate;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作接口类 ，数据库表名：SatelliteMenu 
	/// </summary>
	public partial interface ISatelliteMenuService : IBaseService<SatelliteMenu>
	{
        void getByMenuIds(string menuIds);

        List<SatelliteMenu> GetListBySatelliteUserId(long userId);
       
    }
}
