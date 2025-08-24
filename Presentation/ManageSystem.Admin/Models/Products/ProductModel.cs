using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Products;
using System.Web.Mvc;
using ManageSystem.Core.Domain.Products;

namespace ManageSystem.Admin.Models.Products
{
	/// <summary>
	/// 模型类 ，数据库表名：Product 
	/// </summary>
	 [Validator(typeof(ProductValidator))]
	public partial class ProductModel : BaseEntityModel
	{
		/// <summary>
		/// 商品名称
		/// <summary>
		[HtmlDisplayAttribute("商品名称","商品名称",true)]
		public String Name { get; set; }

		/// <summary>
		/// 状态，1：下架  2：上架
		/// <summary>
		[HtmlDisplayAttribute("状态","状态", true)]
		public Int32 Status { get; set; }

        /// <summary>
        /// 状态列表
        /// <summary>
        [HtmlDisplayAttribute("状态", "状态", true)]
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 商品分类，关联ProductCategory表
        /// <summary>
        [HtmlDisplayAttribute("商品分类","商品分类", true)]
		public long ProductCategoryId { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }


        /// <summary>
        /// 商品分类列表
        /// <summary>
        [HtmlDisplayAttribute("商品分类", "商品分类", true)]
        public IList<SelectListItem> ProductCategoryList { get; set; }

        /// <summary>
        /// 商品品牌，关联ProductBrand表
        /// <summary>
        [HtmlDisplayAttribute("商品品牌","商品品牌", true)]
		public long ProductBrandId { get; set; }

        /// <summary>
        /// 商品品牌名称
        /// </summary>
        public string ProductBrandName { get; set; }

        /// <summary>
        /// 商品品牌列表
        /// <summary>
        [HtmlDisplayAttribute("商品品牌", "商品品牌", true)]
        public IList<SelectListItem> ProductBrandList { get; set; }

        /// <summary>
        /// 商品编码，唯一
        /// <summary>
        [HtmlDisplayAttribute("商品编码","商品编码")]
		public String Code { get; set; }

		/// <summary>
		/// 商品成本价
		/// <summary>
		[HtmlDisplayAttribute("商品成本价","商品成本价", true)]
		public Decimal CostPrice { get; set; }

		/// <summary>
		/// 商品市场价
		/// <summary>
		[HtmlDisplayAttribute("商品市场价","商品市场价", true)]
		public Decimal MarketPrice { get; set; }

		/// <summary>
		/// 商品详细
		/// <summary>
		[HtmlDisplayAttribute("商品详细","商品详细", true)]
		public String Content { get; set; }

		/// <summary>
		/// 库存数量
		/// <summary>
		[HtmlDisplayAttribute("库存数量","库存数量", true)]
		public Int32 Count { get; set; }

        /// <summary>
        /// 商品对应的图片集合
        /// </summary>
        public List<ProductImage> ProductImageList { get; set; }

        /// <summary>
        /// 商品图片的json数据，保存或者修改的时候用
        /// </summary>
        public string ProductImageJson { get; set; }

        /// <summary>
        /// 商品的主图
        /// </summary>
        public string ProductImage { get; set; }

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
