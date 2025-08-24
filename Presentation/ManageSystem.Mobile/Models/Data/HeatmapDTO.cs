using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Mobile.Models.Data
{
    public class HeatmapDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<HeatmapItemDTO> DataItems { get; set; }
    }

    public class HeatmapItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }
    }
}