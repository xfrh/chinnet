using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Researches;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Researches
{
	/// <summary>
	/// 模型类 ，数据库表名：Research 
	/// </summary>
	 [Validator(typeof(ResearchValidator))]
	public partial class ResearchModel : BaseEntityModel
	{

		/// <summary>
		/// 研究名称
		/// <summary>
		[HtmlDisplayAttribute("研究名称","研究名称")]
		public String Name { get; set; }

		/// <summary>
		/// 研究编码
		/// <summary>
		[HtmlDisplayAttribute("研究编码","研究编码")]
		public String Code { get; set; }

        /// <summary>
        /// 科研区域
        /// <summary>
        [HtmlDisplayAttribute("科研区域", "科研区域")]
        public long AreaId { get; set; }


        /// <summary>
        /// 所在区域的名称
        /// <summary>
        [HtmlDisplayAttribute("区域的名称", "区域的名称")]
        public String AreaName { get; set; }

        /// <summary>
        /// 所在区域的完整名称，省市区三级使用空格分割
        /// <summary>
        [HtmlDisplayAttribute("区域的完整名称", "区域的完整名称")]
        public String AreaFullName { get; set; }

        /// <summary>
        /// 科研时间
        /// <summary>
        [HtmlDisplayAttribute("科研时间","科研时间")]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// 参与要求
		/// <summary>
		[HtmlDisplayAttribute("参与要求","参与要求")]
		public String Require { get; set; }

		/// <summary>
		/// 科研发起人
		/// <summary>
		[HtmlDisplayAttribute("科研发起人","科研发起人")]
		public String Author { get; set; }

		/// <summary>
		/// 科研类型id
		/// <summary>
		[HtmlDisplayAttribute("科研类型id","科研类型id")]
		public long ResearchTypeId { get; set; }

        /// <summary>
        /// 科研类型名称
        /// <summary>
        public string ResearchTypeName { get; set; }


        /// <summary>
        /// 研究Logo图片地址
        /// <summary>
        [HtmlDisplayAttribute("研究Logo图片地址", "研究Logo图片地址")]
        public String CoverImage { get; set; }

        /// <summary>
        /// 所有科研类型
        /// </summary>
        public IList<SelectListItem> ResearchTypeList { get; set; }

        /// <summary>
        /// 研究Logo
        /// <summary>
        [HtmlDisplayAttribute("研究Logo","研究Logo")]
		public String LogoImage { get; set; }

        /// <summary>
        /// 研究详细
        /// <summary>
        [HtmlDisplayAttribute("详细内容", "详细内容")]
        public String Content { get; set; }

        /// <summary>
        /// 研究简介
        /// <summary>
        [HtmlDisplayAttribute("研究简介","研究简介")]
		public String Remark { get; set; }

		/// <summary>
		/// 发布用户的id
		/// <summary>
		[HtmlDisplayAttribute("发布用户的id","发布用户的id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 发布用户的姓名
		/// <summary>
		[HtmlDisplayAttribute("发布用户的姓名","发布用户的姓名")]
		public String MemberName { get; set; }

		/// <summary>
		/// 状态
		/// <summary>
		[HtmlDisplayAttribute("状态","状态")]
		public Int32 Status { get; set; }

        /// <summary>
        /// 会议状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        ///报名数量，冗余字段，同时更新报名记录表
        /// </summary>
        [HtmlDisplayAttribute("报名数量", "报名数量")]
        public int ApplyCount { get; set; }

        /// <summary>
        /// 查看数量，冗余字段，同时更更新查看记录表
        /// </summary>
        [HtmlDisplayAttribute("查看数量", "查看数量")]
        public int ViewCount { get; set; }

        /// <summary>
        /// 收藏数量，冗余字段，同时更更新收藏记录表
        /// </summary>
        [HtmlDisplayAttribute("收藏数量", "收藏数量")]
        public int CollectCount { get; set; }

        /// <summary>
        /// 评论数量，冗余字段，同时更更新评论记录表
        /// </summary>
        [HtmlDisplayAttribute("评论数量", "评论数量")]
        public int CommentCount { get; set; }

        /// <summary>
        /// 综合评分，最大值为5分，向上取整
        /// </summary>
        public int Grade { get; set; }
    }


}
