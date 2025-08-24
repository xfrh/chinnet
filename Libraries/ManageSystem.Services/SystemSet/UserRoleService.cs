using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作类 ，数据库表名：UserRole 
	/// </summary>
	public partial class UserRoleService :  BaseService<UserRole>, IUserRoleService
	{

		public UserRoleService(IRepository<UserRole> repository): base(repository)
		{
			

		}


        public void Insert(Userinfo user, string roleIds)
        {
            //删除原来的数据
            this.Delete(m => m.UserinfoId == user.Id);

            if (string.IsNullOrWhiteSpace(roleIds)) return;

            //重新插入数据
            string[] array = roleIds.Split(',');
            foreach (var item in array)
            {
                var entity = new UserRole()
                {
                    RoleId = long.Parse(item),
                    UserinfoId =user.Id 
                };

                this.Insert(entity);
            }

        }

        public string GetRoleId(string UserId)
        {
            long userid = long.Parse(UserId);
           return this._repository.Table.First(x => x.UserinfoId == userid).RoleId.ToString();
        }

    }
}
