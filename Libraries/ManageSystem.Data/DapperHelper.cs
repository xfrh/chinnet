using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data
{
    public class DapperHelper
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["ManageSystemContext"].ToString();

        /// <summary>
        /// 获取链接对象
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            return conn;
        }

        /// <summary>
        /// 解析参数，获取对应的 
        /// </summary>
        /// <param name="param">参数，支持的格式：new { Name = "lisi", Age = 1  } </param>
        /// <param name="where">拼接SQL查询的条件，会自动在条件前面加 " AND " </param>
        /// <returns>返回Dapper支持的参数格式</returns>
        public static SpringSqlParameters GetParameters(object param, ref string where)
        {
            SpringSqlParameters sp = new SpringSqlParameters();

            if (param == null) return sp;

            foreach (var propertyInfo in param.GetType().GetProperties())
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(param, null);

                sp.Add(name, value);
                where += string.Format(" AND {0} = @{0} ", name);
            }

            return sp;
        }
    }
}