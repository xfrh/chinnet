using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Integrals
{
	/// <summary>
	/// 实体类 ，数据库表名：IntegralSetting 
	/// </summary>
	public partial class IntegralSetting : BaseEntity
	{

		/// <summary>
		/// 制度名称
		/// <summary>
		public String Name { get; set; }

        /// <summary>
        ///操作类型，（1、上传医学数据   2、创建信息动态  3、创建科研合作）
        /// <summary>
        public int Type { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        public bool Status { get; set; }

        /// <summary>
        /// 赠送积分数量
        /// <summary>
        public Decimal Value { get; set; }

		/// <summary>
		/// 说明
		/// <summary>
		public String Remark { get; set; }


	}
}
