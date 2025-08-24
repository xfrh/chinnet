using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core;

namespace ManageSystem.Core.Domain.Survey
{
	/// <summary>
	///问卷调查管理  问卷调查管理-问卷调查信息  实体类 ，数据库表名：Survey_Survey 
	/// </summary>
	public partial class Survey_Survey : BaseEntity
    {

		/// <summary>
		/// 调查名称
		/// <summary>
		private String  name = ""; 
		/// <summary>
		/// 调查名称
		/// <summary>
		public String Name {set { name  = value; } get { return name ; } }
		
		/// <summary>
		/// 问卷二维码（生成二维码）
		/// <summary>
		private String  qR = ""; 
		/// <summary>
		/// 问卷二维码（生成二维码）
		/// <summary>
		public String QR {set { qR  = value; } get { return qR ; } }
		
		/// <summary>
		/// 开始时间
		/// <summary>
		private DateTime  startTime = DateTime.Parse("1900-01-01"); 
		/// <summary>
		/// 开始时间
		/// <summary>
		public DateTime StartTime {set { startTime  = value; } get { return startTime ; } }
		
		/// <summary>
		/// 结束时间
		/// <summary>
		private DateTime  endTime = DateTime.Parse("1900-01-01"); 
		/// <summary>
		/// 结束时间
		/// <summary>
		public DateTime EndTime {set { endTime  = value; } get { return endTime ; } }
		
		/// <summary>
		/// 开放权限（1、开放问卷和2、隐私问卷，开放的则不需要登录，隐私的需要登录） 
		/// <summary>
		private long rood = 0L; 
		/// <summary>
		/// 开放权限（1、开放问卷和2、隐私问卷，开放的则不需要登录，隐私的需要登录） 
		/// <summary>
		public long Rood {set { rood  = value; } get { return rood ; } }
		
		/// <summary>
		/// 状态，1：启用  2：禁用
		/// <summary>
		private Int32  state = 0; 
		/// <summary>
		/// 状态，1：启用  2：禁用
		/// <summary>
		public Int32 State {set { state  = value; } get { return state ; } }
		
		/// <summary>
		/// 排序编号，正序排列
		/// <summary>
		private Int32  sort = 0; 
		/// <summary>
		/// 排序编号，正序排列
		/// <summary>
		public Int32 Sort {set { sort  = value; } get { return sort ; } }
		
		/// <summary>
		/// 说明信息
		/// <summary>
		private String  remark = ""; 
		/// <summary>
		/// 说明信息
		/// <summary>
		public String Remark {set { remark  = value; } get { return remark ; } }
		


	}
}
