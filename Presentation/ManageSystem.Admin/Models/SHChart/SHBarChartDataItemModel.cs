using ManageSystem.Core.Domain.SHChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.SHChart
{
    public class SHBarChartDataItemModel
    {
        public long Id { get; set; }
        public SHChartTypeEnum ChartType { get; set; }
        public string BarColor { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Display { get; set; }
        public int Sort { get; set; }
        public List<SHBarChartDataItemWithAntibioticModel> Antibiotics { get; set; }       
    }
    public class SHBarChartDataItemWithAntibioticModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class SHBarChartDataItemTableModel
    {
       public string Name { get; set; }
        public string MIC_Range { get; set; }
        public string MIC50 { get; set; }
        public string MIC90 { get; set; }
        public string S { get; set; }
        public string SDD { get; set; }
        public string I { get; set; }
        public string R { get; set; }
    }
}