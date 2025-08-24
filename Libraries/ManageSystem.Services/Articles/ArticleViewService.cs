using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Log;
using System.Linq.Expressions;

namespace ManageSystem.Services.Articles
{
    /// <summary>
    /// 操作类 ，数据库表名：ArticleView 
    /// </summary>
    public partial class ArticleViewService : BaseService<ArticleView>, IArticleViewService
    {
        private IArticleService ArticleService;
        private ISystemLogService SystemLogService;

        public ArticleViewService(IRepository<ArticleView> repository,
               IArticleService articleService,
                 ISystemLogService systemLogService

              ) : base(repository)
        {
            this.ArticleService = articleService;
            this.SystemLogService = systemLogService;
        }


        public IPagedList<ArticleView> QueryPage(string userinfoName, long articleId, string articleName, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0); ;
            if (!string.IsNullOrWhiteSpace(userinfoName))
                query = query.Where(m => m.MemberName.Equals(userinfoName));

            if (articleId > 0)
                query = query.Where(m => m.ArticleId == articleId);

            if (!string.IsNullOrWhiteSpace(articleName))
            {
                var articleEntity = this.ArticleService.Query(m => m.Name.Contains(articleName)).Select(x => x.Id).ToList();
                if (articleEntity != null)
                    query = query.Where(m => articleEntity.Contains(m.ArticleId));
            }

            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<ArticleView>(query.ToList(), pageIndex, pageSize);

            return list;
        }


        /// <summary>
        /// 文章查看
        /// </summary>
        /// <param name="articleId">文章id</param>
        /// <param name="openId">用户微信openId</param>
        /// <param name="userinfoId">用户id</param>
        /// <param name="userName">用户姓名</param>
        /// <returns></returns>
        public bool View(long articleId, string openId, long userinfoId = 0, string userName = "")
        {
            if (articleId <= 0 || string.IsNullOrWhiteSpace(openId)) return false;

            var article = this.ArticleService.QueryEntity(articleId);

            int count = this.Query(m => m.Mark > 0 && m.ArticleId == articleId).Count();
            if (count > 0)
                return false;  //已经有查看记录

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //添加记录
                    ArticleView model = new ArticleView()
                    {
                        ArticleId = article.Id,
                        ArticleName = article.Name,
                        MemberId = userinfoId,
                        MemberName = userName,
                        Describe = ""
                    };
                    this.Insert(model);

                    //更新主表数据
                    article.ViewCount += 1;
                    this.ArticleService.Update(article);

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error);
            }

            return true;
        }

        public override void Delete(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return;

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    string[] idArray = ids.Split(',');
                    var list = this.Query(m => idArray.Contains(m.Id.ToString()));

                    foreach (var item in list)
                    {
                        //删除记录表
                        base.Delete(item);

                        //更新主表数据
                        var articleEntity = this.ArticleService.QueryEntity(item.ArticleId);
                        articleEntity.ViewCount -= 1;

                        this.ArticleService.Update(articleEntity);
                    }
                    tran.Commit();
                }

                con.Close();

                return;
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error);
            }

        }
    }
}
