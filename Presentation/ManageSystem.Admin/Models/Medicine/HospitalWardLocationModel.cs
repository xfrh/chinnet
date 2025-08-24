using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Medicine;
using ManageSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Medicine
{
    [Validator(typeof(HospitalWardLocationValidator))]
    public class HospitalWardLocationModel
    {
        /// <summary>
        /// Key
        /// </summary>
        public long Id { get; set; } = Core.Utility.CommonHelper.GuidToLongID;
        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 所属医院
        /// </summary>
        [HtmlDisplay("所属医院", "所属医院")]
        public string Name { get; set; }
        /// <summary>
        /// Ward
        /// </summary>
        [HtmlDisplay("Ward", "Ward")]
        public string Ward { get; set; }
        /// <summary>
        /// Department<br />
        /// WHONET对应的字段: 专业类别
        /// </summary>
        [HtmlDisplay("Department", "Department")]
        public string Department { get; set; }
        /// <summary>
        /// Location<br />
        /// WHONET对应的字段: 科室
        /// </summary>
        [HtmlDisplay("Location", "Location")]
        public string Location { get; set; }
        /// <summary>
        /// LocationType<br />
        /// WHONET对应的字段: 科室类别
        /// </summary>
        [HtmlDisplay("Location_Type", "Location_Type")]
        public string LocationType { get; set; }

        public void Trim()
        {
            Ward = (Ward ?? "").Trim();
            Department = (Department ?? "").Trim();
            Location = (Location ?? "").Trim();
            LocationType = (LocationType ?? "").Trim();
        }
    }
}