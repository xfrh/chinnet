using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Mobile.Models.Data
{
    public class SHBarChartDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Data_type { get; set; }
        public List<SHBarChartDataItemDTO> DataItems { get; set; }
    }

    public class SHBarChartDataItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }
    }
}