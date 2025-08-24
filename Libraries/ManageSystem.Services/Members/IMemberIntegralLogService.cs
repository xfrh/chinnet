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
    /// 操作接口类 ，数据库表名：MemberIntegralLog 
    /// </summary>
    public partial interface IMemberIntegralLogService : IBaseService<MemberIntegralLog>
    {

        /// <summary>
        /// 获取指定用户的积分使用记录
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        IQueryable<MemberIntegralLog> Query(long memberId);

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MemberIntegralLog> QueryPage(string loginId, string name, string remark, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actionSource"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        bool Update(MemberIntegralLog entity, ActionSource actionSource, Account account);


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actionSource"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        bool Insert(MemberIntegralLog entity, ActionSource actionSource, Account account);
    }
}
