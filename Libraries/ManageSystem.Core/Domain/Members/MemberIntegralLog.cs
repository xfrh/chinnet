using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Members
{
	/// <summary>
	/// 实体类 ，数据库表名：MemberIntegralLog 
	/// </summary>
	public partial class MemberIntegralLog : BaseEntity
	{

		/// <summary>
		/// 会员id
		/// <summary>
		public long MemberId { get; set; }
		/// <summary>
		/// 积分数量
		/// <summary>
		public Decimal Value { get; set; }
		/// <summary>
		/// 操作类型
		/// <summary>
		public String Type { get; set; }
		/// <summary>
		/// 操作来源，前台  、微信、网页
		/// <summary>
		public String Source { get; set; }
		/// <summary>
		/// 操作说明
		/// <summary>
		public String Remark { get; set; }


        /// <summary>
        /// 数据id
        /// </summary>
        public long DataId { get; set; }


    }
}
