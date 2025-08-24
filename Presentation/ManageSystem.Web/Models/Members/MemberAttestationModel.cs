using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Members;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Members
{
	/// <summary>
	/// 模型类 ，数据库表名：MemberAttestation 
	/// </summary>
	 [Validator(typeof(MemberAttestationValidator))]
	public partial class MemberAttestationModel : BaseEntityModel
	{

		/// <summary>
		/// 所属城市
		/// <summary>
		[HtmlDisplayAttribute("所属城市","所属城市")]
		public long AreaId { get; set; }
 
        /// <summary>
        /// 城市名称
        /// <summary>
        [HtmlDisplayAttribute("城市名称","城市名称")]
		public String AreaName { get; set; }

		/// <summary>
		/// 所属医院
		/// <summary>
		[HtmlDisplayAttribute("所属医院","所属医院")]
		public long HospitalId { get; set; }
        public IList<SelectListItem> HospitalList { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        [HtmlDisplayAttribute("医院名称","医院名称")]
		public String HospitalName { get; set; }

		/// <summary>
		/// 所属科室
		/// <summary>
		[HtmlDisplayAttribute("所属科室","所属科室")]
		public long HospitalDepartmentId { get; set; }
        public IList<SelectListItem> HospitalDepartmentList { get; set; }

        /// <summary>
        /// 医院科室名称
        /// <summary>
        [HtmlDisplayAttribute("医院科室名称","医院科室名称")]
		public String HospitalDepartmentName { get; set; }

		/// <summary>
		/// 医生职称
		/// <summary>
		[HtmlDisplayAttribute("医生职称","医生职称")]
		public long DoctorTitleId { get; set; }
        public IList<SelectListItem> DoctorTitleList { get; set; }

        /// <summary>
        /// 医生职称名称
        /// <summary>
        [HtmlDisplayAttribute("医生职称名称","医生职称名称")]
		public String DoctorTitleName { get; set; }

		/// <summary>
		/// 审核状态  1：待审核   2：通过审核   3：取消
		/// <summary>
		[HtmlDisplayAttribute("审核状态","审核状态  1：待审核   2：通过审核   3：取消")]
		public Int32 Status { get; set; }

        public string StatusName { get; set; }

        /// <summary>
        /// 是否已经提交过
        /// </summary>
        public bool IsSuccess { get; set; }

    }
}
