using Dapper;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Data;
using ManageSystem.Services.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityJuryService : BaseService<DataQuality_Jury>, IDataQualityJuryService
    {
        private IMemberService _memberService;
        public DataQualityJuryService(IRepository<DataQuality_Jury> repository, IMemberService memberService) : base(repository)
        {
            _memberService = memberService;
        }

        public int GetJuryCountByDataQualityScoreId(long dataQualityScoreId)
        {
            string sql = $@"SELECT COUNT(1) FROM dbo.DataQuality_Jury AS t1 
LEFT JOIN dbo.DataQuality_Hospital AS t2 ON t2.DataQuality_Score_Id = t1.DataQuality_Score_Id AND t2.Id = t1.DataQuality_Hospital_Id
WHERE t1.DataQuality_Score_Id = {dataQualityScoreId} AND t1.Mark > 0 AND t2.Mark > 0";

            using (var connection = DapperHelper.GetConnection())
            {
                connection.Open();
                int count = connection.ExecuteScalar<int>(sql);
                connection.Close();
                return count;
            }
        }

        public IEnumerable<long> GetJuryIdByDataQualityScoreId(long dataQualityScoreId)
        {
            string sql = $@"SELECT DISTINCT [t1].[Jury_Id] FROM dbo.[DataQuality_Jury] AS [t1] 
LEFT JOIN dbo.[DataQuality_Hospital] AS [t2] ON [t2].[DataQuality_Score_Id] = [t1].[DataQuality_Score_Id] AND [t2].[Id] = [t1].[DataQuality_Hospital_Id]
WHERE [t1].[Mark] > 0 AND [t2].[Mark] > 0 AND [t1].[DataQuality_Score_Id] = {dataQualityScoreId};";
            using (var connection = DapperHelper.GetConnection())
            {
                connection.Open();
                IEnumerable<long> result = connection.Query<long>(sql);
                connection.Close();
                return result;
            }
        }
        public List<string> GetJuryNames(long dataQualityScoreId, long dataQualityHospitalId)
        {
            List<long> ids = Query(r => r.DataQuality_Score_Id == dataQualityScoreId && r.DataQuality_Hospital_Id == dataQualityHospitalId).Select(r => r.Jury_Id).ToList();
            if (ids == null || ids.Count() == 0)
            {
                return new List<string>();
            }

            return _memberService.Query(r => ids.Contains(r.Id)).AsEnumerable().Select(r => string.IsNullOrWhiteSpace(r.Name) ? r.Phone : r.Name).ToList();
        }
    }
}
