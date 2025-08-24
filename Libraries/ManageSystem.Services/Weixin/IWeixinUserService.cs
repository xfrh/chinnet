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
	/// 操作接口类 ，数据库表名：WeixinUser 
	/// </summary>
	public  partial interface IWeixinUserService : IBaseService<WeixinUser>
	{

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subscribe"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<WeixinUser> QueryPage(string name, string subscribeValue, string startTime, string endTime, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 用户关注以后 添加或者修改微信用户记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UserSubscribe(WeixinUser newEntity);


        /// <summary>
        /// 用户取消关注以后 修改微信用户记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UserUnsubscribe(WeixinUser newEntity);
    }
}
