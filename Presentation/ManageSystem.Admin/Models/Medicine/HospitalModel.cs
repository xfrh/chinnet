using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Medicine;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Medicine
{
	/// <summary>
	/// 模型类 ，数据库表名：Hospital 
	/// </summary>
	 [Validator(typeof(HospitalValidator))]
	public partial class HospitalModel : BaseEntityModel
	{

        public string IdString { get; set; }
        public string InsertTimestring { get; set; }

        /// <summary>
        /// 医院名称
        /// <summary>
        [HtmlDisplayAttribute("医院名称","医院名称")]
		public String Name { get; set; }

		/// <summary>
		/// 排序
		/// <summary>
		[HtmlDisplayAttribute("排序","排序")]
		public Int32 Sort { get; set; }

		/// <summary>
		/// 是否启用
		/// <summary>
		[HtmlDisplayAttribute("是否启用","是否启用")]
		public bool State { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("是否启用", "是否启用")]
        public string StateValue { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("是否启用", "是否启用")]
        public IList<SelectListItem> StateList { get; set; }

        /// <summary>
        /// 医院地址
        /// <summary>
        [HtmlDisplayAttribute("医院地址","医院地址")]
		public String Address { get; set; }

		/// <summary>
		/// 医院介绍
		/// <summary>
		[HtmlDisplayAttribute("医院介绍","医院介绍")]
		public String Content { get; set; }

		/// <summary>
		/// 联系人
		/// <summary>
		[HtmlDisplayAttribute("联系人","联系人")]
		public String ContactsUser { get; set; }

		/// <summary>
		/// 联系方式
		/// <summary>
		[HtmlDisplayAttribute("联系方式","联系方式")]
		public String ContactsTel { get; set; }

		/// <summary>
		/// 备注信息
		/// <summary>
		[HtmlDisplayAttribute("备注信息","备注信息")]
		public String Describe { get; set; }

        /// <summary>
        /// 医院管理员会员Id
        /// <summary>
        [HtmlDisplayAttribute("医院管理员", "医院管理员会员Id")]
        public long MemberId { get; set; }

        /// <summary>
        /// 医院管理员会员姓名
        /// <summary>
        [HtmlDisplayAttribute("医院管理员", "医院管理员会员姓名")]
        public string MemberName{ get; set; }

        /// <summary>
        /// 操作按钮
        /// </summary>
        public string ActionHtml { get; set; }

        /// <summary>
        /// 省份id
        /// <summary>
        [HtmlDisplayAttribute("所属省份", "所属省份")]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [HtmlDisplayAttribute("省份名称", "省份名称")]
        public String ProvinceName { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [HtmlDisplayAttribute("省份名称", "省份名称")]
        public IList<SelectListItem> ProvinceList { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        [HtmlDisplayAttribute("医院代码", "医院代码")]
        public String Code { get; set; }
    }

    /// <summary>
    /// 医院设置医院管理员的数据实体
    /// </summary>
    public class HostpitalSetAdminModel
    {
        [HtmlDisplayAttribute("医院Id", "医院Id")]
        public long Id { get; set; }

        [HtmlDisplayAttribute("医院名称", "医院名称")]
        public string HostpitalName { get; set; }

        [HtmlDisplayAttribute("医院管理员", "医院管理员Id")]
        public long MemberId { get; set; }

        [HtmlDisplayAttribute("管理员姓名", "医院管理员姓名")]
        public string MemberName { get; set; }

        [HtmlDisplayAttribute("医院管理员", "医院管理员Id")]
        public IList<SelectListItem> MmeberList { get; set; }
    }

    /// <summary>
    /// 模型类 ，数据库表名：ProjectHospital 
    /// </summary>
    public class ProjectHospitalModel: BaseEntityModel
    {
        private int _ProjectType = 0;
        private long _HospitalId = 0;
        private string _HospitalTitle = string.Empty;
        private long _MemberId = 0;
        private string _MemberLoginId = string.Empty;
        private string _MemberPassword = string.Empty;

        /// <summary>
        /// 多中心研究类型
        /// </summary>
        public int ProjectType { get { return _ProjectType; } set { _ProjectType = value; } }
        public long HospitalId { get { return _HospitalId; } set { _HospitalId = value; } }
        public string HospitalTitle { get { return _HospitalTitle; } set { _HospitalTitle = value; } }
        public long MemberId { get { return _MemberId; } set { _MemberId = value; } }
        public string MemberLoginId { get { return _MemberLoginId; } set { _MemberLoginId = value; } }
        public string MemberPassword { get { return _MemberPassword; } set { _MemberPassword = value; } }
    }
}
