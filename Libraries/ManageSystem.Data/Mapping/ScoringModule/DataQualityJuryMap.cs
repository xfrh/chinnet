using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.ScoringModule
{
    public partial class DataQualityJuryMap : ManageSystemEntityTypeConfiguration<DataQuality_Jury>
    {
        public DataQualityJuryMap()
        {
            ToTable("DataQuality_Jury");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
