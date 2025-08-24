using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Core.Domain.ScoringModule.Enum;
using ManageSystem.Core.Domain.ScoringModule.Statistics;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityScoreService : BaseService<DataQuality_Score>, IDataQualityScoreService
    {
        private readonly IDataQualityHospitalPageInitService _hospitalPageInitService;
        private readonly ICacheManager _cacheManager;
        public DataQualityScoreService(IRepository<DataQuality_Score> repository, IDataQualityHospitalPageInitService hospitalPageInitService, ICacheManager cacheManager) : base(repository)
        {
            _hospitalPageInitService = hospitalPageInitService;
            _cacheManager = cacheManager;
        }

        public long GetLastValidMedicalDataId(long hospitalId)
        {
            string sql = $"SELECT TOP(1) t2.Id FROM dbo.MedicalData AS t2 WHERE HospitalId = {hospitalId} AND t2.[Status] = 1 AND t2.[AntibioticResultStatue] = 3 ORDER BY t2.InsertTime DESC, t2.Id DESC";
            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<long>(sql);
            }
        }

        public int Count(long organismId, long hospitalId, int value, params string[] fields)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(1)
FROM dbo.MedicalAntibioticResult AS t1
WHERE t1.[Mark] > 0 AND t1.[IsValid] = 1 AND t1.MedicalDataId = (SELECT TOP(1) t2.Id FROM dbo.MedicalData AS t2 WHERE HospitalId = @hospitalId AND t2.[Status] = 1 AND t2.[AntibioticResultStatue] = 3 ORDER BY t2.InsertTime DESC, t2.Id DESC)
AND t1.[OrganismId] = @organismId ");
            if (fields != null && fields.Length > 0)
            {
                List<string> temp = new List<string>();
                foreach (var field in fields)
                {
                    temp.Add($" t1.[{field}] = @value ");
                }
                sql.Append($"AND ( {string.Join(" OR ", temp)} )");
            }
            sql.Append(";");

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString(), new { organismId, hospitalId, value });
            }
        }

        public int GetTotalCount(int year, long? hospitalId = null)
        {
            StringBuilder sql = new StringBuilder($@"SELECT COUNT(1) FROM dbo.MedicalAntibioticResult AS t 
WHERE t.[Mark] > 0 AND t.[IsValid] = 1 ");

            if (hospitalId != null && hospitalId.Value > 0)
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[HospitalId] = {hospitalId.Value} AND t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }
            else
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString());
            }
        }

        public IPagedList<DataQuality_Score> QueryPage(string title, string intro, DataQuality_Score_Status? status, int page, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0);

            if (status != null)
            {
                query = query.Where(r => r.Status == status.Value);
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(r => r.Title.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(intro))
            {
                query = query.Where(r => r.Intro.Contains(intro));
            }
            query = query.OrderByDescending(m => m.InsertTime)
                .ThenByDescending(r => r.UpdateTime)
                .ThenBy(r => r.Id);

            var list = new PagedList<DataQuality_Score>(query.ToList(), page, pageSize);

            return list;
        }

        /// <summary>
        /// 移动端分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<DataQuality_Score> QueryPageWithWap(long juryId, int page, int pageSize = int.MaxValue)
        {
            using (var db = new Data.ManageSystemContext())
            {
                var query = db.Database.SqlQuery<DataQuality_Score>($@"SELECT DISTINCT t1.* FROM dbo.DataQuality_Score AS t1
LEFT JOIN dbo.DataQuality_Hospital AS t2 ON t2.DataQuality_Score_Id = t1.Id
LEFT JOIN dbo.DataQuality_Jury AS t3 ON t3.DataQuality_Score_Id = t2.DataQuality_Score_Id AND t3.DataQuality_Hospital_Id = t2.Id
WHERE t1.Mark > 0 AND t1.[Status] IN (0,2,5) AND t2.Mark >0 AND t3.Mark > 0 AND t3.Jury_Id = {juryId};")
                    .AsQueryable()
                    .OrderByDescending(r => r.InsertTime)
                    .ThenBy(r => r.BeginTime)
                    .ThenBy(r => r.EndTime)
                    .ThenBy(r => r.Id); ;
                return new PagedList<DataQuality_Score>(query.ToList(), page, pageSize);
            }
            //var query = base._repository.Table.Where(t => t.Mark > 0 && (t.Status == Core.Domain.ScoringModule.Enum.DataQuality_Score_Status.Finish || t.Status == Core.Domain.ScoringModule.Enum.DataQuality_Score_Status.Underway || t.Status == Core.Domain.ScoringModule.Enum.DataQuality_Score_Status.Cancel))
            //    .OrderByDescending(r => r.InsertTime)
            //    .ThenBy(r => r.BeginTime)
            //    .ThenBy(r => r.EndTime)
            //    .ThenBy(r => r.Id);

            //return new PagedList<DataQuality_Score>(query.ToList(), page, pageSize);
        }

        public List<MedicalAntibioticResult> QueryMedicalAntibioticResult(long medicalDataId)
        {
            String sql = $"SELECT * FROM dbo.MedicalAntibioticResult WHERE MedicalDataId = {medicalDataId} AND [Mark] > 0 AND [IsValid] = 1";
            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.Query<MedicalAntibioticResult>(sql).ToList();
            }
        }
        /// <summary>
        /// 评分页面重点监测耐药菌关键数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public DataQuality_Score_MedicalAntibioticResult_Statistic GetCount(int year, long hospitalId)
        {
            #region T-SQL
            string sql = @"SELECT 
SUM(CASE [RowIndex] WHEN 1 THEN [Count] ELSE 0 END) AS [DataTotal],
SUM(CASE [RowIndex] WHEN 2 THEN [Count] ELSE 0 END) AS [DataCount],
SUM(CASE [RowIndex] WHEN 3 THEN [Count] ELSE 0 END) AS [MRSATotal],
SUM(CASE [RowIndex] WHEN 4 THEN [Count] ELSE 0 END) AS [MRSACount],
SUM(CASE [RowIndex] WHEN 5 THEN [Count] ELSE 0 END) AS [VREfmTotal],
SUM(CASE [RowIndex] WHEN 6 THEN [Count] ELSE 0 END) AS [VREfmCount],
SUM(CASE [RowIndex] WHEN 7 THEN [Count] ELSE 0 END) AS [CRKPTotal],
SUM(CASE [RowIndex] WHEN 8 THEN [Count] ELSE 0 END) AS [CRKPCount],
SUM(CASE [RowIndex] WHEN 9 THEN [Count] ELSE 0 END) AS [CRPATotal],
SUM(CASE [RowIndex] WHEN 10 THEN [Count] ELSE 0 END) AS [CRPACount],
SUM(CASE [RowIndex] WHEN 11 THEN [Count] ELSE 0 END) AS [CRABTotal],
SUM(CASE [RowIndex] WHEN 12 THEN [Count] ELSE 0 END) [CRABCount],
SUM(CASE [RowIndex] WHEN 13 THEN [Count] ELSE 0 END) [CTX_CRO_R_EcoTotal],
SUM(CASE [RowIndex] WHEN 14 THEN [Count] ELSE 0 END) [CTX_CRO_R_EcoCount]
FROM(
SELECT 1 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND EXISTS(SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId])
UNION ALL
SELECT 2 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId] AND [t2].[HospitalId] = @HospitalID)
UNION ALL
SELECT 3 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId])
AND EXISTS (SELECT [t3].[Id] FROM [dbo].[MedicalOrganism] AS [t3] WHERE [t3].[GroupName] = '金黄色葡萄球菌' AND [t3].[Mark] > 0 AND [t].[OrganismId] = [t3].[Id]) 
UNION ALL
SELECT 4 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId] AND [t2].[HospitalId] = @HospitalID)
AND EXISTS (SELECT [t3].[Id] FROM [dbo].[MedicalOrganism] AS [t3] WHERE [t3].[GroupName] = '金黄色葡萄球菌' AND [t3].[Mark] > 0 AND [t].[OrganismId] = [t3].[Id]) 
UNION ALL
SELECT 5 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismCode] = 'efm' 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId])
UNION ALL
SELECT 6 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismCode] = 'efm' 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId] AND [t2].[HospitalId] = @HospitalID)
UNION ALL
SELECT 7 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 535 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId])
UNION ALL
SELECT 8 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 535 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId] AND [t2].[HospitalId] = @HospitalID)
UNION ALL
SELECT 9 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 732 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId])
UNION ALL
SELECT 10 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 732 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId] AND [t2].[HospitalId] = @HospitalID)
UNION ALL
SELECT 11 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 1007 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId])
UNION ALL
SELECT 12 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 1007 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId] AND [t2].[HospitalId] = @HospitalID)
UNION ALL
SELECT 13 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 1093 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId])
UNION ALL
SELECT 14 AS [RowIndex], COUNT(1) AS [Count] FROM dbo.MedicalAntibioticResult AS [t] 
WHERE [t].[Mark] > 0 AND [t].[IsValid] = 1 AND [t].[OrganismId] = 1093 
AND EXISTS (SELECT [t2].[Id] FROM [dbo].[MedicalData] AS [t2] WHERE [t2].[Mark] > 0 AND YEAR([t2].[InsertTime]) = @Year AND [t2].[Id] = [t].[MedicalDataId] AND [t2].[HospitalId] = @HospitalID)
) AS tb;";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Year", year);
            parameters.Add("@HospitalID", hospitalId);
            #endregion

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<DataQuality_Score_MedicalAntibioticResult_Statistic>(sql, parameters, commandTimeout: 1800);
            }
        }
        public int GetMRSACount(int year, long? hospitalId = null)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(1) FROM dbo.MedicalAntibioticResult AS t 
