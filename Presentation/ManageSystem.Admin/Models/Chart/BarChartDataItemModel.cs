using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Chart
{
    public class BarChartDataItemModel
    {
        public long Id { get; set; }
        public ChartTypeEnum ChartType { get; set; }
        public string BarColor { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Display { get; set; }
        public int Sort { get; set; }
        public List<BarChartDataItemWithAntibioticModel> Antibiotics { get; set; }
    }

    public class BarChartDataItemWithAntibioticModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
