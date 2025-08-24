using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;
using System.Data.SqlClient;
using ManageSystem.Data;
using Dapper;

namespace ManageSystem.Services.Medicine
{
	/// <summary>
	/// 操作类 ，数据库表名：HospitalDepartment 
	/// </summary>
	public partial class HospitalDepartmentService :  BaseService<HospitalDepartment>, IHospitalDepartmentService
	{

		public HospitalDepartmentService(IRepository<HospitalDepartment> repository): base(repository)
		{
			
		}

        public List<HospitalDepartment> GetHospitalDepartments(long hospitalId)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[HospitalDepartment] where HospitalId=@HospitalId");
                List<HospitalDepartment> ateamodel = conn.Query<HospitalDepartment>(sql, new { HospitalId = hospitalId })?.ToList();
                return ateamodel;
            }
        }

        public IPagedList<HospitalDepartment> QueryPage(string name, string state, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(state))
            {
                int stateTemp = int.Parse(state);
                query = query.Where(m => m.Status == (stateTemp == 1));
            }

            query = query.OrderBy(m => m.Sort);

            var list = new PagedList<HospitalDepartment>(query.ToList(), pageIndex, pageSize);

            return list;
        }

    }
}
