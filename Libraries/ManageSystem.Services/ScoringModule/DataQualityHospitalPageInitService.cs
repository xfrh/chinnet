using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public class DataQualityHospitalPageInitService : BaseService<DataQuality_Hospital_PageInit>, IDataQualityHospitalPageInitService
    {
        public DataQualityHospitalPageInitService(IRepository<DataQuality_Hospital_PageInit> repository) : base(repository)
        {

        }
    }
}
