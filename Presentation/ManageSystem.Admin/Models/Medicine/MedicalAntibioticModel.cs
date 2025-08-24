using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Medicine;

namespace ManageSystem.Admin.Models.Medicine
{
	/// <summary>
	/// 模型类 ，数据库表名：MedicalAntibiotic 
	/// </summary>
	 [Validator(typeof(MedicalAntibioticValidator))]
	public partial class MedicalAntibioticModel : BaseEntityModel
	{

		/// <summary>
		/// 抗生素名称
		/// <summary>
		[HtmlDisplayAttribute("抗生素名称","抗生素名称")]
		public String Name { get; set; }

		/// <summary>
		/// 抗生素编码
		/// <summary>
		[HtmlDisplayAttribute("抗生素编码","抗生素编码")]
		public String Code { get; set; }

		/// <summary>
		/// 说明
		/// <summary>
		[HtmlDisplayAttribute("说明","说明")]
		public String Remark { get; set; }

		/// <summary>
		/// 排序字段
		/// <summary>
		[HtmlDisplayAttribute("排序字段","排序字段")]
		public Int32 Sort { get; set; }


        
    }
}
