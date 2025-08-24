using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.ScoringModule
{
    public class DataQuality_Hospital_PageInit : BaseEntity
    {
        public long DataQuality_Score_Id { get; set; }
        public long DataQuality_Hospital_Id { get; set; }
        public string GroupName { get; set; }
        public int GroupSort { get; set; }
        public string Item_Text { get; set; }
        public double Item_Value { get; set; }
        public int Item_Sort { get; set; }
        public DateTime SyncTime { get; set; }
        public DateTime? LastTime { get; set; }
    }
}
