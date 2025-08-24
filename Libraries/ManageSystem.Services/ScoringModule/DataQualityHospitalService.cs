using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityHospitalService : BaseService<DataQuality_Hospital>, IDataQualityHospitalService
    {
        private IDataQualityJuryService _dataQualityJuryService;
        public DataQualityHospitalService(IRepository<DataQuality_Hospital> repository, IDataQualityJuryService dataQualityJuryService) : base(repository)
        {
            _dataQualityJuryService = dataQualityJuryService;
        }

        public bool CheckHospital(long dataQualityScoreId, long hospitalId)
        {
            return base.Count(r => r.DataQuality_Score_Id == dataQualityScoreId && r.Hospital_Id == hospitalId && r.Mark > 0) > 0;
        }

        public IPagedList<DataQuality_Hospital> QueryPage(long dataQualityScoreId, int? status, int page, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0 && m.DataQuality_Score_Id == dataQualityScoreId);

            if (status != null)
            {
                query = query.Where(r => r.Status == status.Value);
            }
            query = query.OrderBy(m => m.Sort)
                .ThenByDescending(r => r.InsertTime)
                .ThenByDescending(r => r.UpdateTime)
                .ThenBy(r => r.Id);

            var list = new PagedList<DataQuality_Hospital>(query.ToList(), page, pageSize);

            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="juryId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<DataQuality_Hospital> QueryPage(long dataQualityScoreId, long juryId, int page, int pageSize = int.MaxValue)
        {
            IDbConnection dbConnection = Data.DapperHelper.GetConnection();
            if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
            IEnumerable<long> ids = dbConnection.Query<long>(@"SELECT DISTINCT t1.[Id] FROM dbo.DataQuality_Hospital AS t1 LEFT JOIN dbo.DataQuality_Jury AS t2 ON t2.DataQuality_Score_Id = t1.DataQuality_Score_Id AND t2.DataQuality_Hospital_Id = t1.Id
WHERE t1.DataQuality_Score_Id = @DataQuality_Score_Id AND t1.[Mark] > 0 AND t1.[Status] = 1 AND t2.[Mark] > 0 AND t2.Jury_Id = @Jury_Id;", new { DataQuality_Score_Id = dataQualityScoreId, Jury_Id = juryId });

            var query = base._repository.Table.Where(m => m.DataQuality_Score_Id == dataQualityScoreId && ids.Contains(m.Id) && m.Mark > 0)
                .OrderBy(m => m.Sort)
                .ThenByDescending(r => r.InsertTime)
                .ThenByDescending(r => r.UpdateTime)
                .ThenBy(r => r.Id);

            var list = new PagedList<DataQuality_Hospital>(query.ToList(), page, pageSize);

            return list;
        }
        public bool DeleteUnSaveDataQualityHospitalByDataQualityScoreId(long dataQualityScoreId)
        {
            List<DataQuality_Hospital> entities = Query(r => r.DataQuality_Score_Id == dataQualityScoreId && r.Status == 0);
            _repository.Delete(entities);
            return true;
        }

        /// <summary>
        /// 获取指定评委在指定评分中分配的医院名称
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="juryId"></param>
        /// <returns></returns>
        public List<string> GetHospitalNameByJuryIdWithDataQualityScore(long dataQualityScoreId, long juryId)
        {
            string sql = $@"SELECT h1.[Name] FROM dbo.Hospital AS h1
LEFT JOIN dbo.DataQuality_Hospital AS h2 ON h2.Hospital_Id = h1.Id AND h2.Mark > 0
LEFT JOIN dbo.DataQuality_Jury AS h3 ON h3.DataQuality_Hospital_Id = h2.Id AND h3.DataQuality_Score_Id = h2.DataQuality_Score_Id AND h3.Jury_Id = {juryId} AND h3.[Mark] > 0
WHERE h3.DataQuality_Score_Id = {dataQualityScoreId} AND h1.[Mark] > 0 AND h2.[Mark] > 0 AND h2.[Status] = 1 AND h3.[Mark] >0
ORDER BY h2.[Sort] ASC, h2.[InsertTime] DESC, h2.[UpdateTime] DESC, h2.[Id] ASC;";
           
            return  DapperHelper.GetConnection().Query<string>(sql, null).ToList();
        }
    }
}
