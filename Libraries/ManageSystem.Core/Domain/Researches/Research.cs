using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Researches
{
	/// <summary>
	/// 实体类 ，数据库表名：Research 
	/// </summary>
	public partial class Research : BaseEntity
	{

		/// <summary>
		/// 研究名称
		/// <summary>
		public String Name { get; set; }
		/// <summary>
		/// 研究编码
		/// <summary>
		public String Code { get; set; }
        /// <summary>
        /// 所在区域的id，最后一级区
        /// <summary>
        public long AreaId { get; set; }

        /// <summary>
        /// 所在区域的省份Id，方便前台查询
        /// <summary>
        public long AreaProvinceId { get; set; }

        /// <summary>
        /// 所在区域的名称
        /// <summary>
        public String AreaName { get; set; }

        /// <summary>
        /// 所在区域的完整名称，省市区三级使用空格分割
        /// <summary>
        public String AreaFullName { get; set; }

        /// <summary>
        /// 科研时间
        /// <summary>
        public DateTime StartTime { get; set; }
		/// <summary>
		/// 参与要求
		/// <summary>
		public String Require { get; set; }
		/// <summary>
		/// 科研发起人
		/// <summary>
		public String Author { get; set; }
		/// <summary>
		/// 科研类型id
		/// <summary>
		public long ResearchTypeId { get; set; }
		/// <summary>
		/// 研究Logo
		/// <summary>
		public String CoverImage { get; set; }

		/// <summary>
		/// 研究简介
		/// <summary>
		public String Remark { get; set; }

        /// <summary>
		/// 研究详细
		/// <summary>
		public String Content { get; set; }

        /// <summary>
        /// 发布用户的id
        /// <summary>
        public long MemberId { get; set; }
		/// <summary>
		/// 发布用户的姓名
		/// <summary>
		public String MemberName { get; set; }
		/// <summary>
		/// 状态
		/// <summary>
		public Int32 Status { get; set; }

        /// <summary>
        ///报名数量，冗余字段，同时更新报名记录表
        /// </summary>
        public int ApplyCount { get; set; }

        /// <summary>
        /// 查看数量，冗余字段，同时更更新查看记录表
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 收藏数量，冗余字段，同时更更新收藏记录表
        /// </summary>
        public int CollectCount { get; set; }

        /// <summary>
        /// 评论数量，冗余字段，同时更更新评论记录表
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 综合评分，最大值为5分，向上取整
        /// </summary>
        public int Grade { get; set; }

        

    }
}
