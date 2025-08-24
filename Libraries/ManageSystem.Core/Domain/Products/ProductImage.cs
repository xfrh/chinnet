using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Products
{
	/// <summary>
	/// 实体类 ，数据库表名：ProductImage 
	/// </summary>
	public partial class ProductImage : BaseEntity
	{

		/// <summary>
		/// 原始图片名称
		/// <summary>
		public String OldFileName { get; set; }
		/// <summary>
		/// 新文件名称，上传以后在服务器上的文件名称
		/// <summary>
		public String NewFileName { get; set; }
		/// <summary>
		/// 图片路径，相对路径
		/// <summary>
		public String Path { get; set; }
		/// <summary>
		/// 排序，正序
		/// <summary>
		public Int32 Sort { get; set; }
		/// <summary>
		/// 是否主图  0：否  1：是，一个商品只能有一个主图
		/// <summary>
		public bool IsMain { get; set; }
		/// <summary>
		/// 商品Id
		/// <summary>
		public long ProductId { get; set; }


	}
}
