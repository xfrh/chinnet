using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Chart;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Chart
{
    /// <summary>
    /// 
    /// </summary>
    [Validator(typeof(DataSegmentValidator))]
    public partial class DataSegmentModel : BaseEntityModel
    {
        /// <summary>
        /// 数据段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }
        /// <summary>
        /// 数据段所属项目
        /// </summary>
        public DataSegmentEnum ProjectType { get; set; }
    }
}