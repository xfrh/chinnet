using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Researches
{
	/// <summary>
	/// 实体类 ，数据库表名：ResearchType 
	/// </summary>
	public partial class ResearchType : BaseEntity
	{

		/// <summary>
		/// 类型名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 排序编号
		/// <summary>
		public Int32 Sort { get; set; }
		/// <summary>
		/// 上级id
		/// <summary>
		public long ResearchTypeId { get; set; }


	}
}