WHERE t.[Mark] > 0 AND t.[IsValid] = 1 
AND t.[OrganismId] IN (SELECT [t1].[Id] FROM dbo.MedicalOrganism AS [t1] WHERE [t1].[GroupName] = '金黄色葡萄球菌' AND t1.[Mark] > 0) 
AND (t.[OXA_NM] = 3 OR t.[FOX_ND30] = 3 OR t.[FOX_NM] = 3) ");

            if (hospitalId != null && hospitalId.Value > 0)
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[HospitalId] = {hospitalId.Value} AND t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }
            else
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString());
            }
        }

        public int GetVREfmCount(int year, long? hospitalId = null)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(1) FROM dbo.MedicalAntibioticResult AS t 
WHERE t.[Mark] > 0 AND t.[IsValid] = 1 
AND t.[OrganismName] LIKE '屎肠球菌' ");

            if (hospitalId != null && hospitalId.Value > 0)
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[HospitalId] = {hospitalId.Value} AND t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }
            else
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString());
            }
        }

        public int GetCRKPCount(int year, long? hospitalId = null)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(1) FROM dbo.MedicalAntibioticResult AS t 
WHERE t.[Mark] > 0 AND t.[IsValid] = 1 AND t.[OrganismId] = 535 ");

            if (hospitalId != null && hospitalId.Value > 0)
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[HospitalId] = {hospitalId.Value} AND t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }
            else
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString());
            }
        }

        public int GetCRPACount(int year, long? hospitalId = null)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(1) FROM dbo.MedicalAntibioticResult AS t 
