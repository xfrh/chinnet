using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Researches;

namespace ManageSystem.Admin.Models.Researches
{
	/// <summary>
	/// 模型类 ，数据库表名：ResearchType 
	/// </summary>
	 [Validator(typeof(ResearchTypeValidator))]
	public partial class ResearchTypeModel : BaseEntityModel
	{

		/// <summary>
		/// 类型名称
		/// <summary>
		[HtmlDisplayAttribute("类型名称","类型名称")]
		public String Name { get; set; }

		/// <summary>
		/// 排序编号
		/// <summary>
		[HtmlDisplayAttribute("排序编号","排序编号")]
		public Int32 Sort { get; set; }

		/// <summary>
		/// 上级id
		/// <summary>
		[HtmlDisplayAttribute("上级id","上级id")]
		public long ResearchTypeId { get; set; }



	}
}
