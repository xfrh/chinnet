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
    /// 模型类 ，数据库表名：ResearchComment 
    /// </summary>
    [Validator(typeof(ResearchCommentValidator))]
	public partial class ResearchCommentModel : BaseEntityModel
	{

        public string IdString { get { return Id.ToString(); }  }
        /// <summary>
        /// 评论的时间，字符串
        /// </summary>
        public string InsertTimeString { get; set; }

        /// <summary>
        /// 用户的id
        /// <summary>
        [HtmlDisplayAttribute("用户的id","用户的id")]
		public long MemberId { get; set; }

		/// <summary>
		/// 用户的姓名
		/// <summary>
		[HtmlDisplayAttribute("用户的姓名","用户的姓名")]
		public String MemberName { get; set; }

        /// <summary>
        /// 用户的昵称
        /// <summary>
        [HtmlDisplayAttribute("用户的姓名", "用户的姓名")]
        public String MemberNickName { get; set; }

        /// <summary>
        /// 用户头像图片地址
        /// </summary>
        public string MemberHeadImage { get; set; }


        /// <summary>
        /// 科研合作的id
        /// <summary>
        [HtmlDisplayAttribute("科研合作议的id", "科研合作的id")]
		public long ResearchId { get; set; }

        /// <summary>
        ///科研合作的名称
        /// <summary>
        [HtmlDisplayAttribute("科研合作的名称", "科研合作的名称")]
		public String ResearchName { get; set; }

        /// <summary>
        /// Ip地址
        /// <summary>
        [HtmlDisplayAttribute("Ip地址","Ip地址")]
		public String Ip { get; set; }

		/// <summary>
		/// 操作者浏览器名称
		/// <summary>
		[HtmlDisplayAttribute("操作者浏览器名称","操作者浏览器名称")]
		public String BrowserName { get; set; }

		/// <summary>
		/// 评论内容
		/// <summary>
		[HtmlDisplayAttribute("评论内容","评论内容")]
		public String Content { get; set; }

		/// <summary>
		/// 上级评论
		/// <summary>
		[HtmlDisplayAttribute("上级评论","上级评论")]
		public long ParentId { get; set; }


        /// <summary>
        /// 该评论所对应的下级评论集合
        /// </summary>
        public List<ResearchCommentModel> ItemCommentList { get; set; }


        /// <summary>
        /// 当前评论所处的深度，0表示顶级评论，1表示1级，以此内推
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 该评论的下级评论数量，只记录下一级的。多级不处理
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 评论类型，1：普通会员   2：会议主办方  3：后台管理员
        /// </summary>
        public int Type { get; set; }

    }


    /// <summary>
    /// 会员中心 科研合作管理  科研的评论记录
    /// </summary>
    [Serializable]
    public class ResearchDetailCommentModel
    {
     
        /// <summary>
        /// 科研合作数据
        /// </summary>
        public ResearchModel Research { get; set; }



    }
}
