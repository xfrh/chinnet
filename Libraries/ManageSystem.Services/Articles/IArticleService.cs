using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Articles;

namespace ManageSystem.Services.Articles
{
	/// <summary>
	/// 操作接口类 ，数据库表名：Article 
	/// </summary>
	public  partial interface IArticleService : IBaseService<Article>
	{

        /// <summary>
        /// 根据文章id获取文章的名称
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        string GetArticleName(long articleId);
    


        /// <summary>
        ///  根据id或者ids获取集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IList<Article> Query(string ids);

            /// <summary>
            /// 分页查询数据 (后台)
            /// </summary>
            /// <param name="name"></param>
            /// <param name="parentFunctionId"></param>
            /// <param name="type"></param>
            /// <param name="pageIndex"></param>
            /// <param name="pageSize"></param>
            /// <returns></returns>
            IPagedList<Article> QueryPage(string name, long typeId, int state, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 分页查询数据 (微信 , web)
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Article> QueryPage(string typeId, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
