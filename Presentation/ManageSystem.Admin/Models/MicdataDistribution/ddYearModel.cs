using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.MicdataDistribution;
using ManageSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.MicdataDistribution
{
    [Validator(typeof(ddYearValidator))]
    public partial class ddYearModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
       [HtmlDisplayAttribute("主键id", "主键id")]
        public long year_id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
      [HtmlDisplayAttribute("年份", "年份")]
        public string title { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [HtmlDisplayAttribute("排序", "排序")]
        public int sortid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [HtmlDisplayAttribute("是否启用", "是否启用")]
        public bool isvalid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [HtmlDisplayAttribute("创建时间", "创建时间")]
        public DateTime created { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [HtmlDisplayAttribute("创建人", "创建人")]
        public long created_by { get; set; }
    }
}