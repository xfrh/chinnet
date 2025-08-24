using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{
	/// <summary>
	/// 折点实体类 ，数据库表名：MedicalAntibioticRule 
	/// </summary>
	public partial class MedicalAntibioticRule : BaseEntity
	{
        /// <summary>
        /// 规则名称
        /// <summary>
        public String Name { get; set; }

        /// <summary>
        /// 编码，理论值上：抗生素的编码_检测方法的简写
        /// <summary>
        public String Code { get; set; }

        /// <summary>
        /// 细菌Id，关联细菌表
        /// <summary>
        public long OrganismId { get; set; }
		/// <summary>
		/// 细菌名称，关联细菌表
		/// <summary>
		public String OrganismName { get; set; }
		/// <summary>
		/// 细菌编码，关联细菌表
		/// <summary>
		public String OrganismCode { get; set; }
		/// <summary>
		/// 抗生素Id，关联抗生素表
		/// <summary>
		public long AntibioticId { get; set; }
		/// <summary>
		/// 抗生素名称，关联抗生素表
		/// <summary>
		public String AntibioticName { get; set; }
		/// <summary>
		/// 抗生素编码，关联抗生素表
		/// <summary>
		public String AntibioticCode { get; set; }
		/// <summary>
		/// 检测方法 ，1：纸片法（ND）   2：MIC法 （NM）  3：Etest法（NE）
		/// <summary>
		public Int32 CheckType { get; set; }

		/// <summary>
		/// 敏感最小值（包含该值）
		/// <summary>
		public float SensitivityMin { get; set; }
		/// <summary>
		/// 敏感最大值（包含该值）
		/// <summary>
		public float SensitivityMax { get; set; }
		/// <summary>
		/// 中介最小值（包含该值）
		/// <summary>
		public float IntermediaryMin { get; set; }
		/// <summary>
		/// 中介最大值（包含该值）
		/// <summary>
		public float IntermediaryMax { get; set; }
		/// <summary>
		/// 耐药最小值（包含该值）
		/// <summary>
		public float ResistanceMin { get; set; }
		/// <summary>
		/// 耐药最大值（包含该值）
		/// <summary>
		public float ResistanceMax { get; set; }
		/// <summary>
		/// 说明
		/// <summary>
		public String Remark { get; set; }


	}
}
