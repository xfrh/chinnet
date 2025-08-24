using ManageSystem.Core.Domain.ScoringModule.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.ScoringModule.Log
{
    public partial class DataQualityActionLogMap : ManageSystemEntityTypeConfiguration<DataQuality_Action_Log>
    {
        public DataQualityActionLogMap()
        {
            ToTable("DataQuality_Action_Log");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
