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
	/// 模型类 ，数据库表名：MemberCart 
	/// </summary>
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
        /// 商品主图
        /// </summary>
        public string ProductImage { get; set; }

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

        /// <summary>
        /// 单行购物车总价， MarketPrice *  Count
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        ///  可用状态，不可用的话在购物车页面禁用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 可用状态的说明内容
        /// </summary>
        public string StatusMessage { get; set; }

    }


    /// <summary>
    /// 购物车页面提交结算的数据实体
    /// </summary>
    public class CartSubmitModel
    {
        /// <summary>
        /// 购物车表的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }
    }
}
