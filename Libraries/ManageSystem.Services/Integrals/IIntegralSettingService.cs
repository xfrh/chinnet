using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Integrals;

namespace ManageSystem.Services.Integrals
{
	/// <summary>
	/// 操作接口类 ，数据库表名：IntegralSetting 
	/// </summary>
	public  partial interface IIntegralSettingService : IBaseService<IntegralSetting>
	{

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<IntegralSetting> QueryPage(string name, long type, string status, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
