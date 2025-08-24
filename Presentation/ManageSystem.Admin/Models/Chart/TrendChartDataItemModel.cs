using ManageSystem.Core.Domain.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Chart
{
    public class TrendChartDataItemModel
    {
        public long Id { get; set; }
        public ChartTypeEnum ChartType { get; set; }
        public string ChartColor { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Display { get; set; }
        public int Sort { get; set; }
        public List<TrendChartDataItemWithAntibioticModel> Antibiotics { get; set; }
    }

    public class TrendChartDataItemWithAntibioticModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}