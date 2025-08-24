using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core;

namespace ManageSystem.Core.Domain.Survey
{
	/// <summary>
	///问卷调查管理  问卷调查管理-问卷题目选项  实体类 ，数据库表名：Survey_subjectOption 
	/// </summary>
	public partial class Survey_SubjectOption : BaseEntity
    {

		/// <summary>
		/// 选项
		/// <summary>
		private String  name = ""; 
		/// <summary>
		/// 选项
		/// <summary>
		public String Name {set { name  = value; } get { return name ; } }
		
		/// <summary>
		/// 父级
		/// <summary>
		private String  parentId = ""; 
		/// <summary>
		/// 父级
		/// <summary>
		public String ParentId {set { parentId  = value; } get { return parentId ; } }
		
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
