using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core;

namespace ManageSystem.Core.Domain.Survey
{
	/// <summary>
	///问卷调查管理  问卷调查管理-问卷记录  实体类 ，数据库表名：Survey_Record 
	/// </summary>
	public partial class Survey_Record : BaseEntity
    {

		/// <summary>
		/// 调查ID  关联Survey_Survey表
		/// <summary>
		private long  surveyId = 0L; 
		/// <summary>
		/// 调查ID  关联Survey_Survey表
		/// <summary>
		public long SurveyId {set { surveyId  = value; } get { return surveyId ; } }
		
		/// <summary>
		/// 回答1
		/// <summary>
		private String  content1 = ""; 
		/// <summary>
		/// 回答1
		/// <summary>
		public String Content1 {set { content1  = value; } get { return content1 ; } }
		
		/// <summary>
		/// 回答2
		/// <summary>
		private String  content2 = ""; 
		/// <summary>
		/// 回答2
		/// <summary>
		public String Content2 {set { content2  = value; } get { return content2 ; } }
		
		/// <summary>
		/// 回答3
		/// <summary>
		private String  content3 = ""; 
		/// <summary>
		/// 回答3
		/// <summary>
		public String Content3 {set { content3  = value; } get { return content3 ; } }
		
		/// <summary>
		/// 回答4
		/// <summary>
		private String  content4 = ""; 
		/// <summary>
		/// 回答4
		/// <summary>
		public String Content4 {set { content4  = value; } get { return content4 ; } }
		
		/// <summary>
		/// 回答5
		/// <summary>
		private String  content5 = ""; 
		/// <summary>
		/// 回答5
		/// <summary>
		public String Content5 {set { content5  = value; } get { return content5 ; } }
		
		/// <summary>
		/// 回答6
		/// <summary>
		private String  content6 = ""; 
		/// <summary>
		/// 回答6
		/// <summary>
		public String Content6 {set { content6  = value; } get { return content6 ; } }
		
		/// <summary>
		/// 回答7
		/// <summary>
		private String  content7 = ""; 
		/// <summary>
		/// 回答7
		/// <summary>
		public String Content7 {set { content7  = value; } get { return content7 ; } }
		
		/// <summary>
		/// 回答8
		/// <summary>
		private String  content8 = ""; 
		/// <summary>
		/// 回答8
		/// <summary>
		public String Content8 {set { content8  = value; } get { return content8 ; } }
		
		/// <summary>
		/// 回答9
		/// <summary>
		private String  content9 = ""; 
		/// <summary>
		/// 回答9
		/// <summary>
		public String Content9 {set { content9  = value; } get { return content9 ; } }
		
		/// <summary>
		/// 回答10
		/// <summary>
		private String  content10 = ""; 
		/// <summary>
		/// 回答10
		/// <summary>
		public String Content10 {set { content10  = value; } get { return content10 ; } }
		
		/// <summary>
		/// 回答11
		/// <summary>
		private String  content11 = ""; 
		/// <summary>
		/// 回答11
		/// <summary>
		public String Content11 {set { content11  = value; } get { return content11 ; } }
		
		/// <summary>
		/// 回答12
		/// <summary>
		private String  content12 = ""; 
		/// <summary>
		/// 回答12
		/// <summary>
		public String Content12 {set { content12  = value; } get { return content12 ; } }
		
		/// <summary>
		/// 回答13
		/// <summary>
		private String  content13 = ""; 
		/// <summary>
		/// 回答13
		/// <summary>
		public String Content13 {set { content13  = value; } get { return content13 ; } }
		
		/// <summary>
		/// 回答14
		/// <summary>
		private String  content14 = ""; 
		/// <summary>
		/// 回答14
		/// <summary>
		public String Content14 {set { content14  = value; } get { return content14 ; } }
		
		/// <summary>
		/// 回答15
		/// <summary>
		private String  content15 = ""; 
		/// <summary>
		/// 回答15
		/// <summary>
		public String Content15 {set { content15  = value; } get { return content15 ; } }
		
		/// <summary>
		/// 回答16
		/// <summary>
		private String  content16 = ""; 
		/// <summary>
		/// 回答16
		/// <summary>
		public String Content16 {set { content16  = value; } get { return content16 ; } }
		
