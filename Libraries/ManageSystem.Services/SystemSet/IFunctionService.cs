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
	/// 操作接口类 ，数据库表名：Function 
	/// </summary>
	public  partial interface IFunctionService : IBaseService<Function>
	{
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentFunctionId"></param>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
         IPagedList<Function> QueryPage(string name, long parentFunctionId,int type, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 根据角色id获取功能集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<Function> QueryByRoleId(long roleId);

        /// <summary>
        /// 根据用户id获取可以访问的功能集合
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        List<Function> QueryByUserId(long userId);

    }
}
