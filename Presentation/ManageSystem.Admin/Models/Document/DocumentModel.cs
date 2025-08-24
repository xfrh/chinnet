using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Document;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Document
{
    [Validator(typeof(DocumentValidator))]
    public partial class DocumentModel : BaseEntityModel
    {
        /// <summary>
        /// 资料标题
        /// </summary>
        [HtmlDisplay("资料标题", "资料标题")]
        public string Title { get; set; }

        /// <summary>
        /// 链接显示文本
        /// </summary>
        [HtmlDisplay("链接文本", "链接文本")]
        public string UrlName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [HtmlDisplay("排序", "排序")]
        public int Sort { get; set; } = 1;

        /// <summary>
        /// 是否启用
        /// </summary>
        [HtmlDisplay("是否启用", "是否启用")]
        public bool Status { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [HtmlDisplay("是否置顶", "是否置顶")]
        public bool weChattop { get; set; }

        /// <summary>
        /// 资料所属<br />
        /// 1:不限<br />
        /// 2:CHINET<br />
        /// 3:CRE<br />
        /// 4:CR替加环素
        /// </summary>
        [HtmlDisplay("资料所属", "资料所属")]
        public List<int> Owners { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [HtmlDisplay("链接地址", "链接地址")]
        public string Link { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        [HtmlDisplay("上传文件", "上传文件")]
        public HttpPostedFileBase PostFile { get; set; } = null;

        /// <summary>
        /// 所属卫星网
        /// </summary>
         [HtmlDisplay("所属卫星网", "所属卫星网")]
        public List<SelectListItem> BelongSate { get; set; }

        [HtmlDisplay("所属卫星网", "所属卫星网")]
        public long Satellite { get; set; }
    }
}