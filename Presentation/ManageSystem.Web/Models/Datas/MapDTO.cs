using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Datas
{
    public class MapProvinceName
    {
        public string name { get; set; }

        public int  value  { get; set; }
    }

    public class MapHospital
    {
        public string name { get; set; }

        public string value { get; set; }
    }

    public class MapList
    {
        public List<MapProvinceName> ProvinceName { get; set; }

        public List<MapHospital> Hospital { get; set; }
    }
}