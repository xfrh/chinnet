using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Products
{
	/// <summary>
	/// 实体类 ，数据库表名：Product 
	/// </summary>
	public partial class Product : BaseEntity
	{

		/// <summary>
		/// 商品名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 状态，1：下架  2：上架
		/// <summary>
		public Int32 Status { get; set; }
		/// <summary>
		/// 商品分类，关联ProductCategory表
		/// <summary>
		public long ProductCategoryId { get; set; }
		/// <summary>
		/// 商品品牌，关联ProductBrand表
		/// <summary>
		public long ProductBrandId { get; set; }
		/// <summary>
		/// 商品编码，唯一
		/// <summary>
		public String Code { get; set; }
		/// <summary>
		/// 商品成本价
		/// <summary>
		public Decimal CostPrice { get; set; }
		/// <summary>
		/// 商品市场价
		/// <summary>
		public Decimal MarketPrice { get; set; }
		/// <summary>
		/// 商品详细
		/// <summary>
		public String Content { get; set; }
		/// <summary>
		/// 库存数量
		/// <summary>
		public Int32 Count { get; set; }

        /// <summary>
		/// 购买次数，每次下单成功加1
		/// <summary>
		public Int32 BuyCount { get; set; }

        /// <summary>
        /// 商品的主图，冗余字段，同步商品图片表的主图
        /// <summary>
        public String MainImage { get; set; }


    }
}
