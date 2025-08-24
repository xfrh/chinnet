using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.Chart
{
    public partial class DataSegmentMap : ManageSystemEntityTypeConfiguration<Chart_DataSegment>
    {
        public DataSegmentMap()
        {
            this.ToTable("Chart_DataSegment");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Name).IsMaxLength();
            this.Property(p => p.ProjectType).IsRequired();
        }
    }
}
