using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Weixin;

namespace ManageSystem.Services.Weixin
{
	/// <summary>
	/// 操作接口类 ，数据库表名：WeixinMenu 
	/// </summary>
	public  partial interface IWeixinMenuService : IBaseService<WeixinMenu>
	{
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        string CreateMenu();

        /// <summary>
        /// 根据 菜单key获取菜单的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        WeixinMenu QueryEntity(string key);
	}
}
