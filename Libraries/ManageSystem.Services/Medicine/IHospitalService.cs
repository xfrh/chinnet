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
    /// 操作接口类 ，数据库表名：Hospital 
    /// </summary>
    public partial interface IHospitalService : IBaseService<Hospital>
    {
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Hospital> QueryPage(string name, string stateValue, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stateValue"></param>
        /// <param name="isTeam"></param>
        /// <param name="contacts"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Hospital> QueryPage(string name, int? stateValue, int? isTeam, string contacts, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 查询所有参与CR项目的医院，用户登录帐号是使用：CRFW 开头的医院
        /// </summary>
        /// <returns></returns>
        List<Hospital> QueryListCR();

        /// <summary>
        /// 查询所有医院
        /// </summary>
        /// <returns></returns>
        List<Hospital> QueryList();


        IPagedList<Hospital> QueryList(string ProjectType, int pageIndex = 0, int pageSize = int.MaxValue);

    }

    /// <summary>
    ///  操作接口类 ，多中心研究：数据表名：ProjectHospital
    /// </summary>
    public partial interface IProjectHospitalService : IBaseService<ProjectHospital>
    {

    }
}
