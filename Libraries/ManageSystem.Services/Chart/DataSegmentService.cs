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
    public class DataSegmentService : BaseService<Chart_DataSegment>, IDataSegmentService
    {
        private readonly IHeatmapService HeatmapService;
        private readonly ISystemLogService SystemLogService;
        public DataSegmentService(IRepository<Chart_DataSegment> repository, IHeatmapService heatmapService, ISystemLogService systemLogService) : base(repository)
        {
            HeatmapService = heatmapService;
            SystemLogService = systemLogService;
        }

        public bool OnDeleteByBarChartWithTransaction(long id)
        {
            if (id <= 0) return false;
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        Chart_DataSegment entity = dbContext.Set<Chart_DataSegment>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_BarChart> mainEntity = dbContext.Set<Chart_BarChart>().Where(r => r.DataSegmentId == entity.Id && r.Mark > 0);
                        foreach (Chart_BarChart item in mainEntity)
                        {
                            item.Mark = 0;
                            item.DeleteTime = entity.DeleteTime;
                            item.Name = $"{item.Name}_DELETE";
                            item.Describe = "删除数据段时同步删除数据报表";
                            IQueryable<Chart_BarChartWithItemData> dataItems = dbContext.Set<Chart_BarChartWithItemData>().Where(r => r.BarChartId == item.Id && r.Mark > 0);
                            foreach (Chart_BarChartWithItemData inner in dataItems)
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

        public bool OnDeleteByHeatmapWithTransaction(long id)
        {
            if (id <= 0) return false;
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        Chart_DataSegment entity = dbContext.Set<Chart_DataSegment>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_Heatmap> heatmaps = dbContext.Set<Chart_Heatmap>().Where(r => r.DataSegmentId == entity.Id && r.Mark > 0);
                        foreach (Chart_Heatmap item in heatmaps)
                        {
                            item.Mark = 0;
                            item.DeleteTime = entity.DeleteTime;
                            item.Name = $"{item.Name}_DELETE";
                            item.Describe = "删除数据段时同步删除数据报表";
                            IQueryable<Chart_HeatmapItem> heatmapItems = dbContext.Set<Chart_HeatmapItem>().Where(r => r.HeatmapId == item.Id && r.Mark > 0);
                            foreach (Chart_HeatmapItem inner in heatmapItems)
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

        public bool OnDeleteByTrendChartWithTransaction(long id)
        {
            if (id <= 0) return false;
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        Chart_DataSegment entity = dbContext.Set<Chart_DataSegment>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_TrendChart> mainEntity = dbContext.Set<Chart_TrendChart>().Where(r => r.DataSegmentId == entity.Id && r.Mark > 0);
                        foreach (Chart_TrendChart item in mainEntity)
                        {
                            item.Mark = 0;
                            item.DeleteTime = entity.DeleteTime;
                            item.Name = $"{item.Name}_DELETE";
                            item.Describe = "删除数据段时同步删除数据报表";
                            IQueryable<Chart_TrendChartWithItemData> dataItems = dbContext.Set<Chart_TrendChartWithItemData>().Where(r => r.TrendChartId == item.Id && r.Mark > 0);
                            foreach (Chart_TrendChartWithItemData inner in dataItems)
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
