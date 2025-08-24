using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Weixin
{
	/// <summary>
	/// 实体类 ，数据库表名：WeixinUser 
	/// </summary>
	public partial class WeixinUser : BaseEntity
	{

		/// <summary>
		/// 用户昵称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 用户性别
		/// <summary>
		public int Sex { get; set; }
		/// <summary>
		/// 用户的标识
		/// <summary>
		public String Openid { get; set; }
		/// <summary>
		/// 关注状态 
		/// <summary>
		public bool Subscribe { get; set; }
		/// <summary>
		/// 关注时间
		/// <summary>
		public DateTime SubscribeTime { get; set; }
		/// <summary>
		/// 取消关注时间
		/// <summary>
		public DateTime UnsubscribeTime { get; set; }
		/// <summary>
		/// 所在区域
		/// <summary>
		public String Area { get; set; }


	}
}
