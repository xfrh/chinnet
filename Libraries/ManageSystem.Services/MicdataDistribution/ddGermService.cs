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
    public partial class ddGermService : IddGermService
    {
        /// <summary>
        /// 删除选中的细菌
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(string ids)
        {
            string sql = string.Format("delete from ddGerm where germ_id in(" + ids + ")");
            DapperHelper.GetConnection().Execute(sql);
        }
        /// <summary>
        /// 后台列表页
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ddGerm> GetddGerms(string title, string code ,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select  * from  ddGerm order by mark desc";
                IEnumerable<ddGerm> result = conn.Query<ddGerm>(sql.ToString());
                IEnumerable<ddGerm> query = from m in result                                              
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
                return new PagedList<ddGerm>(query.ToList(), pageIndex, pageSize);
            }
        }
        /// <summary>
        /// 查询默认显示的细菌
        /// </summary>
        /// <returns></returns>
        public List<ddGerm> GetdefaultGerms()
        {
            List<ddGerm> list = new List<ddGerm>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from ddGerm where isvalid=1 and isdefault=1");
                IEnumerable<ddGerm> result = conn.Query<ddGerm>(sql);
                list = result.ToList();
            }
            return list;
        }

        /// <summary>
        /// web页面选择细菌查询
        /// </summary>
        /// <returns></returns>
        public List<ddGerm> GetGerms()
        {
            List<ddGerm> list = new List<ddGerm>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from ddGerm where isvalid=1 and mark=1  order by sortid");
                IEnumerable<ddGerm> result = conn.Query<ddGerm>(sql);
                list = result.ToList();
            }
            return list;
        }

        /// <summary>
        /// 查询排序号是否存在
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public ddGerm GetSort(int sort)
        {
            string sql = string.Format("select count(0) from ddGerm where sortid='{0}'", sort);
            return DapperHelper.GetConnection().Query<ddGerm>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int insertGerms(ddGerm model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into ddGerm values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",model.group_id,model.mark,model.title,model.title_en,model.code,model.sortid,model.isvalid,model.isdefault,model.created,model.created_by);
                result = conn.Execute(sql);               
            }
            return result;
        }
        /// <summary>
        /// 根据id查询细菌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ddGerm QueryEntity(long id)
        {
            string sql = string.Format("select * from ddGerm where germ_id='{0}'", id);
            return DapperHelper.GetConnection().Query<ddGerm>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 根据细菌名CODE查询细菌
        /// </summary>
        /// <param name="Germ"></param>
        /// <returns></returns>
        public ddGerm QueryEntityCode(string code)
        {
            string sql = string.Format("select * from ddGerm where code='{0}'", code);
            return DapperHelper.GetConnection().Query<ddGerm>(sql).FirstOrDefault();
        }

        /// <summary>
        /// 编辑细菌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateddGerm(ddGerm model)
        {
            string sql = string.Format("update ddGerm set title='{0}',title_en='{1}',code='{2}',sortid='{3}',isvalid='{4}',isdefault='{5}',mark='{6}' where germ_id='{7}'", model.title, model.title_en, model.code, model.sortid, model.isvalid,model.isdefault,model.mark,model.germ_id);
            return DapperHelper.GetConnection().Execute(sql);
        }
        /// <summary>
        /// 修改细菌mark
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateddGermMark(string code)
        {
            string sql = string.Format("update ddGerm set mark=1 where code='{0}'",code);
            return DapperHelper.GetConnection().Execute(sql);
        }

        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int UpdateSort(int sort)
        {
            string sql = string.Format("update ddGerm set sortid=sortid+1 where sortid>='{0}'", sort);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
