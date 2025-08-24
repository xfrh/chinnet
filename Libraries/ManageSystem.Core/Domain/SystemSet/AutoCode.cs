using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
	/// <summary>
	/// 实体类 ，数据库表名：AutoCode 
	/// </summary>
	public partial class AutoCode : BaseEntity
	{
		/// <summary>
		/// 类型，用数字来标识
		/// <summary>
		public Int32 Type { get; set; }
		/// <summary>
		/// 名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 前缀
		/// <summary>
		public String Prefix { get; set; }
		/// <summary>
		/// 后缀
		/// <summary>
		public String Suffix { get; set; }
		/// <summary>
		/// 最大值
		/// <summary>
		public long MaxValue { get; set; }
		/// <summary>
		/// 增量，每次递增的量
		/// <summary>
		public Int32 Increment { get; set; }
		/// <summary>
		/// 长度
		/// <summary>
		public Int32 Length { get; set; }
		/// <summary>
		/// 备注说明
		/// <summary>
		public String Remark { get; set; }


    }
}
