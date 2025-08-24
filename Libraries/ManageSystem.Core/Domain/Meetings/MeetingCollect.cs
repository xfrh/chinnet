using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Meetings
{
	/// <summary>
	/// 实体类 ，数据库表名：MeetingCollect 
	/// </summary>
	public partial class MeetingCollect : BaseEntity
	{

		/// <summary>
		/// 用户的id
		/// <summary>
		public long MemberId { get; set; }
		/// <summary>
		/// 用户的姓名
		/// <summary>
		public String MemberName { get; set; }
		/// <summary>
		/// 信息动态的id
		/// <summary>
		public long MeetingId { get; set; }
		/// <summary>
		/// 信息动态的名称
		/// <summary>
		public String MeetingName { get; set; }
		/// <summary>
		/// Ip地址
		/// <summary>
		public String Ip { get; set; }
		/// <summary>
		/// 操作者浏览器名称
		/// <summary>
		public String BrowserName { get; set; }


	}
}
