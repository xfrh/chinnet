using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Services.SHChart;
using ManageSystem.Services.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageSystem.Core.Domain.SHChart;

namespace ManageSystem.Services.SHChart
{
   public class SHDataSegmentService : BaseService<Chart_SHDataSegment>, ISHDataSegmentService
    {
        private readonly ISystemLogService SystemLogService;
        public SHDataSegmentService(IRepository<Chart_SHDataSegment> repository, ISystemLogService systemLogService) : base(repository)
        {
            SystemLogService = systemLogService;
        }

        public bool OnDeleteByBarSHChartWithTransaction(long id)
        {
            if (id <= 0) return false;
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        Chart_SHDataSegment entity = dbContext.Set<Chart_SHDataSegment>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_SHBarChart> mainEntity = dbContext.Set<Chart_SHBarChart>().Where(r => r.DataSegmentId == entity.Id && r.Mark > 0);
                        foreach (Chart_SHBarChart item in mainEntity)
                        {
                            item.Mark = 0;
                            item.DeleteTime = entity.DeleteTime;
                            item.Name = $"{item.Name}_DELETE";
                            item.Describe = "删除数据段时同步删除数据报表";
                            IQueryable<Chart_SHBarChartWithItemData> dataItems = dbContext.Set<Chart_SHBarChartWithItemData>().Where(r => r.SHBarChartId == item.Id && r.Mark > 0);
                            foreach (Chart_SHBarChartWithItemData inner in dataItems)
                            {
                                inner.Mark = 0;
                                inner.DeleteTime = item.DeleteTime;
                                inner.Describe = "报表同步删除";
                            }
                        }

                        dbContext.SaveChanges();

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, Core.Domain.Log.SystemLogLevel.Error);
                return false;
            }
        }

       
    }
}
