using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Documents;
using ManageSystem.Core;
using System.Linq.Expressions;
using ManageSystem.Core.DynamicLinq;
using ManageSystem.Data;
using Dapper;

namespace ManageSystem.Services.Documents
{
    /// <summary>
    /// 操作类 ，数据库表名：Document 
    /// </summary>
    public partial class DocumentService : BaseService<Document>, IDocumentService
    {

        public DocumentService(IRepository<Document> repository) : base(repository)
        {

        }

        /// <summary>
        /// 分页查询  网页
        /// </summary>
        /// <returns></returns>
        public IQueryable<Document> Query()
        {
            var query = this._repository.Table.Where(m => m.Mark > 0 && m.Status == true);          
            query = query.OrderBy(m => m.Sort);
            return query;

        }

        public IPagedList<Document> QueryPage(string name, int page, int pageSize)
        {
            List<int> validMark = new List<int> { 1, 2 };
            Expression<Func<Document, bool>> predicate = r => validMark.Contains(r.Mark);
            if (!string.IsNullOrWhiteSpace(name))
            {
                predicate = predicate.And(r => r.Name.Contains(name));
            }

            IQueryable<Document> query = Query(predicate)
                .OrderByDescending(o => o.InsertTime)
                //.OrderBy(o => o.Sort)
                .ThenByDescending(o => o.InsertTime)
                .AsQueryable();

            return new PagedList<Document>(query, page, pageSize);
        }

        public int CurrentMaxSort()
        {
            return Query(r => r.Mark > 0).OrderByDescending(r => r.Sort).Select(r => r.Sort).FirstOrDefault();
        }

        public List<Document> QueryDelete(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<Document>();
            }

            string sql = $@"SELECT * FROM dbo.Document WHERE Id IN ({string.Join(",", ids)});";

            return DapperHelper.GetConnection().Query<Document>(sql, null).ToList();
        }

        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int UpdateSort(int sort)
        {          
              string sql =string.Format("update Document set sort=sort+1 where sort>='{0}'", sort);
              return DapperHelper.GetConnection().Execute(sql);                
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int weChattop(long Id)
        {
            string sql= string.Format("update Document set weChattop=1 where Id='{0}' ", Id);
            return DapperHelper.GetConnection().Execute(sql);
        }
        /// <summary>
        /// 查询排序号是否存在
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public Document GetSort(int sort)
        {
            string sql = string.Format("select count(0) from Document where Sort='{0}'", sort);
            return DapperHelper.GetConnection().Query<Document>(sql).FirstOrDefault();
        }

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int Cancelceiling(long Id)
        {
            string sql = string.Format("update Document set weChattop=0 where Id='{0}' ", Id);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
