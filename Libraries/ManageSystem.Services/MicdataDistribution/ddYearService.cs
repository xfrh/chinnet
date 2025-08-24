using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.MicdataDistribution;
using ManageSystem.Data;

namespace ManageSystem.Services.MicdataDistribution
{
    public partial class ddYearService : IddYearService
    {
        public int Delete(string ids)
        {
            throw new NotImplementedException();
        }

        public IPagedList<ddYear> GetddYears(string title, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"  select  * from  ddYear";
                IEnumerable<ddYear> result = conn.Query<ddYear>(sql.ToString());
                IEnumerable<ddYear> query = from m in result
                                            select m;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    query = from m in query
                            where m.title.Contains(title)
                            select m;
                }
                return new PagedList<ddYear>(query.ToList(), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 后台数据列表年份查询
        /// </summary>
        /// <param name="yearid"></param>
        /// <returns></returns>
        public string GetDdYear(long yearid)
        {
            string year = "";
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select title from ddYear where isvalid=1 and year_id='{0}'", yearid);
                ddYear result = conn.Query<ddYear>(sql).FirstOrDefault();
                year = result.title;
            }
            return year;
        }
        /// <summary>
        /// web下拉选择
        /// </summary>
        /// <returns></returns>
        public List<ddYear> GetDdYears()
        {
            List<ddYear> list = new List<ddYear>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select * from ddYear where isvalid=1 order by title");
                IEnumerable<ddYear> result = conn.Query<ddYear>(sql);
                list = result.ToList();
            }
            return list;
            
        }
        /// <summary>
        /// 查询排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public ddYear GetSort(int sort)
        {
            string sql = string.Format("select count(0) from ddYear where sortid='{0}'", sort);
            return DapperHelper.GetConnection().Query<ddYear>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 添加纪念封
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int insertYears(ddYear model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into ddYear values('{0}','{1}','{2}','{3}','{4}')", model.title, model.sortid, model.isvalid, model.created, model.created_by);
                result = conn.Execute(sql);
            }
            return result;
        }
        /// <summary>
        /// 年份返填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ddYear QueryEntity(long id)
        {
            string sql = string.Format("select * from ddYear where year_id='{0}'", id);
            return DapperHelper.GetConnection().Query<ddYear>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 修改年份
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateddYears(ddYear model)
        {
            string sql = string.Format("update ddYear set title='{0}',sortid='{1}',isvalid='{2}' where year_id='{3}'", model.title, model.sortid, model.isvalid, model.year_id);
            return DapperHelper.GetConnection().Execute(sql);
        }
        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int UpdateSort(int sort)
        {
            string sql = string.Format("update ddYear set sortid=sortid+1 where sortid>='{0}'", sort);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
