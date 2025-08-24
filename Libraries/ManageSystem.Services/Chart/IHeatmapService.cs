using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public interface IHeatmapService : IBaseService<Chart_Heatmap>
    {
        /// <summary>
        /// 获取下一个排序值
        /// </summary>
        /// <returns></returns>
        int NextSort();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="heatmap"></param>
        /// <param name="heatmapItems"></param>
        /// <returns></returns>
        bool OnCreateHeatmapByTransaction(Chart_Heatmap heatmap, List<Chart_HeatmapItem> heatmapItems);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        bool OnEditHeatmapByTransaction(Chart_Heatmap heatmap, List<Chart_HeatmapItem> heatmapItems);

        /// <summary>
        /// 删除热图数据报表
        /// </summary>
        /// <param name="id">Chart_Heatmap.Id</param>
        /// <returns></returns>
        bool OnDeleteHeatmapByTransaction(long id);

        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int UpdateSort(int sort);
    }
}
