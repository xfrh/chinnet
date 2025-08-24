using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.ScoringModule
{
    public partial class DataQualityScoreRecordMap : ManageSystemEntityTypeConfiguration<DataQuality_Score_Record>
    {
        public DataQualityScoreRecordMap()
        {
            ToTable("DataQuality_Score_Record");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
