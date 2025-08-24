using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.Articles
{
    /// <summary>
    /// 操作类 ，数据库表名：Article 
    /// </summary>
    public partial class ArticleService : BaseService<Article>, IArticleService
    {

        public ArticleService(IRepository<Article> repository) : base(repository)
        {

        }

        public string GetArticleName(long articleId)
        {
            if (articleId <= 0) return "";

            var entity = this.QueryEntity(articleId);
            if (entity == null) return "";

            return entity.Name;
        }

        /// <summary>
        /// 根据id或者ids获取集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<Article> Query(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return null;

            var query = base._repository.Table.Where(m => m.Mark > 0 && m.State == (int)ArticleState.Normal); ;
            string[] array = ids.Split(','); //id集合，格式： 1，3，4,5,6
            query = query.Where(m => array.Contains(m.Id.ToString()));

            IList<Article> list = query.ToList();
            if (list == null || !list.Any()) return null;

            //对数据进行排序
            IList<Article> newList = new List<Article>();
            for (int i = 0; i < list.Count; i++)
            {
                var temp = list.Where(m => m.Id == long.Parse(array[i])).FirstOrDefault();
                if (temp == null) continue;

                newList.Add(temp);
            }

            return newList;
        }

        public IPagedList<Article> QueryPage(string name, long typeId, int state, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0); ;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));
            if (typeId > 0)
                query = query.Where(m => m.ArticleTypeId == typeId);

            if (state > 0)
                query = query.Where(m => m.State == state);

            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<Article>(query.ToList(), pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 微信端分页
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Article> QueryPage(string typeId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0 && m.State == (int)ArticleState.Normal); ;
            if (!string.IsNullOrWhiteSpace(typeId) && typeId.IsLong() && long.Parse(typeId) > 0)
            {
                long temp = long.Parse(typeId);
                query = query.Where(m => m.ArticleTypeId == temp);
            }


            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<Article>(query.ToList(), pageIndex, pageSize);

            return list;
        }
    }
}
