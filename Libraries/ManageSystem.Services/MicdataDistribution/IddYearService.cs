using ManageSystem.Core;
using ManageSystem.Core.Domain.MicdataDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.MicdataDistribution
{
    public partial interface IddYearService
    {
        List<ddYear> GetDdYears();

        string GetDdYear(long yearid);


        IPagedList<ddYear> GetddYears(string title, int pageIndex = 0, int pageSize = 2147483647);

        int insertYears(ddYear model);
        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int UpdateSort(int sort);

        /// <summary>
        /// 查询排序号是存在
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        ddYear GetSort(int sort);

        /// <summary>
        /// 根据id查询细菌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ddYear QueryEntity(long id);

        /// <summary>
        /// 编辑年份
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateddYears(ddYear model);

        /// <summary>
        ///根据条件删除数据
        /// </summary>
        /// <param name="ids"></param>
        int Delete(string ids);
    }
}
