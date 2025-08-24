using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;

namespace ManageSystem.Web.Models.Orders
{
	/// <summary>
	/// 模型类 ，数据库表名：OrderItem 
	/// </summary>
	public partial class OrderItemModel : BaseEntityModel
	{

		/// <summary>
		/// 订单Id
		/// <summary>
		[HtmlDisplayAttribute("订单Id","订单Id")]
		public long OrderId { get; set; }

		/// <summary>
		/// 商品Id
		/// <summary>
		[HtmlDisplayAttribute("商品Id","商品Id")]
		public long ProductId { get; set; }

		/// <summary>
		/// 商品编码
		/// <summary>
		[HtmlDisplayAttribute("商品编码","商品编码")]
		public String ProductCode { get; set; }

		/// <summary>
		/// 商品名称
		/// <summary>
		[HtmlDisplayAttribute("商品名称","商品名称")]
		public String ProductName { get; set; }

		/// <summary>
		/// 购买数量
		/// <summary>
		[HtmlDisplayAttribute("购买数量","购买数量")]
		public Int32 Count { get; set; }

		/// <summary>
		/// 商品成本价
		/// <summary>
		[HtmlDisplayAttribute("商品成本价","商品成本价")]
		public Decimal CostPrice { get; set; }

		/// <summary>
		/// 商品市场价
		/// <summary>
		[HtmlDisplayAttribute("商品市场价","商品市场价")]
		public Decimal MarketPrice { get; set; }

		/// <summary>
		/// 总金额
		/// <summary>
		[HtmlDisplayAttribute("总金额","总金额")]
		public Decimal Amount  { get; set; }

		/// <summary>
		/// 订单备注
		/// <summary>
		[HtmlDisplayAttribute("订单备注","订单备注")]
		public String Remark { get; set; }

        /// <summary>
        /// 商品图片（默认主图）
        /// <summary>
        public String ProductImage { get; set; }


    }
}
