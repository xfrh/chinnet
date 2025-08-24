using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data.Mapping.Chart
{
    public class TrendChartMap : ManageSystemEntityTypeConfiguration<Chart_TrendChart>
    {
        public TrendChartMap()
        {
            ToTable("Chart_TrendChart");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Name).IsMaxLength().IsRequired();
            this.Property(p => p.Title).IsMaxLength().IsRequired();
            this.Property(p => p.SubTitle).IsMaxLength().IsRequired();
        }
    }
}
