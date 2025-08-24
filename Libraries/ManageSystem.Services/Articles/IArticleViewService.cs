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
	/// 操作接口类 ，数据库表名：ArticleView 
	/// </summary>
	public  partial interface IArticleViewService : IBaseService<ArticleView>
	{

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="userinfoName"></param>
        /// <param name="articleId"></param>
        /// <param name="articleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ArticleView> QueryPage(string userinfoName, long articleId, string articleName, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 文章查看
        /// </summary>
        /// <param name="articleId">文章id</param>
        /// <param name="openId">用户微信openId</param>
        /// <param name="userinfoId">用户id</param>
        /// <param name="userName">用户姓名</param>
        /// <returns></returns>
        bool View(long articleId, string openId, long userinfoId = 0, string userName = "");

    }
}
