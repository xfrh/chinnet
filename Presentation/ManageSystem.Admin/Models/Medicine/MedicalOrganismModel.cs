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
	/// 模型类 ，数据库表名：MedicalOrganism 
	/// </summary>
	 [Validator(typeof(MedicalOrganismValidator))]
	public partial class MedicalOrganismModel : BaseEntityModel
	{

		/// <summary>
		/// 细菌名称
		/// <summary>
		[HtmlDisplayAttribute("细菌名称","细菌名称")]
		public String Name { get; set; }

		/// <summary>
		/// 细菌类型
		/// <summary>
		[HtmlDisplayAttribute("细菌类型","细菌类型")]
		public String Type { get; set; }

		/// <summary>
		/// 细菌编码
		/// <summary>
		[HtmlDisplayAttribute("细菌编码","细菌编码")]
		public String Code { get; set; }

		/// <summary>
		/// 说明
		/// <summary>
		[HtmlDisplayAttribute("说明","说明")]
		public String Remark { get; set; }

		/// <summary>
		/// 所属细菌类型，一级分类
		/// <summary>
		[HtmlDisplayAttribute("所属细菌类型，一级分类","所属细菌类型，一级分类")]
		public long OrganismTypeId1 { get; set; }

		/// <summary>
		/// 所属细菌类型，二级分类
		/// <summary>
		[HtmlDisplayAttribute("所属细菌类型，二级分类","所属细菌类型，二级分类")]
		public long OrganismTypeId2 { get; set; }

		/// <summary>
		/// 排序字段
		/// <summary>
		[HtmlDisplayAttribute("排序字段","排序字段")]
		public Int32 Sort { get; set; }



	}
}
