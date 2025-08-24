using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.ScoringModule.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityActionLogService : BaseService<DataQuality_Action_Log>, IDataQualityActionLogService
    {
        public DataQualityActionLogService(IRepository<DataQuality_Action_Log> repository) : base(repository)
        {
        }

        public IPagedList<DataQuality_Action_Log> QueryPage(long dataQualityScoreId, string userName, string actionType, string content, DateTime? begin, DateTime? end, int page, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0 && m.DataQuality_Score_Id == dataQualityScoreId);

            if (!string.IsNullOrWhiteSpace(userName))
            {
                IDbConnection dbConnection = Data.DapperHelper.GetConnection();
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                IEnumerable<long> ids_1 = dbConnection.Query<long>(@"SELECT DISTINCT t1.Id FROM dbo.DataQuality_Action_Log AS t1 LEFT JOIN dbo.Userinfo AS t2 ON t2.Id = t1.Operator_Id
WHERE t1.DataQuality_Score_Id = @DataQuality_Score_Id AND t1.[Platform] = @Platform AND t1.[Mark] > 0 AND t2.Mark > 0 AND t2.[Name] LIKE @Name;", new { DataQuality_Score_Id = dataQualityScoreId, Platform = (int)Core.Domain.Log.ActionSource.Admin, Name = $"%{userName}%" });

                IEnumerable<long> ids_2 = dbConnection.Query<long>(@"SELECT DISTINCT t1.Id FROM dbo.DataQuality_Action_Log AS t1 LEFT JOIN dbo.Member AS t2 ON t2.Id = t1.Operator_Id 
WHERE t1.DataQuality_Score_Id = @DataQuality_Score_Id AND t1.[Platform] = @Platform AND t1.[Mark] > 0 AND [t2].[Mark] > 0 AND (t2.[Name] LIKE @Name OR t2.[NickName] LIKE @Name);", new { DataQuality_Score_Id = dataQualityScoreId, Platform = (int)Core.Domain.Log.ActionSource.Mobile, Name = $"%{userName}%" });

                query = query.Where(r => ids_1.Contains(r.Id) || ids_2.Contains(r.Id));
            }

            if (!string.IsNullOrWhiteSpace(actionType) && !actionType.ToLowerInvariant().Equals("null"))
            {
                query = query.Where(r => r.Action_Type == actionType);
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                query = query.Where(r => r.Action_Log.Contains(content));
            }

            if (begin != null)
            {
                query = query.Where(r => System.Data.Entity.SqlServer.SqlFunctions.DateDiff("dd", r.Action_Time, begin.Value) <= 0);
            }

            if (end != null)
            {
                query = query.Where(r => System.Data.Entity.SqlServer.SqlFunctions.DateDiff("dd", end.Value, r.Action_Time) <= 0);
            }

            query = query.OrderByDescending(m => m.Action_Time)
                 .ThenByDescending(r => r.InsertTime)
                 .ThenByDescending(r => r.UpdateTime)
                 .ThenBy(r => r.Id);

            var list = new PagedList<DataQuality_Action_Log>(query.ToList(), page, pageSize);

            return list;
        }
    }
}
