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
	/// 操作接口类 ，数据库表名：DoctorTitle 
	/// </summary>
	public  partial interface IDoctorTitleService : IBaseService<DoctorTitle>
	{
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DoctorTitle> QueryPage(string name, string stateValue, int pageIndex = 0, int pageSize = int.MaxValue);

        List<DoctorTitle> QueryByParentId(long parentId);
    }
}
