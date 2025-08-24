using ManageSystem.Core;
using ManageSystem.Core.Domain.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Teams
{
   public interface ITeamService : IBaseService<Team>
    {
        /// <summary>
        /// 添加成员单位表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddTeam(Team model);
        /// <summary>
        /// 修改成员单位表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateTeam(Team model);

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        int deleteTeam(string TeamId);

        /// <summary>
        /// 前台CHINET成员单位查询
        /// </summary>
        /// <returns></returns>
        List<Team> GetTeams();

        List<Team> GetTeamsSatellite(string city);

        /// <summary>
        ///前台上海网成员单位查询
        /// </summary>
        /// <returns></returns>
        List<Team> GetSHTeams();


        int CreateTeamClassfy(string satellieCity);
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>      
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Team> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string GetUsers(long userid);

        /// <summary>
        /// 查询分类用作下拉列表
        /// </summary>
        /// <returns></returns>
        List<Teamclassify> GetTeamclassifies();

        /// <summary>
        /// 根据id查询一个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Team GetTeamsbyid(long id);

        /// <summary>
        /// 根据医院id查询一个对象
        /// </summary>
        /// <param name="HospitalId"></param>
        /// <returns></returns>
        Team GetTeamsbyHospitalId(long HospitalId);

        IPagedList<Team> SHQueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 查询成员单位
        /// </summary>
        /// <returns></returns>
        List<Team> TeamList();
    }
}
