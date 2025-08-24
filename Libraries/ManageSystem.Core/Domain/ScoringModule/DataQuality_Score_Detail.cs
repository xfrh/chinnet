using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule
{
    /// <summary>
    /// 评分记录
    /// </summary>
    public partial class DataQuality_Score_Detail : BaseEntity
    {
        public long DataQuality_Score_Id { get; set; }
        public long DataQuality_Hospital_Id { get; set; }
        public long DataQuality_Jury_Id { get; set; }
        public long DataQuality_Score_Record_Id { get; set; }
        public int DataQuality_Score_Group_Index { get; set; }
        public string DataQuality_Score_Group { get; set; }
        public int DataQuality_Score_Sort { get; set; }
        public string DataQuality_Score_Item { get; set; }
        public string DataQuality_Score_Value { get; set; }
        public double DataQuality_Score_Score { get; set; }
    }
}
