using Dapper;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.DataInput;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial class OLMemberService : BaseService<MemberHospital>, IOLMemberService
    {
        public OLMemberService(IRepository<MemberHospital> repository

         ) : base(repository)
        {

        }
        /// <summary>
        /// 查询用户项目关联表
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetOLuserproject(long? projectid, long userId)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select COUNT(0) counts from [dbo].[OLuserproject] where userid='{0}'and projectid='{1}'",userId, projectid);
                result = conn.Query<MemberHospital>(sql).ToList().FirstOrDefault().counts;
            }
            return result;
        }

        public List<MemberHospital> Querylist(string LoginId, string phone, string hospital)
        {
            //var query = this._repository.Table.Where(m=>m.Mark>0);
            IEnumerable<MemberHospital> query =null;

            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.Id,a.NickName,a.Name,a.LoginId,a.Phone,b.Name as HospitalName,a.HospitalId,a.Median  from  Member a left join Hospital b on  a.HospitalId=b.Id where a.Mark>0");
                query = conn.Query<MemberHospital>(sql);
            }        
            if (!string.IsNullOrWhiteSpace(LoginId))
                query = query.Where(m => m.LoginId.Contains(LoginId.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(m => m.Phone != "" && m.Phone != null);
                query = query.Where(m => m.Phone.Contains(phone.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(hospital))
            {
                using (var conn = DapperHelper.GetConnection())
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    List<long> hospitalIds = conn.Query<long>("SELECT Id FROM dbo.Hospital WHERE [Name] LIKE @HospitalName AND [Mark] > 0;", new { HospitalName = $"%{hospital}%" }).ToList();
                    query = query.Where(m =>hospitalIds.Contains(m.HospitalId));
                }
            }
            return query.OrderByDescending(m => m.InsertTime).ToList();
        }

        public List<MemberHospital> Queryuserlist1(long? projectid)
        {
            List<MemberHospital> result = new List<MemberHospital>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.Id,a.NickName,a.Name,a.LoginId,a.Phone,b.Name as HospitalName,a.HospitalId,a.Median  from  Member a left join Hospital b on  a.HospitalId=b.Id where a.Mark>0 and a.Id  in (select userid from [dbo].[OLuserproject] where projectid ='{0}')", projectid);
                result = conn.Query<MemberHospital>(sql).ToList();
            }
            return result;
        }

        public List<MemberHospital> Queryuserlist2(long? projectid)
        {
            List<MemberHospital> result = new List<MemberHospital>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.Id,a.NickName,a.Name,a.LoginId,a.Phone,b.Name as HospitalName,a.HospitalId,a.Median  from  Member a left join Hospital b on  a.HospitalId=b.Id where a.Mark>0 and a.Id not in (select userid from [dbo].[OLuserproject] where projectid ='{0}')", projectid);
                result = conn.Query<MemberHospital>(sql).ToList();
            }
            return result;
        }
    }
}
