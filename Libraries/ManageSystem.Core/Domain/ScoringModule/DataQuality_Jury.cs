using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule
{
    /// <summary>
    /// 医院评委
    /// </summary>
    public partial class DataQuality_Jury : BaseEntity
    {
        /// <summary>
        /// 评分基本信息ID
        /// </summary>
        public long DataQuality_Score_Id { get; set; }
        /// <summary>
        /// 关联医院ID
        /// </summary>
        public long DataQuality_Hospital_Id { get; set; }
        /// <summary>
        /// 评委ID/现有MemberID
        /// </summary>
        public long Jury_Id { get; set; }
    }
}
