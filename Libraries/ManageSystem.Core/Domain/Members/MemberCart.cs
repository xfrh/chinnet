using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Members
{
	/// <summary>
	/// 实体类 ，数据库表名：MemberCart 
	/// </summary>
	public partial class MemberCart : BaseEntity
	{

		/// <summary>
		/// 会员Id
		/// <summary>
		public long MemberId { get; set; }
		/// <summary>
		/// 商品Id
		/// <summary>
		public long ProductId { get; set; }
		/// <summary>
		/// 商品名称
		/// <summary>
		public String ProductName { get; set; }
		/// <summary>
		/// 商品市场价
		/// <summary>
		public Decimal MarketPrice { get; set; }
		/// <summary>
		/// 商品数量
		/// <summary>
		public Int32 Count { get; set; }


	}
}
