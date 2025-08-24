using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作接口类 ，数据库表名：UserRole 
	/// </summary>
	public  partial interface IUserRoleService : IBaseService<UserRole>
	{
        void Insert(Userinfo role, string roleIds);

		string GetRoleId(string UserId);

	}
}
