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
    /// 操作接口类 ，数据库表名：DataGermYear 
    /// </summary>
    public partial interface IDataGermYearService : IBaseService<DataGermYear>
    {
        /// <summary>
        /// 根据细菌名称和年份的名称查询数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="germName">细菌名称</param>
        /// <returns></returns>
        List<DataGermYear> Query(int year, string germName);

        /// <summary>
        /// 获取所有的细菌名称
        /// </summary>
        /// <returns></returns>
        List<string> QueryGerm();

        /// <summary>
        /// 获取所有的年份
        /// </summary>
        /// <returns></returns>
        List<string> QueryYear();

        /// <summary>
        /// 根据阶段名称获取数据
        /// </summary>
        /// <param name="stage">阶段名称</param>
        /// <returns></returns>
        List<DataGermYear> QueryByStage(string stage);

    }
}
