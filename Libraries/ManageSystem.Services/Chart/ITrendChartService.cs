using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public interface ITrendChartService : IBaseService<Chart_TrendChart>
    {
        /// <summary>
        /// 获取下一个排序值
        /// </summary>
        /// <param name="dataSegmentId">
        /// 数据段Id<br />
        /// Chart_DataSegment.Id
        /// </param>
        /// <returns></returns>
        int NextSort(long dataSegmentId);

        /// <summary>
        /// 新增趋势图报表
        /// </summary>
        /// <param name="mainEntity"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        bool OnCreateByTransaction(Chart_TrendChart mainEntity, List<Chart_TrendChartWithItemData> dataItems);

        /// <summary>
        /// 编辑趋势图报表
        /// </summary>
        /// <param name="mainEntity"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        bool OnEditByTransaction(Chart_TrendChart mainEntity, List<Chart_TrendChartWithItemData> dataItems);

        /// <summary>
        /// 删除趋势图数据报表
        /// </summary>
        /// <param name="id">Chart_BarChart.Id</param>
        /// <returns></returns>
        bool OnDeleteByTransaction(long id);

        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int UpdateSort(int sort);
    }
}
