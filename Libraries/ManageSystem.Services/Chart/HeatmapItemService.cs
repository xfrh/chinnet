using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public class HeatmapItemService : BaseService<Chart_HeatmapItem>, IHeatmapItemService
    {
        public HeatmapItemService(IRepository<Chart_HeatmapItem> repository) : base(repository)
        {

        }
    }
}
