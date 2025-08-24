using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.DataInput;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial class OLTemplateService : BaseService<OLTemplate>, IOLTemplateService 
    { 
             public OLTemplateService(IRepository<OLTemplate> repository

             ) : base(repository)
        {

        }
    /// <summary>
    /// 添加字段模板
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public int AddTemplate(OLTemplate model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into [dbo].[OLtemplate] values('{0}','{1}','{2}','{3}','{4}') ",model.Id,model.template_name,model.created_byId,model.InsertTime,model.Updatetime);
                result = conn.Execute(sql);
            }
            return result;
        }

        /// <summary>
        /// 删除字段模板
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int delete(long Id)
        {

            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("delete from  [dbo].[OLtemplate] where Id='{0}' ",Id);
                result = conn.Execute(sql);               
            }
            return result;
        }

        /// <summary>
        /// 查询模板是否关联了项目
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public int GetLProjects(long templateId)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select COUNT(*) as sums from [dbo].[OLdataInput] where templateId='{0}'", templateId);
                 result = conn.Query<OLTemplate>(sql).FirstOrDefault().sums;
            }
            return result;
        }

        public OLTemplate GetLTemplate(long Id)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[OLtemplate] where Id={0}", Id);
               var result = conn.Query<OLTemplate>(sql).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// 项目添加页面字段模板下拉列表
        /// </summary>
        /// <returns></returns>
        public List<OLTemplate> GetLTemplatelist()
        {
            List<OLTemplate> list = new List<OLTemplate>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[OLtemplate]");
                IEnumerable<OLTemplate> result = conn.Query<OLTemplate>(sql);
                list = result.ToList();
            }
            return list;
        }
       
        /// <summary>
        /// 字段模板列表
        /// </summary>
        /// <param name="createdbyId"></param>
        /// <returns></returns>
        public List<OLTemplate> Querylist()
        {
           
            List<OLTemplate> list = new List<OLTemplate>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.*,b.Name from[dbo].[OLtemplate] a left join Member b on  a.created_byId = b.Id ");
                IEnumerable<OLTemplate> result = conn.Query<OLTemplate>(sql);
                list = result.ToList();
            }
            return list;
        }

        /// <summary>
        /// 修改字段模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int updateOLTemplate(OLTemplate model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("Update [dbo].[OLtemplate] set  template_name='{0}',created_byId='{1}',Updatetime='{2}' where Id='{3}' ", model.template_name, model.created_byId,model.Updatetime, model.Id);
                result = conn.Execute(sql);
            }
            return result;
        }
    }
}
