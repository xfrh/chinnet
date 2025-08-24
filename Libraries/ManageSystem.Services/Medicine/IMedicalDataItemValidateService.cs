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
    /// 操作接口类 ，数据库表名：MedicalDataItemValidate
    /// </summary>
    public partial interface IMedicalDataItemValidateService : IBaseService<MedicalDataItemValidate>
    {

        /// <summary>
        /// 根据医学数据Id获取对应的错误分页数据
        /// </summary>
        /// <param name="medicalDataId">对应的医学数据Id</param>
        /// <param name="level">0：表示全部  1：提示  2：警告  3：错误</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MedicalDataItemValidate> QueryPage(long medicalDataId, int level, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
