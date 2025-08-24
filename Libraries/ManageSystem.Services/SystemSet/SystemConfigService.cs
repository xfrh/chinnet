using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作类 ，数据库表名：SystemConfig 
	/// </summary>
	public partial class SystemConfigService :  BaseService<SystemConfig>, ISystemConfigService
	{

		public SystemConfigService(IRepository<SystemConfig> repository): base(repository)
		{
			
		}

        /// <summary>
        /// 根据Key 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SystemConfig QueryEntityByKey(string key)
        {
            return this.QueryEntity(m => m.Key.Equals(key));
        }


    }
}
