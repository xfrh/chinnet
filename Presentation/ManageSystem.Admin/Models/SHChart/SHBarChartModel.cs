using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.SHChart;
using ManageSystem.Core.Domain.SHChart;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.SHChart
{
    [Validator(typeof(SHBarChartValidator))]
    public class SHBarChartModel: BaseEntityModel
    {
        /// <summary>
        /// 所属数据段
        /// </summary>
        [HtmlDisplay("所属数据段", "所属数据段")]
        public long DataSegmentId { get; set; }

        /// <summary>
        /// 数据段下拉数据
        /// </summary>
        public List<SelectListItem> DropDataSegment { get; set; } = new List<SelectListItem> { new SelectListItem { Text = "请选择", Value = "null" } };

        /// <summary>
        /// 报表名称
        /// </summary>
        [HtmlDisplay("报表名称", "报表名称")]
        public string Name { get; set; }

        /// <summary>
        /// 报表标题
        /// </summary>
        [HtmlDisplay("报表标题", "报表标题")]
        public string Title { get; set; }

        /// <summary>
        /// 报表副标题
        /// </summary>
        [HtmlDisplay("报表副标题", "报表副标题")]
        public string SubTitle { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [HtmlDisplay("排序编号", "排序编号")]
        public int Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [HtmlDisplay("状态", "状态")]
        public bool Display { get; set; }

        /// <summary>
        /// 【状态】下拉数据
        /// </summary>
        public List<SelectListItem> DropDisplay { get; } = new List<SelectListItem> { new SelectListItem { Text = "显示", Value = "True", Selected = true }, new SelectListItem { Text = "不显示", Value = "False" } };

        /// <summary>
        /// 数值单位
        /// </summary>
        [HtmlDisplay("数值单位", "数值单位")]
        public string Unit { get; set; } = "%";

        /// <summary>
        /// 移动端显示比例<br />
        /// 1-100%
        /// </summary>
        [HtmlDisplay("移动端显示比例", "移动端显示比例")]
        public double MobileDisplayScale { get; set; }

        /// <summary>
        /// 数据类型<br />
        /// 单数据/多数据
        /// </summary>
        [HtmlDisplay("数据类型", "数据类型")]
        public ChartSHDataItemType DataItemType { get; set; }

        /// <summary>
        /// 抗生素集合<br />
        /// 仅多数据展示时有效
        /// </summary>
        public List<SHBarChartDataItemWithAntibioticModel> Antibiotics { get; set; }

        /// <summary>
        /// 数据项
        /// </summary>
        public List<SHBarChartDataItemModel> ItemModel { get; set; } = new List<SHBarChartDataItemModel>();

        public string MICName { get; set; }
        public string MIC_Range { get; set; }
        public string MIC50 { get; set; }
        public string MIC90 { get; set; }
        public string S { get; set; }
        public string SDD { get; set; }
        public string I { get; set; }
        public string R { get; set; }
    }
}