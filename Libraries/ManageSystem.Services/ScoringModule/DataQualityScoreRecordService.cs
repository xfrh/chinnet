using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Services.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityScoreRecordService : BaseService<DataQuality_Score_Record>, IDataQualityScoreRecordService
    {
        private IMemberService _memberService;
        private IDataQualityScoreDetailService _detailService;

        public DataQualityScoreRecordService(IRepository<DataQuality_Score_Record> repository, IMemberService memberService, IDataQualityScoreDetailService detailService) : base(repository)
        {
            _memberService = memberService;
            _detailService = detailService;
        }

        public IPagedList<DataQuality_Score_Record> QueryPage(long dataQualityScoreId, string juryName, string hospital, DateTime? begin, DateTime? end, int page, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0 && m.DataQuality_Score_Id == dataQualityScoreId);

            if (!string.IsNullOrWhiteSpace(juryName))
            {
                IDbConnection dbConnection = Data.DapperHelper.GetConnection();
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                IEnumerable<long> ids = dbConnection.Query<long>(@"SELECT DISTINCT t1.Id FROM dbo.DataQuality_Score_Record AS t1 
LEFT JOIN dbo.DataQuality_Jury AS t2 ON t2.DataQuality_Score_Id = t1.DataQuality_Score_Id AND t2.Id = t1.DataQuality_Jury_Id
LEFT JOIN dbo.Member AS t3 ON t3.Id = t2.Jury_Id
WHERE t1.[Mark] > 0 AND t2.[Mark] > 0 AND t3.Mark > 0 AND (t3.[Name] LIKE @Name OR t3.[NickName] LIKE @Name OR t3.[Phone] LIKE @Name);", new { Name = $"%{juryName}%" });
                query = query.Where(r => ids.Contains(r.Id));
            }

            if (!string.IsNullOrWhiteSpace(hospital))
            {
                IDbConnection dbConnection = Data.DapperHelper.GetConnection();
                if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                IEnumerable<long> ids = dbConnection.Query<long>(@"SELECT DISTINCT t1.Id FROM dbo.DataQuality_Score_Record AS t1 
LEFT JOIN dbo.DataQuality_Hospital AS t2 ON t2.DataQuality_Score_Id = t1.DataQuality_Score_Id AND t2.Id = t1.DataQuality_Hospital_Id
LEFT JOIN dbo.Hospital AS t3 ON t3.Id = t2.Hospital_Id
WHERE t1.[Mark] > 0 AND t2.[Mark] > 0 AND t2.[Status] = 1 AND t3.Mark > 0 AND t3.[Name] LIKE @Name;", new { Name = $"%{hospital}%" });
                query = query.Where(r => ids.Contains(r.Id));
            }

            if (begin != null)
            {
                query = query.Where(r => System.Data.Entity.SqlServer.SqlFunctions.DateDiff("dd", r.Evaluate_Time, begin.Value) <= 0);
            }

            if (end != null)
            {
                query = query.Where(r => System.Data.Entity.SqlServer.SqlFunctions.DateDiff("dd", r.Evaluate_Time, end.Value) >= 0);
            }

            query = query.OrderByDescending(m => m.Evaluate_Time)
                .ThenByDescending(r => r.InsertTime)
                .ThenByDescending(r => r.UpdateTime)
                .ThenBy(r => r.Id);

            var list = new PagedList<DataQuality_Score_Record>(query.ToList(), page, pageSize);

            return list;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public bool Insert(DataQuality_Score_Record model, List<DataQuality_Score_Detail> details)
        {
            try
            {
                Data.IDbContext dbContext = new Data.ManageSystemContext();
                var ds = dbContext.Set<DataQuality_Score_Record>();
                ds.Add(model);
                var ds2 = dbContext.Set<DataQuality_Score_Detail>();
                foreach (var item in details)
                {
                    ds2.Add(item);
                }
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
