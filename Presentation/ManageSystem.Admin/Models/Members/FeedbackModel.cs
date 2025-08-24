using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Members;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Members
{
	/// <summary>
	/// 模型类 ，数据库表名：Feedback 
	/// </summary>
	 [Validator(typeof(FeedbackValidator))]
	public partial class FeedbackModel : BaseEntityModel
	{

		/// <summary>
		/// 会员id
		/// <summary>
		[HtmlDisplayAttribute("会员id","会员id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 会员姓名
		/// <summary>
		[HtmlDisplayAttribute("会员姓名","会员姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 是否已查看
		/// <summary>
		[HtmlDisplayAttribute("是否已查看","是否已查看")]
		public bool Status { get; set; }

        public int StatusValue { get; set; }

        [HtmlDisplayAttribute("查看状态", "查看状态")]
        public IList<SelectListItem> StatusList { get; set; }


        /// <summary>
        /// 反馈标题
        /// <summary>
        [HtmlDisplayAttribute("反馈标题","反馈标题")]
		public String Name { get; set; }

		/// <summary>
		/// 反馈内容
		/// <summary>
		[HtmlDisplayAttribute("反馈内容","反馈内容")]
		public String Content { get; set; }



	}
}
