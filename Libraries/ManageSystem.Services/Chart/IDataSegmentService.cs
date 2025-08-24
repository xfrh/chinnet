using ManageSystem.Core;
using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public interface IDataSegmentService : IBaseService<Chart_DataSegment>
    {
        /// <summary>
        /// 删除数据段
        /// </summary>
        /// <param name="id">Chart_DataSegment.Id</param>
        /// <returns></returns>
        bool OnDeleteByHeatmapWithTransaction(long id);

        /// <summary>
        /// 删除数据段
        /// </summary>
        /// <param name="id">Chart_DataSegment.Id</param>
        /// <returns></returns>
        bool OnDeleteByBarChartWithTransaction(long id);

        /// <summary>
        /// 删除数据段
        /// </summary>
        /// <param name="id">Chart_DataSegment.Id</param>
        /// <returns></returns>
        bool OnDeleteByTrendChartWithTransaction(long id);
    }
}
