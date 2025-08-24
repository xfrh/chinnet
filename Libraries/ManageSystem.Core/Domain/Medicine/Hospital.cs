using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web.Hosting;
using NPOI.HSSF.Record;


namespace ManageSystem.Core.Domain.Medicine
{
	/// <summary>
	/// 实体类 ，数据库表名：Hospital 
	/// </summary>
	public partial class Hospital : BaseEntity
	{
        private string _Name = string.Empty;
        private int _Sort = 99;
        private bool _State = true;
        private string _Address = string.Empty;
        private string _Content = string.Empty;
        private string _ContactsUser = string.Empty;
        private string _ContactsTel = string.Empty;
        private long _MemberId = 0;
        private string _Code = string.Empty;
        private long _ProvinceId = 0;
        private string _ProvinceName = string.Empty;
        private bool _IsTeam = false;

        /// <summary>
        /// 医院名称
        /// <summary>
        public String Name { get{ return _Name; } set{ _Name = value; } }
		/// <summary>
		/// 排序
		/// <summary>
		public Int32 Sort { get{ return _Sort; } set{ _Sort = value; } }
		/// <summary>
		/// 是否启用
		/// <summary>
		public bool State { get{ return _State; } set{ _State = value; } }
		/// <summary>
		/// 医院地址
		/// <summary>
		public String Address { get{ return _Address; } set{ _Address = value; } }
		/// <summary>
		/// 医院介绍
		/// <summary>
		public String Content { get{ return _Content; } set{ _Content = value; } }
		/// <summary>
		/// 联系人
		/// <summary>
		public String ContactsUser { get{ return _ContactsUser; } set{ _ContactsUser = value; } }
		/// <summary>
		/// 联系方式
		/// <summary>
		public String ContactsTel { get{ return _ContactsTel; } set{ _ContactsTel = value; } }

        /// <summary>
        /// 医院管理员会员Id
        /// <summary>
        public long MemberId { get{ return _MemberId; } set{ _MemberId = value; } }

        /// <summary>
        /// 医院编码，为卫生部统一分配的标准代码，标准6字符
        /// </summary>
        public String Code { get{ return _Code; } set{ _Code = value; } }

        /// <summary>
        /// 省份id
        /// <summary>
        public long ProvinceId { get{ return _ProvinceId; } set{ _ProvinceId = value; } }

        /// <summary>
        /// 省份名称
        /// </summary>
        public String ProvinceName { get{ return _ProvinceName; } set{ _ProvinceName = value; } }

        /// <summary>
        ///是否是成员单位
        /// </summary>
        public bool IsTeam { get{ return _IsTeam; } set{ _IsTeam = value; } }

    }

    /// <summary>
    /// 实体类 ，多中心研究/医院，表名：ProjectHospital
    /// </summary>
    public partial class ProjectHospital: BaseEntity
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
        public string MemberPassword{ get { return _MemberPassword; } set { _MemberPassword = value; } }
    }

}
