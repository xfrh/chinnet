using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Teams
{
    public class HospitalTeamList : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
