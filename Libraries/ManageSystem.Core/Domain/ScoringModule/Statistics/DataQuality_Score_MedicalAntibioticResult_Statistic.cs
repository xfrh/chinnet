using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule.Statistics
{
    public class DataQuality_Score_MedicalAntibioticResult_Statistic
    {
        public int DataTotal { get; set; }
        public int DataCount { get; set; }
        public int MRSATotal { get; set; }
        public int MRSACount { get; set; }
        public int VREfmTotal { get; set; }
        public int VREfmCount { get; set; }
        public int CRKPTotal { get; set; }
        public int CRKPCount { get; set; }
        public int CRPATotal { get; set; }
        public int CRPACount { get; set; }
        public int CRABTotal { get; set; }
        public int CRABCount { get; set; }
        public int CTX_CRO_R_EcoTotal { get; set; }
        public int CTX_CRO_R_EcoCount { get; set; }
    }
}
