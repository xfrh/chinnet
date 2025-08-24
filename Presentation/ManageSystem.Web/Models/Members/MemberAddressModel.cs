using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Members;

namespace ManageSystem.Web.Models.Members
{
	/// <summary>
	/// 模型类 ，数据库表名：MemberAddress 
	/// </summary>
	 [Validator(typeof(MemberAddressValidator))]
	public partial class MemberAddressModel : BaseEntityModel
	{

		/// <summary>
		/// 会员id
		/// <summary>
		[HtmlDisplayAttribute("所属会员", "所属会员", true)]
		public long MemberId { get; set; }

        /// <summary>
        /// 会员登录帐号
        /// <summary>
        [HtmlDisplayAttribute("会员登录帐号", "会员登录帐号")]
        public string MemberLoginId{ get; set; }

        /// <summary>
        /// 收货人姓名
        /// <summary>
        [HtmlDisplayAttribute("收货人姓名","收货人姓名", true)]
		public String Name { get; set; }

		/// <summary>
		/// 邮箱地址
		/// <summary>
		[HtmlDisplayAttribute("邮箱地址","邮箱地址")]
		public String Email { get; set; }

        /// <summary>
        /// 详细地址，例如：江苏路135号903
        /// <summary>
        [HtmlDisplayAttribute("详细地址", "详细地址", true)]
        public String Address { get; set; }
        /// <summary>
        /// 省市区，例如：河北省 沧州市 沧县
        /// <summary>
        [HtmlDisplayAttribute("省市区", "省市区", true)]
        public String Area { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码","手机号码", true)]
		public String Phone { get; set; }

		/// <summary>
		/// 邮编
		/// <summary>
		[HtmlDisplayAttribute("邮编","邮编")]
		public String ZipPostalCode { get; set; }

		/// <summary>
		/// 固定电话
		/// <summary>
		[HtmlDisplayAttribute("固定电话","固定电话")]
		public String Tel { get; set; }

		/// <summary>
		/// 是否为默认，1：是  0：不是
		/// <summary>
		[HtmlDisplayAttribute("是否为默认","是否为默认，1：是  0：不是")]
		public bool IsMain { get; set; }

        /// <summary>
        /// 收货地址中的省份id
        /// <summary>
        [HtmlDisplayAttribute("收货地址中的省份id","收货地址中的省份id")]
		public long ProvinceId { get; set; }

		/// <summary>
		/// 收货地址中的市id
		/// <summary>
		[HtmlDisplayAttribute("收货地址中的市id","收货地址中的市id")]
		public long CityId { get; set; }

		/// <summary>
		/// 收货地址中的区id
		/// <summary>
		[HtmlDisplayAttribute("收货地址中的区id","收货地址中的区id")]
		public long DistrictsId { get; set; }

		/// <summary>
		/// 用户备注
		/// <summary>
		[HtmlDisplayAttribute("用户备注","用户备注")]
		public String Remark { get; set; }



	}
}
