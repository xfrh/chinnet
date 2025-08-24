using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Services.Members
{
	/// <summary>
	/// 操作接口类 ，数据库表名：Feedback 
	/// </summary>
	public  partial interface IFeedbackService : IBaseService<Feedback>
	{

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Feedback> QueryPage(string name, int status, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
