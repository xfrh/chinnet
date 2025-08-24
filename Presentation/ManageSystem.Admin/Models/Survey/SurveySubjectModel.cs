using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework.Mvc;

namespace ManageSystem.Admin.Models.Survey
{
	/// <summary>
	///问卷调查管理  问卷调查管理-问卷题目  模型类 ，数据库表名：Survey_subject 
	/// </summary>
	public partial class SurveySubjectModel : BaseEntityModel
    {

		/// <summary>
		/// 题目
		/// <summary>
		public String Name { get; set; }

		/// <summary>
		/// 状态，1：启用  2：禁用
		/// <summary>
		public Int32 State { get; set; }

		/// <summary>
		/// 排序编号，正序排列
		/// <summary>
		public Int32 Sort { get; set; }

		/// <summary>
		/// 说明信息
		/// <summary>
		public String Remark { get; set; }



	}
}
