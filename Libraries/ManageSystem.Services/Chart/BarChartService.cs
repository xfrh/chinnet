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
    public class BarChartService : BaseService<Chart_BarChart>, IBarChartService
    {
        private readonly ISystemLogService SystemLogService;

        public BarChartService(IRepository<Chart_BarChart> repository, ISystemLogService systemLogService) : base(repository)
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
		FROM dbo.[Chart_BarChart] 
	WHERE DataSegmentId = {dataSegmentId} AND [Mark] > 0
	) AS T
	WHERE T.SortGroupIdx = 1
) AS [t2] WHERE [t2].[Sort] <> [t2].[Value];";

            int sort = DapperHelper.GetConnection().ExecuteScalar<int?>(sql, null) ?? 0;
            if (sort > 0)
            {
                return sort;
            }

            return DapperHelper.GetConnection().ExecuteScalar<int>($"SELECT ISNULL(MAX([Sort]), 0) + 1  AS [Value] FROM dbo.[Chart_BarChart] WHERE DataSegmentId = {dataSegmentId} AND [Mark] > 0;", null);
        }

        public bool OnCreateBarChartByTransaction(Chart_BarChart barChart, List<Chart_BarChartWithItemData> dataItems)
        {
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        dbContext.Set<Chart_BarChart>().Add(barChart);

                        int sort = 1;
                        dataItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = barChart.InsertTime;
                            item.UpdateTime = barChart.InsertTime;
                            item.BarChartId = barChart.Id;
                            item.Sort = barChart.DataItemType == ChartDataItemType.MultipleData ? sort++ : item.Sort;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = barChart.DeleteTime;
                            dbContext.Set<Chart_BarChartWithItemData>().Add(item);
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

        public bool OnDeleteBarChartByTransaction(long id)
        {
            if (id <= 0) return false;

            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        Chart_BarChart entity = dbContext.Set<Chart_BarChart>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_BarChartWithItemData> dataItems = dbContext.Set<Chart_BarChartWithItemData>().Where(r => r.BarChartId == entity.Id);
                        foreach (Chart_BarChartWithItemData item in dataItems)
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

        public bool OnEditBarChartByTransaction(Chart_BarChart barChart, List<Chart_BarChartWithItemData> dataItems)
        {
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        barChart.Mark = 2;
                        barChart.UpdateTime = DateTime.Now;
                        barChart.Version += 1;

                        // 删除旧数据
                        IQueryable<Chart_BarChartWithItemData> oldDataItems = dbContext.Set<Chart_BarChartWithItemData>().Where(r => r.BarChartId == barChart.Id);
                        foreach (Chart_BarChartWithItemData item in oldDataItems)
                        {
                            dbContext.Set<Chart_BarChartWithItemData>().Remove(item);
                        }

                        // 添加新数据
                        int sort = 1;
                        dataItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = barChart.InsertTime;
                            item.UpdateTime = barChart.InsertTime;
                            item.BarChartId = barChart.Id;
                            item.Sort = barChart.DataItemType == ChartDataItemType.MultipleData ? sort++ : item.Sort;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = barChart.DeleteTime;
                            dbContext.Set<Chart_BarChartWithItemData>().Add(item);
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
            string sql = string.Format("update Chart_BarChart set sort=sort+1 where sort>='{0}'", sort);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
