using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.DataInput
{
    public partial class MemberHospital:BaseEntity
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string LoginId { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string HospitalName { get; set; }
        public long HospitalId { get; set; }
        public DateTime InsertTime { get; set; }

        public string Median { get; set; }

        public int counts { get; set; }

    }
}
