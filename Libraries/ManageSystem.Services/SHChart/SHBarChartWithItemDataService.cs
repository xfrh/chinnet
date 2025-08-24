using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Domain.SHChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.SHChart
{
   public class SHBarChartWithItemDataService : BaseService<Chart_SHBarChartWithItemData>, ISHBarChartWithItemDataService
    {
        public SHBarChartWithItemDataService(IRepository<Chart_SHBarChartWithItemData> repository) : base(repository)
        {
        }
    }
}
