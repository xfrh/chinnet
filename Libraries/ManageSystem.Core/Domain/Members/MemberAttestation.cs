using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Members
{
	/// <summary>
	/// 实体类 ，数据库表名：MemberAttestation 
	/// </summary>
	public partial class MemberAttestation : BaseEntity
	{
        /// <summary>
        /// 所属会员
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
		/// 会员姓名
		/// <summary>
		public String MemberName { get; set; }

        /// <summary>
        /// 所属城市
        /// <summary>
        public long AreaId { get; set; }
		/// <summary>
		/// 城市名称
		/// <summary>
		public String AreaName { get; set; }
		/// <summary>
		/// 所属医院
		/// <summary>
		public long HospitalId { get; set; }
		/// <summary>
		/// 医院名称
		/// <summary>
		public String HospitalName { get; set; }
		/// <summary>
		/// 所属科室
		/// <summary>
		public long HospitalDepartmentId { get; set; }
		/// <summary>
		/// 医院科室名称
		/// <summary>
		public String HospitalDepartmentName { get; set; }
		/// <summary>
		/// 医生职称
		/// <summary>
		public long DoctorTitleId { get; set; }
		/// <summary>
		/// 医生职称名称
		/// <summary>
		public String DoctorTitleName { get; set; }
		/// <summary>
		/// 审核状态  1：待审核   2：通过审核   3：取消
		/// <summary>
		public Int32 Status { get; set; }


	}
}
