using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;

namespace ManageSystem.Services.Articles
{
	/// <summary>
	/// 操作类 ，数据库表名：ArticleType 
	/// </summary>
	public partial class ArticleTypeService :  BaseService<ArticleType>, IArticleTypeService
	{

		public ArticleTypeService(IRepository<ArticleType> repository): base(repository)
		{
			
		}

        public string GetTypeName(long typeId)
        {
            try
            {
                var entity = this.QueryEntity(typeId);
                return entity.Name;
            }
            catch (Exception)
            {

            }

            return "";
        }

        public IPagedList<ArticleType> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0); ;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            query = query.OrderBy(m => m.Sort);

            var list = new PagedList<ArticleType>(query.ToList(), pageIndex, pageSize);

            return list;
        }
    }
}
