using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.Chart
{
    public partial class BarChartWithItemDataMap : ManageSystemEntityTypeConfiguration<Chart_BarChartWithItemData>
    {
        public BarChartWithItemDataMap()
        {
            ToTable("Chart_BarChartWithItemData");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Name).IsMaxLength().IsRequired();
            this.Property(p => p.Value).IsMaxLength();
        }
    }
}
