using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Chart;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Chart
{
    [Validator(typeof(HeatmapValidator))]
    public class HeatmapModel : BaseEntityModel
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
        /// 图表类型<br />
        /// 暂时取值有且仅有 1: 地图<br />
        /// 其他值无效
        /// </summary>
        [HtmlDisplay("图表类型", "图表类型")]
        public byte ChartType { get; set; } = 1;

        /// <summary>
        /// 【图标类型】下拉数据
        /// </summary>
        public List<SelectListItem> DropChartType { get; } = new List<SelectListItem> { new SelectListItem { Text = "地图", Value = "1", Selected = true } };

        public List<HeatmapItemModel> ItemModel { get; set; } = new List<HeatmapItemModel>();

    }
}