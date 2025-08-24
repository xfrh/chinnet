using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Datas
{
    public class BarChartDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<BarChartDataItemDTO> DataItems { get; set; }
    }

    public class BarChartDataItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }
    }
}