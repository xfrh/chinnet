using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.MICapply;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models
{
    [Validator(typeof(MICPermissionapplicationValidators))]
    public class MICPermissionapplication : BaseEntityModel
    {
        ///// <summary>
        ///// MIC申请表ID
        ///// <summary>
        //[HtmlDisplayAttribute("MIC申请表Id", "MIC申请表Id")]
        //public int ID { get; set; }

        public int MID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string Department { get; set; }
        public string Provincesandcities { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string EditInfo { get; set; }
        public DateTime Applicationtime { get; set; }
        public int Applicationstate { get; set; }
        public IList<SelectListItem> CompanyNameList { get; set; }
        public string Smid { get; set; }
    }
}