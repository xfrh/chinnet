using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Services.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public class TrendChartWithItemDataService : BaseService<Chart_TrendChartWithItemData>, ITrendChartWithItemDataService
    {
        private readonly ISystemLogService SystemLogService;

        public TrendChartWithItemDataService(IRepository<Chart_TrendChartWithItemData> repository, ISystemLogService systemLogService) : base(repository)
        {
            SystemLogService = systemLogService;
        }
    }
}
