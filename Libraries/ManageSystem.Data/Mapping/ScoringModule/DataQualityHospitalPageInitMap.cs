using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.ScoringModule
{
    public partial class DataQualityHospitalPageInitMap : ManageSystemEntityTypeConfiguration<DataQuality_Hospital_PageInit>
    {
        public DataQualityHospitalPageInitMap()
        {
            ToTable("DataQuality_Hospital_PageInit");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