WHERE t.[Mark] > 0 AND t.[IsValid] = 1 AND t.[OrganismId] = 732 ");

            if (hospitalId != null && hospitalId.Value > 0)
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[HospitalId] = {hospitalId.Value} AND t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }
            else
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString());
            }
        }

        public int GetCRABCount(int year, long? hospitalId = null)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(1) FROM dbo.MedicalAntibioticResult AS t 
WHERE t.[Mark] > 0 AND t.[IsValid] = 1 AND t.[OrganismId] = 1007 ");

            if (hospitalId != null && hospitalId.Value > 0)
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[HospitalId] = {hospitalId.Value} AND t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }
            else
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString());
            }
        }

        public int GetCTX_CRO_R_EcoCount(int year, long? hospitalId = null)
        {
            StringBuilder sql = new StringBuilder(@"SELECT COUNT(1) FROM dbo.MedicalAntibioticResult AS t 
WHERE t.[Mark] > 0 AND t.[IsValid] = 1 AND t.[OrganismId] = 1093 ");

            if (hospitalId != null && hospitalId.Value > 0)
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[HospitalId] = {hospitalId.Value} AND t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }
            else
            {
                sql.Append($" AND t.[MedicalDataId] IN (SELECT t2.Id FROM dbo.MedicalData AS t2 WHERE t2.[Mark] > 0 AND YEAR(t2.InsertTime) = {year}) ");
            }

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                return dbConnection.ExecuteScalar<int>(sql.ToString());
            }
        }

        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="hid"></param>
        /// <returns></returns>
        public IEnumerable<DataQuality_Hospital_PageInit> GetHospitalPageInit(long sid, long hid)
        {
            return _hospitalPageInitService.Query(r => r.DataQuality_Score_Id == sid && r.DataQuality_Hospital_Id == hid && r.Mark > 0);
        }

        #region 评分自动处理模块
        public void HospitalAutoPageInit(long id)
        {
            bool isDeleteLock = true; //清除分布式锁
            string cacheKey = "ManageSystem.Services.ScoringModule.DataQualityScoreService.HospitalAutoPageInit." + id;
            try
            {
                // 验证分布式锁 ，系统5分钟内还未处理完成则自动释放
                if (_cacheManager.ExistLock(cacheKey))
                {
                    isDeleteLock = false;
                    throw new Exception("接口请求过于频繁，请等待处理完成");
                }

                if (!_cacheManager.AddLock(cacheKey, 60))
                {
                    isDeleteLock = false;
                    throw new Exception("接口请求过于频繁，请等待处理完成");
                }

                DateTime now = DateTime.Now;
                IEnumerable<DataQuality_Hospital> untreated = HospitalAuto_Underway(now);
                if (!untreated.Any())
                {
                    return;
                }

                List<DataQuality_Hospital_PageInit> results = new List<DataQuality_Hospital_PageInit>();

                foreach (DataQuality_Hospital item in untreated)
                {
                    long medicalDataId = GetLastValidMedicalDataId(item.Hospital_Id);
                    List<MedicalAntibioticResult> medicalAntibioticResults = QueryMedicalAntibioticResult(medicalDataId);

                    #region 药敏品种合理性

                    #region 1. 大肠埃希菌/肺炎克雷伯菌
                    // 大肠埃希菌 ID = 1093
                    // 肺炎克雷伯菌 ID = 535
                    var _1093_535_medicalAntibioticResults = medicalAntibioticResults.Where(r => new List<long> { 535, 1093 }.Contains(r.OrganismId) && r.Mark > 0 && r.IsValid);
                    int _1_count = _1093_535_medicalAntibioticResults.Count();
                    int _1_score = 0;
                    /**
                     * 氨苄西林、哌拉西林/他唑巴坦、头孢唑林、头孢呋辛、头孢噻肟（或头孢曲松）、头孢他啶、头孢吡肟、阿米卡星
                     * 菌株数量 ≥85% 不扣分
                     * 菌株数量 ≤84% -1分/药物
                     **/
                    // 氨苄西林 AMP_NM,AMP_ND10
                    double _1_amp_count = _1093_535_medicalAntibioticResults.Where(r => r.AMP_NM == 3 || r.AMP_ND10 == 3).Count();
                    if ((_1_amp_count / _1_count) < 85.0) _1_score += 1;
                    // 哌拉西林/他唑巴坦 TZP_NM,TZP_ND100
                    double _1_tzp_count = _1093_535_medicalAntibioticResults.Where(r => r.TZP_NM == 3 || r.TZP_ND100 == 3).Count();
                    if ((_1_tzp_count / _1_count) < 85.0) _1_score += 1;
                    // 头孢唑林 CZO_NM,CZO_ND30
                    double _1_czo_count = _1093_535_medicalAntibioticResults.Where(r => r.CZO_NM == 3 || r.CZO_ND30 == 3).Count();
                    if ((_1_czo_count / _1_count) < 85.0) _1_score += 1;
                    // 头孢呋辛 CXM_NM,CXM_ND30
                    double _1_cxm_count = _1093_535_medicalAntibioticResults.Where(r => r.CXM_NM == 3 || r.CXM_ND30 == 3).Count();
                    if ((_1_cxm_count / _1_count) < 85.0) _1_score += 1;
                    // 头孢噻肟（或头孢曲松） CTX_NM,CTX_NE,CTX_ND30
                    double _1_ctx_count = _1093_535_medicalAntibioticResults.Where(r => r.CTX_NM == 3 || r.CTX_NE == 3 || r.CTX_ND30 == 3 || r.CRO_NM == 3 || r.CRO_ND30 == 3).Count();
                    if ((_1_ctx_count / _1_count) < 85.0) _1_score += 1;
                    // 头孢他啶 CAZ_NM,CAZ_ND30
                    double _1_caz_count = _1093_535_medicalAntibioticResults.Where(r => r.CAZ_NM == 3 || r.CAZ_ND30 == 3).Count();
                    if ((_1_caz_count / _1_count) < 85.0) _1_score += 1;
                    // 头孢吡肟 FEP_NM,FEP_ND30
                    double _1_fep_count = _1093_535_medicalAntibioticResults.Where(r => r.FEP_NM == 3 || r.FEP_ND30 == 3).Count();
                    if ((_1_fep_count / _1_count) < 85.0) _1_score += 1;
                    // 阿米卡星 AMK_NM,AMK_ND30
                    double _1_amk_count = _1093_535_medicalAntibioticResults.Where(r => r.AMK_NM == 3 || r.AMK_ND30 == 3).Count();
                    if ((_1_amk_count / _1_count) < 85) _1_score += 1;

                    /**
                     * 多黏菌素、替加环素
                     * 菌株数量 ≥10% 不扣分
                     * 菌株数量 ≤9% -1分/药物
                     **/
                    // 多黏菌素 POL_NM,POL_ND300
                    double _1_pol_count = _1093_535_medicalAntibioticResults.Where(r => r.POL_NM == 3 || r.POL_ND300 == 3).Count();
                    if ((_1_pol_count / _1_count) < 10) _1_score += 1;
                    // 替加环素 TGC_NM, TGC_ND15
                    double _1_tgc_count = _1093_535_medicalAntibioticResults.Where(r => r.TGC_NM == 3 || r.TGC_ND15 == 3).Count();
                    if ((_1_tgc_count / _1_count) < 10) _1_score += 1;

                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "药敏品种合理性(30分)",
                        GroupSort = 3,
                        Item_Text = "大肠埃希菌/肺炎克雷伯菌",
                        Item_Value = 0 - _1_score,
                        Item_Sort = 1,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 2. 铜绿假单胞菌
                    // 铜绿假单胞菌 ID = 732
                    var _732_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 732 && r.Mark > 0 && r.IsValid);
                    int _2_count = _732_medicalAntibioticResults.Count();
                    int _2_score = 0;
                    /**
                     * 哌拉西林/他唑巴坦、头孢他啶、头孢吡肟、阿米卡星、环丙沙星（或左氧氟沙星）
                     * 菌株数量 ≥85% 不扣分，
                     * 菌株数量 ≤84% -1分/药物
                     */
                    // 哌拉西林/他唑巴坦 TZP_NM,TZP_ND100
                    double _2_tzp_count = _732_medicalAntibioticResults.Where(r => r.TZP_NM == 3 || r.TZP_ND100 == 3).Count();
                    if ((_2_tzp_count / _2_count) < 85.0) _2_score += 1;
                    // 头孢他啶 CAZ_NM,CAZ_ND30
                    double _2_caz_count = _732_medicalAntibioticResults.Where(r => r.CAZ_NM == 3 || r.CAZ_ND30 == 3).Count();
                    if ((_2_caz_count / _2_count) < 85.0) _2_score += 1;
                    // 头孢吡肟 FEP_NM,FEP_ND30
                    double _2_fep_count = _732_medicalAntibioticResults.Where(r => r.FEP_NM == 3 || r.FEP_ND30 == 3).Count();
                    if ((_2_fep_count / _2_count) < 85.0) _2_score += 1;
                    // 阿米卡星 AMK_NM,AMK_ND30
                    double _2_amk_count = _732_medicalAntibioticResults.Where(r => r.AMK_NM == 3 || r.AMK_ND30 == 3).Count();
                    if ((_2_amk_count / _2_count) < 85.0) _2_score += 1;
                    // 环丙沙星（或左氧氟沙星） CIP_NM, CIP_ND5
                    double _2_cip_count = _732_medicalAntibioticResults.Where(r => r.CIP_NM == 3 || r.CIP_ND5 == 3 || r.LVX_NM == 3 || r.LVX_ND5 == 3).Count();
                    if ((_2_cip_count / _2_count) < 85.0) _2_score += 1;

                    /**
                     * 多黏菌素
                     * 菌株数量 ≥10% 不扣分
                     * 菌株数量 ≤9% -1分/药物
                     */
                    double _2_pol_count = _732_medicalAntibioticResults.Where(r => r.POL_NM == 3 || r.POL_ND300 == 3).Count();
                    if ((_2_pol_count / _2_count) < 10.0) _2_score += 1;

                    // 单项扣分
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "药敏品种合理性(30分)",
                        GroupSort = 3,
                        Item_Text = "铜绿假单胞菌",
                        Item_Value = 0 - _2_score,
                        Item_Sort = 2,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 3. 鲍曼不动杆菌
                    // 鲍曼不动杆菌 ID = 1007
                    var _1007_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 1007 && r.Mark > 0 && r.IsValid);
                    int _3_count = _1007_medicalAntibioticResults.Count();
                    int _3_score = 0;
                    /**
                     * 哌拉西林/他唑巴坦、头孢哌酮/舒巴坦、头孢他啶、头孢吡肟、阿米卡星、环丙沙星（或左氧氟沙星）
                     * 菌株数量 ≥85% 不扣分，
                     * 菌株数量 ≤84% -1分/药物
                     */
                    // 哌拉西林/他唑巴坦 TZP_NM,TZP_ND100
                    double _3_tzp_count = _1007_medicalAntibioticResults.Where(r => r.TZP_NM == 3 || r.TZP_ND100 == 3).Count();
                    if ((_3_tzp_count / _3_count) < 85.0) _3_score += 1;
                    // 头孢哌酮/舒巴坦 CSL_NM,CSL_ND30,CSL_ND75
                    double _3_csl_count = _1007_medicalAntibioticResults.Where(r => r.CSL_NM == 3 || r.CSL_ND30 == 3 || r.CSL_ND75 == 3).Count();
                    if ((_3_csl_count / _3_count) < 85.0) _3_score += 1;
                    // 头孢他啶 CAZ_NM,CAZ_ND30
                    double _3_caz_count = _1007_medicalAntibioticResults.Where(r => r.CAZ_NM == 3 || r.CAZ_ND30 == 3).Count();
                    if ((_3_caz_count / _3_count) < 85.0) _3_score += 1;
                    // 头孢吡肟 FEP_NM,FEP_ND30
                    double _3_fep_count = _1007_medicalAntibioticResults.Where(r => r.FEP_NM == 3 || r.FEP_ND30 == 3).Count();
                    if ((_3_fep_count / _3_count) < 85.0) _3_score += 1;
                    // 阿米卡星 AMK_NM,AMK_ND30
                    double _3_amk_count = _1007_medicalAntibioticResults.Where(r => r.AMK_NM == 3 || r.AMK_ND30 == 3).Count();
                    if ((_3_amk_count / _3_count) < 85.0) _3_score += 1;
                    // 环丙沙星（或左氧氟沙星） CIP_NM, CIP_ND5
                    double _3_cip_count = _1007_medicalAntibioticResults.Where(r => r.CIP_NM == 3 || r.CIP_ND5 == 3 || r.LVX_NM == 3 || r.LVX_ND5 == 3).Count();
                    if ((_3_cip_count / _3_count) < 85.0) _3_score += 1;

                    /**
                     * 多黏菌素、替加环素
                     * 菌株数量 ≥10% 不扣分
                     * 菌株数量 ≤9% -1分/药物
                     */
                    // 多黏菌素 POL_NM,POL_ND300
                    double _3_pol_count = _1007_medicalAntibioticResults.Where(r => r.POL_NM == 3 || r.POL_ND300 == 3).Count();
                    if ((_3_pol_count / _3_count) < 10.0) _3_score += 1;
                    // 替加环素 TGC_NM, TGC_ND15
                    double _3_tgc_count = _1007_medicalAntibioticResults.Where(r => r.TGC_NM == 3 || r.TGC_ND15 == 3).Count();
                    if ((_3_tgc_count / _3_count) < 10.0) _3_score += 1;

                    // 单项扣分
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "药敏品种合理性(30分)",
                        GroupSort = 3,
                        Item_Text = "鲍曼不动杆菌",
                        Item_Value = 0 - _3_score,
                        Item_Sort = 3,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 4. 金黄色葡萄球菌
                    // 金黄色葡萄球菌 ID = 1218
                    var _1218_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 1218 && r.Mark > 0 && r.IsValid);
                    int _4_count = _1218_medicalAntibioticResults.Count();
                    int _4_score = 0;
                    /**
                     * 青霉素、头孢西丁（或苯唑西林）、红霉素、克林霉素、万古霉素、环丙沙星（或左氧氟沙星）
                     * 菌株数量 ≥85% 不扣分，
                     * 菌株数量 ≤84% -1分/药物
                     */
                    // 青霉素 PEN_NM,PEN_NE,PEN_ND10
                    double _4_pen_count = _1218_medicalAntibioticResults.Where(r => r.PEN_NM == 3 || r.PEN_NE == 3 || r.PEN_ND10 == 3).Count();
                    if ((_4_pen_count / _4_count) < 85.0) _4_score += 1;

                    // 头孢西丁（或苯唑西林） FOX_NM,FOX_ND30 苯唑西林 OXA_NM,OXA_ND1
                    double _4_fox_count = _1218_medicalAntibioticResults.Where(r => r.FOX_NM == 3 || r.FOX_ND30 == 3 || r.OXA_NM == 3 || r.OXA_ND1 == 3).Count();
                    if ((_4_fox_count / _4_count) < 85.0) _4_score += 1;

                    // 红霉素 ERY_NM,ERY_ND15
                    double _4_ery_count = _1218_medicalAntibioticResults.Where(r => r.ERY_NM == 3 || r.ERY_ND15 == 3).Count();
                    if ((_4_ery_count / _4_count) < 85.0) _4_score += 1;

                    // 克林霉素 CLI_NM, CLI_ND2
                    double _4_cli_count = _1218_medicalAntibioticResults.Where(r => r.CLI_NM == 3 || r.CLI_ND2 == 3).Count();
                    if ((_4_cli_count / _4_count) < 85.0) _4_score += 1;

                    // 万古霉素 VAN_NM,VAN_NE,VAN_ND30
                    double _4_van_count = _1218_medicalAntibioticResults.Where(r => r.VAN_NM == 3 || r.VAN_NE == 3 || r.VAN_ND30 == 3).Count();
                    if ((_4_van_count / _4_count) < 85.0) _4_score += 1;

                    // 环丙沙星（或左氧氟沙星） CIP_NM, CIP_ND5 左氧氟沙星 LVX_NM,LVX_ND5
                    double _4_cip_count = _1218_medicalAntibioticResults.Where(r => r.CIP_NM == 3 || r.CIP_ND5 == 3 || r.LVX_NM == 3 || r.LVX_ND5 == 3).Count();
                    if ((_4_cip_count / _4_count) < 85.0) _4_score += 1;

                    // 单项扣分
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "药敏品种合理性(30分)",
                        GroupSort = 3,
                        Item_Text = "金黄色葡萄球菌",
                        Item_Value = 0 - _4_score,
                        Item_Sort = 4,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 5. 肺炎链球菌
                    // 肺炎链球菌 ID = 493
                    var _493_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 493 && r.Mark > 0 && r.IsValid);
                    int _5_count = _493_medicalAntibioticResults.Count();
                    int _5_score = 0;
                    /**
                     * 头孢曲松（或头孢噻肟）、左旋氧氟沙星（或莫西沙星）、万古霉素、利奈唑胺、青霉素MIC
                     * 菌株数量 ≥85% 不扣分，
                     * 菌株数量 ≤84% -1分/药物
                     */
                    // 头孢噻肟（或头孢曲松） CTX_NM,CTX_NE,CTX_ND30
                    double _5_ctx_count = _493_medicalAntibioticResults.Where(r => r.CTX_NM == 3 || r.CTX_NE == 3 || r.CTX_ND30 == 3 || r.CRO_NM == 3 || r.CRO_ND30 == 3).Count();
                    if ((_5_ctx_count / _5_count) < 85.0) _5_score += 1;

                    // 左旋氧氟沙星 LVX_NM,LVX_ND5 莫西沙星 MFX_NM,MFX_ND,MFX_ND5
                    double _5_lvx_count = _493_medicalAntibioticResults.Where(r => r.LVX_NM == 3 || r.LVX_ND5 == 3 || r.MFX_NM == 3 || r.MFX_ND == 3 || r.MFX_ND5 == 3).Count();
                    if ((_5_lvx_count / _5_count) < 85.0) _5_score += 1;

                    // 万古霉素 VAN_NM,VAN_NE,VAN_ND30
                    double _5_van_count = _493_medicalAntibioticResults.Where(r => r.VAN_NM == 3 || r.VAN_NE == 3 || r.VAN_ND30 == 3).Count();
                    if ((_5_van_count / _5_count) < 85.0) _5_score += 1;

                    // 利奈唑胺 LNZ_NM,LNZ_ND30
                    double _5_lnz_count = _493_medicalAntibioticResults.Where(r => r.LNZ_NM == 3 || r.LNZ_ND30 == 3).Count();
                    if ((_5_lnz_count / _5_count) < 85.0) _5_score += 1;

                    // 青霉素 PEN_NM,PEN_NE
                    double _5_pen_count = _493_medicalAntibioticResults.Where(r => r.PEN_NM == 3 || r.PEN_NE == 3).Count();
                    if ((_5_pen_count / _5_count) < 85.0) _5_score += 1;

                    // 单项扣分
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "药敏品种合理性(30分)",
                        GroupSort = 3,
                        Item_Text = "肺炎链球菌",
                        Item_Value = 0 - _5_score,
                        Item_Sort = 5,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 6. 粪肠球菌
                    // 粪肠球菌 ID = 908
                    var _908_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 908 && r.Mark > 0 && r.IsValid);
                    int _6_count = _908_medicalAntibioticResults.Count();
                    int _6_score = 0;
                    /**
                     * 氨苄西林、高浓度庆大霉素/链霉素、万古霉素
                     * 菌株数量 ≥85% 不扣分，
                     * 菌株数量 ≤84% -1分/药物
                     */
                    // 氨苄西林 AMP_NM,AMP_ND10
                    double _6_amp_count = _908_medicalAntibioticResults.Where(r => r.AMP_NM == 3 || r.AMP_ND10 == 3).Count();
                    if ((_6_amp_count / _6_count) < 85.0) _6_score += 1;

                    // 高浓度庆大霉素 GEH_NM,GEH_ND120
                    double _6_geh_count = _908_medicalAntibioticResults.Where(r => r.GEH_NM == 3 || r.GEH_ND120 == 3).Count();
                    if ((_6_geh_count / _6_count) < 85.0) _6_score += 1;

                    // 链霉素 STR_NM,STR_ND10
                    double _6_str_count = _908_medicalAntibioticResults.Where(r => r.STR_NM == 3 || r.STR_ND10 == 3).Count();
                    if ((_6_str_count / _6_count) < 85.0) _6_score += 1;

                    // 万古霉素 VAN_NM,VAN_NE,VAN_ND30
                    double _6_van_count = _908_medicalAntibioticResults.Where(r => r.VAN_NM == 3 || r.VAN_NE == 3 || r.VAN_ND30 == 3).Count();
                    if ((_6_van_count / _6_count) < 85.0) _6_score += 1;

                    // 单项扣分
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "药敏品种合理性(30分)",
                        GroupSort = 3,
                        Item_Text = "粪肠球菌",
                        Item_Value = 0 - _6_score,
                        Item_Sort = 6,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 7. 流感嗜血杆菌和卡他莫拉菌
                    // 流感嗜血杆菌 ID = 678
                    // 卡他莫拉菌 ID = 560
                    var _560_678_medicalAntibioticResults = medicalAntibioticResults.Where(r => (r.OrganismId == 678 || r.OrganismId == 560) && r.Mark > 0 && r.IsValid);
                    int _7_count = _560_678_medicalAntibioticResults.Count();
                    int _7_score = 0;
                    /**
                     * β-内酰胺酶
                     * 菌株数量 ≥85% 不扣分，
                     * 菌株数量 ≤84% -1分/药物
                     */
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "药敏品种合理性(30分)",
                        GroupSort = 3,
                        Item_Text = "流感嗜血杆菌和卡他莫拉菌",
                        Item_Value = 0 - _7_score,
                        Item_Sort = 7,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #endregion

                    #region 重点监测耐药菌
                    var mainMonitor = GetCount(now.Year, item.Hospital_Id);

                    #region 甲氧西林耐药金黄色葡萄球菌，MRSA
                    // 甲氧西林耐药金黄色葡萄球菌，MRSA
                    double t_mrsa_rate = Math.Round((double)mainMonitor.MRSATotal / mainMonitor.DataTotal, 2) * 100;
                    double h_mrsa_rate = Math.Round((double)mainMonitor.MRSACount / mainMonitor.DataCount, 2) * 100;
                    double mrsa_score = MainScoreCalculation(t_mrsa_rate, h_mrsa_rate);
                    double f_mrsa_score = mrsa_score * 0.09;

                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "重点监测耐药菌(50分)",
                        GroupSort = 4,
                        Item_Text = "甲氧西林耐药金葡菌",
                        Item_Value = f_mrsa_score,
                        Item_Sort = 1,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 万古霉素耐药屎肠球菌，VREfm
                    // 万古霉素耐药屎肠球菌，VREfm
                    double t_vrefm_rate = Math.Round((double)mainMonitor.VREfmTotal / mainMonitor.DataTotal, 2) * 100;
                    double h_vrefm_rate = Math.Round((double)mainMonitor.VREfmCount / mainMonitor.DataCount, 2) * 100;
                    double vrefm_score = MainScoreCalculation(t_vrefm_rate, h_vrefm_rate);
                    double f_vrefm_score = vrefm_score * 0.04;
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "重点监测耐药菌(50分)",
                        GroupSort = 4,
                        Item_Text = "万古霉素耐药屎肠球菌",
                        Item_Value = f_vrefm_score,
                        Item_Sort = 2,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 碳青霉烯类耐药肺炎克雷伯菌， CRKP
                    // 碳青霉烯类耐药肺炎克雷伯菌， CRKP
                    double t_crkp_count = Math.Round((double)mainMonitor.CRKPTotal / mainMonitor.DataTotal, 2) * 100;
                    double h_crkp_count = Math.Round((double)mainMonitor.CRKPCount / mainMonitor.DataCount, 2) * 100;
                    double crkp_score = MainScoreCalculation(t_crkp_count, h_crkp_count);
                    double f_crkp_score = crkp_score * 0.12;
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "重点监测耐药菌(50分)",
                        GroupSort = 4,
                        Item_Text = "碳青霉烯类耐药肺炎克雷伯菌",
                        Item_Value = f_crkp_score,
                        Item_Sort = 3,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 碳青霉烯类耐药铜绿假单胞菌，CRPA
                    // 碳青霉烯类耐药铜绿假单胞菌，CRPA
                    double t_crpa_count = Math.Round((double)mainMonitor.CRPATotal / mainMonitor.DataTotal, 2) * 100;
                    double h_crpa_count = Math.Round((double)mainMonitor.CRPACount / mainMonitor.DataCount, 2) * 100;
                    double crpa_score = MainScoreCalculation(t_crpa_count, h_crpa_count);
                    double f_crpa_score = crpa_score * 0.08;
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "重点监测耐药菌(50分)",
                        GroupSort = 4,
                        Item_Text = "碳青霉烯类耐药铜绿假单胞菌",
                        Item_Value = f_crpa_score,
                        Item_Sort = 4,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 碳青霉烯类耐药鲍曼不动杆菌，CRAB
                    // 碳青霉烯类耐药鲍曼不动杆菌，CRAB
                    double t_crab_count = Math.Round((double)mainMonitor.CRABTotal / mainMonitor.DataTotal, 2) * 100;
                    double h_crab_count = Math.Round((double)mainMonitor.CRABCount / mainMonitor.DataCount, 2) * 100;
                    double crab_score = MainScoreCalculation(t_crab_count, h_crab_count);
                    double f_crab_score = crab_score * 0.09;
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "重点监测耐药菌(50分)",
                        GroupSort = 4,
                        Item_Text = "碳青霉烯类耐药鲍曼不动杆菌",
                        Item_Value = f_crab_score,
                        Item_Sort = 5,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #region 头孢噻肟/头孢曲松耐药大肠埃希菌，CTX/CRO-R-eco
                    // 头孢噻肟/头孢曲松耐药大肠埃希菌，CTX/CRO-R-eco
                    double t_ctx_count = Math.Round((double)mainMonitor.CRABTotal / mainMonitor.DataTotal, 2) * 100;
                    double h_ctx_count = Math.Round((double)mainMonitor.CRABCount / mainMonitor.DataCount, 2) * 100;
                    double ctx_score = MainScoreCalculation(t_ctx_count, h_ctx_count);
                    double f_ctx_score = ctx_score * 0.08;
                    results.Add(new DataQuality_Hospital_PageInit
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataQuality_Score_Id = item.DataQuality_Score_Id,
                        DataQuality_Hospital_Id = item.Id,
                        GroupName = "重点监测耐药菌(50分)",
                        GroupSort = 4,
                        Item_Text = "头孢噻肟/头孢曲松耐药大肠埃希菌",
                        Item_Value = f_ctx_score,
                        Item_Sort = 6,
                        SyncTime = now,
                        LastTime = null,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    });
                    #endregion

                    #endregion
                }

                HospitalAuto_PageInit_InsertOrUpdate(results);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (isDeleteLock)
                {
                    _cacheManager.DeleteLock(cacheKey);
                }
            }
        }

        /// <summary>
        /// 在进行中的评分医院
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private IEnumerable<DataQuality_Hospital> HospitalAuto_Underway(DateTime dateTime)
        {
            string SQL_Underway = @"SELECT 
	[T2].*
FROM [dbo].[DataQuality_Score] AS [T1]
LEFT JOIN [dbo].[DataQuality_Hospital] AS [T2] ON [T2].[DataQuality_Score_Id] = [T1].[Id] AND [T2].[Mark] > 0 AND [T2].[Status] = 1
WHERE 
	[T1].[Mark] > 0 AND 
	[T1].[Status] = 2 AND 
	DATEDIFF(SECOND, [T1].[BeginTime], @DateTime) >= 0 AND 
	DATEDIFF(SECOND, @DateTime, [T1].[EndTime]) > 0 AND 
	[T2].[Id] IS NOT NULL AND 
	[T2].[Id] > 0 AND 
	[T2].[Status] = 1
ORDER BY [T1].[BeginTime] ASC;";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateTime", dateTime);

            string SQL_Complete = @"SELECT * FROM [dbo].[DataQuality_Hospital_PageInit]  WHERE [Mark] > 0 AND DATEDIFF(DAY, [SyncTime], @DateTime) = 0;";

            using (IDbConnection dbConnection = Data.DapperHelper.GetConnection())
            {
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                IEnumerable<DataQuality_Hospital> data_Underway = dbConnection.Query<DataQuality_Hospital>(SQL_Underway, parameters);
                IEnumerable<DataQuality_Hospital_PageInit> data_Complete = dbConnection.Query<DataQuality_Hospital_PageInit>(SQL_Complete, parameters);

                var completeIds = data_Underway.Join(data_Complete,
                      t1 => new { t1.DataQuality_Score_Id, DataQuality_Hospital_Id = t1.Id },
                      t2 => new { t2.DataQuality_Score_Id, DataQuality_Hospital_Id = t2.DataQuality_Hospital_Id },
                      (t1, t2) => new DataQuality_Hospital { Id = t1.Id })
                    .Select(item => item.Id).Distinct();

                return data_Underway.Where(item => !completeIds.Contains(item.Id)).ToList();
            }
        }

        /// <summary>
        /// 重点监测耐药菌分值计算
        /// </summary>
        /// <param name="totalRate"></param>
        /// <param name="hospitalRate"></param>
        /// <returns></returns>
        private static double MainScoreCalculation(double totalRate, double hospitalRate)
        {
            double score = 0;
            if (totalRate != hospitalRate)
            {
                if (totalRate > hospitalRate)
                {
                    if ((totalRate - hospitalRate) <= 10.0)
                    {
                        score += 10;
                    }
                    else if ((totalRate - hospitalRate) <= 20.0)
                    {
                        score += 20;
                    }
                    else if ((totalRate - hospitalRate) <= 30.0)
                    {
                        score += 30;
                    }
                    else if ((totalRate - hospitalRate) <= 40.0)
                    {
                        score += 40;
                    }
                    else
                    {
                        score += 50;
                    }
                }
                else if (hospitalRate > totalRate)
                {
                    if ((hospitalRate - totalRate) <= 10.0)
                    {
                        score -= 10;
                    }
                    else if ((hospitalRate - totalRate) <= 20.0)
                    {
                        score -= 20;
                    }
                    else if ((hospitalRate - totalRate) <= 30.0)
                    {
                        score -= 30;
                    }
                    else if ((hospitalRate - totalRate) <= 40.0)
                    {
                        score -= 40;
                    }
                    else
                    {
                        score -= 50;
                    }
                }
            }
            else
            {
                score = 50;
            }

            return score;
        }

        private void HospitalAuto_PageInit_InsertOrUpdate(List<DataQuality_Hospital_PageInit> entities)
        {
            IDbContext c = EngineContext.Current.Resolve<IDbContext>();
            using (DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection)
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                using (var tran = con.BeginTransaction())
                {
                    foreach (var item in entities)
                    {
                        _hospitalPageInitService.Update(source => source.DataQuality_Score_Id == item.DataQuality_Score_Id && source.DataQuality_Hospital_Id == item.DataQuality_Hospital_Id && source.GroupName == item.GroupName && source.Item_Text == item.Item_Text, update => new DataQuality_Hospital_PageInit { Mark = 0, DeleteTime = DateTime.Now });
                        _hospitalPageInitService.Insert(item);
                    }
                    tran.Commit();

                }
            }
        }
        #endregion

    }
}
