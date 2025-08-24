using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Log;

namespace ManageSystem.Services.Members
{
	/// <summary>
	/// 操作接口类 ，数据库表名：MemberCart 
	/// </summary>
	public  partial interface IMemberCartService : IBaseService<MemberCart>
	{

        /// <summary>
        /// 根据用户id获取对应的购物车数据
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        List<MemberCart> QueryByMember(long memberId);

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="entity">购物车数据实体</param>
        /// <param name="account">操作用户</param>
        /// <param name="source">日志数据来源</param>
        /// <returns></returns>
        bool AddCart(MemberCart entity, Account account, ActionSource source);
	}
}
