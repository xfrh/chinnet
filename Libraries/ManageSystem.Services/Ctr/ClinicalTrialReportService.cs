using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Ctr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Ctr
{
    public partial class ClinicalTrialReportService : BaseService<ClinicalTrialReport>, IClinicalTrialReportService
    {
        public ClinicalTrialReportService(IRepository<ClinicalTrialReport> repository) : base(repository)
        {

        }
    }
}
