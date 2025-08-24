using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Medicine
{
    public class ProjectSituation
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public int Year { get; set; }
        public string MemberName { get; set; }
        public string Quarter { get; set; }
        public string HospitalName { get; set; }
        public string UploadFilePath { get; set; }
        public string DisposeFilePath { get; set; }
        public string Complete { get; set; }
        public string hangintheair { get; set; }
        public string LoginId { get; set; }
    }   
}