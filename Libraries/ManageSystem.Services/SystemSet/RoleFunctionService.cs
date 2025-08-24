using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作类 ，数据库表名：RoleFunction 
	/// </summary>
	public partial class RoleFunctionService :  BaseService<RoleFunction>, IRoleFunctionService
	{

		public RoleFunctionService(IRepository<RoleFunction> repository): base(repository)
		{
			
		}

        public void Insert(Role role, string functionIds)
        {
             //删除原来的数据
            this.Delete(m => m.RoleId == role.Id);

            if (string.IsNullOrWhiteSpace(functionIds)) return;

            //重新插入数据
            string[] array = functionIds.Split(',');
            foreach (var item in array)
            {
                var entity = new RoleFunction()
                {
                    RoleId = role.Id,
                    FunctionId=long.Parse(item)
                };

                this.Insert(entity);
            }

        }

    }
}
