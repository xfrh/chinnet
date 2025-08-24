using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core;

namespace ManageSystem.Services.Members
{
	/// <summary>
	/// 操作类 ，数据库表名：Feedback 
	/// </summary>
	public partial class FeedbackService :  BaseService<Feedback>, IFeedbackService
	{

		public FeedbackService(IRepository<Feedback> repository): base(repository)
		{
			
		}

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Feedback> QueryPage(string name, int status, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (status > 0)
                query = query.Where(m => m.Status == (status ==1));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Feedback>(query, pageIndex, pageSize);
        }
    }
}
