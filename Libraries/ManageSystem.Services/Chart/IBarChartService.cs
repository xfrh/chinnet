using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public interface IBarChartService : IBaseService<Chart_BarChart>
    {
        /// <summary>
        /// 获取下一个排序值
        /// </summary>
        /// <returns></returns>
        int NextSort(long dataSegmentId);

        /// <summary>
        /// 新增柱状图报表
        /// </summary>
        /// <param name="barChart"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        bool OnCreateBarChartByTransaction(Chart_BarChart barChart, List<Chart_BarChartWithItemData> dataItems);

        /// <summary>
        /// 编辑柱状图报表
        /// </summary>
        /// <param name="barChart"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        bool OnEditBarChartByTransaction(Chart_BarChart barChart, List<Chart_BarChartWithItemData> dataItems);

        /// <summary>
        /// 删除柱状图数据报表
        /// </summary>
        /// <param name="id">Chart_BarChart.Id</param>
        /// <returns></returns>
        bool OnDeleteBarChartByTransaction(long id);

        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int UpdateSort(int sort);
    }
}
