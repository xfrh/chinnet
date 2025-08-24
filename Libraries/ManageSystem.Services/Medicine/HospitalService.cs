using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;
using ManageSystem.Data;
using Dapper;
using System.Data.SqlClient;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作类 ，数据库表名：Hospital 
    /// </summary>
    public partial class HospitalService : BaseService<Hospital>, IHospitalService
    {

        public HospitalService(IRepository<Hospital> repository) : base(repository)
        {

        }

        public IPagedList<Hospital> QueryPage(string name, string state, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(state))
            {
                int stateTemp = int.Parse(state);
                query = query.Where(m => m.State == (stateTemp == 1));
            }

            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<Hospital>(query.ToList(), pageIndex, pageSize);

            return list;
        }

        public IPagedList<Hospital> QueryPage(string name, int? stateValue, int? isTeam, string contacts, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (stateValue != null)
            {
                query = query.Where(m => m.State == (stateValue.Value == 1));
            }

            if (isTeam != null)
            {
                query = query.Where(m => m.IsTeam == (isTeam.Value == 1));
            }

            if (!string.IsNullOrWhiteSpace(contacts))
            {
                query = query.Where(m => m.ContactsUser.Contains(contacts));
            }

            query = query.OrderBy(m => m.Sort);

            var list = new PagedList<Hospital>(query.ToList(), pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 查询所有参与CR项目的医院，用户登录帐号是使用：CRFW 开头的医院
        /// </summary>
        /// <returns></returns>
        public List<Hospital> QueryListCR()
        {
            string sql = @" SELECT * FROM  dbo.Hospital WHERE Mark > 0 AND ID IN 
                                    (SELECT  HospitalId FROM  dbo.Member WHERE Mark > 0 AND LoginId LIKE 'CRFW%') ";

            return DapperHelper.GetConnection().Query<Hospital>(sql, null).ToList();
        }

        public List<Hospital> QueryList()
        {
            string sql = @" SELECT * FROM  dbo.Hospital WHERE Mark > 0";
            return DapperHelper.GetConnection().Query<Hospital>(sql, null).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectType"></param>
        /// <returns></returns>
        public IPagedList<Hospital> QueryList(string ProjectType, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select  a.* from (select HospitalId from [dbo].[Member]  where ProjectUploadItem='{0}' and Status=2 and Mark>0 group by HospitalId) b left join  Hospital a  on a.Id = b.HospitalId ", ProjectType);
                IEnumerable<Hospital> result = conn.Query<Hospital>(sql.ToString());
                IEnumerable<Hospital> query = from m in result
                                          select m;              
                return new PagedList<Hospital>(query.ToList(),pageIndex, pageSize);
            }
        }
    }

    /// <summary>
    /// 操作类 ，多中心研究，数据库表名：Hospital
    /// </summary>
    public partial class ProjectHospitalService : BaseService<ProjectHospital>, IProjectHospitalService
    {
        public ProjectHospitalService(IRepository<ProjectHospital> repository) : base(repository)
        {

        }
    }
}
