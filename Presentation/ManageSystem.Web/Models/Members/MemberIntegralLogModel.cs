using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;

namespace ManageSystem.Web.Models.Members
{
	/// <summary>
	/// 模型类 ，数据库表名：MemberIntegralLog 
	/// </summary>
	public partial class MemberIntegralLogModel : BaseEntityModel
	{

		/// <summary>
		/// 会员id
		/// <summary>
		[HtmlDisplayAttribute("会员id","会员id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 积分数量
		/// <summary>
		[HtmlDisplayAttribute("积分数量","积分数量")]
		public Decimal Value { get; set; }

		/// <summary>
		/// 操作类型
		/// <summary>
		[HtmlDisplayAttribute("操作类型","操作类型")]
		public String Type { get; set; }

		/// <summary>
		/// 操作来源，前台  、微信、网页
		/// <summary>
		[HtmlDisplayAttribute("操作来源，前台  、微信、网页","操作来源，前台  、微信、网页")]
		public String Source { get; set; }

		/// <summary>
		/// 操作说明
		/// <summary>
		[HtmlDisplayAttribute("操作说明","操作说明")]
		public String Remark { get; set; }



	}
}
