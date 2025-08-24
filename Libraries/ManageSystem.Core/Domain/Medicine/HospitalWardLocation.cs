
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
    /// 
    /// </summary>
    public partial class HospitalWardLocation : BaseEntity
    {
        public string Name { get; set; }
        public string Ward { get; set; }
        public string Department_CN { get; set; }
        public string Department_EN { get; set; }
        public string Location { get; set; }
        public string Location_Type { get; set; }
        public int Sort { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        public long HospitalId { get; set; }

        //public PagedList<HospitalWardLocation> PageList { get; set; }

    }
}
