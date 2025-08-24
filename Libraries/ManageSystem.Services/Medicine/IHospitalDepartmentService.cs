using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;

namespace ManageSystem.Services.Medicine
{
	/// <summary>
	/// 操作接口类 ，数据库表名：HospitalDepartment 
	/// </summary>
	public  partial interface IHospitalDepartmentService : IBaseService<HospitalDepartment>
	{
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<HospitalDepartment> QueryPage(string name, string stateValue, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 根据医院ID获取对应科室
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        List<HospitalDepartment> GetHospitalDepartments(long hospitalId);
    }
}
