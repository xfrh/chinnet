using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Teams;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Teams
{
    public class TeamService : BaseService<Team>, ITeamService
    {
        public TeamService(IRepository<Team> repository) : base(repository)
        {
        }
        /// <summary>
        /// 添加成员表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddTeam(Team model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into Team values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}' )", model.Hospitalid, model.Title, model.People, model.Images, model.Displayimages, model.classifyId, model.InsertTime, model.Createby_Id, model.project_code, model.UpdateTime);
                result = conn.Execute(sql);
            }
            return result;
        }
        /// <summary>
        /// 删除成员单位
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        public int deleteTeam(string TeamId)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("delete from Team where Id  in(" + TeamId + ")");
                result = conn.Execute(sql);
            }
            return result;
        }

        public List<Teamclassify> GetTeamclassifies()
        {
            List<Teamclassify> list = new List<Teamclassify>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format(" select * from Teamclassify order by classifyname");
                IEnumerable<Teamclassify> result = conn.Query<Teamclassify>(sql);
                list = result.ToList();
            }
            return list;
        }

        /// <summary>
        /// 前台CHINET成员单位查询
        /// </summary>
        /// <returns></returns>
        public List<Team> GetTeams()
        {
            List<Team> list = new List<Team>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from  Team where classifyId=1  order by Title");
                IEnumerable<Team> result = conn.Query<Team>(sql);
                list = result.ToList();
            }
            return list;
        }

        public List<Team> GetTeamsSatellite(string name)
        {
            List<Team> list = new List<Team>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select a.* from Team a left join Teamclassify b on a.classifyId=b.Id where b.classifyname = '"+ name + "'");
                IEnumerable<Team> result = conn.Query<Team>(sql);
                list = result.ToList();
            }
            return list;
        }

        /// <summary>
        /// 前台上海网成员单位查询
        /// </summary>
        /// <returns></returns>
        public List<Team> GetSHTeams()
        {
            List<Team> list = new List<Team>();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from  Team where classifyId=2  order by Title");
                IEnumerable<Team> result = conn.Query<Team>(sql);
                list = result.ToList();
            }
            return list;
        }

        public int CreateTeamClassfy(string satelittedName)
        {
            try
            {
                int result = 0;
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format("insert into Teamclassify (classifyname,createby_Id,InsertTime,UpdateTime) values('" + satelittedName + "',1,'" + DateTime.Now.ToString("G") + "','" + DateTime.Now.ToString("G") + "')");
                    result = conn.Execute(sql);
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        /// <summary>
        /// 根据id查询一个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Team GetTeamsbyid(long id)
        {
            Team result = new Team();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from Team where Id='{0}'", id);
                result = conn.Query<Team>(sql).FirstOrDefault();

            }
            return result;
        }
        /// <summary>
        /// 查询用户
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

        /// <summary>
        /// 分页查询成员单位
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Team> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select  * from  Team where classifyId=1  order by Title ";
                IEnumerable<Team> result = conn.Query<Team>(sql.ToString());
                IEnumerable<Team> query = from m in result
                                          select m;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = from m in query
                            where m.Title.Contains(name)
                            select m;
                }
                return new PagedList<Team>(query.ToList(), pageIndex, pageSize);
            }
        }
        /// <summary>
        /// 修改成员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateTeam(Team model)
        {
            int result = 0;
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                if (model.Images == null)
                {
                    string sql = string.Format("Update Team set Hospitalid='{0}',Title='{1}',People='{2}',Displayimages='{3}',classifyId='{4}',UpdateTime='{5}',project_code='{6}' where Id='{7}'", model.Hospitalid, model.Title, model.People, model.Displayimages, model.classifyId, model.UpdateTime, model.project_code, model.Id);
                    result = conn.Execute(sql);
                }
                else
                {
                    string sql = string.Format("Update Team set Hospitalid='{0}',Title='{1}',People='{2}',Images='{3}',Displayimages='{4}',classifyId='{5}',UpdateTime='{6}',project_code='{7}' where Id='{8}'", model.Hospitalid, model.Title, model.People, model.Images, model.Displayimages, model.classifyId, model.UpdateTime, model.project_code, model.Id);
                    result = conn.Execute(sql);
                }

            }
            return result;
        }

        public IPagedList<Team> SHQueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select  * from  Team where classifyId=2  order by Title ";
                IEnumerable<Team> result = conn.Query<Team>(sql.ToString());
                IEnumerable<Team> query = from m in result
                                          select m;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = from m in query
                            where m.Title.Contains(name)
                            select m;
                }
                return new PagedList<Team>(query.ToList(), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 根据医院id查询对象
        /// </summary>
        /// <param name="HospitalId"></param>
        /// <returns></returns>
        public Team GetTeamsbyHospitalId(long HospitalId)
        {
            Team result = new Team();
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  * from Team where Hospitalid='{0}'", HospitalId);
                //string sql = string.Format("select  * from Team where classifyId={0} and  Hospitalid='{1}'", HospitalId);
                result = conn.Query<Team>(sql).FirstOrDefault();

            }
            return result;
        }

        /// <summary>
        /// 分页查询成员单位
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Team> TeamList()
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = @"select  t.*,h.ProvinceId,c.ShortName as ProvinceName  from  Team t left join Hospital h on h.Id=t.Hospitalid left join Area c on h.ProvinceId = c.Id where t.classifyId=1 order by t.Title ";
                IEnumerable<Team> result = conn.Query<Team>(sql.ToString());

                return result.ToList();
            }
        }
    }
}
