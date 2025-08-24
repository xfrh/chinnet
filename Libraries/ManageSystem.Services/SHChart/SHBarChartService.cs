using Dapper;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.SHChart;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.SHChart
{
   public class SHBarChartService : BaseService<Chart_SHBarChart>, ISHBarChartService
    {
        private readonly ISystemLogService SystemLogService;
        public SHBarChartService(IRepository<Chart_SHBarChart> repository, ISystemLogService systemLogService) : base(repository)
        {
            SystemLogService = systemLogService;
        }
        /// <summary>
        /// 表格查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<Chart_SHBarChart> Get_SHBarChartsTable(long Id)
        {
            string sql = string.Format("select * from Chart_SHBarChart where Id='{0}'", Id);
            return DapperHelper.GetConnection().Query<Chart_SHBarChart>(sql).ToList();
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
		FROM dbo.[Chart_SHBarChart] 
	WHERE DataSegmentId = {dataSegmentId} AND [Mark] > 0
	) AS T
	WHERE T.SortGroupIdx = 1
) AS [t2] WHERE [t2].[Sort] <> [t2].[Value];";

            int sort = DapperHelper.GetConnection().ExecuteScalar<int?>(sql, null) ?? 0;
            if (sort > 0)
            {
                return sort;
            }

            return DapperHelper.GetConnection().ExecuteScalar<int>($"SELECT ISNULL(MAX([Sort]), 0) + 1  AS [Value] FROM dbo.[Chart_SHBarChart] WHERE DataSegmentId = {dataSegmentId} AND [Mark] > 0;", null);
        }

        public bool OnCreateBarChartByTransaction(Chart_SHBarChart barChart, List<Chart_SHBarChartWithItemData> dataItems)
        {
            try
            {
                using (var dbContext = new Data.ManageSystemContext())
                {
                    using (var trans = dbContext.Database.BeginTransaction())
                    {
                        dbContext.Set<Chart_SHBarChart>().Add(barChart);

                        int sort = 1;
                        dataItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = barChart.InsertTime;
                            item.UpdateTime = barChart.InsertTime;
                            item.SHBarChartId = barChart.Id;
                            item.Sort = barChart.DataItemType == ChartSHDataItemType.MultipleData ? sort++ : item.Sort;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = barChart.DeleteTime;
                            dbContext.Set<Chart_SHBarChartWithItemData>().Add(item);
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

        public bool OnCreateTableTransaction(Chart_SHBarChart model)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("insert into Chart_SHBarChart values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')", model.Id, model.DataSegmentId, model.Name, model.Title, model.SubTitle, model.Sort, model.Display, model.Default, model.Unit, model.MobileDisplayScale, model.DataItemType, model.Antibiotics, model.InsertTime, model.UpdateTime, model.DeleteTime, model.Version, model.Mark, model.Describe);
                int result = conn.Execute(sql);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
               
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
                        Chart_SHBarChart entity = dbContext.Set<Chart_SHBarChart>().Find(id);

                        if (entity == null || entity.Id <= 0) return false;

                        entity.DeleteTime = DateTime.Now;
                        entity.Mark = 0;
                        entity.Name = $"{entity.Name}_DELETE";

                        IQueryable<Chart_SHBarChartWithItemData> dataItems = dbContext.Set<Chart_SHBarChartWithItemData>().Where(r => r.SHBarChartId == entity.Id);
                        foreach (Chart_SHBarChartWithItemData item in dataItems)
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

        public bool OnDeleteTableTransaction(long id)
        {
            string sql = string.Format("delete  from  Chart_SHBarChart  where Id='{0}'", id);
            if (DapperHelper.GetConnection().Execute(sql) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OnEditBarChartByTransaction(Chart_SHBarChart barChart, List<Chart_SHBarChartWithItemData> dataItems)
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
                        IQueryable<Chart_SHBarChartWithItemData> oldDataItems = dbContext.Set<Chart_SHBarChartWithItemData>().Where(r => r.SHBarChartId == barChart.Id);
                        foreach (Chart_SHBarChartWithItemData item in oldDataItems)
                        {
                            dbContext.Set<Chart_SHBarChartWithItemData>().Remove(item);
                        }

                        // 添加新数据
                        int sort = 1;
                        dataItems.ForEach(item =>
                        {
                            item.Id = CommonHelper.GuidToLongID;
                            item.InsertTime = barChart.InsertTime;
                            item.UpdateTime = barChart.InsertTime;
                            item.SHBarChartId = barChart.Id;
                            item.Sort = barChart.DataItemType == ChartSHDataItemType.MultipleData ? sort++ : item.Sort;
                            item.Mark = 1;
                            item.Version = 1;
                            item.Describe = null;
                            item.DeleteTime = barChart.DeleteTime;
                            dbContext.Set<Chart_SHBarChartWithItemData>().Add(item);
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

        public bool OnEditTableTransaction(Chart_SHBarChart model)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("update Chart_SHBarChart set  Name='{0}',Title='{1}',Sort='{2}',Display='{3}',Antibiotics='{4}',UpdateTime='{5}' where Id='{6}'", model.Name, model.Title, model.Sort, model.Display, model.Antibiotics, model.UpdateTime,model.Id);
                int result = conn.Execute(sql);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public int UpdateSort(int sort)
        {
            string sql = string.Format("update Chart_SHBarChart set sort=sort+1 where sort>='{0}'", sort);
            return DapperHelper.GetConnection().Execute(sql);
        }
    }
}
