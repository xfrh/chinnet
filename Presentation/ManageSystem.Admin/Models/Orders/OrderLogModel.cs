using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Orders;

namespace ManageSystem.Admin.Models.Orders
{
	/// <summary>
	/// 模型类 ，数据库表名：OrderLog 
	/// </summary>
	 [Validator(typeof(OrderLogValidator))]
	public partial class OrderLogModel : BaseEntityModel
	{

		/// <summary>
		/// 订单Id
		/// <summary>
		[HtmlDisplayAttribute("订单Id","订单Id")]
		public long OrderId { get; set; }

		/// <summary>
		/// 操作类型
		/// <summary>
		[HtmlDisplayAttribute("操作类型","操作类型")]
		public String Type { get; set; }

		/// <summary>
		/// 操作来源，1：前台   2：后台
		/// <summary>
		[HtmlDisplayAttribute("操作来源","操作来源，1：前台   2：后台")]
		public String Source { get; set; }

		/// <summary>
		/// 操作人id，注意前后台用户
		/// <summary>
		[HtmlDisplayAttribute("操作人id，注意前后台用户","操作人id，注意前后台用户")]
		public long UserId { get; set; }

		/// <summary>
		/// 操作人姓名，注意前后台用户
		/// <summary>
		[HtmlDisplayAttribute("操作人姓名，注意前后台用户","操作人姓名，注意前后台用户")]
		public String UserName { get; set; }

		/// <summary>
		/// 操作内容
		/// <summary>
		[HtmlDisplayAttribute("操作内容","操作内容")]
		public String Content { get; set; }



	}
}
