using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;

namespace ManageSystem.Services.Articles
{
	/// <summary>
	/// 操作接口类 ，数据库表名：ArticleType 
	/// </summary>
	public  partial interface IArticleTypeService : IBaseService<ArticleType>
	{
        /// <summary>
        /// 根据id获取分类的名称，如果没有反会空
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        string GetTypeName(long typeId);

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentFunctionId"></param>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ArticleType> QueryPage(string name,  int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
