using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Domain.MicdataDistribution;
using ManageSystem.Data;

namespace ManageSystem.Services.MicdataDistribution
{
    public class ddDocumentItemService : IddDocumentItemService
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ddDocumentItemCreate(ddDocumentItem model)
        {
            string sql = string.Format("insert into ddDocumentItem values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", model.document_id,model.patinet_id,model.ward,model.specimem,model.specimem_date,model.Specimen_Type,model.organism,model.organism_Type,model.antibiotics,model.antibiotics_value,model.isvalid);
            return DapperHelper.GetConnection().Execute(sql);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int deleteDocumentItem(string ids)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {              
                string sql = string.Format("delete from ddDocumentItem where document_id in(" + ids + ")");
                var result = conn.Execute(sql);
                return result;
            }
        }

        /// <summary>
        /// 查询mic的表头
        /// </summary>
        /// <returns></returns>
        public string GetCategoryValues()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select title from [dbo].[ddCategoryValue] where code = 'MIC'");
                string result = conn.Query<ddCategoryValue>(sql).FirstOrDefault().title;
                return result;
            }
        }
        /// <summary>
        /// 前段查询数据
        /// </summary>
        /// <param name="gremcode"></param>
        /// <param name="yeraid"></param>
        /// <param name="antibiotics"></param>
        /// <returns></returns>
        public List<ddDocumentItem> GetDdDocuments(string[] gremcode, string  yeraid, string  antibiotics)
        {
            string wherestr1 = "";
            for (int i = 0; i < gremcode.Length; i++)
            {
                if (wherestr1 == "")
                {
                    wherestr1 = "a.organism ='" + gremcode[i] + "'";
                }
                else
                {
                    wherestr1 += "or a.organism ='" + gremcode[i] + "'";
                }
            }           
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select b.document_id did,a.* from ddDocumentItem a inner join ddDocument b on a.document_id=b.document_id where b.year_id="+yeraid+" and("+wherestr1+ ")and a.antibiotics like'%" + antibiotics + "%'");
                List<ddDocumentItem> result = conn.Query<ddDocumentItem>(sql).ToList();
                return result;
            }
            
        }
        /// <summary>
        /// 分页查询详情数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ddDocumentItem> QueryPage(long Id, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select  * from  ddDocumentItem where document_id="+Id+ "";
                IEnumerable<ddDocumentItem> result = conn.Query<ddDocumentItem>(sql.ToString());             
                return new PagedList<ddDocumentItem>(result.ToList(), pageIndex, pageSize);
            }
        }
    }
}
