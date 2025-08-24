using ManageSystem.Core.Domain.Ctr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping
{
    public partial class ClinicalTrialReportMap : ManageSystemEntityTypeConfiguration<ClinicalTrialReport>
    {
        public ClinicalTrialReportMap()
        {
            this.ToTable("ClinicalTrialReport");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
