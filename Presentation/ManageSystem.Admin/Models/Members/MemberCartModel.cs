using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Members;

namespace ManageSystem.Admin.Models.Members
{
	/// <summary>
	/// 模型类 ，数据库表名：MemberCart 
	/// </summary>
	 [Validator(typeof(MemberCartValidator))]
	public partial class MemberCartModel : BaseEntityModel
	{

		/// <summary>
		/// 会员Id
		/// <summary>
		[HtmlDisplayAttribute("会员Id","会员Id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 商品Id
		/// <summary>
		[HtmlDisplayAttribute("商品Id","商品Id")]
		public long ProductId { get; set; }

		/// <summary>
		/// 商品名称
		/// <summary>
		[HtmlDisplayAttribute("商品名称","商品名称")]
		public String ProductName { get; set; }

		/// <summary>
		/// 商品市场价
		/// <summary>
		[HtmlDisplayAttribute("商品市场价","商品市场价")]
		public Decimal MarketPrice { get; set; }

		/// <summary>
		/// 商品数量
		/// <summary>
		[HtmlDisplayAttribute("商品数量","商品数量")]
		public Int32 Count { get; set; }



	}
}
