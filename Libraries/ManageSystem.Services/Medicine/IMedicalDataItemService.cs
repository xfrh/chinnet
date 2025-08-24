using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Teams;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MedicalDataItem 
    /// </summary>
    public partial interface IMedicalDataItemService : IBaseService<MedicalDataItem>
    {
        /// <summary>
        /// 根据医学数据Id获取对应的分页数据
        /// </summary>
        /// <param name="medicalDataId">对应的医学数据Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MedicalDataItem> QueryPage(long medicalDataId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 根据医学数据的Id，产生Excel数据，成功返回文件的相对路径
        /// </summary>
        /// <param name="entity">医学数据</param>
        ///  <param name="hospital">所属医院</param>
        /// <returns></returns>
        string GetExcel(MedicalData entity, HospitalTeamList hospitalTeam);

        /// <summary>
        /// 根据医学数据的Id，生产DBF文件数据，成功返回文件的相对路径
        /// </summary>
        /// <param name="entity">医学数据</param>
        /// <param name="hospital">所属医院</param>
        /// <returns></returns>
        string GetDBF(MedicalData entity, HospitalTeamList hospitalTeam);

        /// <summary>
        /// 生成新的容错文件的路径
        /// </summary>
        /// <param name="entity">上传的医学数据</param>
        /// <param name="hospital">所属医院</param>
        /// <returns></returns>
        string GetNewFilePath(MedicalData entity, HospitalTeamList hospitalTeam);

    }
}
