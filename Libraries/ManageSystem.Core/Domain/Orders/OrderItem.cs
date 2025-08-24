using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Orders
{
	/// <summary>
	/// 实体类 ，数据库表名：OrderItem 
	/// </summary>
	public partial class OrderItem : BaseEntity
	{

		/// <summary>
		/// 订单Id
		/// <summary>
		public long OrderId { get; set; }
		/// <summary>
		/// 商品Id
		/// <summary>
		public long ProductId { get; set; }
		/// <summary>
		/// 商品编码
		/// <summary>
		public String ProductCode { get; set; }
		/// <summary>
		/// 商品名称
		/// <summary>
		public String ProductName { get; set; }
		/// <summary>
		/// 购买数量
		/// <summary>
		public Int32 Count { get; set; }
		/// <summary>
		/// 商品成本价
		/// <summary>
		public Decimal CostPrice { get; set; }
		/// <summary>
		/// 商品市场价
		/// <summary>
		public Decimal MarketPrice { get; set; }
		/// <summary>
		/// 总金额
		/// <summary>
		public Decimal Amount  { get; set; }
		/// <summary>
		/// 订单备注
		/// <summary>
		public String Remark { get; set; }
        /// <summary>
        /// 商品图片（默认主图）
        /// <summary>
        public String ProductImage { get; set; }
        
    }
}
