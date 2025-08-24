using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Orders
{
	/// <summary>
	/// 实体类 ，数据库表名：OrderLog 
	/// </summary>
	public partial class OrderLog : BaseEntity
	{

		/// <summary>
		/// 订单Id
		/// <summary>
		public long OrderId { get; set; }
		/// <summary>
		/// 操作类型
		/// <summary>
		public String Type { get; set; }
        /// <summary>
        /// 操作来源，前台  、微信、网页
        /// <summary>
        public string Source { get; set; }
		/// <summary>
		/// 操作人id，注意前后台用户
		/// <summary>
		public long UserId { get; set; }
		/// <summary>
		/// 操作人姓名，注意前后台用户
		/// <summary>
		public String UserName { get; set; }
		/// <summary>
		/// 操作内容
		/// <summary>
		public String Content { get; set; }

        /// <summary>
        /// 订单的json数据
        /// <summary>
        public String JsonData { get; set; }
    }
}
