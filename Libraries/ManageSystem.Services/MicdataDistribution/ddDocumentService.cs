using ManageSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageSystem.Core.Domain.MicdataDistribution;
using System.Data.SqlClient;
using ManageSystem.Data;
using Dapper;
using ManageSystem.Core.Domain.Users;

namespace ManageSystem.Services.MicdataDistribution
{
    public class ddDocumentService : IddDocumentService
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ddDocumentCreate(ddDocument model)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {       
               string sql = string.Format("insert into ddDocument values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",model.document_id,model.hospital_id,model.category_id,model.year_id,model.germ_id,model.antibiotic_id,model.title,model.file_path,model.datacount,model.organism,model.header,model.sortid,model.isvalid,model.created,model.created_by);
                int result = conn.Execute(sql);
                return result;
            }
        }
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int deleteDocument(string ids)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("delete from ddDocument where document_id in("+ ids + ")");
                int result = conn.Execute(sql);
                int result1 = 0;
                if (result > 0)
                {
                    string sql1 = string.Format("delete from ddDocumentItem where document_id in(" + ids + ")");
                     result1 = conn.Execute(sql1);
                }
                return result1;
            }
        }

        public ddDocument Document(long Id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from ddDocument where document_id="+Id+"");
                ddDocument document = conn.Query<ddDocument>(sql).FirstOrDefault();
                return document;
            }
        }

        /// <summary>
        /// 数据上传列表
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="year_id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ddDocument> GetddDocument(string filename, long year_id, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select  * from  ddDocument order by created desc";
                IEnumerable<ddDocument> result = conn.Query<ddDocument>(sql.ToString());
                IEnumerable<ddDocument> query = from m in result
                                                 where m.isvalid == true
                                                 select m;
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    query = from m in query
                            where m.title.Contains(filename)
                            select m;
                }
                if (year_id!=0)
                {                   
                    query = from m in query
                            where m.year_id.Equals(year_id)
                            select m;
                }
                
                return new PagedList<ddDocument>(query.ToList(), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 查询操作人
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUsers(long userid)
        {
            string username = "";
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from Userinfo where Id='{0}'", userid);
                Userinfo result = conn.Query<Userinfo>(sql).FirstOrDefault();
                username = result.Name;
            }
            return username;
        }
    }
}
