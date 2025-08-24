using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.DataInput;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Data;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial class OLProjectService : BaseService<OLProject>,IOLProjectService
    {
        public OLProjectService(IRepository<OLProject> repository

           ) : base(repository)
        {

        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>   
        public int AddOLProject(OLProject model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into OLProject values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",model.Id,model.templateId,model.project_name,model.starttime,model.endtime,model.state,model.created_byId,model.InsertTime,model.Updatetime);
                result = conn.Execute(sql);
            }
            return result;
        }
        /// <summary>
        /// 查询用户绑定数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int CountProjectMember(long Id)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select COUNT(0) as counts from [dbo].[OLuserproject] where userid='{0}'",Id);
                result = conn.Query<OLUserproject>(sql).ToList().FirstOrDefault().Counts;
            }
            return result;
        }

        /// <summary>
        /// 给项目绑定用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateProjectMember(OLUserproject model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into OLuserproject values('{0}','{1}','{2}','{3}','{4}','{5}')", model.Id,model.projectid,model.userid,model.created_byId, model.InsertTime, model.Updatetime);
                result = conn.Execute(sql);
            }
            return result;
        }

        /// <summary>
        /// 根据删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int delete(long Id)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {             
                    string sql = string.Format("delete from OLProject where id = '{0}'", Id);
                    result = conn.Execute(sql);                    
            }
            return result;
        }

        /// <summary>
        /// 删除项目绑定的用户
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int deleteProjectMember(long Id, long userId)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("delete from OLuserproject where projectid = '{0}' and userid='{1}'", Id,userId);
                result = conn.Execute(sql);
            }
            return result;
        }

        /// <summary>
        /// 查询病原菌
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public List<OLDataInput> Getgermname(long projectid, long createdId)
        {
            List<OLDataInput> result = new List<OLDataInput>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select germname,count(germname)sums from [OLdataInput] where projectid ='{0}' and created_byId='{1}'  group by germname", projectid,createdId);
                result = conn.Query<OLDataInput>(sql).ToList();
            }
            return result;
        }

        /// <summary>
        /// 成员的项目列表级下拉返填
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<OLProject> GetLProjects(long memberId,long projecId)
        {
            List<OLProject> result = new List<OLProject>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (projecId > 0)
                {

                    string sql = string.Format("select  * from  OLproject where Id in (select projectid from  OLuserproject where userid='{0}' and projectid='{1}')", memberId, projecId);
                    result = conn.Query<OLProject>(sql).ToList();
                  
                }
                else
                {
                    string sql = string.Format("select  * from  OLproject where Id in (select projectid from  OLuserproject where userid='{0}')", memberId);
                    result = conn.Query<OLProject>(sql).ToList();
                }
            }
            return result;
        }

        /// <summary>
        /// 查询项目是否有数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<OLDataInput> GetOLDataInput(long Id)
        {
            List<OLDataInput> result = new List<OLDataInput>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select* from[dbo].[OLdataInput] where projectid='{0}'", Id);
                result = conn.Query<OLDataInput>(sql).ToList();
            }
            return result;
        }


        /// <summary>
        /// 管理员的项目列表
        /// </summary>
        /// <returns></returns>
        public List<OLProject> GetOLProjects(long projecId)
        {
            List<OLProject> result = new List<OLProject>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (projecId > 0)
                {
                    string sql = string.Format("select a.*,b.Name from OLproject a left join Member b on  a.created_byId=b.Id where a.Id='{0}'",projecId);
                    result = conn.Query<OLProject>(sql).ToList();
                } else
                {
                    string sql = string.Format("select a.*,b.Name from OLproject a left join Member b on  a.created_byId=b.Id");
                    result = conn.Query<OLProject>(sql).ToList();
                }
            }
            return result;
        }

        /// <summary>
        /// 查询项目是否已绑定模板是否有数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OLDataInput GetProjecttemData(long? projectid)
        {
            OLDataInput result = new OLDataInput();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from  OLdataInput where projectid='{0}'", projectid);
                result = conn.Query<OLDataInput>(sql).ToList().FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// 查询项目是否已绑定模板
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OLProject GetProjecttem(long? projectid)
        {
            OLProject result = new OLProject();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from  OLproject where Id='{0}'", projectid);
                result = conn.Query<OLProject>(sql).ToList().FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// 根据字段模板id查询模板是否绑定项目
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>

        public OLProject GetProjecttemp(long? tempId)
        {
            OLProject result = new OLProject();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from  OLproject where templateId ='{0}'", tempId);
                result = conn.Query<OLProject>(sql).ToList().FirstOrDefault();
            }
            return result;
        }
        /// <summary>
        /// 每个项目数据的数量
        /// </summary>
        /// <returns></returns>
        public List<OLProject> Project()
        {
            List<OLProject> result = new List<OLProject>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  a.project_name,b.projectid Id,c.Name,b.created_byId, count(b.created_byId) projectsums   from ([dbo].[OLproject] a left join [OLdataInput]b on a.Id=b.projectid) left join Member c on b.created_byId=c.Id group by b.created_byId, b.projectid, a.project_name,c.Name");
                //string sql = string.Format("select a.project_name,b.projectid Id, count(projectid) projectsums   from [dbo].[OLproject] a left join [OLdataInput]b on a.Id=b.projectid group by b.projectid, a.project_name");
                result = conn.Query<OLProject>(sql).ToList();
            }
            return result;
        }
        /// <summary>
        /// 每个项目数据已审核的数量
        /// </summary>
        /// <returns></returns>
        public List<OLProject> Projectauditor(long projectid, long createby_Id)
        {
            List<OLProject> result = new List<OLProject>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select created_byId, count(auditor)as auditorsums from [dbo].[OLdataInput]  where auditor !=''and  projectid='{0}' and created_byId='{1}' group by created_byId", projectid, createby_Id);
                //string sql = string.Format("select created_byId, count(auditor)as auditorsums from [dbo].[OLdataInput]  where auditor !=''and  projectid='{0}' group by created_byId", projectid);
                //string sql = string.Format("select count(auditor)as auditorsums from [dbo].[OLdataInput]  where auditor !=''and  projectid='{0}'", projectid);
                result = conn.Query<OLProject>(sql).ToList();
            }
            return result;
        }

        /// <summary>
        /// 修改用户上传数据权限
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="privil"></param>
        /// <returns></returns>
        public int UpdateMember(long Id,string privil)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("update Member set Median='{0}'where Id = '{1}'", privil, Id);
                result = conn.Execute(sql);
            }
            return result;
        }


        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int updateOLProject(OLProject model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("update OLProject set project_name='{0}',starttime='{1}',endtime='{2}',state='{3}',UpdateTime='{4}',templateId='{5}' where Id = '{6}'", model.project_name,model.starttime,model.endtime,model.state, model.Updatetime, model.templateId,model.Id);
                result = conn.Execute(sql);
            }
            return result;
        }

       

        /// <summary>
        /// /编辑项目返填
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<OLProject> UPOLProjects(long Id)
        {
            List<OLProject> result = new List<OLProject>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from OLproject  where Id='{0}'", Id);
                result = conn.Query<OLProject>(sql).ToList();
            }
            return result;
        }
    }
}
