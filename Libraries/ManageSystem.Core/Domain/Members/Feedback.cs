using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Members
{
	/// <summary>
	/// 实体类 ，数据库表名：Feedback 
	/// </summary>
	public partial class Feedback : BaseEntity
	{

		/// <summary>
		/// 会员id
		/// <summary>
		public long MemberId { get; set; }
		/// <summary>
		/// 会员姓名
		/// <summary>
		public String MemberName { get; set; }
		/// <summary>
		/// 是否已查看
		/// <summary>
		public bool Status { get; set; }
		/// <summary>
		/// 反馈标题
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 反馈内容
		/// <summary>
		public String Content { get; set; }


	}
}
