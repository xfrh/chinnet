using Dapper;
using ManageSystem.Core;
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
    public partial class OLFieldmodelService : IOLFieldmodelService
    {
        /// <summary>
        /// 添加模板字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddFieldmodel(OLFieldmodel model)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into OLfieldmodel values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", model.templateId,model.field_name,model.field_code,model.default_value,model.created_byId,model.InsertTime,model.Updatetime,model.sfbt,model.order_px);
                int result = conn.Execute(sql);
                return result;
            }
           
        }
        /// <summary>
        /// 根据模板id删除字段
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public int delete(long templateId)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {                
                string sql1 = string.Format("delete from  [dbo].[OLfieldmodel] where templateId='{0}' ", templateId);
                int result = conn.Execute(sql1);
                return result;
            }
        }

        /// <summary>
        /// 查询模板字端code
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public OLFieldmodel GetLFieldfield_code(long templateId)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql1 = string.Format("SELECT STUFF((SELECT ',' + convert(varchar(10),field_code) FROM [OLfieldmodel] WHERE templateId='{0}' FOR xml path('')),1,1,'') as field_code", templateId);
                var result = conn.Query<OLFieldmodel>(sql1).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// 根据id和字段名称查询字段
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public OLFieldmodel GetLFieldmodel(long templateId, string fieldName)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select * from  [dbo].[OLfieldmodel] where templateId={0} and field_name='{1}'", templateId, fieldName);
                var result = conn.Query<OLFieldmodel>(sql).FirstOrDefault();
                return result;
            }
           
        }

        /// <summary>
        /// 数据添加页面字段配置
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public List<OLFieldmodel> GetLFieldmodellist(long projectid)
        {
            List<OLFieldmodel> result = new List<OLFieldmodel>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select * from  [dbo].[OLfieldmodel] where templateId=(select templateId  from[dbo].[OLproject] where Id='{0}')", projectid);
                result = conn.Query<OLFieldmodel>(sql).ToList();
            }
            return result;
        }

        /// <summary>
        /// 字段模板的返填
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<OLFieldmodel> GetLFieldmodels(long Id)
        {
            List<OLFieldmodel> result = new List<OLFieldmodel>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[OLtemplate] a left join  [dbo].[OLfieldmodel] b on a.Id=b.templateId where templateId='{0}' order by order_px asc", Id);
                 result = conn.Query<OLFieldmodel>(sql).ToList();
            }
            return result;
        }
        /// <summary>
        /// 修改模板字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int updateOLTemplate(OLFieldmodel model)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" Update OLfieldmodel set  templateId='{0}',field_name='{1}',field_code='{2}',default_value='{3}',created_byId='{4}',InsertTime='{5}',updatetime='{6}',sfbt='{8}' where Id='{7}'", model.templateId, model.field_name, model.field_code, model.default_value, model.created_byId, model.InsertTime, model.Updatetime, model.Id,model.sfbt);
                int result = conn.Execute(sql);
                return result;
            }
        }
    }
}
