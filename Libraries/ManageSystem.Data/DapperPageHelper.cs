using Dapper;
using ManageSystem.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data
{
    /// <summary>
    /// Dapper 分页查询助手
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperPageHelper
    {
        public DapperPageHelper()
        {

        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="sql"> 完整的SQL，不包含Where，格式：SELECT Id,Name,Age FROM Member </param>
        /// <param name="where">查询条件，格式1：AND Name ='test' AND Age = 1 ，方式2：AND Name =@Name  AND Age = @Age</param>
        /// <param name="orderby">排序sql，格式： ORDER BY ID DESC</param>
        /// <param name="tableName">数据库表名，不写将获取泛型的名称</param>
        /// <param name="param">接受的参数，如果查询条件是方式2：new { Name = "lisi", Age = 1  }</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageToEnumerable<T>(string sql, string where, string orderby, string tableName = "", object param = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //如果没有传递表名，则使用泛型的名称
            if (String.IsNullOrWhiteSpace(tableName))
                tableName = typeof(T).Name;

            IDbConnection connection = DapperHelper.GetConnection();

            //查询的数据
            StringBuilder sb = new StringBuilder();
            sb.Append(" {0}  WHERE  1 = 1 {2}  {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY  ");
            sb.Replace("{0}", sql);
            sb.Replace("{2}", string.IsNullOrWhiteSpace(where) ? "" : where);
            sb.Replace("{3}", orderby);
            sb.Replace("{4}", (pageSize * pageIndex).ToString());
            sb.Replace("{5}", pageSize.ToString());
            IEnumerable<T> data = connection.Query<T>(sb.ToString(), param);
            return data;
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="sql"> 完整的SQL，不包含Where，格式：SELECT Id,Name,Age FROM Member </param>
        /// <param name="where">查询条件，格式1：AND Name ='test' AND Age = 1 ，方式2：AND Name =@Name  AND Age = @Age</param>
        /// <param name="orderby">排序sql，格式： ORDER BY ID DESC</param>
        /// <param name="tableName">数据库表名，不写将获取泛型的名称</param>
        /// <param name="param">接受的参数，如果查询条件是方式2：new { Name = "lisi", Age = 1  }</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        public IPagedList<T> QueryPage<T>(string sql, string where, string orderby, string tableName = "", object param = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //如果没有传递表名，则使用泛型的名称
            if (String.IsNullOrWhiteSpace(tableName))
                tableName = typeof(T).Name;

            IDbConnection connection = DapperHelper.GetConnection();

            //查询的数据
            StringBuilder sb = new StringBuilder();
            sb.Append(" {0}  WHERE  1 = 1 {2}  {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY  ");
            sb.Replace("{0}", sql);
            sb.Replace("{2}", string.IsNullOrWhiteSpace(where) ? "" : where);
            sb.Replace("{3}", orderby);
            sb.Replace("{4}", (pageSize * pageIndex).ToString());
            sb.Replace("{5}", pageSize.ToString());
            IEnumerable<T> data = connection.Query<T>(sb.ToString(), param);

            //获取总行数
            StringBuilder sbCount = new StringBuilder();
            sbCount.Append("  SELECT COUNT(*) FROM  {0}  WHERE  1=1   {1}   ");
            sbCount.Replace("{0}", tableName);
            sbCount.Replace("{1}", string.IsNullOrWhiteSpace(where) ? "" : where);
            int dataCount = connection.ExecuteScalar<int>(sbCount.ToString(), param);

            //构建返回的数据
            return new PagedList<T>(data, pageIndex, pageSize, dataCount);
        }


        /// <summary>
        /// 分页查询数据，多表联合查询
        /// </summary>
        /// <param name="sql"> 完整的SQL，不包含表名和Where，格式：SELECT Id,Name,Age</param>
        /// <param name="where">查询条件，格式1：AND Name ='test' AND Age = 1 ，方式2：AND Name =@Name  AND Age = @Age</param>
        /// <param name="orderby">排序sql，格式： ORDER BY ID DESC</param>
        /// <param name="tableName">数据库表名，不写将获取泛型的名称，支持多表的组合，例如: App_FestivalScoreSendLog AS AF LEFT JOIN dbo.Member_Member AS MM ON AF.MemberId = MM.Id</param>
        /// <param name="param">接受的参数，如果查询条件是方式2：new { Name = "lisi", Age = 1  }</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        public IPagedList<T> QueryPageUnite<T>(string sql, string where, string orderby, string tableName, object param = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //如果没有传递表名，则使用泛型的名称
            if (String.IsNullOrWhiteSpace(tableName))
                tableName = typeof(T).Name;

            IDbConnection connection = DapperHelper.GetConnection();

            //查询的数据
            StringBuilder sb = new StringBuilder();
            sb.Append(" {0} {1} WHERE  1 = 1 {2}  {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY  ");
            sb.Replace("{0}", sql);
            sb.Replace("{1}", tableName);
            sb.Replace("{2}", string.IsNullOrWhiteSpace(where) ? "" : where);
            sb.Replace("{3}", orderby);
            sb.Replace("{4}", (pageSize * pageIndex).ToString());
            sb.Replace("{5}", pageSize.ToString());
            IEnumerable<T> data = connection.Query<T>(sb.ToString(), param);

            //获取总行数
            StringBuilder sbCount = new StringBuilder();
            sbCount.Append("  SELECT COUNT(*) FROM  {0}  WHERE  1=1   {1}   ");
            sbCount.Replace("{0}", tableName);
            sbCount.Replace("{1}", string.IsNullOrWhiteSpace(where) ? "" : where);
            int dataCount = connection.ExecuteScalar<int>(sbCount.ToString(), param);

            //构建返回的数据
            return new PagedList<T>(data, pageIndex, pageSize, dataCount);
        }
    }
}
