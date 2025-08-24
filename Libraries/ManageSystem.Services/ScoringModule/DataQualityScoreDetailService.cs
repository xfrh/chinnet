using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityScoreDetailService : BaseService<DataQuality_Score_Detail>, IDataQualityScoreDetailService
    {
        public DataQualityScoreDetailService(IRepository<DataQuality_Score_Detail> repository) : base(repository)
        {

        }
    }
}
