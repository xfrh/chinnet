using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Domain.Sate;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作类 ，数据库表名：UserRole 
	/// </summary>
	public partial class SatelliteRoleService :  BaseService<SatelliteRole>, ISatelliteRoleService
    {

		public SatelliteRoleService(IRepository<SatelliteRole> repository): base(repository)
		{
			

		}


        public void Insert(SatelliteUser user, string roleIds)
        {
            //删除原来的数据
            this.Delete(m => m.SatelliteUserId == user.Id);

            if (string.IsNullOrWhiteSpace(roleIds)) return;

            //重新插入数据
            string[] array = roleIds.Split(',');
            foreach (var item in array)
            {
                var entity = new SatelliteRole()
                {
                    RoleId = long.Parse(item),
                    SatelliteUserId =user.Id 
                };

                this.Insert(entity);
            }

        }

    }
}
