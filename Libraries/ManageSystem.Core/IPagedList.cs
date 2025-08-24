using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core
{
    /// <summary>
    /// 分页的数据接口
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// 当前第几页
        /// </summary>
        int PageIndex { get; }
        
        /// <summary>
        /// 每页行数
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 总行数
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 是否上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
