using ManageSystem.Core.Caching;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Medicine.Statistics;
using ManageSystem.Core.DynamicLinq;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using ManageSystem.Services.Medicine.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine.Statistics
{

    /// <summary>
    /// 医学数据统计报表 的  医学数据统计  业务处理类
    /// 
    /// </summary>
    public class MedicalStatisticsService
    {
        private readonly IDbContext DbContext = null;
        private readonly ICacheManager CacheManager;
        private readonly IMedicalDataItemService medicalDataItemService;
        private readonly IMedicalAntibioticResultService _medicalAntibioticResult;
        private readonly IMedicalAntibioticService _medicalAntibioticService;

        public MedicalStatisticsService()
        {
            medicalDataItemService = EngineContext.Current.Resolve<IMedicalDataItemService>();
            this.DbContext = EngineContext.Current.Resolve<IDbContext>();
            this.CacheManager = EngineContext.Current.Resolve<ICacheManager>();
            this._medicalAntibioticResult = EngineContext.Current.Resolve<IMedicalAntibioticResultService>();
            this._medicalAntibioticService = EngineContext.Current.Resolve<IMedicalAntibioticService>();
        }

        /// <summary>
        /// 按年份统计医学数据
        /// </summary>
        /// <returns></returns>
        public List<MedicalByYear> GetByYearData()
        {
            string sql = @"  SELECT [Year],
                      ( SELECT COUNT(*)  FROM dbo.MedicalDataItem(NOLOCK)    WHERE Mark>0 AND YEAR(InsertTime)=M.[Year] ) AS MedicalCount  
                  FROM dbo.MedicalData(NOLOCK)   AS M     GROUP BY  [Year]    ORDER BY  [Year]   ";

            return this.DbContext.SqlQuery<MedicalByYear>(sql).ToList();
        }

        /// <summary>
        /// 按省份统计医学数据
        /// </summary>
        /// <returns></returns>
        public List<MedicalByArea> GetByAreaData()
        {
            string sql = @"  
                    SELECT Id,Name,(
		                    SELECT COUNT(id) FROM  dbo.MedicalDataItem(NOLOCK) WHERE Mark>0 AND MedicalDataId IN 
		                     (
			                     SELECT  id FROM  dbo.MedicalData(NOLOCK) WHERE Mark>0 AND AreaId=Area.Id  
		                     ) 
	                    )AS MedicalCount  
              FROM dbo.Area(NOLOCK)WHERE Mark>0 AND ParentId = 0 ORDER BY MedicalCount DESC ";

            return this.DbContext.SqlQuery<MedicalByArea>(sql).ToList();
        }

        /// <summary>
        /// 按标本类型统计医学数据
        /// </summary>
        /// <returns></returns>
        public List<MedicalBySpecimen> GetBySpecimenData()
        {
            string sql = @"  	
                    SELECT SpecimenId,(SELECT  Name FROM dbo.Specimen WHERE ID =M.SpecimenId  ) AS SpecimenName ,
		                (
			                SELECT COUNT(id) FROM  dbo.MedicalDataItem(NOLOCK) WHERE Mark>0 AND MedicalDataId IN 
			                (
				                SELECT  id FROM  dbo.MedicalData(NOLOCK) WHERE Mark>0 AND  SpecimenId = M.SpecimenId
			                ) 
		                ) AS MedicalCount 
                     FROM  MedicalData(NOLOCK) AS M WHERE   Mark>0  GROUP BY  SpecimenId    ORDER BY  MedicalCount  DESC  ";

            return this.DbContext.SqlQuery<MedicalBySpecimen>(sql).ToList();
        }

        /// <summary>
        /// 按按年份和季度统计医学数据
        /// </summary>
        /// <returns></returns>
        public List<MedicalByYearAndQuarter> GetByYearAndQuarter(int year)
        {
            string sql = string.Format(@"  
            SELECT  Quarter,YEAR,
		    (
			   SELECT COUNT(*) FROM  dbo.MedicalDataItem(NOLOCK) WHERE Mark>0 AND MedicalDataId IN 
			        (
				        SELECT  id FROM  dbo.MedicalData(NOLOCK) WHERE Mark>0 AND Quarter=MD.Quarter AND Year = MD.Year
			        ) 
		        )AS MedicalCount  
		        FROM  MedicalData(NOLOCK)  AS MD WHERE Year={0}  GROUP BY Quarter , MD.Year ORDER BY Quarter
      ", year);

            return this.DbContext.SqlQuery<MedicalByYearAndQuarter>(sql).ToList();
        }

        /// <summary>
        /// 医学数据统计 -- 医学数据明细中的细菌数量统计
        /// </summary>
        /// <param name="medicalDataId">医学数据的id，0表示统计所有的数据，大于0表示统计指定的上传数据</param>
        /// <param name="topCount">按照数量倒序，所需要的排名前几位</param>
        /// <returns></returns>
        public List<StatisticsMedicalOrganism> GetByOrganism(long medicalDataId = 0, int topCount = 10, int year = 1900)
        {
            string sql = $@"WITH  TempTable AS 
(
	SELECT (SELECT COUNT(*) FROM  dbo.[MedicalDataItem] WHERE [Mark] > 0 AND [IsValid] = 1 AND [OrganismId] = [MedicalOrganism].[Id] AND [MedicalDataId] IN ( SELECT [Id] FROM dbo.[MedicalData] WHERE [Status] = 1 {(medicalDataId > 0 ? $"AND Id = {medicalDataId}" : "")} {(year > 1900 ? $"AND YEAR(InsertTime) = {year}" : "")} ) )  AS [DataCount] , 
	* 
	FROM dbo.[MedicalOrganism]  
	WHERE dbo.[MedicalOrganism].[Mark] > 0 AND dbo.[MedicalOrganism].[Id] <> 997
)
SELECT {(topCount > 0 ? $"TOP({topCount})" : "")} [Id], [Name], [Code], [DataCount] FROM [TempTable] ORDER BY [DataCount] DESC;";
            List<StatisticsMedicalOrganism> list = this.DbContext.SqlQuery<StatisticsMedicalOrganism>(sql).ToList();
            if (list == null || !list.Any()) return null;

            int count = list.Sum(m => m.DataCount);
            foreach (var item in list)
            {
                //去除细菌名称中的英文名称
                if (!string.IsNullOrWhiteSpace(item.Name))
                {
                    int lastIndex = item.Name.IndexOf("(");
                    if (lastIndex >= 0)
                        item.Name = item.Name.Substring(0, lastIndex);
                }

                //计算占的比例
                if (count > 0)
                {
                    item.Ratio = item.DataCount * 100.0m / count;
                    item.Ratio = item.Ratio.GetDecimal2();
                }
                else
                {
                    item.Ratio = 0m;
                }
            }

            return list;
        }

        /// <summary>
        /// 医学数据统计 --  革兰阳性菌菌种分布
        /// </summary>
        /// <returns></returns>
        public List<MedicalByGramPositiveBacteria> GetByGramPositiveBacteria()
        {
            string sql = @"
                 WITH  TempTable
                      AS (
                             SELECT (SELECT COUNT(*) FROM  dbo.MedicalDataItem WHERE  Mark>0 AND IsValid =1  AND OrganismId =MedicalOrganism.Id  AND   MedicalDataId IN ( SELECT id FROM dbo.MedicalData WHERE Status =1 AND Mark>0 )
                        )  AS DataCount ,* FROM  dbo.MedicalOrganism  WHERE dbo.MedicalOrganism.Mark>0  AND  OrganismTypeId1 = 2
                  )

                   SELECT TOP 10  Id ,Name ,Code  ,DataCount FROM  TempTable ORDER BY DataCount DESC 
            ";

            List<MedicalByGramPositiveBacteria> list = this.DbContext.SqlQuery<MedicalByGramPositiveBacteria>(sql).ToList();
            if (list == null || !list.Any()) return null;


            int count = list.Sum(m => m.DataCount);
            foreach (var item in list)
            {
                //去除细菌名称中的英文名称
                if (!string.IsNullOrWhiteSpace(item.Name))
                    item.Name = item.Name.Substring(0, item.Name.IndexOf("("));

                //计算占的比例
                item.Ratio = (item.DataCount / Convert.ToDecimal(count)) * 100;
                item.Ratio = Math.Round(item.Ratio, 2, MidpointRounding.AwayFromZero);
            }

            return list;
        }

        /// <summary>
        /// 医学数据统计 --  革兰阳性菌菌种分布
        /// </summary>
        /// <returns></returns>
        public List<MedicalByGramNegativeBacteria> GetByGramNegativeBacteria()
        {
            string sql = @"
                 WITH  TempTable
                      AS (
                             SELECT (SELECT COUNT(*) FROM  dbo.MedicalDataItem WHERE  Mark>0 AND IsValid =1  AND OrganismId =MedicalOrganism.Id  AND   MedicalDataId IN ( SELECT id FROM dbo.MedicalData WHERE Status =1 AND Mark>0 )
                        )  AS DataCount ,* FROM  dbo.MedicalOrganism  WHERE dbo.MedicalOrganism.Mark>0  AND  OrganismTypeId1 = 1
                  )

                   SELECT TOP 10  Id ,Name  ,Code  ,DataCount FROM  TempTable ORDER BY DataCount DESC 
            ";

            List<MedicalByGramNegativeBacteria> list = this.DbContext.SqlQuery<MedicalByGramNegativeBacteria>(sql).ToList();
            if (list == null || !list.Any()) return null;

            int count = list.Sum(m => m.DataCount);
            foreach (var item in list)
            {
                //去除细菌名称中的英文名称
                if (!string.IsNullOrWhiteSpace(item.Name))
                    item.Name = item.Name.Substring(0, item.Name.IndexOf("("));

                //计算占的比例
                item.Ratio = (item.DataCount / Convert.ToDecimal(count)) * 100;
                item.Ratio = Math.Round(item.Ratio, 2, MidpointRounding.AwayFromZero);
            }

            return list;
        }

        /// <summary>
        /// 医学数据统计 -- 细菌在标本中占比
        /// </summary>
        /// <returns></returns>
        public List<MedicalByBacteriaInSpecimen> GetByBacteriaInSpecimen()
        {
            string sql = @"
                 SELECT  TOP 10  Id,Name,Code,  
                 (  SELECT COUNT(*) FROM  dbo.MedicalDataItem WHERE  Mark>0 AND IsValid =1  AND SPEC_TYPE =MedicalSpecType.Code  AND   MedicalDataId IN ( SELECT Id FROM dbo.MedicalData WHERE Status =1  AND Mark>0 ) )  AS DataCount
                 FROM  dbo.MedicalSpecType  WHERE  Mark>0  ORDER BY DataCount DESC 
            ";

            List<MedicalByBacteriaInSpecimen> list = this.DbContext.SqlQuery<MedicalByBacteriaInSpecimen>(sql).ToList();
            if (list == null || !list.Any()) return null;


            int count = list.Sum(m => m.DataCount);
            foreach (var item in list)
            {
                //计算占的比例
                item.Ratio = (item.DataCount / Convert.ToDecimal(count)) * 100;
                item.Ratio = Math.Round(item.Ratio, 2, MidpointRounding.AwayFromZero);
            }

            return list;
        }

        /// <summary>
        /// 医学数据统计 -- MRSA对抗菌药物的敏感率和耐药率
        /// MRSA 表示的是金黄色葡萄球菌(Staphylococcus aureus)
        /// </summary>
        /// <param name="provinceId">省份id，需要查询的省份</param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByMRSASensitivityDrug(long provinceId = 0)
        {
            long organismId = 1218; //金黄色葡萄球菌(Staphylococcus aureus)

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            //string cacheKey = "medical.statistics.mrsasensitivitydrug";
            //List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            //if (list != null && list.Any()) return list;

            List<MedicalByAntibioticResult> list = this.GetByMedicalAntibioticResult(organismId, provinceId);
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            //    this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        /// <summary>
        /// 医学数据统计 --  MRCNS对抗菌药物的敏感率和耐药率
        /// MRCNS 表示的是溶血葡萄球菌(Staphylococcus haemolyticus)
        /// </summary>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByMRCNSSensitivityDrug()
        {
            long organismId = 949; //溶血葡萄球菌(Staphylococcus haemolyticus)

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            //string cacheKey = "medical.statistics.mrcnssensitivitydrug";
            //List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            //if (list != null && list.Any()) return list;

            List<MedicalByAntibioticResult> list = this.GetByMedicalAntibioticResult(organismId);
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            // this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        /// <summary>
        /// 医学数据统计 --   MSSA对抗菌药物的敏感率和耐药率
        /// MRCNS 表示的是  无
        /// </summary>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByMSSASensitivityDrug()
        {
            long organismId = 0; //无

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            string cacheKey = "medical.statistics.mssasensitivitydrug";
            List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            if (list != null && list.Any()) return list;

            list = this.GetByMedicalAntibioticResult(organismId);
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        /// <summary>
        /// 医学数据统计 --   大肠埃希菌对抗敏感率和耐药率
        /// </summary>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByEscherichiaColiSensitivityDrug()
        {
            long organismId = 1093;

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            string cacheKey = "medical.statistics.escherichiacolisensitivitydrug";
            List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            if (list != null && list.Any()) return list;

            list = this.GetByMedicalAntibioticResult(organismId);
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        /// <summary>
        /// 医学数据统计 -- 查询细菌敏感值统计报表
        /// </summary>
        /// <param name="organismId">细菌Id</param>
        /// <param name="provinceId">省份Id</param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByMedicalAntibioticResult(long organismId, long provinceId = 0)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                // 多参数写法 var country = "Australia";   var keyWords = "Beach, Sun"; var destinations = context.Database.SqlQuery<DestinationSummary>("dbo.GetDestinationSummary @p0, @p1", country, keyWords);
                var list = new List<MedicalByAntibioticResult>();
                if (provinceId <= 0)
                    list = c.SqlQuery<MedicalByAntibioticResult>("GetMedicalAntibioticResult @p0", (int)organismId).ToList();
                else
                    list = c.SqlQuery<MedicalByAntibioticResult>("GetMedicalAntibioticResult_Province @p0,@p1", (int)organismId, provinceId).ToList();

                if (list == null || !list.Any()) return null;

                //计算比例
                foreach (var item in list)
                {
                    item.ResistanceRatio = (item.ResistanceCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.ResistanceCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);
                    item.SensitiveRatio = (item.SensitiveCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.SensitiveCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);
                    item.IntermediaryRatio = (item.IntermediaryCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.IntermediaryCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);

                    //容错数据，防止结果超出100，减最大的那个
                    if ((item.ResistanceRatio + item.SensitiveRatio + item.IntermediaryRatio) > 100)
                    {
                        if (item.ResistanceCount > item.SensitiveCount)
                        {
                            if (item.ResistanceCount > item.IntermediaryCount)
                                item.ResistanceRatio = 100 - (item.SensitiveRatio + item.IntermediaryRatio);
                            else
                                item.IntermediaryRatio = 100 - (item.ResistanceRatio + item.SensitiveRatio);
                        }
                        else
                        {
                            if (item.SensitiveCount > item.IntermediaryCount)
                                item.SensitiveRatio = 100 - (item.ResistanceRatio + item.IntermediaryRatio);
                            else
                                item.IntermediaryRatio = 100 - (item.ResistanceRatio + item.SensitiveRatio);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                var logService = EngineContext.Current.Resolve<ISystemLogService>();
                logService.Insert(ex, SystemLogLevel.Error);
                return null;
            }

        }

        /// <summary>
        /// 医学数据统计 -- 查询细菌敏感值统计报表
        /// </summary>
        /// <param name="organismId">细菌Id</param>
        /// <param name="provinceId">省份Id</param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByMedicalAntibioticResult(long medicalDataId, long organismId, long provinceId = 0)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                var list = new List<MedicalByAntibioticResult>();
                if (provinceId <= 0)
                    list = c.SqlQuery<MedicalByAntibioticResult>("GetMedicalAntibioticResult_OrganismId_MedicalDataId @p0,@p1", organismId, medicalDataId).ToList();
                else
                    list = c.SqlQuery<MedicalByAntibioticResult>("GetMedicalAntibioticResult_Province @p0,@p1", organismId, provinceId).ToList();

                if (list == null || !list.Any()) return null;

                //计算比例
                foreach (var item in list)
                {
                    item.ResistanceRatio = (item.ResistanceCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.ResistanceCount) / item.DataCount * 100, 1, MidpointRounding.AwayFromZero);
                    item.SensitiveRatio = (item.SensitiveCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.SensitiveCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);
                    item.IntermediaryRatio = (item.IntermediaryCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.IntermediaryCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);

                    //容错数据，防止结果超出100，减最大的那个
                    if ((item.ResistanceRatio + item.SensitiveRatio + item.IntermediaryRatio) > 100)
                    {
                        if (item.ResistanceCount > item.SensitiveCount)
                        {
                            if (item.ResistanceCount > item.IntermediaryCount)
                                item.ResistanceRatio = 100 - (item.SensitiveRatio + item.IntermediaryRatio);
                            else
                                item.IntermediaryRatio = 100 - (item.ResistanceRatio + item.SensitiveRatio);
                        }
                        else
                        {
                            if (item.SensitiveCount > item.IntermediaryCount)
                                item.SensitiveRatio = 100 - (item.ResistanceRatio + item.IntermediaryRatio);
                            else
                                item.IntermediaryRatio = 100 - (item.ResistanceRatio + item.SensitiveRatio);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                var logService = EngineContext.Current.Resolve<ISystemLogService>();
                logService.Insert(ex, SystemLogLevel.Error);
                return null;
            }

        }

        /// <summary>
        /// 医学数据统计 -- 查询细菌敏感值统计报表
        /// </summary>
        /// <param name="organismId">细菌Id</param>
        /// <param name="provinceId">省份Id</param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByMedicalAntibioticResultByAge(long medicalDataId, long organismId, long medicalDataItemId)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                var list = c.SqlQuery<MedicalByAntibioticResult>("GetMedicalAntibioticResult_OrganismId_MedicalDataId_ByAge @p0,@p1,@p2", organismId, medicalDataId, medicalDataItemId).ToList();

                if (list == null || !list.Any()) return new List<MedicalByAntibioticResult>();

                //计算比例
                foreach (var item in list)
                {
                    item.ResistanceRatio = (item.ResistanceCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.ResistanceCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);
                    item.SensitiveRatio = (item.SensitiveCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.SensitiveCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);
                    item.IntermediaryRatio = (item.IntermediaryCount == 0) ? 0 : Math.Round(Convert.ToDecimal(item.IntermediaryCount) / item.DataCount * 100, 2, MidpointRounding.AwayFromZero);

                    //容错数据，防止结果超出100，减最大的那个
                    if ((item.ResistanceRatio + item.SensitiveRatio + item.IntermediaryRatio) > 100)
                    {
                        if (item.ResistanceCount > item.SensitiveCount)
                        {
                            if (item.ResistanceCount > item.IntermediaryCount)
                                item.ResistanceRatio = 100 - (item.SensitiveRatio + item.IntermediaryRatio);
                            else
                                item.IntermediaryRatio = 100 - (item.ResistanceRatio + item.SensitiveRatio);
                        }
                        else
                        {
                            if (item.SensitiveCount > item.IntermediaryCount)
                                item.SensitiveRatio = 100 - (item.ResistanceRatio + item.IntermediaryRatio);
                            else
                                item.IntermediaryRatio = 100 - (item.ResistanceRatio + item.SensitiveRatio);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                var logService = EngineContext.Current.Resolve<ISystemLogService>();
                logService.Insert(ex, SystemLogLevel.Error);
                return new List<MedicalByAntibioticResult>();
            }

        }

        /// <summary>
        /// 医学数据统计 -- 分离菌在各类标本中的分布
        /// </summary>
        /// <param name="medicalDataId">医学数据的id</param>
        /// <param name="deleteZero">是否删除0的数据</param>
        /// <returns></returns>
        public List<StatisticsMedicalSpecTypeModel> SpecTypeQuery(long medicalDataId, bool deleteZero = true)
        {
            string sql = string.Format(@"
                 SELECT Id,Name,
                 ( SELECT COUNT(*) FROM  MedicalDataItem WHERE MedicalDataId={0}  AND Mark>0 AND IsValid = 1 AND SPEC_TYPE=MedicalSpecType.Code ) AS Count
                  FROM  dbo.MedicalSpecType  WHERE dbo.MedicalSpecType.Mark > 0 
                ", medicalDataId);

            var data = this.DbContext.SqlQuery<StatisticsMedicalSpecTypeModel>(sql).ToList();
            if (data == null || !data.Any())
                return null;

            int amount = data.Sum(m => m.Count);

            foreach (var item in data)
            {
                item.Amount = amount;
                item.Ratio = item.Count <= 0 ? 0L : Convert.ToDecimal(item.Count) * 100 / Convert.ToDecimal(item.Amount);
            }

            if (deleteZero)
                data = data.Where(m => m.Ratio > 0).OrderByDescending(m => m.Ratio).ToList();

            return data;
        }

        /// <summary>
        /// 医学数据统计 --  葡萄球菌属(Staphylococcus sp.)的敏感率和耐药率
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByStaphylococcus(long medicalDataId = 0)
        {
            long organismId = 66; // 葡萄球菌属(Staphylococcus sp.)

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            //string cacheKey = "medical.statistics.mrsasensitivitydrug";
            //List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            //if (list != null && list.Any()) return list;
            List<MedicalByAntibioticResult> list = new List<MedicalByAntibioticResult>();
            if (medicalDataId == 0)
            {
                list = this.GetByMedicalAntibioticResult(organismId);
            }
            else
            {
                list = this.GetByMedicalAntibioticResult(medicalDataId, organismId, provinceId: 0);
            }
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            //    this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        #region 医学数据统计 -- 敏感率和耐药率

        /// <summary>
        /// 临床主要分离菌种分布，前20位
        /// </summary>
        /// <param name="medicalDataId">上传数据Id</param>
        /// <returns></returns>
        public List<StatisticsMedicalOrganism> SpecTypeQueryTop20(long medicalDataId, int year)
        {
            //临床主要分离菌种分布 表格，只要前20个数据
            var tempList1 = new MedicalStatisticsService().GetByOrganism(medicalDataId, 0, year);
            tempList1 = tempList1 ?? new List<StatisticsMedicalOrganism>();
            var tempList2 = tempList1.OrderByDescending(m => m.DataCount).ToList();
            int amount1 = tempList1.Sum(m => m.DataCount); //总的数据量
            int anthorAmount1 = tempList1.Sum(m => m.DataCount) - tempList2.Sum(m => m.DataCount); //其他总数量
            tempList2.Add(new StatisticsMedicalOrganism() { Name = "其他", DataCount = anthorAmount1, Ratio = -1 }); //-1特殊标记
            tempList2.Add(new StatisticsMedicalOrganism() { Name = "合计", DataCount = amount1, Ratio = -1 });
            return tempList2;
        }

        /// <summary>
        ///  医学数据统计 --  表3：葡萄球菌属对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public MedicalDataWordMRSAModel GetByMedicalDataTable3(long medicalDataId)
        {
            #region 获取数据
            /*
                规则说明：

                查询“金黄色葡萄球菌”菌属，不是细菌
                MRSA：OXA_NM>=4，FOX_ND30<=21，FOX_NM>=8 满足三者任一条件
                MSSA：OXA_NM<=2，FOX_ND30>=22，FOX_NM<=4 满足三个字段任一条件，且另外两个字段空白或也敏感
             */

            // 分别查询 MRSA、MSSA 的数据
            // 0：无效值  1：敏感   2：中介   3：耐药

            MedicalDataWordMRSAModel result = new MedicalDataWordMRSAModel();

            // 金黄色葡萄球菌
            var dataList = this._medicalAntibioticResult.QueryMRSAList(medicalDataId, "金黄色葡萄球菌");
            dataList = dataList ?? new List<MedicalAntibioticResult>();

            // MRSA 的耐药情况（OXA_NM、FOX_ND30、FOX_NM，3个任意一个耐药）
            var mrsaList = dataList.Where(m => (m.OXA_NM == 3 || m.FOX_ND30 == 3 || m.FOX_NM == 3)).ToList() ?? new List<MedicalAntibioticResult>();
            // 耐药结果数据
            var mrsaResultList = this._medicalAntibioticResult.GetResult(mrsaList);
            result.MrsaCount = mrsaList.Count();

            // MSSA 的耐药情况（OXA_NM、FOX_ND30、FOX_NM，3个任意一个敏感） 
            //var mssaList = dataList.Where(m => ((m.OXA_NM == 1 && m.FOX_ND30 == 0 && m.FOX_NM == 0) || (m.OXA_NM == 1 && m.FOX_ND30 == 1 && m.FOX_NM == 0) || (m.OXA_NM == 1 && m.FOX_ND30 == 0 && m.FOX_NM == 1))).ToList() ?? new List<MedicalAntibioticResult>();
            var mssaList = dataList.Where(m => !(m.OXA_NM == 3 || m.FOX_ND30 == 3 || m.FOX_NM == 3)).ToList() ?? new List<MedicalAntibioticResult>();

            // 耐药结果数据
            var mssaResultList = this._medicalAntibioticResult.GetResult(mssaList);
            result.MssaCount = mssaList.Count();
            #endregion

            #region 计算数据
            result.List = new List<MedicalDataWordMRSAItemModel>();

            // MRSA
            foreach (var item in mrsaResultList)
            {
                result.List.Add(new MedicalDataWordMRSAItemModel()
                {
                    OrganismId = item.OrganismId,
                    AntibioticCode = item.AntibioticCode,
                    AntibioticId = item.AntibioticId,
                    AntibioticName = item.AntibioticName,
                    MRSAAntibioticCount = item.DataCount,
                    MRSAR = item.ResistanceRatio.ToString3(1),
                    MRSAS = item.SensitiveRatio.ToString3(1)
                });
            }

            // MSSA
            foreach (var item in mssaResultList)
            {
                var temp = result.List.Where(m => m.AntibioticId == item.AntibioticId).FirstOrDefault();
                if (temp != null && temp.AntibioticId > 0)
                {
                    temp.MSSAAntibioticCount = item.DataCount;
                    temp.MSSAR = item.ResistanceRatio.ToString3();
                    temp.MSSAS = item.SensitiveRatio.ToString3();
                }
                else
                {
                    result.List.Add(new MedicalDataWordMRSAItemModel()
                    {
                        OrganismId = item.OrganismId,
                        AntibioticCode = item.AntibioticCode,
                        AntibioticId = item.AntibioticId,
                        AntibioticName = item.AntibioticName,
                        MSSAAntibioticCount = item.DataCount,
                        MSSAR = item.ResistanceRatio.ToString3(),
                        MSSAS = item.SensitiveRatio.ToString3()
                    });
                }
            }
            return result;

            #endregion

        }

        public MedicalDataWordMRSAModel GetByMedicalDataTable3_2(long medicalDataId)
        {
            #region 获取数据

            MedicalDataWordMRSAModel result = new MedicalDataWordMRSAModel();

            // MRCNS 凝固酶阴性葡萄球菌
            var dataList2 = this._medicalAntibioticResult.QueryMRSAList(medicalDataId, "凝固酶阴性葡萄球菌");
            dataList2 = dataList2 ?? new List<MedicalAntibioticResult>();
            //   MRCNS 的耐药情况： OXA_NM>=0.5，FOX_ND30空白或FOX_ND30<=24
            //                                       FOX_ND30 <= 24，OXA_NM空白或OXA_NM >= 0.5
            var mrcnsList = dataList2.Where(m => m.OXA_NM == 3 || m.FOX_ND30 == 3).ToList() ?? new List<MedicalAntibioticResult>();
            List<long> organismIds = mrcnsList.Select(r => r.OrganismId).Distinct().ToList();
            var mrcnsResultList = this._medicalAntibioticResult.GetResultByOrganismIds(mrcnsList, organismIds);  //耐药结果数据
            result.MrcnsCount = mrcnsList.Count();

            // MSCNS  的耐药情况： OXA_NM<=0.25，FOX_ND30空白或FOX_ND30>=25
            //                                     FOX_ND30 >= 25，OXA_NM空白或OXA_NM <= 0.25
            // （ OXA_NM或FOX_ND30，其中一个敏感则敏感，2个不一样则不要）

            //var mscnsList = dataList2.Where(m => ((m.OXA_NM == 1 && m.FOX_ND30 == 1) || (m.OXA_NM == 1 && m.FOX_ND30 == 0) || (m.OXA_NM == 0 && m.FOX_ND30 == 1))).ToList() ?? new List<MedicalAntibioticResult>();

            var mscnsList = dataList2.Where(m => !(m.OXA_NM == 3 || m.FOX_ND30 == 3)).ToList() ?? new List<MedicalAntibioticResult>();
            List<long> organismIds2 = mrcnsList.Select(r => r.OrganismId).Distinct().ToList();
            var mscnsResultList = this._medicalAntibioticResult.GetResultByOrganismIds(mscnsList, organismIds2);  //耐药结果数据
            result.MscnsCount = mscnsList.Count();

            #endregion

            #region 计算数据
            result.List = new List<MedicalDataWordMRSAItemModel>();

            //MRCNS
            foreach (var item in mrcnsResultList)
            {
                var temp = result.List.Where(m => m.AntibioticId == item.AntibioticId).FirstOrDefault();
                if (temp != null && temp.AntibioticId > 0)
                {
                    temp.MRCNSAntibioticCount = item.DataCount;
                    temp.MRCNSR = item.ResistanceRatio.ToString3();
                    temp.MRCNSS = item.SensitiveRatio.ToString3();
                }
                else
                {
                    result.List.Add(new MedicalDataWordMRSAItemModel()
                    {
                        OrganismId = item.OrganismId,
                        AntibioticCode = item.AntibioticCode,
                        AntibioticId = item.AntibioticId,
                        AntibioticName = item.AntibioticName,
                        MRCNSAntibioticCount = item.DataCount,
                        MRCNSR = item.ResistanceRatio.ToString3(),
                        MRCNSS = item.SensitiveRatio.ToString3()
                    });
                }
            }

            //MSCNS
            foreach (var item in mscnsResultList)
            {
                var temp = result.List.Where(m => m.AntibioticId == item.AntibioticId).FirstOrDefault();
                if (temp != null && temp.AntibioticId > 0)
                {
                    temp.MSCNSAntibioticCount = item.DataCount;
                    temp.MSCNSR = item.ResistanceRatio.ToString3();
                    temp.MSCNSS = item.SensitiveRatio.ToString3();
                }
                else
                {
                    result.List.Add(new MedicalDataWordMRSAItemModel()
                    {
                        OrganismId = item.OrganismId,
                        AntibioticCode = item.AntibioticCode,
                        AntibioticId = item.AntibioticId,
                        AntibioticName = item.AntibioticName,
                        MSCNSAntibioticCount = item.DataCount,
                        MSCNSR = item.ResistanceRatio.ToString3(),
                        MSCNSS = item.SensitiveRatio.ToString3()
                    });
                }
            }

            return result;

            #endregion

        }
        /// <summary>
        /// 医学数据统计 --  表4：粪肠球菌和屎肠球菌对抗菌耐药率和敏感率
        /// </summary>
        /// <param name="medicalDataId"></param>
        ///  <param name="antibioticResultList">耐药性结果</param>
        /// <returns></returns>
        public Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> GetByMedicalDataTable4(long medicalDataId, List<GetAntibioticResultModel> antibioticResultList)
        {
            long organismId_1 = 908, // 粪肠球菌(Enterococcus faecalis)
              organismId_2 = 996; // 屎肠球菌(Enterococcus faecium)

            List<GetAntibioticResultModel> list1 = antibioticResultList.Where(m => m.OrganismId == organismId_1).ToList() ?? new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> list2 = antibioticResultList.Where(m => m.OrganismId == organismId_2).ToList() ?? new List<GetAntibioticResultModel>();

            List<string> names = new List<string>();
            names.AddRange(list1.Select(r => r.AntibioticName));
            names.AddRange(list2.Select(r => r.AntibioticName));

            Dictionary<string, int> thead = new Dictionary<string, int>
            {
                ["粪肠球菌"] = list1.Select(r => r.DataCount).Sum(),
                ["屎肠球菌"] = list2.Select(r => r.DataCount).Sum()
            };

            Dictionary<string, List<string>> tbody = new Dictionary<string, List<string>>();

            foreach (var name in names)
            {
                var d = new
                {
                    v1 = list1.Where(r => r.AntibioticName == name).Select(r => Math.Round(r.ResistanceRatio, 1, MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(),
                    v2 = list1.Where(r => r.AntibioticName == name).Select(r => Math.Round(r.SensitiveRatio, 1, MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(),
                    v3 = list2.Where(r => r.AntibioticName == name).Select(r => Math.Round(r.ResistanceRatio, 1, MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(),
                    v4 = list2.Where(r => r.AntibioticName == name).Select(r => Math.Round(r.SensitiveRatio, 1, MidpointRounding.AwayFromZero)).FirstOrDefault().ToString()
                };

                tbody[name] = new List<string> {
                    list1.Where(r => r.AntibioticName == name).Select(r => Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(),
                    list1.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero) ).FirstOrDefault().ToString(),
                    list2.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(),
                    list2.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString()
                };
            }

            Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> tuple = new Tuple<Dictionary<string, int>, Dictionary<string, List<string>>>(thead, tbody);
            return tuple;
        }

        /// <summary>
        /// 医学数据统计 --  表7：儿童和成人患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> GetByMedicalDataTable7(long medicalDataId, List<GetAntibioticResultModel> antibioticResultList)
        {
            long organismId = 493; // 肺炎链球菌(Streptococcus pneumoniae)

            List<GetAntibioticResultModel> list1 = this._medicalAntibioticResult.GetResult(medicalDataId, organismId, "'sf'") ?? new List<GetAntibioticResultModel>();
            if (list1 == null || !list1.Any()) return null;

            List<string> names = new List<string>();
            names.AddRange(list1.Select(r => r.AntibioticName));

            names = names.GroupBy(r => r).Select(r => r.Key).ToList();
            Dictionary<string, int> thead = new Dictionary<string, int>
            {
                ["肺炎链球菌"] = (!list1.Any()) ? 0 : list1[0].OrganismCount
            };

            Dictionary<string, List<string>> tbody = new Dictionary<string, List<string>>();

            foreach (var name in names)
            {
                tbody[name] = new List<string> {
                    list1.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                    list1.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                };
            }

            Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> tuple = new Tuple<Dictionary<string, int>, Dictionary<string, List<string>>>(thead, tbody);
            return tuple;
        }

        /// <summary>
        /// 医学数据统计 --  表7：儿童和成人患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率
        /// 废弃函数，不区分成人和儿童了
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        //public Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> GetByMedicalDataTable7(long medicalDataId)
        //{
        //    long organismId = 493; // 肺炎链球菌(Streptococcus pneumoniae)

        //    List<MedicalByAntibioticResult> ped_list = new List<MedicalByAntibioticResult>();
        //    List<MedicalByAntibioticResult> adu_list = new List<MedicalByAntibioticResult>();

        //    #region 取数据

        //    var medicalDataItems = medicalDataItemService.Query(m => m.Mark > 0 && m.MedicalDataId == medicalDataId && m.OrganismId == organismId);

        //    medicalDataItems.ForEach(item =>
        //    {
        //        if (item.AGE.IsInt() || item.AGE.ToLower().Contains("y"))
        //        {
        //            //年
        //            int age = item.AGE.Replace("y", "").GetInt();
        //            if (age >= 1 && age <= 14)
        //                item.PAT_TYPE = "ped";
        //            else if (age >= 15 && age <= 65)
        //                item.PAT_TYPE = "adu";
        //            else if (age > 65)
        //                item.PAT_TYPE = "ger";
        //        }
        //        else if (item.AGE.ToLower().Contains("d"))
        //        {
        //            int age = item.AGE.Replace("d", "").GetInt();
        //            if (age <= 28)
        //                item.PAT_TYPE = "new";
        //            else if (age > 28)
        //                item.PAT_TYPE = "ped";
        //        }
        //        else if (item.AGE.ToLower().Contains("w"))
        //        {
        //            int age = item.AGE.Replace("w", "").GetInt();
        //            if (age <= 4)
        //                item.PAT_TYPE = "new";
        //            else if (age > 4)
        //                item.PAT_TYPE = "ped";
        //        }
        //        else if (item.AGE.ToLower().Contains("m"))
        //        {
        //            int age = item.AGE.Replace("m", "").GetInt();
        //            if (age >= 1)
        //                item.PAT_TYPE = "ped";
        //        }
        //    });

        //    // 儿童
        //    var ped_ids = medicalDataItems.Where(r => r.PAT_TYPE == "ped").Select(r => r.Id).ToList();

        //    // 成人
        //    var adu_ids = medicalDataItems.Where(r => r.PAT_TYPE == "adu").Select(r => r.Id).ToList();

        //    // 儿童
        //    foreach (var id in ped_ids)
        //    {
        //        var datas = GetByMedicalAntibioticResultByAge(medicalDataId, organismId, id);
        //        if (datas.Count > 0)
        //        {
        //            ped_list.AddRange(datas);
        //        }
        //    }

        //    // 成人
        //    foreach (var id in adu_ids)
        //    {
        //        var datas = GetByMedicalAntibioticResultByAge(medicalDataId, organismId, id);
        //        if (datas.Count > 0)
        //        {
        //            adu_list.AddRange(datas);
        //        }
        //    }
        //    #endregion

        //    List<string> names = new List<string>();

        //    if (ped_list.Select(r => r.AntibioticName).Any())
        //    {
        //        names.AddRange(ped_list.Select(r => r.AntibioticName).ToList());
        //    }
        //    if (adu_list.Select(r => r.AntibioticName).Any())
        //    {
        //        names.AddRange(adu_list.Select(r => r.AntibioticName).ToList());
        //    }

        //    names = names.GroupBy(r => r).Select(r => r.Key).ToList();

        //    Dictionary<string, int> thead = new Dictionary<string, int>
        //    {
        //        ["儿童"] = ped_list.Select(r => r.DataCount).Sum(),
        //        ["成人"] = adu_list.Select(r => r.DataCount).Sum()
        //    };

        //    Dictionary<string, List<string>> tbody = new Dictionary<string, List<string>>();

        //    foreach (var name in names)
        //    {
        //        tbody[name] = new List<string> {
        //            // 儿童 耐药率
        //            ped_list.Where(r => r.AntibioticName == name).Select(r => r.ResistanceRatio).FirstOrDefault().ToString("N"),
        //            // 儿童 敏感率
        //            ped_list.Where(r => r.AntibioticName == name).Select(r => r.SensitiveRatio).FirstOrDefault().ToString("N"),

        //            // 成人 耐药率
        //            adu_list.Where(r => r.AntibioticName == name).Select(r => r.ResistanceRatio).FirstOrDefault().ToString("N"),
        //            // 成人 敏感率
        //            adu_list.Where(r => r.AntibioticName == name).Select(r => r.SensitiveRatio).FirstOrDefault().ToString("N")
        //        };
        //    }
        //    return new Tuple<Dictionary<string, int>, Dictionary<string, List<string>>>(thead, tbody);
        //}

        /// <summary>
        /// 医学数据统计 --  表8：肠杆菌科细菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="medicalDataId"></param>
        ///  <param name="antibioticResultList">耐药性结果</param>
        /// <returns></returns>
        public Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> GetByMedicalDataTable8(long medicalDataId, List<GetAntibioticResultModel> antibioticResultList)
        {

            long organismId_1 = 1093, // 大肠埃希菌(Escherichia coli)
              organismId_2 = 535, // 肺炎克雷伯菌(Klebsiella pneumoniae)
              organismId_3 = 882, // 奇异变形杆菌(Proteus mirabilis)
              organismId_4 = 516, // 阴沟肠杆菌(Enterobacter cloacae)
              organismId_5 = 601, // 粘质沙雷菌(Serratia marcescens)
              organismId_6 = 460; // 弗劳地柠檬酸杆菌(Citrobacter freundii)

            List<GetAntibioticResultModel> list1 = antibioticResultList.Where(m => m.OrganismId == organismId_1).ToList() ?? new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> list2 = antibioticResultList.Where(m => m.OrganismId == organismId_2).ToList() ?? new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> list3 = antibioticResultList.Where(m => m.OrganismId == organismId_3).ToList() ?? new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> list4 = antibioticResultList.Where(m => m.OrganismId == organismId_4).ToList() ?? new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> list5 = antibioticResultList.Where(m => m.OrganismId == organismId_5).ToList() ?? new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> list6 = antibioticResultList.Where(m => m.OrganismId == organismId_6).ToList() ?? new List<GetAntibioticResultModel>();

            List<string> names = new List<string>();
            names.AddRange(list1.Select(r => r.AntibioticName));
            names.AddRange(list2.Select(r => r.AntibioticName));
            names.AddRange(list3.Select(r => r.AntibioticName));
            names.AddRange(list4.Select(r => r.AntibioticName));
            names.AddRange(list5.Select(r => r.AntibioticName));
            names.AddRange(list6.Select(r => r.AntibioticName));

            names = names.GroupBy(r => r).Select(r => r.Key).ToList();
            Dictionary<string, int> thead = new Dictionary<string, int>
            {
                ["大肠埃希菌"] = (!list1.Any()) ? 0 : list1[0].OrganismCount,
                ["肺炎克雷伯菌"] = (!list2.Any()) ? 0 : list2[0].OrganismCount,
                ["奇异变形杆菌"] = (!list3.Any()) ? 0 : list3[0].OrganismCount,
                ["阴沟肠杆菌"] = (!list4.Any()) ? 0 : list4[0].OrganismCount,
                ["黏质沙雷菌"] = (!list5.Any()) ? 0 : list5[0].OrganismCount,
                ["弗劳地柠檬酸杆菌"] = (!list6.Any()) ? 0 : list6[0].OrganismCount
            };

            Dictionary<string, List<string>> tbody = new Dictionary<string, List<string>>();

            foreach (var name in names)
            {
                tbody[name] = new List<string> {
                    list1.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                    list1.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero )).FirstOrDefault().ToString(""),
                    list2.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                    list2.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero )).FirstOrDefault().ToString(""),
                    list3.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                    list3.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero )).FirstOrDefault().ToString(""),
                    list4.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                    list4.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero )).FirstOrDefault().ToString(""),
                    list5.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                    list5.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero )).FirstOrDefault().ToString(""),
                    list6.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.ResistanceRatio,1,MidpointRounding.AwayFromZero)).FirstOrDefault().ToString(""),
                    list6.Where(r => r.AntibioticName == name).Select(r =>Math.Round( r.SensitiveRatio,1,MidpointRounding.AwayFromZero )).FirstOrDefault().ToString("")
                };
            }

            Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> tuple = new Tuple<Dictionary<string, int>, Dictionary<string, List<string>>>(thead, tbody);
            return tuple;
        }

        /// <summary>
        /// 医学数据统计 --  表11：不发酵糖革兰阴性杆菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> GetByMedicalDataTable11(long medicalDataId, List<GetAntibioticResultModel> antibioticResultList)
        {
            long organismId_1 = 732, // 铜绿假单胞菌(Pseudomonas aeruginosa)
              organismId_2 = 1007, // 鲍曼不动杆菌(Acinetobacter baumannii) 
              organismId_3 = 1155, // 嗜麦芽窄食单胞菌(Smaltophilia) 
              organismId_4 = 1135; // 洋葱伯克霍尔德菌(Burkholderia cepacia)

            List<Model.GetAntibioticResultModel> list1 = antibioticResultList.Where(m => m.OrganismId == organismId_1).ToList() ?? new List<GetAntibioticResultModel>();
            List<Model.GetAntibioticResultModel> list2 = antibioticResultList.Where(m => m.OrganismId == organismId_2).ToList() ?? new List<GetAntibioticResultModel>();
            List<Model.GetAntibioticResultModel> list3 = antibioticResultList.Where(m => m.OrganismId == organismId_3).ToList() ?? new List<GetAntibioticResultModel>();
            List<Model.GetAntibioticResultModel> list4 = antibioticResultList.Where(m => m.OrganismId == organismId_4).ToList() ?? new List<GetAntibioticResultModel>();

            List<string> names = new List<string>();
            names.AddRange(list1.Select(r => r.AntibioticName));
            names.AddRange(list2.Select(r => r.AntibioticName));
            names.AddRange(list3.Select(r => r.AntibioticName));
            names.AddRange(list4.Select(r => r.AntibioticName));

            names = names.GroupBy(r => r).Select(r => r.Key).ToList();
            Dictionary<string, int> thead = new Dictionary<string, int>
            {
                ["铜绿假单胞菌"] = (!list1.Any()) ? 0 : list1[0].OrganismCount,
                ["鲍曼不动杆菌"] = (!list2.Any()) ? 0 : list2[0].OrganismCount,
                ["嗜麦芽窄食单胞菌"] = (!list3.Any()) ? 0 : list3[0].OrganismCount,
                ["洋葱伯克霍尔德菌"] = (!list4.Any()) ? 0 : list4[0].OrganismCount
            };

            Dictionary<string, List<string>> tbody = new Dictionary<string, List<string>>();

            foreach (var name in names)
            {
                tbody[name] = new List<string> {
                    list1.Where(r => r.AntibioticName == name).Select(r =>r.ResistanceRatio.GetDecimal2(1) ).FirstOrDefault().ToString(""),
                    list1.Where(r => r.AntibioticName == name).Select(r =>r.SensitiveRatio.GetDecimal2(1)  ).FirstOrDefault().ToString(""),
                    list2.Where(r => r.AntibioticName == name).Select(r =>r.ResistanceRatio.GetDecimal2(1) ).FirstOrDefault().ToString(""),
                    list2.Where(r => r.AntibioticName == name).Select(r =>r.SensitiveRatio.GetDecimal2(1)).FirstOrDefault().ToString(""),
                    list3.Where(r => r.AntibioticName == name).Select(r =>r.ResistanceRatio.GetDecimal2(1) ).FirstOrDefault().ToString(""),
                    list3.Where(r => r.AntibioticName == name).Select(r =>r.SensitiveRatio.GetDecimal2(1) ).FirstOrDefault().ToString(""),
                    list4.Where(r => r.AntibioticName == name).Select(r =>r.ResistanceRatio.GetDecimal2(1)  ).FirstOrDefault().ToString(""),
                    list4.Where(r => r.AntibioticName == name).Select(r =>r.SensitiveRatio.GetDecimal2(1)).FirstOrDefault().ToString("")
                };
            }

            Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> tuple = new Tuple<Dictionary<string, int>, Dictionary<string, List<string>>>(thead, tbody);
            return tuple;
        }

        /// <summary>
        /// 医学数据统计 --  表12：流感嗜血杆菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> GetMedicalDataTableByhin(long medicalDataId)
        {
            long organismId = 678; // 流感嗜血杆菌(Haemophilus influenzae)

            List<MedicalByAntibioticResult> total_list = new List<MedicalByAntibioticResult>();
            List<MedicalByAntibioticResult> ped_list = new List<MedicalByAntibioticResult>();
            List<MedicalByAntibioticResult> adu_list = new List<MedicalByAntibioticResult>();

            List<GetAntibioticResultModel> resultList = new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> pedResultModelList = new List<GetAntibioticResultModel>();
            List<GetAntibioticResultModel> aduResultModelList = new List<GetAntibioticResultModel>();

            #region 取数据
            List<MedicalDataItem> medicalDataItems = medicalDataItemService.Query(m => m.Mark > 0 && m.MedicalDataId == medicalDataId && m.OrganismId == organismId && m.IsValid);

            medicalDataItems.ForEach(item =>
            {
                int _age = 0;
                if (item.AGE.IsInt() || item.AGE.ToLowerInvariant().Contains("y"))
                {
                    _age = item.AGE.ToLowerInvariant().Replace("y", "").GetInt() * 365;
                    ////年
                    //int age = item.AGE.Replace("y", "").GetInt();
                    //if (age >= 1 && age <= 14)
                    //    item.PAT_TYPE = "ped";
                    //else if (age >= 15 && age <= 65)
                    //    item.PAT_TYPE = "adu";
                    //else if (age > 65)
                    //    item.PAT_TYPE = "ger";
                }
                else if (item.AGE.ToLowerInvariant().Contains("d"))
                {
                    _age = item.AGE.ToLowerInvariant().Replace("d", "").GetInt();
                    //int age = item.AGE.ToLowerInvariant().Replace("d", "").GetInt();
                    //if (age <= 28)
                    //    item.PAT_TYPE = "new";
                    //else if (age > 28)
                    //    item.PAT_TYPE = "ped";
                }
                else if (item.AGE.ToLowerInvariant().Contains("w"))
                {
                    _age = item.AGE.ToLowerInvariant().Replace("w", "").GetInt() * 7;
                    //int age = item.AGE.Replace("w", "").GetInt();
                    //if (age <= 4)
                    //    item.PAT_TYPE = "new";
                    //else if (age > 4)
                    //    item.PAT_TYPE = "ped";
                }
                else if (item.AGE.ToLowerInvariant().Contains("m"))
                {
                    _age = item.AGE.ToLowerInvariant().Replace("w", "").GetInt() * 30;
                    //int age = item.AGE.Replace("m", "").GetInt();
                    //if (age >= 1)
                    //    item.PAT_TYPE = "ped";
                }

                if (_age >= 18 * 365)
                {
                    item.PAT_TYPE = "adu";
                }
                else
                {
                    item.PAT_TYPE = "ped";
                }
            });

            // 儿童
            List<long> ped_ids = medicalDataItems.Where(r => r.PAT_TYPE == "ped").Select(r => r.Id).ToList();

            // 成人
            List<long> adu_ids = medicalDataItems.Where(r => r.PAT_TYPE == "adu").Select(r => r.Id).ToList();

            // 全部
            List<long> all_ids = medicalDataItems.Where(r => r.PAT_TYPE == "adu" || r.PAT_TYPE == "ped").Select(r => r.Id).ToList();

            List<MedicalAntibioticResult> totalResultList = _medicalAntibioticResult.Query(r => r.MedicalDataId == medicalDataId && all_ids.Contains(r.MedicalDataItemId) && r.Mark > 0 && r.IsValid);
            List<MedicalAntibioticResult> pedResultList = _medicalAntibioticResult.Query(r => r.MedicalDataId == medicalDataId && ped_ids.Contains(r.MedicalDataItemId) && r.Mark > 0 && r.IsValid);
            List<MedicalAntibioticResult> aduResultList = _medicalAntibioticResult.Query(r => r.MedicalDataId == medicalDataId && adu_ids.Contains(r.MedicalDataItemId) && r.Mark > 0 && r.IsValid);

            #endregion

            #region 关联药物
            List<MedicalAntibiotic> antibioticList = _medicalAntibioticService.Query(r => r.Mark > 0);

            foreach (MedicalAntibiotic antibioticItem in antibioticList)
            {
                resultList.Add(new GetAntibioticResultModel()
                {
                    OrganismId = 678, // 固定值
                    OrganismCode = "hin", // 固定值
                    OrganismName = "流感嗜血杆菌(Haemophilus influenzae)", // 固定值
                    AntibioticCode = antibioticItem.Code,
                    AntibioticId = antibioticItem.Id,
                    AntibioticName = antibioticItem.Name,
                    OrganismCount = ped_ids.Count() + adu_ids.Count()
                });

                aduResultModelList.Add(new GetAntibioticResultModel()
                {
                    OrganismId = 678, // 固定值
                    OrganismCode = "hin", // 固定值
                    OrganismName = "流感嗜血杆菌(Haemophilus influenzae)", // 固定值
                    AntibioticCode = antibioticItem.Code,
                    AntibioticId = antibioticItem.Id,
                    AntibioticName = antibioticItem.Name,
                    OrganismCount = adu_ids.Count()
                });

                pedResultModelList.Add(new GetAntibioticResultModel()
                {
                    OrganismId = 678, // 固定值
                    OrganismCode = "hin", // 固定值
                    OrganismName = "流感嗜血杆菌(Haemophilus influenzae)", // 固定值
                    AntibioticCode = antibioticItem.Code,
                    AntibioticId = antibioticItem.Id,
                    AntibioticName = antibioticItem.Name,
                    OrganismCount = ped_ids.Count()
                });
            }

            #endregion

            #region 计算耐药
            totalResultList.ForEach(node =>
            {
                //设置数据的耐药性数量  优先级：E-TSET > MIC > KB （_NE > __NM > _ND）	 
                this.GetResultNodeCount(resultList, organismId, "AMK", node.AMK_NE,node.AMK_NM, node.AMK_ND30);
                this.GetResultNodeCount(resultList, organismId, "AMP", node.AMP_NE,node.AMP_NM, node.AMP_ND10);
                this.GetResultNodeCount(resultList, organismId, "AMX", node.AMX_NM, node.AMX_ND30, node.AMX_ND25);
                this.GetResultNodeCount(resultList, organismId, "ATM", node.ATM_NE,node.ATM_NM, node.ATM_ND30);
                this.GetResultNodeCount(resultList, organismId, "AMC", node.AMC_NM, node.AMC_ND20,node.AMC_NE);
                this.GetResultNodeCount(resultList, organismId, "AZM", node.AZM_NE,node.AZM_NM, node.AZM_ND15);
                this.GetResultNodeCount(resultList, organismId, "CAZ", node.CAZ_NE,node.CAZ_NM, node.CAZ_ND30);
                this.GetResultNodeCount(resultList, organismId, "CEC", node.CEC_NE,node.CEC_NM, node.CEC_ND30);
                this.GetResultNodeCount(resultList, organismId, "CEP", node.CEP_ND30);
                this.GetResultNodeCount(resultList, organismId, "CFP", node.CFP_NE,node.CFP_NM, node.CFP_ND75);
                this.GetResultNodeCount(resultList, organismId, "CHL", node.CHL_NE,node.CHL_NM, node.CHL_ND30);
                this.GetResultNodeCount(resultList, organismId, "CIP", node.CIP_NE,node.CIP_NM, node.CIP_ND5);
                this.GetResultNodeCount(resultList, organismId, "CLI", node.CLI_NE,node.CLI_NM, node.CLI_ND2);
                this.GetResultNodeCount(resultList, organismId, "CRB", node.CRB_ND100);
                this.GetResultNodeCount(resultList, organismId, "CRO", node.CRO_NE,node.CRO_NM, node.CRO_ND30);
                this.GetResultNodeCount(resultList, organismId, "CSL", node.CSL_NM, node.CSL_ND30, node.CSL_ND75);
                this.GetResultNodeCount(resultList, organismId, "CTT", node.CTT_NE,node.CTT_NM, node.CTT_ND30);
                this.GetResultNodeCount(resultList, organismId, "CTX", node.CTX_NE,node.CTX_NE, node.CTX_NM, node.CTX_ND30);
                this.GetResultNodeCount(resultList, organismId, "CXM", node.CXM_NE,node.CXM_NM, node.CXM_ND30);
                this.GetResultNodeCount(resultList, organismId, "CZO", node.CZO_NE,node.CZO_NM, node.CZO_ND30);
                this.GetResultNodeCount(resultList, organismId, "CZX", node.CZX_ND30);
                this.GetResultNodeCount(resultList, organismId, "DOR", node.DOR_NE,node.DOR_NM, node.DOR_ND10);
                this.GetResultNodeCount(resultList, organismId, "DOX", node.DOX_NE,node.DOX_NM,node.DOX_ND30);
                this.GetResultNodeCount(resultList, organismId, "ERY", node.ERY_NE,node.ERY_NM, node.ERY_ND15);
                this.GetResultNodeCount(resultList, organismId, "ETP", node.ETP_NE,node.ETP_NM, node.ETP_ND10);
                this.GetResultNodeCount(resultList, organismId, "FEP", node.FEP_NE,node.FEP_NM, node.FEP_ND30);
                this.GetResultNodeCount(resultList, organismId, "FOS", node.FOS_NE,node.FOS_NM, node.FOS_ND200);
                this.GetResultNodeCount(resultList, organismId, "FOX", node.FOX_NE,node.FOX_NM, node.FOX_ND30);
                this.GetResultNodeCount(resultList, organismId, "GEH", node.GEH_NM, node.GEH_ND120);
                this.GetResultNodeCount(resultList, organismId, "GEN", node.GEN_NE,node.GEN_NM, node.GEN_ND10);
                this.GetResultNodeCount(resultList, organismId, "IPM", node.IPM_NE,node.IPM_NM, node.IPM_ND10);
                this.GetResultNodeCount(resultList, organismId, "LNZ", node.LNZ_NE,node.LNZ_NM, node.LNZ_ND30);
                this.GetResultNodeCount(resultList, organismId, "LVX", node.LVX_NE,node.LVX_NM, node.LVX_ND5);
                this.GetResultNodeCount(resultList, organismId, "MAN", node.MAN_ND30);
                this.GetResultNodeCount(resultList, organismId, "MEM", node.MEM_NE,node.MEM_NM, node.MEM_ND10);
                this.GetResultNodeCount(resultList, organismId, "MET", node.MET_NM, node.MET_ND5);
                this.GetResultNodeCount(resultList, organismId, "MEZ", node.MEZ_ND75);
                this.GetResultNodeCount(resultList, organismId, "MFX", node.MFX_NM, node.MFX_ND5, node.MFX_ND);
                this.GetResultNodeCount(resultList, organismId, "MNO", node.MNO_NE,node.MNO_NM, node.MNO_ND30);
                this.GetResultNodeCount(resultList, organismId, "NET", node.NET_NE,node.NET_NM, node.NET_ND30);
                this.GetResultNodeCount(resultList, organismId, "NIT", node.NIT_NE,node.NIT_NM, node.NIT_ND300);
                this.GetResultNodeCount(resultList, organismId, "NOR", node.NOR_ND10);
                this.GetResultNodeCount(resultList, organismId, "NOV", node.NOV_ND5);
                this.GetResultNodeCount(resultList, organismId, "OFX", node.OFX_ND5);
                this.GetResultNodeCount(resultList, organismId, "OXA", node.OXA_NE,node.OXA_NM, node.OXA_ND1);
                this.GetResultNodeCount(resultList, organismId, "PEN", node.PEN_NE, node.PEN_NM, node.PEN_ND10);
                this.GetResultNodeCount(resultList, organismId, "PIP", node.PIP_NE,node.PIP_NM, node.PIP_ND100);
                this.GetResultNodeCount(resultList, organismId, "POL", node.POL_NE,node.POL_NM, node.POL_ND300);
                this.GetResultNodeCount(resultList, organismId, "QDA", node.QDA_NE,node.QDA_NM, node.QDA_ND15);
                this.GetResultNodeCount(resultList, organismId, "RIF", node.RIF_NE,node.RIF_NM, node.RIF_ND5);
                this.GetResultNodeCount(resultList, organismId, "SAM", node.SAM_NE,node.SAM_NM, node.SAM_ND10);
                this.GetResultNodeCount(resultList, organismId, "SSS", node.SSS_ND200);
                this.GetResultNodeCount(resultList, organismId, "STH", node.STH_NE,node.STH_NM, node.STH_ND300);
                this.GetResultNodeCount(resultList, organismId, "STR", node.STR_NE,node.STR_NM, node.STR_ND10);
                this.GetResultNodeCount(resultList, organismId, "SXT", node.SXT_NE,node.SXT_NM, node.SXT_ND1_2);
                this.GetResultNodeCount(resultList, organismId, "TCC", node.TCC_NE,node.TCC_NM, node.TCC_ND75);
                this.GetResultNodeCount(resultList, organismId, "TCY", node.TCY_NE,node.TCY_NM, node.TCY_ND30);
                this.GetResultNodeCount(resultList, organismId, "TEC", node.TEC_NE,node.TEC_NM, node.TEC_ND30);
                this.GetResultNodeCount(resultList, organismId, "TGC", node.TGC_NE,node.TGC_NM, node.TGC_ND15);
                this.GetResultNodeCount(resultList, organismId, "TIC", node.TIC_NE,node.TIC_NM, node.TIC_ND75);
                this.GetResultNodeCount(resultList, organismId, "TOB", node.TOB_NE,node.TOB_NM, node.TOB_ND10);
                this.GetResultNodeCount(resultList, organismId, "TZP", node.TZP_NE,node.TZP_NM, node.TZP_ND100);
                this.GetResultNodeCount(resultList, organismId, "VAN", node.VAN_NE, node.VAN_NM, node.VAN_ND30);
                this.GetResultNodeCount(resultList, organismId, "CPT", node.CPT_NE, node.CPT_NM, node.CPT_ND30);
                this.GetResultNodeCount(resultList, organismId, "CZA", node.CZA_NE, node.CZA_NM, node.CZA_ND30);
                this.GetResultNodeCount(resultList, organismId, "AZA", node.AZA_NE, node.AZA_NM, node.AZA_ND30);
                this.GetResultNodeCount(resultList, organismId, "CZT", node.CZT_NE, node.CZT_NM, node.CZT_ND30);
                this.GetResultNodeCount(resultList, organismId, "COL", node.COL_NE, node.COL_NM, node.COL_ND10);//新增
            });

            aduResultList.ForEach(node =>
            {
                //设置数据的耐药性数量  优先级：E-TSET > MIC > KB （_NE > __NM > _ND）	 
                this.GetResultNodeCount(aduResultModelList, organismId, "AMK", node.AMK_NE, node.AMK_NM, node.AMK_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "AMP", node.AMP_NE, node.AMP_NM, node.AMP_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "AMX", node.AMX_NM, node.AMX_ND30, node.AMX_ND25);
                this.GetResultNodeCount(aduResultModelList, organismId, "ATM", node.ATM_NE, node.ATM_NM, node.ATM_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "AMC", node.AMC_NM, node.AMC_ND20, node.AMC_NE);
                this.GetResultNodeCount(aduResultModelList, organismId, "AZM", node.AZM_NE, node.AZM_NM, node.AZM_ND15);
                this.GetResultNodeCount(aduResultModelList, organismId, "CAZ", node.CAZ_NE, node.CAZ_NM, node.CAZ_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CEC", node.CEC_NE, node.CEC_NM, node.CEC_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CEP", node.CEP_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CFP", node.CFP_NE, node.CFP_NM, node.CFP_ND75);
                this.GetResultNodeCount(aduResultModelList, organismId, "CHL", node.CHL_NE, node.CHL_NM, node.CHL_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CIP", node.CIP_NE, node.CIP_NM, node.CIP_ND5);
                this.GetResultNodeCount(aduResultModelList, organismId, "CLI", node.CLI_NE, node.CLI_NM, node.CLI_ND2);
                this.GetResultNodeCount(aduResultModelList, organismId, "CRB", node.CRB_ND100);
                this.GetResultNodeCount(aduResultModelList, organismId, "CRO", node.CRO_NE, node.CRO_NM, node.CRO_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CSL", node.CSL_NM, node.CSL_ND30, node.CSL_ND75);
                this.GetResultNodeCount(aduResultModelList, organismId, "CTT", node.CTT_NE, node.CTT_NM, node.CTT_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CTX", node.CTX_NE, node.CTX_NE, node.CTX_NM, node.CTX_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CXM", node.CXM_NE, node.CXM_NM, node.CXM_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CZO", node.CZO_NE, node.CZO_NM, node.CZO_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CZX", node.CZX_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "DOR", node.DOR_NE, node.DOR_NM, node.DOR_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "DOX", node.DOX_NE, node.DOX_NM, node.DOX_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "ERY", node.ERY_NE, node.ERY_NM, node.ERY_ND15);
                this.GetResultNodeCount(aduResultModelList, organismId, "ETP", node.ETP_NE, node.ETP_NM, node.ETP_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "FEP", node.FEP_NE, node.FEP_NM, node.FEP_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "FOS", node.FOS_NE, node.FOS_NM, node.FOS_ND200);
                this.GetResultNodeCount(aduResultModelList, organismId, "FOX", node.FOX_NE, node.FOX_NM, node.FOX_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "GEH", node.GEH_NM, node.GEH_ND120);
                this.GetResultNodeCount(aduResultModelList, organismId, "GEN", node.GEN_NE, node.GEN_NM, node.GEN_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "IPM", node.IPM_NE, node.IPM_NM, node.IPM_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "LNZ", node.LNZ_NE, node.LNZ_NM, node.LNZ_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "LVX", node.LVX_NE, node.LVX_NM, node.LVX_ND5);
                this.GetResultNodeCount(aduResultModelList, organismId, "MAN", node.MAN_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "MEM", node.MEM_NE, node.MEM_NM, node.MEM_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "MET", node.MET_NM, node.MET_ND5);
                this.GetResultNodeCount(aduResultModelList, organismId, "MEZ", node.MEZ_ND75);
                this.GetResultNodeCount(aduResultModelList, organismId, "MFX", node.MFX_NM, node.MFX_ND5, node.MFX_ND);
                this.GetResultNodeCount(aduResultModelList, organismId, "MNO", node.MNO_NE, node.MNO_NM, node.MNO_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "NET", node.NET_NE, node.NET_NM, node.NET_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "NIT", node.NIT_NE, node.NIT_NM, node.NIT_ND300);
                this.GetResultNodeCount(aduResultModelList, organismId, "NOR", node.NOR_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "NOV", node.NOV_ND5);
                this.GetResultNodeCount(aduResultModelList, organismId, "OFX", node.OFX_ND5);
                this.GetResultNodeCount(aduResultModelList, organismId, "OXA", node.OXA_NE, node.OXA_NM, node.OXA_ND1);
                this.GetResultNodeCount(aduResultModelList, organismId, "PEN", node.PEN_NE, node.PEN_NM, node.PEN_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "PIP", node.PIP_NE, node.PIP_NM, node.PIP_ND100);
                this.GetResultNodeCount(aduResultModelList, organismId, "POL", node.POL_NE, node.POL_NM, node.POL_ND300);
                this.GetResultNodeCount(aduResultModelList, organismId, "QDA", node.QDA_NE, node.QDA_NM, node.QDA_ND15);
                this.GetResultNodeCount(aduResultModelList, organismId, "RIF", node.RIF_NE, node.RIF_NM, node.RIF_ND5);
                this.GetResultNodeCount(aduResultModelList, organismId, "SAM", node.SAM_NE, node.SAM_NM, node.SAM_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "SSS", node.SSS_ND200);
                this.GetResultNodeCount(aduResultModelList, organismId, "STH", node.STH_NE, node.STH_NM, node.STH_ND300);
                this.GetResultNodeCount(aduResultModelList, organismId, "STR", node.STR_NE, node.STR_NM, node.STR_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "SXT", node.SXT_NE, node.SXT_NM, node.SXT_ND1_2);
                this.GetResultNodeCount(aduResultModelList, organismId, "TCC", node.TCC_NE, node.TCC_NM, node.TCC_ND75);
                this.GetResultNodeCount(aduResultModelList, organismId, "TCY", node.TCY_NE, node.TCY_NM, node.TCY_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "TEC", node.TEC_NE, node.TEC_NM, node.TEC_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "TGC", node.TGC_NE, node.TGC_NM, node.TGC_ND15);
                this.GetResultNodeCount(aduResultModelList, organismId, "TIC", node.TIC_NE, node.TIC_NM, node.TIC_ND75);
                this.GetResultNodeCount(aduResultModelList, organismId, "TOB", node.TOB_NE, node.TOB_NM, node.TOB_ND10);
                this.GetResultNodeCount(aduResultModelList, organismId, "TZP", node.TZP_NE, node.TZP_NM, node.TZP_ND100);
                this.GetResultNodeCount(aduResultModelList, organismId, "VAN", node.VAN_NE, node.VAN_NM, node.VAN_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CPT", node.CPT_NE, node.CPT_NM, node.CPT_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CZA", node.CZA_NE, node.CZA_NM, node.CZA_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "AZA", node.AZA_NE, node.AZA_NM, node.AZA_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "CZT", node.CZT_NE, node.CZT_NM, node.CZT_ND30);
                this.GetResultNodeCount(aduResultModelList, organismId, "COL", node.COL_NE, node.COL_NM, node.COL_ND10);//新增
            });

            pedResultList.ForEach(node =>
            {
                //设置数据的耐药性数量  优先级：E-TSET > MIC > KB （_NE > __NM > _ND）	 
                this.GetResultNodeCount(pedResultModelList, organismId, "AMK", node.AMK_NE, node.AMK_NM, node.AMK_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "AMP", node.AMP_NE, node.AMP_NM, node.AMP_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "AMX", node.AMX_NM, node.AMX_ND30, node.AMX_ND25);
                this.GetResultNodeCount(pedResultModelList, organismId, "ATM", node.ATM_NE, node.ATM_NM, node.ATM_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "AMC", node.AMC_NM, node.AMC_ND20, node.AMC_NE);
                this.GetResultNodeCount(pedResultModelList, organismId, "AZM", node.AZM_NE, node.AZM_NM, node.AZM_ND15);
                this.GetResultNodeCount(pedResultModelList, organismId, "CAZ", node.CAZ_NE, node.CAZ_NM, node.CAZ_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CEC", node.CEC_NE, node.CEC_NM, node.CEC_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CEP", node.CEP_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CFP", node.CFP_NE, node.CFP_NM, node.CFP_ND75);
                this.GetResultNodeCount(pedResultModelList, organismId, "CHL", node.CHL_NE, node.CHL_NM, node.CHL_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CIP", node.CIP_NE, node.CIP_NM, node.CIP_ND5);
                this.GetResultNodeCount(pedResultModelList, organismId, "CLI", node.CLI_NE, node.CLI_NM, node.CLI_ND2);
                this.GetResultNodeCount(pedResultModelList, organismId, "CRB", node.CRB_ND100);
                this.GetResultNodeCount(pedResultModelList, organismId, "CRO", node.CRO_NE, node.CRO_NM, node.CRO_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CSL", node.CSL_NM, node.CSL_ND30, node.CSL_ND75);
                this.GetResultNodeCount(pedResultModelList, organismId, "CTT", node.CTT_NE, node.CTT_NM, node.CTT_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CTX", node.CTX_NE, node.CTX_NE, node.CTX_NM, node.CTX_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CXM", node.CXM_NE, node.CXM_NM, node.CXM_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CZO", node.CZO_NE, node.CZO_NM, node.CZO_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CZX", node.CZX_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "DOR", node.DOR_NE, node.DOR_NM, node.DOR_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "DOX", node.DOX_NE, node.DOX_NM, node.DOX_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "ERY", node.ERY_NE, node.ERY_NM, node.ERY_ND15);
                this.GetResultNodeCount(pedResultModelList, organismId, "ETP", node.ETP_NE, node.ETP_NM, node.ETP_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "FEP", node.FEP_NE, node.FEP_NM, node.FEP_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "FOS", node.FOS_NE, node.FOS_NM, node.FOS_ND200);
                this.GetResultNodeCount(pedResultModelList, organismId, "FOX", node.FOX_NE, node.FOX_NM, node.FOX_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "GEH", node.GEH_NM, node.GEH_ND120);
                this.GetResultNodeCount(pedResultModelList, organismId, "GEN", node.GEN_NE, node.GEN_NM, node.GEN_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "IPM", node.IPM_NE, node.IPM_NM, node.IPM_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "LNZ", node.LNZ_NE, node.LNZ_NM, node.LNZ_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "LVX", node.LVX_NE, node.LVX_NM, node.LVX_ND5);
                this.GetResultNodeCount(pedResultModelList, organismId, "MAN", node.MAN_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "MEM", node.MEM_NE, node.MEM_NM, node.MEM_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "MET", node.MET_NM, node.MET_ND5);
                this.GetResultNodeCount(pedResultModelList, organismId, "MEZ", node.MEZ_ND75);
                this.GetResultNodeCount(pedResultModelList, organismId, "MFX", node.MFX_NM, node.MFX_ND5, node.MFX_ND);
                this.GetResultNodeCount(pedResultModelList, organismId, "MNO", node.MNO_NE, node.MNO_NM, node.MNO_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "NET", node.NET_NE, node.NET_NM, node.NET_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "NIT", node.NIT_NE, node.NIT_NM, node.NIT_ND300);
                this.GetResultNodeCount(pedResultModelList, organismId, "NOR", node.NOR_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "NOV", node.NOV_ND5);
                this.GetResultNodeCount(pedResultModelList, organismId, "OFX", node.OFX_ND5);
                this.GetResultNodeCount(pedResultModelList, organismId, "OXA", node.OXA_NE, node.OXA_NM, node.OXA_ND1);
                this.GetResultNodeCount(pedResultModelList, organismId, "PEN", node.PEN_NE, node.PEN_NM, node.PEN_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "PIP", node.PIP_NE, node.PIP_NM, node.PIP_ND100);
                this.GetResultNodeCount(pedResultModelList, organismId, "POL", node.POL_NE, node.POL_NM, node.POL_ND300);
                this.GetResultNodeCount(pedResultModelList, organismId, "QDA", node.QDA_NE, node.QDA_NM, node.QDA_ND15);
                this.GetResultNodeCount(pedResultModelList, organismId, "RIF", node.RIF_NE, node.RIF_NM, node.RIF_ND5);
                this.GetResultNodeCount(pedResultModelList, organismId, "SAM", node.SAM_NE, node.SAM_NM, node.SAM_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "SSS", node.SSS_ND200);
                this.GetResultNodeCount(pedResultModelList, organismId, "STH", node.STH_NE, node.STH_NM, node.STH_ND300);
                this.GetResultNodeCount(pedResultModelList, organismId, "STR", node.STR_NE, node.STR_NM, node.STR_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "SXT", node.SXT_NE, node.SXT_NM, node.SXT_ND1_2);
                this.GetResultNodeCount(pedResultModelList, organismId, "TCC", node.TCC_NE, node.TCC_NM, node.TCC_ND75);
                this.GetResultNodeCount(pedResultModelList, organismId, "TCY", node.TCY_NE, node.TCY_NM, node.TCY_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "TEC", node.TEC_NE, node.TEC_NM, node.TEC_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "TGC", node.TGC_NE, node.TGC_NM, node.TGC_ND15);
                this.GetResultNodeCount(pedResultModelList, organismId, "TIC", node.TIC_NE, node.TIC_NM, node.TIC_ND75);
                this.GetResultNodeCount(pedResultModelList, organismId, "TOB", node.TOB_NE, node.TOB_NM, node.TOB_ND10);
                this.GetResultNodeCount(pedResultModelList, organismId, "TZP", node.TZP_NE, node.TZP_NM, node.TZP_ND100);
                this.GetResultNodeCount(pedResultModelList, organismId, "VAN", node.VAN_NE, node.VAN_NM, node.VAN_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CPT", node.CPT_NE, node.CPT_NM, node.CPT_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CZA", node.CZA_NE, node.CZA_NM, node.CZA_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "AZA", node.AZA_NE, node.AZA_NM, node.AZA_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "CZT", node.CZT_NE, node.CZT_NM, node.CZT_ND30);
                this.GetResultNodeCount(pedResultModelList, organismId, "COL", node.COL_NE, node.COL_NM, node.COL_ND10);//新增              
            });
            #endregion

            resultList = resultList.Where(m => m.IntermediaryCount > 0 || m.ResistanceCount > 0 || m.SensitiveCount > 0).ToList();
            pedResultModelList = pedResultModelList.Where(m => m.IntermediaryCount > 0 || m.ResistanceCount > 0 || m.SensitiveCount > 0).ToList();
            aduResultModelList = aduResultModelList.Where(m => m.IntermediaryCount > 0 || m.ResistanceCount > 0 || m.SensitiveCount > 0).ToList();


            IEnumerable<string> fields = from item in typeof(MedicalDataItem).GetProperties() select item.Name;

            AmendDataCount(resultList, medicalDataItems.Where(r => r.PAT_TYPE == "adu" || r.PAT_TYPE == "ped").ToList(), fields);
            AmendDataCount(aduResultModelList, medicalDataItems.Where(r => r.PAT_TYPE == "adu").ToList(), fields);
            AmendDataCount(pedResultModelList, medicalDataItems.Where(r => r.PAT_TYPE == "ped").ToList(), fields);

            // 药物名称
            List<string> names = new List<string>();
            resultList.ForEach(item => { names.Add(item.AntibioticName); });
            names = names.GroupBy(r => r).Select(r => r.Key).ToList();

            Dictionary<string, int> thead = new Dictionary<string, int>
            {
                ["合计"] = all_ids.Count(),
                ["成人"] = adu_ids.Count(),
                ["儿童"] = ped_ids.Count()
            };

            Dictionary<string, List<string>> tbody = new Dictionary<string, List<string>>();

            foreach (var name in names)
            {
                var _hj = resultList.Where(r => r.AntibioticName == name).FirstOrDefault();
                var _cr = aduResultModelList.Where(r => r.AntibioticName == name).FirstOrDefault();
                var _et = pedResultModelList.Where(r => r.AntibioticName == name).FirstOrDefault();
                tbody[name] = new List<string> {
                    // 合计： 数量，耐药率，敏感率
                    (_hj?.DataCount ?? 0).ToString(),
                    (_hj?.ResistanceRatio ?? 0).ToString(),
                    (_hj?.SensitiveRatio ?? 0).ToString(),
                    // 成人： 数量，耐药率，敏感率
                    (_cr?.DataCount ?? 0).ToString(),
                    (_cr?.ResistanceRatio ?? 0).ToString(),
                    (_cr?.SensitiveRatio ?? 0).ToString(),
                    // 儿童： 数量，耐药率，敏感率
                    (_et?.DataCount ?? 0).ToString(),
                    (_et?.ResistanceRatio ?? 0).ToString(),
                    (_et?.SensitiveRatio ?? 0).ToString()
                };
            }
            return new Tuple<Dictionary<string, int>, Dictionary<string, List<string>>>(thead, tbody);
        }

        private static void AmendDataCount(List<GetAntibioticResultModel> resultList, List<MedicalDataItem> medicalDataItems, IEnumerable<string> fields)
        {
            foreach (var item in resultList)
            {
                Expression<Func<MedicalDataItem, bool>> predicate = r => r.ORGANISM == item.OrganismCode && r.IsValid == true && r.Mark > 0;

                List<string> fieldItems = fields.Where(r => r.StartsWith(item.AntibioticCode)).ToList();

                Expression<Func<MedicalDataItem, bool>> _innerPredicate = r => false;


                foreach (var fieldItem in fieldItems)
                {
                    predicate = predicate.And(r => !(r.GetType().GetProperty(fieldItem).GetValue(r, null) ?? "").ToString().Trim().Equals("0"));
                    _innerPredicate = _innerPredicate.Or(r => (r.GetType().GetProperty(fieldItem).GetValue(r, null) ?? "").ToString().Trim().Length > 0);
                }
                predicate = predicate.And(_innerPredicate);

                item.DataCount = medicalDataItems.Count(predicate.Compile());
            }
        }

        private void GetResultNodeCount(List<GetAntibioticResultModel> resultList, long organismId, string code, int value1, int value2 = 0, int value3 = 0, int value4 = 0)
        {
            var model = resultList.Where(m => m.OrganismId == organismId && m.AntibioticCode.ToUpper().Equals(code.ToUpper())).FirstOrDefault();
            if (model == null) return;

            //0：无效值  1：敏感   2：中介   3：耐药
            if (value1 > 0)
            {
                switch (value1)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value2 > 0)
            {
                switch (value2)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value3 > 0)
            {
                switch (value3)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value4 > 0)
            {
                switch (value4)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
        }
        #endregion

        /// <summary>
        /// 医学数据统计 --  不发酵革兰阴性杆菌(Non-fermenting gram negative rods)
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByNonFermentingGramNegativeRods(long medicalDataId = 0)
        {
            long organismId = 240; // 非发酵革兰阴性杆菌(Non-fermenting gram negative rods)

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            //string cacheKey = "medical.statistics.mrsasensitivitydrug";
            //List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            //if (list != null && list.Any()) return list;
            List<MedicalByAntibioticResult> list = new List<MedicalByAntibioticResult>();
            if (medicalDataId == 0)
            {
                list = this.GetByMedicalAntibioticResult(organismId);
            }
            else
            {
                list = this.GetByMedicalAntibioticResult(medicalDataId, organismId, provinceId: 0);
            }
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            //    this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        /// <summary>
        /// 医学数据统计 --  流感嗜血杆菌(Haemophilus influenzae)
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByHaemophilusInfluenzae(long medicalDataId = 0)
        {
            long organismId = 678; // 流感嗜血杆菌(Haemophilus influenzae)

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            //string cacheKey = "medical.statistics.mrsasensitivitydrug";
            //List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            //if (list != null && list.Any()) return list;
            List<MedicalByAntibioticResult> list = new List<MedicalByAntibioticResult>();
            if (medicalDataId == 0)
            {
                list = this.GetByMedicalAntibioticResult(organismId);
            }
            else
            {
                list = this.GetByMedicalAntibioticResult(medicalDataId, organismId, provinceId: 0);
            }
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            //    this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        /// <summary>
        /// 医学数据统计 --  肺炎链球菌(Streptococcus pneumoniae)
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        public List<MedicalByAntibioticResult> GetByStreptococcusPneumoniae(long medicalDataId)
        {
            long organismId = 493; // 肺炎链球菌(Streptococcus pneumoniae)

            //默认使用缓存 ，暂时不开启缓存，如果有需要直接取消下面的注释即可
            //string cacheKey = "medical.statistics.mrsasensitivitydrug";
            //List<MedicalByAntibioticResult> list = this.CacheManager.Get<List<MedicalByAntibioticResult>>(cacheKey);
            //if (list != null && list.Any()) return list;
            List<MedicalByAntibioticResult> list = new List<MedicalByAntibioticResult>();
            if (medicalDataId == 0)
            {
                list = this.GetByMedicalAntibioticResult(organismId);
            }
            else
            {
                list = this.GetByMedicalAntibioticResult(medicalDataId, organismId, provinceId: 0);
            }
            if (list == null || !list.Any()) return null;

            //缓存10分钟
            //    this.CacheManager.Set(cacheKey, list, 10);
            return list;
        }

        /// <summary>
        /// 医学数据统计 -- 细菌在标本中占比
        /// </summary>
        /// <param name="medicalDataId">上传Id</param>
        /// <param name="specTpye">标本类型编码，多个采用英文逗号分隔</param>
        /// <returns></returns>
        public List<StatisticsMedicalSpecTypeModel> GetByBacteriaInSpecimenRatio(long medicalDataId, string specTpye, params SqlParameter[] sqlParameters)
        {
            string sql = $@"SELECT [OrganismName] AS [Name], COUNT(OrganismName) AS [Count] FROM dbo.MedicalDataItem WHERE Mark > 0 AND IsValid = 1 AND MedicalDataId = {medicalDataId} AND [SPEC_TYPE]  IN ({specTpye})  GROUP BY OrganismName;";

            List<StatisticsMedicalSpecTypeModel> list = this.DbContext.SqlQuery<StatisticsMedicalSpecTypeModel>(sql, sqlParameters).ToList();

            if (list == null || !list.Any()) return null;

            int count = list.Sum(m => m.Count);
            foreach (var item in list)
            {
                //计算占的比例
                item.Ratio = count > 0 ? (item.Count * 100m / count).GetDecimal2() : 0.0m;
            }

            if (list == null || !list.Any()) return list;

            return list.OrderByDescending(m => m.Ratio).ToList();
        }

        public Dictionary<string, Tuple<int, decimal>> GetTable01_2020(long medicalDataId)
        {
            Dictionary<string, Tuple<int, decimal>> data_table_01 = new Dictionary<string, Tuple<int, decimal>>();
            int hxdCount = GetCountBySpecIn(medicalDataId, "at,fn,br,no,rl,ru,sp,th,tr,ta,lu,mo,ea,em,eo,ba,tn");
            int ndCount = GetCountBySpecIn(medicalDataId, "ue,ur,uc,cv,ub,uz");
            int blCount = GetCountBySpecIn(medicalDataId, "bl");
            int sfCount = GetCountBySpecIn(medicalDataId, "sf");
            int sknyCount = GetCountBySpecIn(medicalDataId, "as,ad,ps,pt,ux,ui,sw,wd,fi,sb,um,ud,de,ac,pt,ak,ul");
            int wjtyCount = GetCountBySpecIn(medicalDataId, "ab,am,bi,mi,di,fl,ga,pf,bn,su");
            int szdfmwCount = GetCountBySpecIn(medicalDataId, "gn,gf,gm,va,sm,cx,pl,ut,iu,ed");
            int fbCount = GetCountBySpecIn(medicalDataId, "st,re,mc");
            int otherCount = GetCountBySpecNotIn(medicalDataId, "at,fn,br,no,rl,ru,sp,th,tr,ta,lu,mo,ea,em,eo,ba,tn,ue,ur,uc,cv,ub,uz,bl,sf,as,ad,ps,pt,ux,ui,sw,wd,fi,sb,um,ud,de,ac,pt,ak,ul,ab,am,bi,mi,di,fl,ga,pf,bn,su,gn,gf,gm,va,sm,cx,pl,ut,iu,ed,st,re,mc");

            int totalCount = hxdCount + ndCount + blCount + sfCount + sknyCount + wjtyCount + szdfmwCount + fbCount + otherCount;
            if (totalCount == 0)
            {
                data_table_01.Add("呼吸道标本", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("尿液标本", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("血液标本", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("脑脊液标本", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("伤口脓液标本", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("无菌体液", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("生殖道分泌物", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("粪便标本", new Tuple<int, decimal>(0, 0.0m));
                data_table_01.Add("其他", new Tuple<int, decimal>(0, 0.0m));
            }
            else
            {
                decimal hxdRate = hxdCount * 100.0m / totalCount;
                decimal ndRate = ndCount * 100.0m / totalCount;
                decimal blRate = blCount * 100.0m / totalCount;
                decimal sfRate = sfCount * 100.0m / totalCount;
                decimal sknyRate = sknyCount * 100.0m / totalCount;
                decimal wjtyRate = wjtyCount * 100.0m / totalCount;
                decimal szdfmwRate = szdfmwCount * 100.0m / totalCount;
                decimal fbRate = fbCount * 100.0m / totalCount;
                decimal otherRate = otherCount * 100.0m / totalCount;

                data_table_01.Add("呼吸道标本", new Tuple<int, decimal>(hxdCount, hxdRate.GetDecimal2()));
                data_table_01.Add("尿液标本", new Tuple<int, decimal>(ndCount, ndRate.GetDecimal2()));
                data_table_01.Add("血液标本", new Tuple<int, decimal>(blCount, blRate.GetDecimal2()));
                data_table_01.Add("脑脊液标本", new Tuple<int, decimal>(sfCount, sfRate.GetDecimal2()));
                data_table_01.Add("伤口脓液标本", new Tuple<int, decimal>(sknyCount, sknyRate.GetDecimal2()));
                data_table_01.Add("无菌体液", new Tuple<int, decimal>(wjtyCount, wjtyRate.GetDecimal2()));
                data_table_01.Add("生殖道分泌物", new Tuple<int, decimal>(szdfmwCount, szdfmwRate.GetDecimal2()));
                data_table_01.Add("粪便标本", new Tuple<int, decimal>(fbCount, fbRate.GetDecimal2()));
                data_table_01 = data_table_01.OrderByDescending(o => o.Value.Item1).ToDictionary(k => k.Key, v => v.Value);
                data_table_01.Add("其他", new Tuple<int, decimal>(otherCount, otherRate.GetDecimal2()));
            }

            return data_table_01;
        }

        public List<StatisticsMedicalSpecTypeModel> GetTable03_2020(long medicalDataId, string specTpye)
        {
            string sql = $@"SELECT [OrganismName] AS [Name], COUNT(OrganismName) AS [Count] FROM dbo.MedicalDataItem WHERE Mark > 0 AND IsValid = 1 AND MedicalDataId = {medicalDataId} ";
            StringBuilder sqlSpecTpye = new StringBuilder("");
            List<System.Data.SqlClient.SqlParameter> sqlParameters = new List<System.Data.SqlClient.SqlParameter>();
            if (!string.IsNullOrWhiteSpace(specTpye))
            {
                if (specTpye.Contains(","))
                {
                    specTpye = specTpye.Trim().Trim(',').Trim();

                    List<string> specTypeList = specTpye.Split(',').Distinct().ToList();
                    specTypeList.ForEach(item =>
                    {
                        item = item.Trim();
                        sqlSpecTpye.Append($"@{item}, ");
                        sqlParameters.Add(new System.Data.SqlClient.SqlParameter($"@{item}", item));
                    });
                }
                else
                {
                    sqlSpecTpye.Append($"@{specTpye}, ");
                    sqlParameters.Add(new System.Data.SqlClient.SqlParameter($"@{specTpye}", specTpye));
                }
                sql += $" AND [SPEC_TYPE] IN ({sqlSpecTpye.ToString().Trim().TrimEnd(',').Trim()}) ";
            }

            sql += " GROUP BY OrganismName;";

            List<StatisticsMedicalSpecTypeModel> list = this.DbContext.SqlQuery<StatisticsMedicalSpecTypeModel>(sql, sqlParameters.ToArray()).ToList();

            if (list == null || !list.Any()) return null;

            int count = list.Sum(m => m.Count);
            foreach (var item in list)
            {
                //计算占的比例
                item.Ratio = count > 0 ? (item.Count * 100m / count).GetDecimal2() : 0.0m;
            }

            if (list == null || !list.Any()) return list;

            return list.OrderByDescending(m => m.Ratio).ToList();
        }
        /// <summary>
        /// 医学数据统计 -- 细菌在标本中总数
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="specTpye"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int GetCountBySpecIn(long medicalDataId, string specTpye)
        {
            string sql = $@"SELECT COUNT(OrganismName) AS [Count] FROM dbo.MedicalDataItem WHERE Mark > 0 AND IsValid = 1 AND MedicalDataId = {medicalDataId} ";

            StringBuilder sqlSpecTpye = new StringBuilder("");
            List<System.Data.SqlClient.SqlParameter> sqlParameters = new List<System.Data.SqlClient.SqlParameter>();
            if (!string.IsNullOrWhiteSpace(specTpye))
            {
                if (specTpye.Contains(","))
                {
                    specTpye = specTpye.Trim().Trim(',').Trim();

                    List<string> specTypeList = specTpye.Split(',').Distinct().ToList();
                    specTypeList.ForEach(item =>
                    {
                        item = item.Trim();
                        sqlSpecTpye.Append($"@{item}, ");
                        sqlParameters.Add(new System.Data.SqlClient.SqlParameter($"@{item}", item));
                    });
                }
                else
                {
                    sqlSpecTpye.Append($"@{specTpye}, ");
                    sqlParameters.Add(new System.Data.SqlClient.SqlParameter($"@{specTpye}", specTpye));
                }
                sql += $" AND [SPEC_TYPE] IN ({sqlSpecTpye.ToString().Trim().TrimEnd(',').Trim()}) ";
            }


            return this.DbContext.SqlQuery<int>(sql, sqlParameters.ToArray()).FirstOrDefault();

        }

        public int GetCountBySpecNotIn(long medicalDataId, string specTpye)
        {
            string sql = $@"SELECT COUNT(OrganismName) AS [Count] FROM dbo.MedicalDataItem WHERE Mark > 0 AND IsValid = 1 AND MedicalDataId = {medicalDataId} ";

            StringBuilder sqlSpecTpye = new StringBuilder("");
            List<System.Data.SqlClient.SqlParameter> sqlParameters = new List<System.Data.SqlClient.SqlParameter>();
            if (!string.IsNullOrWhiteSpace(specTpye))
            {
                if (specTpye.Contains(","))
                {
                    specTpye = specTpye.Trim().Trim(',').Trim();

                    List<string> specTypeList = specTpye.Split(',').Distinct().ToList();
                    specTypeList.ForEach(item =>
                    {
                        item = item.Trim();
                        sqlSpecTpye.Append($"@{item}, ");
                        sqlParameters.Add(new System.Data.SqlClient.SqlParameter($"@{item}", item));
                    });
                }
                else
                {
                    sqlSpecTpye.Append($"@{specTpye}, ");
                    sqlParameters.Add(new System.Data.SqlClient.SqlParameter($"@{specTpye}", specTpye));
                }
                sql += $" AND [SPEC_TYPE] NOT IN ({sqlSpecTpye.ToString().Trim().TrimEnd(',').Trim()}) ";
            }
            else
            {
                sql += " AND 1 = 2 ";
            }

            return this.DbContext.SqlQuery<int>(sql, sqlParameters.ToArray()).FirstOrDefault();

        }

        #region  根据上传数据ID和细菌Id，查询耐药、中介、敏感的数量，用于检查数据使用，没有具体的功能

        /// <summary>
        /// 根据上传数据ID和细菌Id，查询耐药、中介、敏感的数量
        /// </summary>
        public void GetAntibioticResultCount()
        {
            long organismId = 908;
            long medicalDataId = 5603325627354930845;

            var list = this._medicalAntibioticResult.Query(m => m.MedicalDataId == medicalDataId && m.OrganismId == organismId).ToList();

            //0：无效值  1：敏感   2：中介   3：耐药
            int count = list.Count();  //总数量
            var mg = GetAntibioticResultCountItem(list, 1); // 1：敏感
            var zj = GetAntibioticResultCountItem(list, 2); // 2：中介
            var ny = GetAntibioticResultCountItem(list, 3); // 3：耐药

        }

        private int GetAntibioticResultCountItem(List<MedicalAntibioticResult> list, int value)
        {
            //AMK_ND30	AMX_ND25	AMC_ND20	AZM_ND15	AMP_ND10	SAM_ND10	ATM_ND30	OXA_ND1	POL_ND300	NIT_ND300	SXT_ND1_2	STH_ND300	GEH_ND120	ERY_ND15	CIP_ND5	MET_ND5	CLI_ND2	RIF_ND5	LNZ_ND30	STR_ND10	FOS_ND200	CHL_ND30	MEM_ND10	MNO_ND30	MFX_ND5	PIP_ND100	TZP_ND100	PEN_ND10	GEN_ND10	TCY_ND30	TCC_ND75	TIC_ND75	TEC_ND30	TGC_ND15	FEP_ND30	CXM_ND30	CEC_ND30	CFP_ND75	CSL_ND30	CRO_ND30	CTX_ND30	CAZ_ND30	FOX_ND30	CZO_ND30	TOB_ND10	VAN_ND30	IPM_ND10	LVX_ND5	SSS_ND200	CEP_ND30	CRB_ND100	OFX_ND5	CZX_ND30	MEZ_ND75	NOR_ND10	MAN_ND30	DOX_ND30	NOV_ND5	AMK_NM	AMX_NM	AMC_NM	AZM_NM	AMP_NM	SAM_NM	ATM_NM	OXA_NM	POL_NM	NIT_NM	SXT_NM	STH_NM	GEH_NM	ERY_NM	CIP_NM	MET_NM	CLI_NM	RIF_NM	LNZ_NM	STR_NM	FOS_NM	CHL_NM	MEM_NM	MNO_NM	MFX_NM	PIP_NM	TZP_NM	PEN_NM	GEN_NM	PEN_NE	TCY_NM	TCC_NM	TIC_NM	TEC_NM	TGC_NM	FEP_NM	CXM_NM	CEC_NM	CFP_NM	CSL_NM	CRO_NM	CTX_NM	CAZ_NM	FOX_NM	CZO_NM	TOB_NM	VAN_NM	VAN_NE	IPM_NM	LVX_NM	CTX_NE	CSL_ND75	AMX_ND30	ETP_ND10	ETP_NM	CTT_ND30	CTT_NM	DOR_ND10	DOR_NM	NET_ND30	NET_NM	QDA_ND15	QDA_NM	UploadRowIndex	SPEC_REAS	COMMENT	CCV_NM	CTC_NM	MFX_ND	DAP_NM	CPT_ND30	CPT_NM	CPT_NE	CZA_ND30	CZA_NM	CZA_NE	AZA_ND30	AZA_NM	AZA_NE	CZT_ND30	CZT_NM	CZT_NE TGC_NE DOX_NM

            //var data = list.Where(m =>
            // m.AMK_ND30 == value || m.AMX_ND25 == value || m.AMC_ND20 == value || m.AZM_ND15 == value || m.AMP_ND10 == value || m.SAM_ND10 == value || m.ATM_ND30 == value || m.OXA_ND1 == value || m.POL_ND300 == value || m.NIT_ND300 == value || m.SXT_ND1_2 == value || m.STH_ND300 == value || m.GEH_ND120 == value || m.ERY_ND15 == value || m.CIP_ND5 == value || m.MET_ND5 == value || m.CLI_ND2 == value || m.RIF_ND5 == value || m.LNZ_ND30 == value || m.STR_ND10 == value || m.FOS_ND200 == value || m.CHL_ND30 == value || m.MEM_ND10 == value || m.MNO_ND30 == value || m.MFX_ND5 == value || m.PIP_ND100 == value || m.TZP_ND100 == value || m.PEN_ND10 == value || m.GEN_ND10 == value || m.TCY_ND30 == value || m.TCC_ND75 == value || m.TIC_ND75 == value || m.TEC_ND30 == value || m.TGC_ND15 == value || m.FEP_ND30 == value || m.CXM_ND30 == value || m.CEC_ND30 == value || m.CFP_ND75 == value || m.CSL_ND30 == value || m.CRO_ND30 == value || m.CTX_ND30 == value || m.CAZ_ND30 == value || m.FOX_ND30 == value || m.CZO_ND30 == value || m.TOB_ND10 == value || m.VAN_ND30 == value || m.IPM_ND10 == value || m.LVX_ND5 == value || m.SSS_ND200 == value || m.CEP_ND30 == value || m.CRB_ND100 == value || m.OFX_ND5 == value || m.CZX_ND30 == value || m.MEZ_ND75 == value || m.NOR_ND10 == value || m.MAN_ND30 == value || m.DOX_ND30 == value || m.NOV_ND5 == value || m.AMK_NM == value || m.AMX_NM == value || m.AMC_NM == value || m.AZM_NM == value || m.AMP_NM == value || m.SAM_NM == value || m.ATM_NM == value || m.OXA_NM == value || m.POL_NM == value || m.NIT_NM == value || m.SXT_NM == value || m.STH_NM == value || m.GEH_NM == value || m.ERY_NM == value || m.CIP_NM == value || m.MET_NM == value || m.CLI_NM == value || m.RIF_NM == value || m.LNZ_NM == value || m.STR_NM == value || m.FOS_NM == value || m.CHL_NM == value || m.MEM_NM == value || m.MNO_NM == value || m.MFX_NM == value || m.PIP_NM == value || m.TZP_NM == value || m.PEN_NM == value || m.GEN_NM == value || m.PEN_NE == value || m.TCY_NM == value || m.TCC_NM == value || m.TIC_NM == value || m.TEC_NM == value || m.TGC_NM == value || m.FEP_NM == value || m.CXM_NM == value || m.CEC_NM == value || m.CFP_NM == value || m.CSL_NM == value || m.CRO_NM == value || m.CTX_NM == value || m.CAZ_NM == value || m.FOX_NM == value || m.CZO_NM == value || m.TOB_NM == value || m.VAN_NM == value || m.VAN_NE == value || m.IPM_NM == value || m.LVX_NM == value || m.CTX_NE == value || m.CSL_ND75 == value || m.AMX_ND30 == value || m.ETP_ND10 == value || m.ETP_NM == value || m.CTT_ND30 == value || m.CTT_NM == value || m.DOR_ND10 == value || m.DOR_NM == value || m.NET_ND30 == value || m.NET_NM == value || m.QDA_ND15 == value || m.QDA_NM == value || m.CCV_NM == value || m.CTC_NM == value || m.MFX_ND == value || m.DAP_NM == value || m.CPT_ND30 == value || m.CPT_NM == value || m.CPT_NE == value || m.CZA_ND30 == value || m.CZA_NM == value || m.CZA_NE == value || m.AZA_ND30 == value || m.AZA_NM == value || m.AZA_NE == value || m.CZT_ND30 == value || m.CZT_NM == value || m.CZT_NE == value || m.DOX_NM==value//最后两个新增字段
            //);
            var data = list.Where(m => m.COL_ND10 == value || m.COL_NM == value || m.COL_NE == value || m.AMC_ND20 == value || m.AMC_NM == value || m.AMC_NE == value || m.AMK_ND30 == value || m.AMK_NM == value || m.AMK_NE == value || m.AMP_ND10 == value || m.AMP_NM == value || m.AMP_NE == value || m.ATM_ND30 == value || m.ATM_NM == value || m.ATM_NE == value || m.AZA_ND30 == value || m.AZA_NE == value || m.AZA_NM == value || m.AZM_ND15 == value || m.AZM_NM == value || m.AZM_NE == value || m.CAZ_ND30 == value || m.CAZ_NM == value || m.CAZ_NE == value || m.CEC_ND30 == value || m.CEC_NM == value || m.CEC_NE == value || m.CFP_ND75 == value || m.CFP_NM == value || m.CFP_NE == value || m.CHL_ND30 == value || m.CHL_NM == value || m.CHL_NE == value || m.CIP_ND5 == value || m.CIP_NM == value || m.CIP_NE == value || m.CLI_ND2 == value || m.CLI_NM == value || m.CLI_NE == value || m.CPT_ND30 == value || m.CPT_NE == value || m.CPT_NM == value || m.CRO_ND30 == value || m.CRO_NM == value || m.CRO_NE == value || m.CSL_ND30 == value || m.CSL_ND75 == value || m.CSL_ND75 == value || m.CSL_NM == value || m.CTT_ND30 == value || m.CTT_NM == value || m.CTT_NE == value || m.CTX_ND30 == value || m.CTX_NE == value || m.CTX_NM == value || m.CXM_ND30 == value || m.CXM_NM == value || m.CXM_NE == value || m.CZA_ND30 == value || m.CZA_NE == value || m.CZA_NM == value || m.CZO_ND30 == value || m.CZO_NM == value || m.CZO_NE == value || m.CZT_ND30 == value || m.CZT_NE == value || m.CZT_NM == value || m.DOR_ND10 == value || m.DOR_NM == value || m.DOR_NE == value || m.DOX_ND30 == value || m.DOX_NM == value || m.DOX_NE == value || m.ERY_ND15 == value || m.ERY_NM == value || m.ERY_NE == value || m.ETP_ND10 == value || m.ETP_NM == value || m.ETP_NE == value || m.FEP_ND30 == value || m.FEP_NM == value || m.FEP_NE == value || m.FOS_ND200 == value || m.FOS_NM == value || m.FOS_NE == value || m.FOX_ND30 == value || m.FOX_NM == value || m.FOX_NE == value || m.GEH_ND120 == value || m.GEH_NM == value || m.GEN_ND10 == value || m.GEN_NM == value || m.GEN_NE == value || m.IPM_ND10 == value || m.IPM_NM == value || m.IPM_NE == value || m.LNZ_ND30 == value || m.LNZ_NM == value || m.LNZ_NE == value || m.LVX_ND5 == value || m.LVX_NM == value || m.LVX_NE == value || m.MEM_ND10 == value || m.MEM_NM == value || m.MEM_NE == value || m.MFX_ND5 == value || m.MFX_NM == value || m.MFX_NE == value || m.MNO_ND30 == value || m.MNO_NM == value || m.MNO_NE == value || m.NET_ND30 == value || m.NET_NM == value || m.NET_NE == value || m.NIT_ND300 == value || m.NIT_NM == value || m.NIT_NE == value || m.OFX_ND5 == value || m.OXA_ND1 == value || m.OXA_NM == value || m.OXA_NE == value || m.PEN_ND10 == value || m.PEN_NE == value || m.PEN_NM == value || m.PIP_ND100 == value || m.PIP_NM == value || m.PIP_NE == value || m.POL_ND300 == value || m.POL_NM == value || m.POL_NE == value || m.QDA_ND15 == value || m.QDA_NM == value || m.QDA_NE == value || m.RIF_ND5 == value || m.RIF_NM == value || m.RIF_NE == value || m.SAM_ND10 == value || m.SAM_NM == value || m.SAM_NE == value || m.STH_ND300 == value || m.STH_NM == value || m.STH_NE == value || m.STR_ND10 == value || m.STR_NM == value || m.STR_NE == value || m.SXT_ND1_2 == value || m.SXT_NM == value || m.SXT_NE == value || m.TCC_ND75 == value || m.TCC_NM == value || m.TCC_NE == value || m.TCY_ND30 == value || m.TCY_NM == value || m.TCY_NE == value || m.TEC_ND30 == value || m.TEC_NM == value || m.TEC_NE == value || m.TGC_ND15 == value || m.TGC_NM == value || m.TGC_NE == value || m.TIC_ND75 == value || m.TIC_NM == value || m.TIC_NE == value || m.TOB_ND10 == value || m.TOB_NM == value || m.TOB_NE == value || m.TZP_ND100 == value || m.TZP_NM == value || m.TZP_NE == value || m.VAN_ND30 == value || m.VAN_NE == value || m.VAN_NM == value);

            var counts = data.Count();
            return counts;
        }

        #endregion



    }
}
