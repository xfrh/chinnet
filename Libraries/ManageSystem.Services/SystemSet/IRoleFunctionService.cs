using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作接口类 ，数据库表名：RoleFunction 
	/// </summary>
	public  partial interface IRoleFunctionService : IBaseService<RoleFunction>
	{
        void Insert(Role role, string functionIds);
	}
}
