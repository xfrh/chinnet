using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
	/// <summary>
	/// 实体类 ，数据库表名：UserRole 
	/// </summary>
	public partial class SatelliteMenuRole : BaseEntity
	{

		/// <summary>
		/// 角色id
		/// <summary>
		public long SatelliteMenuId { get; set; }
		/// <summary>
		/// 用户id
		/// <summary>
		public long SatelliteUserId { get; set; }


	}
}
