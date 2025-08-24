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
	/// 操作接口类 ，数据库表名：MemberAddress 
	/// </summary>
	public  partial interface IMemberAddressService : IBaseService<MemberAddress>
	{

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="account"></param>
        void Insert(MemberAddress entity, Account account, ActionSource source);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="account"></param>
        void Update(MemberAddress entity, Account account, ActionSource source);

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MemberAddress> QueryPage(string loginId, string  name, string  phone,int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
