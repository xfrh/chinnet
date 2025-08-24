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
    public class TrendChartService : BaseService<Chart_TrendChart>, ITrendChartService
    {
        private readonly ISystemLogService SystemLogService;

        public TrendChartService(IRepository<Chart_TrendChart> repository, ISystemLogService systemLogService) : base(repository)
        {
            SystemLogService = systemLogService;
        }

        public int NextSort(long dataSegmentId)
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
		FROM dbo.[Chart_TrendChart] 
	WHERE DataSegmentId = {dataSegmentId} AND [Mark] > 0
	) AS T
	WHERE T.SortGroupIdx = 1
) AS [t2] WHERE [t2].[Sort] <> [t2].[Value];";
            int sort = DapperHelper.GetConnection().ExecuteScalar<int?>(sql, null) ?? 0;
            if (sort > 0)
            {
                return sort;
            }

            return DapperHelper.GetConnection().ExecuteScalar<int>($"SELECT ISNULL(MAX([Sort]), 0) + 1  AS [Value] FROM dbo.[Chart_TrendChart] WHERE [DataSegmentId] = {dataSegmentId} AND [Mark] > 0;", null);
        }

        public bool OnCreateByTransaction(Chart_TrendChart mainEntity, List<Chart_TrendChartWithItemData> dataItems)
        {
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        dbContext.Set<Chart_TrendChart>().Add(mainEntity);

                        int sort = 1;
                        dataItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = mainEntity.InsertTime;
                            item.UpdateTime = mainEntity.InsertTime;
                            item.TrendChartId = mainEntity.Id;
                            item.Sort = mainEntity.DataItemType == ChartDataItemType.MultipleData ? sort++ : item.Sort;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = mainEntity.DeleteTime;
                        });
                        dbContext.Set<Chart_TrendChartWithItemData>().AddRange(dataItems);

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

        public bool OnDeleteByTransaction(long id)
        {
            if (id <= 0) return false;

            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        Chart_TrendChart entity = dbContext.Set<Chart_TrendChart>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_TrendChartWithItemData> dataItems = dbContext.Set<Chart_TrendChartWithItemData>().Where(r => r.TrendChartId == entity.Id && r.Mark > 0);
                        foreach (Chart_TrendChartWithItemData item in dataItems)
                        {
                            item.Mark = 0;
                            item.DeleteTime = entity.DeleteTime;
                            item.Describe = "报表同步删除";
                            item.Name = $"{item.Name}_DELETE";
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

        public bool OnEditByTransaction(Chart_TrendChart mainEntity, List<Chart_TrendChartWithItemData> dataItems)
        {
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        mainEntity.Mark = 2;
                        mainEntity.UpdateTime = DateTime.Now;
                        mainEntity.Version += 1;

                        // 删除旧数据
                        IQueryable<Chart_TrendChartWithItemData> oldDataItems = dbContext.Set<Chart_TrendChartWithItemData>().Where(r => r.TrendChartId == mainEntity.Id);
                        foreach (Chart_TrendChartWithItemData item in oldDataItems)
                        {
                            dbContext.Set<Chart_TrendChartWithItemData>().Remove(item);
                        }

                        // 添加新数据
                        int sort = 1;
                        dataItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = mainEntity.InsertTime;
                            item.UpdateTime = mainEntity.InsertTime;
                            item.TrendChartId = mainEntity.Id;
                            item.Sort = mainEntity.DataItemType == ChartDataItemType.MultipleData ? sort++ : item.Sort;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = mainEntity.DeleteTime;
                        });
                        dbContext.Set<Chart_TrendChartWithItemData>().AddRange(dataItems);

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
            string sql = string.Format("update Chart_TrendChart set sort=sort+1 where sort>='{0}'", sort);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