		/// <summary>
		/// 回答17
		/// <summary>
		private String  content17 = ""; 
		/// <summary>
		/// 回答17
		/// <summary>
		public String Content17 {set { content17  = value; } get { return content17 ; } }
		
		/// <summary>
		/// 回答18
		/// <summary>
		private String  content18 = ""; 
		/// <summary>
		/// 回答18
		/// <summary>
		public String Content18 {set { content18  = value; } get { return content18 ; } }
		
		/// <summary>
		/// 回答19
		/// <summary>
		private String  content19 = ""; 
		/// <summary>
		/// 回答19
		/// <summary>
		public String Content19 {set { content19  = value; } get { return content19 ; } }
		
		/// <summary>
		/// 回答20
		/// <summary>
		private String  content20 = ""; 
		/// <summary>
		/// 回答20
		/// <summary>
		public String Content20 {set { content20  = value; } get { return content20 ; } }
		
		/// <summary>
		/// 回答21
		/// <summary>
		private String  content21 = ""; 
		/// <summary>
		/// 回答21
		/// <summary>
		public String Content21 {set { content21  = value; } get { return content21 ; } }
		
		/// <summary>
		/// 回答22
		/// <summary>
		private String  content22 = ""; 
		/// <summary>
		/// 回答22
		/// <summary>
		public String Content22 {set { content22  = value; } get { return content22 ; } }
		
		/// <summary>
		/// 回答23
		/// <summary>
		private String  content23 = ""; 
		/// <summary>
		/// 回答23
		/// <summary>
		public String Content23 {set { content23  = value; } get { return content23 ; } }
		
		/// <summary>
		/// 回答24
		/// <summary>
		private String  content24 = ""; 
		/// <summary>
		/// 回答24
		/// <summary>
		public String Content24 {set { content24  = value; } get { return content24 ; } }
		
		/// <summary>
		/// 回答25
		/// <summary>
		private String  content25 = ""; 
		/// <summary>
		/// 回答25
		/// <summary>
		public String Content25 {set { content25  = value; } get { return content25 ; } }
		
		/// <summary>
		/// 回答26
		/// <summary>
		private String  content26 = ""; 
		/// <summary>
		/// 回答26
		/// <summary>
		public String Content26 {set { content26  = value; } get { return content26 ; } }
		
		/// <summary>
		/// 回答27
		/// <summary>
		private String  content27 = ""; 
		/// <summary>
		/// 回答27
		/// <summary>
		public String Content27 {set { content27  = value; } get { return content27 ; } }
		
		/// <summary>
		/// 回答28
		/// <summary>
		private String  content28 = ""; 
		/// <summary>
		/// 回答28
		/// <summary>
		public String Content28 {set { content28  = value; } get { return content28 ; } }
		
		/// <summary>
		/// 回答29
		/// <summary>
		private String  content29 = ""; 
		/// <summary>
		/// 回答29
		/// <summary>
		public String Content29 {set { content29  = value; } get { return content29 ; } }
		
		/// <summary>
		/// 回答30
		/// <summary>
		private String  content30 = ""; 
		/// <summary>
		/// 回答30
		/// <summary>
		public String Content30 {set { content30  = value; } get { return content30 ; } }
		
		/// <summary>
		/// 回答31
		/// <summary>
		private String  content31 = ""; 
		/// <summary>
		/// 回答31
		/// <summary>
		public String Content31 {set { content31  = value; } get { return content31 ; } }
		
		/// <summary>
		/// 用户Id
		/// <summary>
		private long memberId = 0L; 
		/// <summary>
		/// 用户Id
		/// <summary>
		public long MemberId {set { memberId  = value; } get { return memberId ; } }
		
		/// <summary>
		/// 用户名称
		/// <summary>
		private String  memberName = ""; 
		/// <summary>
		/// 用户名称
		/// <summary>
		public String MemberName {set { memberName  = value; } get { return memberName ; } }

        /// <summary>
        /// 用户手机
        /// <summary>
        private String memberPhone = "";
        /// <summary>
        /// 用户手机
        /// <summary>
        public String MemberPhone { set { memberPhone = value; } get { return memberPhone; } }

        /// <summary>
        /// 权限（1、开放问卷；2、隐私问卷）
        /// <summary>
        private long rood = 0L; 
		/// <summary>
		/// 权限（1、开放问卷；2、隐私问卷）
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
