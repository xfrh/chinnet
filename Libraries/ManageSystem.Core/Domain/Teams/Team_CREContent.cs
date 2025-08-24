using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Teams
{
    /// <summary>
    /// CRE成员单位
    /// </summary>
    public partial class Team_CREContent : BaseEntity
    {
        /// <summary>
        /// CRE成员单位内容
        /// </summary>
        public string Content { get; set; }
    }
}
