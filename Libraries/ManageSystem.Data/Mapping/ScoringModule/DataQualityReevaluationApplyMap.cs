using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.ScoringModule
{
    public class DataQualityReevaluationApplyMap : ManageSystemEntityTypeConfiguration<DataQuality_Reevaluation_Apply>
    {
        public DataQualityReevaluationApplyMap()
        {
            ToTable("DataQuality_Reevaluation_Apply");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
