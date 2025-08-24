using ManageSystem.Core.Domain.SHChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.SHChart
{
   public interface ISHDataSegmentService : IBaseService<Chart_SHDataSegment>
    {
        

        /// <summary>
        /// 删除数据段
        /// </summary>
        /// <param name="id">Chart_DataSegment.Id</param>
        /// <returns></returns>
        bool OnDeleteByBarSHChartWithTransaction(long id);
       
    }
}
