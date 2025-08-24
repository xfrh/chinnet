using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Log
{
	/// <summary>
	/// 实体类 ，数据库表名：ActionLog 
	/// </summary>
	public partial class ActionLog : BaseEntity
	{

		/// <summary>
		/// 操作者浏览器名称
		/// <summary>
		public String BrowserName { get; set; }

		/// <summary>
		/// 操作的IP地址
		/// <summary>
		public String IPAddress { get; set; }

		/// <summary>
		/// 操作的用户id
		/// <summary>
		public long UserinfoId { get; set; }

		/// <summary>
		/// 操作用户的姓名和登录名
		/// <summary>
		public String UserinfoName { get; set; }

		/// <summary>
		/// 日志标题
		/// <summary>
		public String Content { get; set; }

        /// <summary>
        /// 日志详细
        /// <summary>
        public String Detail { get; set; }

        /// <summary>
        /// 日志类型
        /// <summary>
        public String Type { get; set; }

        /// <summary>
		/// 日志来源
		/// <summary>
        public int Source { get; set; }

    }
}
