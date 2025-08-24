using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
	/// <summary>
	/// 实体类 ，数据库表名：SatelliteMenu 
	/// </summary>
	public partial class SatelliteMenu : BaseEntity
	{

		/// <summary>
		/// 角色id
		/// <summary>
		public String Controller { get; set; }
					
		/// <summary>
		/// 角色id
		/// <summary>
		public String Func { get; set; }
		/// <summary>
		/// 角色id
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 用户id
		/// <summary>
		public long FatherId { get; set; }


	}
}
