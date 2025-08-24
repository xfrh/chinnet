using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Domain.SHChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.SHChart
{
    public interface ISHBarChartService : IBaseService<Chart_SHBarChart>
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
        bool OnCreateBarChartByTransaction(Chart_SHBarChart barChart, List<Chart_SHBarChartWithItemData> dataItems);

        /// <summary>
        /// 编辑柱状图报表
        /// </summary>
        /// <param name="barChart"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        bool OnEditBarChartByTransaction(Chart_SHBarChart barChart, List<Chart_SHBarChartWithItemData> dataItems);


        /// <summary>
        /// 添加图表
        /// </summary>
        /// <param name="barChart"></param>
        /// <returns></returns>
        bool OnCreateTableTransaction(Chart_SHBarChart barChart);
        /// <summary>
        /// 编辑图表
        /// </summary>
        /// <param name="barChart"></param>
        /// <returns></returns>
        bool OnEditTableTransaction(Chart_SHBarChart barChart);

        /// <summary>
        /// 删除表格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool OnDeleteTableTransaction(long id);

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

        /// <summary>
        /// 查询表格
        /// </summary>
        /// <returns></returns>
        List<Chart_SHBarChart> Get_SHBarChartsTable(long Id);
    }
}
