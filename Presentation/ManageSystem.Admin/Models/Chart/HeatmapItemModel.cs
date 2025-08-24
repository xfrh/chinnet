using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Chart
{
    public class HeatmapItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Sort { get; set; }
    }
}