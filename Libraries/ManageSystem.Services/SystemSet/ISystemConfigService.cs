using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作接口类 ，数据库表名：SystemConfig 
	/// </summary>
	public  partial interface ISystemConfigService : IBaseService<SystemConfig>
	{
        /// <summary>
        /// 根据Key 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        SystemConfig QueryEntityByKey(string key);


    }
}
