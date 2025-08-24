using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Chart;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Extensions;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Chart
{
    [Validator(typeof(TrendChartValidator))]
    public class TrendChartModel : BaseEntityModel
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
        public ChartDataItemType DataItemType { get; set; }

        /// <summary>
        /// 图表类型<br />
        /// 仅单数据时有效
        /// </summary>
        public ChartTypeEnum ChartType { get; set; } = ChartTypeEnum.Bar;

        /// <summary>
        /// 图表类型下拉选择<br />
        /// 仅单数据时有效
        /// </summary>
        public List<SelectListItem> DropChartType
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = ChartTypeEnum.Bar.GetDescription(), Value = ((byte)ChartTypeEnum.Bar).ToString() },
                    new SelectListItem { Text = ChartTypeEnum.Line.GetDescription(), Value = ((byte)ChartTypeEnum.Line).ToString() },
                };
            }
        }

        /// <summary>
        /// 图表颜色<br />
        /// 仅单数据时该值有效
        /// </summary>
        [HtmlDisplay("图表颜色", "图表颜色")]
        public string ChartColor { get; set; }

        /// <summary>
        /// 抗生素集合<br />
        /// 仅多数据展示时有效
        /// </summary>
        public List<TrendChartDataItemWithAntibioticModel> Antibiotics { get; set; }

        /// <summary>
        /// 数据项
        /// </summary>
        public List<TrendChartDataItemModel> ItemModel { get; set; } = new List<TrendChartDataItemModel>();
    }
}