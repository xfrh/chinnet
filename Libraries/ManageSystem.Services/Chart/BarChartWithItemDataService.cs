using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public class BarChartWithItemDataService : BaseService<Chart_BarChartWithItemData>, IBarChartWithItemDataService
    {
        public BarChartWithItemDataService(IRepository<Chart_BarChartWithItemData> repository) : base(repository)
        {
        }
    }
}
