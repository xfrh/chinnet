using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Meetings
{
	/// <summary>
	/// 实体类 ，数据库表名：MeetingType 
	/// </summary>
	public partial class MeetingType : BaseEntity
	{

		/// <summary>
		/// 会议类型名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 排序编号
		/// <summary>
		public Int32 Sort { get; set; }
		/// <summary>
		/// 上级分类
		/// <summary>
		public long MeetingTypeId { get; set; }


	}
}
