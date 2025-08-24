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
	/// 模型类 ，数据库表名：MedicalAntibioticRule 
	/// </summary>
	 [Validator(typeof(MedicalAntibioticRuleValidator))]
	public partial class MedicalAntibioticRuleModel : BaseEntityModel
	{

		/// <summary>
		/// 细菌Id，关联细菌表
		/// <summary>
		[HtmlDisplayAttribute("细菌Id，关联细菌表","细菌Id，关联细菌表")]
		public long OrganismId { get; set; }

		/// <summary>
		/// 细菌名称，关联细菌表
		/// <summary>
		[HtmlDisplayAttribute("细菌名称，关联细菌表","细菌名称，关联细菌表")]
		public String OrganismName { get; set; }

		/// <summary>
		/// 细菌编码，关联细菌表
		/// <summary>
		[HtmlDisplayAttribute("细菌编码，关联细菌表","细菌编码，关联细菌表")]
		public String OrganismCode { get; set; }

		/// <summary>
		/// 抗生素Id，关联抗生素表
		/// <summary>
		[HtmlDisplayAttribute("抗生素Id，关联抗生素表","抗生素Id，关联抗生素表")]
		public long AntibioticId { get; set; }

		/// <summary>
		/// 抗生素名称，关联抗生素表
		/// <summary>
		[HtmlDisplayAttribute("抗生素名称，关联抗生素表","抗生素名称，关联抗生素表")]
		public String AntibioticName { get; set; }

		/// <summary>
		/// 抗生素编码，关联抗生素表
		/// <summary>
		[HtmlDisplayAttribute("抗生素编码，关联抗生素表","抗生素编码，关联抗生素表")]
		public String AntibioticCode { get; set; }

		/// <summary>
		/// 检测方法 ，1：纸片法（ND）   2：MIC法 （NM）  3：Etest法（NE）
		/// <summary>
		[HtmlDisplayAttribute("检测方法 ，1：纸片法（ND）   2：MIC法 （NM）  3：Etest法（NE）","检测方法 ，1：纸片法（ND）   2：MIC法 （NM）  3：Etest法（NE）")]
		public Int32 CheckType { get; set; }

		/// <summary>
		/// 编码，理论值上：抗生素的编码_检测方法的简写
		/// <summary>
		[HtmlDisplayAttribute("编码，理论值上：抗生素的编码_检测方法的简写","编码，理论值上：抗生素的编码_检测方法的简写")]
		public float Code { get; set; }

		/// <summary>
		/// 敏感最小值（包含该值）
		/// <summary>
		[HtmlDisplayAttribute("敏感最小值（包含该值）","敏感最小值（包含该值）")]
		public float SensitivityMin { get; set; }

		/// <summary>
		/// 敏感最大值（包含该值）
		/// <summary>
		[HtmlDisplayAttribute("敏感最大值（包含该值）","敏感最大值（包含该值）")]
		public float SensitivityMax { get; set; }

		/// <summary>
		/// 中介最小值（包含该值）
		/// <summary>
		[HtmlDisplayAttribute("中介最小值（包含该值）","中介最小值（包含该值）")]
		public float IntermediaryMin { get; set; }

		/// <summary>
		/// 中介最大值（包含该值）
		/// <summary>
		[HtmlDisplayAttribute("中介最大值（包含该值）","中介最大值（包含该值）")]
		public float IntermediaryMax { get; set; }

		/// <summary>
		/// 耐药最小值（包含该值）
		/// <summary>
		[HtmlDisplayAttribute("耐药最小值（包含该值）","耐药最小值（包含该值）")]
		public float ResistanceMin { get; set; }

		/// <summary>
		/// 耐药最大值（包含该值）
		/// <summary>
		[HtmlDisplayAttribute("耐药最大值（包含该值）","耐药最大值（包含该值）")]
		public float ResistanceMax { get; set; }

		/// <summary>
		/// 说明
		/// <summary>
		[HtmlDisplayAttribute("说明","说明")]
		public String Remark { get; set; }



	}
}
