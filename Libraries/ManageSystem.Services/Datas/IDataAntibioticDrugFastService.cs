using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Datas;

namespace ManageSystem.Services.Datas
{
    /// <summary>
    /// 操作接口类 ，数据库表名：DataAntibioticDrugFast 
    /// </summary>
    public partial interface IDataAntibioticDrugFastService : IBaseService<DataAntibioticDrugFast>
    {
        /// <summary>
        /// 根据细菌名称和抗生素的名称查询数据
        /// </summary>
        /// <param name="antibioticName">抗生素名称</param>
        /// <param name="germName">细菌名称</param>
        /// <returns></returns>
        List<DataAntibioticDrugFast> Query(string antibioticName, string germName);

        /// <summary>
        /// 获取所有的细菌名称
        /// </summary>
        /// <returns></returns>
        List<string> QueryGerm();

        /// <summary>
        /// 获取所有的抗生素名称
        /// </summary>
        /// <returns></returns>
        List<string> QueryAntibiotic();

    }
}
