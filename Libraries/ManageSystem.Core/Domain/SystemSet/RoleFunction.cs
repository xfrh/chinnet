using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
	/// <summary>
	/// 实体类 ，数据库表名：RoleFunction 
	/// </summary>
	public partial class RoleFunction : BaseEntity
	{

		/// <summary>
		/// 角色id
		/// <summary>
		public long RoleId { get; set; }
		/// <summary>
		/// 功能id
		/// <summary>
		public long FunctionId { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// 所属功能
        /// </summary>
        public virtual Function Function { get; set; }

    }
}
