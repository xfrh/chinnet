using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ManageSystem.Core.Domain.Articles;

namespace ManageSystem.Web.Models.Articles
{
	/// <summary>
	/// 模型类 ，数据库表名：Article 
	/// </summary>
	public partial class ArticleModel : BaseEntityModel
	{

		/// <summary>
		/// 文章名称
		/// <summary>
		[HtmlDisplayAttribute("文章名称","文章名称")]
		public String Name { get; set; }


		/// <summary>
		/// 文章内容
		/// <summary>
		[HtmlDisplayAttribute("文章内容", "文章内容")]
		public String Content { get; set; }

        /// <summary>
        /// 文章简短内容
        /// <summary>
        [HtmlDisplayAttribute("简单描述", "针对文章的简单描述，不包含HTML")]
        public String ShortContent { get; set; }


        /// <summary>
        /// 封面图片
        /// <summary>
        [HtmlDisplayAttribute("封面图片","封面图片")]
        public String CoverImage { get; set; }

        /// <summary>
        /// 文章作者
        /// <summary>
        [HtmlDisplayAttribute("文章作者","文章作者")]
		public String Author { get; set; }

		/// <summary>
		/// 发布时间
		/// <summary>
		[HtmlDisplayAttribute("发布时间","发布时间")]
		public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 文章状态
        /// <summary>
        [HtmlDisplayAttribute("文章状态", "文章状态")]
        public Int32 State { get; set; }


        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleState ArticleState
        {
            get
            {
                return (ArticleState)this.State;
            }
            set
            {
                this.State = (int)value;
            }
        }

        /// <summary>
        /// 文章状态枚举列表
        /// </summary>
        [HtmlDisplayAttribute("文章状态", "文章状态")]
        public IList<SelectListItem> StateList { get; set; }

        /// <summary>
        /// 文章分类id
        /// </summary>
         [HtmlDisplayAttribute("文章分类", "文章分类")]
        public long ArticleTypeId { get; set; }

        /// <summary>
        /// 文章分类列表
        /// </summary>
        [HtmlDisplayAttribute("文章分类", "文章分类")]
        public IList<SelectListItem> ArticleTypeList { get; set; }

        /// <summary>
        /// 查看次数
        /// </summary>
        [HtmlDisplayAttribute("查看次数", "查看总查看次数")]
        public int ViewCount { get; set; }

        /// <summary>
        /// 所属用户id
        /// </summary>
        [HtmlDisplayAttribute("所属用户", "所属用户id")]
        public long UserinfoId { get; set; }

        /// <summary>
        ///  二维码图片地址
        /// </summary>
        [HtmlDisplayAttribute("二维码图片", "二维码图片地址")]
        public string CodeImage { get; set; }


        /// <summary>
        /// 文章附件集合 
        /// </summary>
        [HtmlDisplayAttribute("文章附件", "文章附件")]
        public List<ArticleAttachmentModel> ArticleAttachmentList { get; set; }
    }
}
