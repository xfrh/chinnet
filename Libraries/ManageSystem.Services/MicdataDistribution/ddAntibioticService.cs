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
    public class ddAntibioticService : IddAntibioticService
    {
       
        /// <summary>
        /// 前台方法
        /// </summary>
        /// <returns></returns>
        public List<ddAntibiotic> GetDdAntibiotics()
        {
            List<ddAntibiotic> list = new List<ddAntibiotic>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select * from  [ddAntibiotic] where isvalid=1 and mark=1 order by sortid");
                IEnumerable<ddAntibiotic> result = conn.Query<ddAntibiotic>(sql);
                list = result.ToList();
            }
            return list;
        }
        /// <summary>
        /// 后台列表页
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ddAntibiotic> Antibioticslist(string title, string code, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select  * from  ddAntibiotic order by mark desc";
                IEnumerable<ddAntibiotic> result = conn.Query<ddAntibiotic>(sql.ToString());
                IEnumerable<ddAntibiotic> query = from m in result
                                            select m;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    query = from m in query
                            where m.title.Contains(title)
                            select m;
                }
                if (!string.IsNullOrWhiteSpace(code))
                {
                    query = from m in query
                            where m.code.ToLower().Contains(code.ToLower())
                            select m;
                }
                return new PagedList<ddAntibiotic>(query.ToList(), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int insertAntibiotic(ddAntibiotic model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into ddAntibiotic values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", model.group_id, model.mark, model.title, model.title_en, model.code, model.sortid, model.isvalid,model.isdefault,model.created, model.created_by);
                result = conn.Execute(sql);
            }
            return result;
        }
        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int UpdateSort(int sort)
        {
            string sql = string.Format("update ddAntibiotic set sortid=sortid+1 where sortid>='{0}'", sort);
            return DapperHelper.GetConnection().Execute(sql);
        }

        /// <summary>
        /// 查询排序号是否存在
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public ddAntibiotic GetSort(int sort)
        {
            string sql = string.Format("select count(0) from ddAntibiotic where sortid='{0}'", sort);
            return DapperHelper.GetConnection().Query<ddAntibiotic>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 根据id查询抗生素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ddAntibiotic QueryEntity(long id)
        {
            string sql = string.Format("select * from ddAntibiotic where antibiotic__id='{0}'", id);
            return DapperHelper.GetConnection().Query<ddAntibiotic>(sql).FirstOrDefault();
        }

        /// <summary>
        /// 编辑抗生素
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateAntibiotic(ddAntibiotic model)
        {
            string sql = string.Format("update ddAntibiotic set title='{0}',title_en='{1}',code='{2}',sortid='{3}',isvalid='{4}',isdefault='{5}',mark='{6}' where antibiotic__id='{7}'", model.title,model.title_en,model.code,model.sortid,model.isvalid,model.isdefault,model.mark, model.antibiotic__id);
            return DapperHelper.GetConnection().Execute(sql);
        }
        /// <summary>
        /// 删除选中的抗生素
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(string ids)
        {         
                string sql = string.Format("delete from ddAntibiotic where antibiotic__id in("+ ids + ")");
                DapperHelper.GetConnection().Execute(sql);          
        }
        /// <summary>
        /// 根据code查询抗生素
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ddAntibiotic QueryEntityCode(string code)
        {
            string sql = string.Format("select * from ddAntibiotic where code='{0}'", code);
            return DapperHelper.GetConnection().Query<ddAntibiotic>(sql).FirstOrDefault();
        }

        /// <summary>
        /// 修改抗生素mark
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int UpdateddAntibioticMark(string code)
        {
            string sql = string.Format("update ddAntibiotic set mark=1 where code='{0}'", code);
            return DapperHelper.GetConnection().Execute(sql);
        }

        /// <summary>
        /// 查询默认抗生素
        /// </summary>
        /// <returns></returns>
        public List<ddAntibiotic> GetdefaultddAntibiotic()
        {
            List<ddAntibiotic> list = new List<ddAntibiotic>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select * from  [ddAntibiotic] where isvalid=1 and isdefault=1");
                IEnumerable<ddAntibiotic> result = conn.Query<ddAntibiotic>(sql);
                list = result.ToList();
            }
            return list;
        }
    }
}
