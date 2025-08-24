using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Demo
{
    public partial class ProductPictureModel 
    {
       
        public int ProductId { get; set; }

        [UIHint("Picture")]
        public int PictureId { get; set; }

        public string PictureUrl { get; set; }

        public int DisplayOrder { get; set; }

        public string OverrideAltAttribute { get; set; }

        public string OverrideTitleAttribute { get; set; }
    }
}