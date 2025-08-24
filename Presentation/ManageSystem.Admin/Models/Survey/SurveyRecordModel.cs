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
	///问卷调查管理  问卷调查管理-问卷记录  模型类 ，数据库表名：Survey_Record 
	/// </summary>
	public partial class SurveyRecordModel : BaseEntityModel
	{

		/// <summary>
		/// 调查ID  关联Survey_Survey表
		/// <summary>
		public long SurveyId { get; set; }

		/// <summary>
		/// 回答1
		/// <summary>
		public String Content1 { get; set; }

		/// <summary>
		/// 回答2
		/// <summary>
		public String Content2 { get; set; }

		/// <summary>
		/// 回答3
		/// <summary>
		public String Content3 { get; set; }

		/// <summary>
		/// 回答4
		/// <summary>
		public String Content4 { get; set; }

		/// <summary>
		/// 回答5
		/// <summary>
		public String Content5 { get; set; }

		/// <summary>
		/// 回答6
		/// <summary>
		public String Content6 { get; set; }

		/// <summary>
		/// 回答7
		/// <summary>
		public String Content7 { get; set; }

		/// <summary>
		/// 回答8
		/// <summary>
		public String Content8 { get; set; }

		/// <summary>
		/// 回答9
		/// <summary>
		public String Content9 { get; set; }

		/// <summary>
		/// 回答10
		/// <summary>
		public String Content10 { get; set; }

		/// <summary>
		/// 回答11
		/// <summary>
		public String Content11 { get; set; }

		/// <summary>
		/// 回答12
		/// <summary>
		public String Content12 { get; set; }

		/// <summary>
		/// 回答13
		/// <summary>
		public String Content13 { get; set; }

		/// <summary>
		/// 回答14
		/// <summary>
		public String Content14 { get; set; }

		/// <summary>
		/// 回答15
		/// <summary>
		public String Content15 { get; set; }

		/// <summary>
		/// 回答16
		/// <summary>
		public String Content16 { get; set; }

		/// <summary>
		/// 回答17
		/// <summary>
		public String Content17 { get; set; }

		/// <summary>
		/// 回答18
		/// <summary>
		public String Content18 { get; set; }

		/// <summary>
		/// 回答19
		/// <summary>
		public String Content19 { get; set; }

		/// <summary>
		/// 回答20
		/// <summary>
		public String Content20 { get; set; }

		/// <summary>
		/// 回答21
		/// <summary>
		public String Content21 { get; set; }

		/// <summary>
		/// 回答22
		/// <summary>
		public String Content22 { get; set; }

		/// <summary>
		/// 回答23
		/// <summary>
		public String Content23 { get; set; }

		/// <summary>
		/// 回答24
		/// <summary>
		public String Content24 { get; set; }

		/// <summary>
		/// 回答25
		/// <summary>
		public String Content25 { get; set; }

		/// <summary>
		/// 回答26
		/// <summary>
		public String Content26 { get; set; }

		/// <summary>
		/// 回答27
		/// <summary>
		public String Content27 { get; set; }

		/// <summary>
		/// 回答28
		/// <summary>
		public String Content28 { get; set; }

		/// <summary>
		/// 回答29
		/// <summary>
		public String Content29 { get; set; }

		/// <summary>
		/// 回答30
		/// <summary>
		public String Content30 { get; set; }

		/// <summary>
		/// 回答31
		/// <summary>
		public String Content31 { get; set; }

		/// <summary>
		/// 用户Id
		/// <summary>
		public long MemberId { get; set; }

		/// <summary>
		/// 用户名称
		/// <summary>
		public String MemberName { get; set; }
        /// <summary>
        /// 用户手机号
        /// <summary>
        public String MemberPhone { get; set; }

        /// <summary>
        /// 权限（1、开放问卷；2、隐私问卷）
        /// <summary>
        public long Rood { get; set; }

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
