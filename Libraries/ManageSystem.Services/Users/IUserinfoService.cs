using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Services.Users
{
	/// <summary>
	/// 操作接口类 ，数据库表名：Userinfo 
	/// </summary>
	public  partial interface IUserinfoService : IBaseService<Userinfo>
	{

        /// <summary>
        /// 根据登录帐号获取一个用户
        /// </summary>
        /// <param name="loginId">登录帐号</param>
        /// <returns></returns>
        Userinfo QueryModelByLoginId(string loginId);

        IPagedList<Userinfo> QueryPage(string loginId,string name,int state, int pageIndex = 0, int pageSize = int.MaxValue);


   

    }
}
