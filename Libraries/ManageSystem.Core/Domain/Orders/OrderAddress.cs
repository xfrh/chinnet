using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Orders
{
	/// <summary>
	/// 实体类 ，数据库表名：OrderAddress 
	/// </summary>
	public partial class OrderAddress : BaseEntity
	{

		/// <summary>
		/// 订单Id
		/// <summary>
		public long OrderId { get; set; }
		/// <summary>
		/// 会员id
		/// <summary>
		public long MemberId { get; set; }
		/// <summary>
		/// 用户收货地址id
		/// <summary>
		public long MemberAddressId { get; set; }
		/// <summary>
		/// 收货人姓名
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 邮箱地址
		/// <summary>
		public String Email { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        public String Address { get; set; }
        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        public String Area { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        public String Phone { get; set; }
		/// <summary>
		/// 邮编
		/// <summary>
		public String ZipPostalCode { get; set; }
		/// <summary>
		/// 固定电话
		/// <summary>
		public String Tel { get; set; }
		/// <summary>
		/// 收货地址中的省份id
		/// <summary>
		public long ProvinceId { get; set; }
		/// <summary>
		/// 收货地址中的市id
		/// <summary>
		public long CityId { get; set; }
		/// <summary>
		/// 收货地址中的区id
		/// <summary>
		public long DistrictsId { get; set; }
		/// <summary>
		/// 备注
		/// <summary>
		public String Remark { get; set; }


	}
}
