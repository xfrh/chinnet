using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.MicdataDistribution;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.MicdataDistribution
{
    [Validator(typeof(ddDocumentValidator))]
    public partial class ddDocumentModel
    {
       
        /// <summary>
        /// 上报数据所属年度
        /// <summary>
        [HtmlDisplayAttribute("年度", "上报数据所属年度")]
        public Int32 year_id { get; set; }

        /// <summary>
        /// 年份下拉列表
        /// </summary>
        public IList<SelectListItem> YearList { get; set; }


        /// <summary>
        /// 主键id
        /// </summary>
        [HtmlDisplayAttribute("主键", "主键")]
        public string document_id { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        [HtmlDisplayAttribute("医院", "医院")]
        public int hospital_id { get; set; }
        /// <summary>
        /// 类别id
        /// </summary>
        [HtmlDisplayAttribute("类别", "类别")]
        public int category_id { get; set; }
        /// <summary>
        /// 细菌id
        /// </summary>
        [HtmlDisplayAttribute("细菌id", "细菌id")]
        public int germ_id { get; set; }
        /// <summary>
        /// 抗生素id
        /// </summary>
        [HtmlDisplayAttribute("抗生素id", "抗生素id")]
        public int antibiotic_id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [HtmlDisplayAttribute("文件名称", "文件名称")]
        public string title { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [HtmlDisplayAttribute("路径", "路径")]
        public string file_path { get; set; }
        /// <summary>
        /// 数据量
        /// </summary>
        [HtmlDisplayAttribute("数据量", "数据量")]
        public string datacount { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [HtmlDisplayAttribute("排序", "排序")]
        public int sortid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [HtmlDisplayAttribute("是否有效", "是否有效")]
        public int isvalid { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [HtmlDisplayAttribute("创建时间", "创建时间")]
        public DateTime created { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [HtmlDisplayAttribute("创建人", "创建人")]
        public long created_by { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        [HtmlDisplay("上传文件", "上传文件")]
        public HttpPostedFileBase PostFile { get; set; } = null;

        /// <summary>
        /// 上传文件
        /// </summary>
        [HtmlDisplay("表头", "表头")]
        public string header { get; set; }
    }
}