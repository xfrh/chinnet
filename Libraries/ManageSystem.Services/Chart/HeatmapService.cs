using Dapper;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Chart
{
    public class HeatmapService : BaseService<Chart_Heatmap>, IHeatmapService
    {
        private readonly IHeatmapItemService HeatmapItemService;
        private readonly ISystemLogService SystemLogService;

        public HeatmapService(IRepository<Chart_Heatmap> repository, IHeatmapItemService heatmapItemService, ISystemLogService systemLogService) : base(repository)
        {
            HeatmapItemService = heatmapItemService;
            SystemLogService = systemLogService;
        }

        public int NextSort()
        {
            if (base.Count(r => r.Mark > 0) == 0)
            {
                return 1;
            }

            string sql = $@"SELECT TOP(1) [t2].[Value] FROM (
	SELECT T.*,ROW_NUMBER() OVER (ORDER BY [Sort] ASC) AS [Value]  FROM (
		SELECT 
			[Sort], 
			ROW_NUMBER() OVER(PARTITION BY [Sort] ORDER BY [Sort] ASC) AS [SortGroupIdx] 
		FROM dbo.[Chart_Heatmap] 
	WHERE [Mark] > 0
	) AS T
	WHERE T.SortGroupIdx = 1
) AS [t2] WHERE [t2].[Sort] <> [t2].[Value];";
            int sort = DapperHelper.GetConnection().ExecuteScalar<int?>(sql, null) ?? 0;
            if (sort > 0)
            {
                return sort;
            }

            return DapperHelper.GetConnection().ExecuteScalar<int>("SELECT ISNULL(MAX([Sort]), 0) + 1  AS [Value] FROM dbo.Chart_Heatmap WHERE [Mark] > 0;", null);
        }

        public bool OnCreateHeatmapByTransaction(Chart_Heatmap heatmap, List<Chart_HeatmapItem> heatmapItems)
        {
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        dbContext.Set<Chart_Heatmap>().Add(heatmap);
                        int sort = 1;
                        heatmapItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = heatmap.InsertTime;
                            item.UpdateTime = heatmap.InsertTime;
                            item.HeatmapId = heatmap.Id;
                            item.Sort = sort++;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = heatmap.DeleteTime;
                            dbContext.Set<Chart_HeatmapItem>().Add(item);
                        });

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

        public bool OnDeleteHeatmapByTransaction(long id)
        {
            if (id <= 0) return false;

            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        Chart_Heatmap entity = dbContext.Set<Chart_Heatmap>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_HeatmapItem> heatmapItems = dbContext.Set<Chart_HeatmapItem>().Where(r => r.HeatmapId == entity.Id);
                        foreach (Chart_HeatmapItem item in heatmapItems)
                        {
                            item.Mark = 0;
                            item.DeleteTime = entity.DeleteTime;
                            item.Describe = "报表同步删除";
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

        public bool OnEditHeatmapByTransaction(Chart_Heatmap heatmap, List<Chart_HeatmapItem> heatmapItems)
        {
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        // 删除旧数据
                        IQueryable<Chart_HeatmapItem> oldHeatmapItems = dbContext.Set<Chart_HeatmapItem>().Where(r => r.HeatmapId == heatmap.Id);
                        foreach (Chart_HeatmapItem item in oldHeatmapItems)
                        {
                            dbContext.Set<Chart_HeatmapItem>().Remove(item);
                        }

                        // 添加新数据
                        int sort = 1;
                        heatmapItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = heatmap.UpdateTime;
                            item.UpdateTime = heatmap.UpdateTime;
                            item.HeatmapId = heatmap.Id;
                            item.Sort = sort++;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = heatmap.DeleteTime;
                            dbContext.Set<Chart_HeatmapItem>().Add(item);
                        });

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
        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int UpdateSort(int sort)
        {
            string sql = string.Format("update Chart_Heatmap set sort=sort+1 where sort>='{0}'", sort);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
