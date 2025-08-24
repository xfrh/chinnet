using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.MicdataDistribution;
using ManageSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.MicdataDistribution
{
    [Validator(typeof(ddGermValidator))]
    public partial class ddGermModel
    {
        /// <summary>
        /// 主键id
        /// <summary>
        [HtmlDisplayAttribute("主键id", "主键id")]
        public long germ_id { get; set; }
        /// <summary>
        /// 类别id
        /// <summary>
        [HtmlDisplayAttribute("类别id", "类别id")]
        public long group_id { get; set; }     
        /// <summary>
        /// 是否有数据值
        /// <summary>
        [HtmlDisplayAttribute("是否前段显示", "是否前段显示")]
        public bool mark { get; set; }
        /// <summary>
        /// 细菌名称
        /// <summary>
        [HtmlDisplayAttribute("细菌名称", "细菌名称")]
        public String title { get; set; }
        /// <summary>
        /// 英文名称
        /// <summary>
        [HtmlDisplayAttribute("英文名称", "英文名称")]
        public String title_en { get; set; }
        /// <summary>
        /// 细菌Code
        /// <summary>
        [HtmlDisplayAttribute("细菌Code", "细菌Code")]
        public String code { get; set; }
        /// <summary>
        /// 排序
        /// <summary>
        [HtmlDisplayAttribute("排序", "排序")]
        public int sortid { get; set; }
        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("是否启用", "是否启用")]
        public bool isvalid { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("是否默认显示", "是否默认显示")]
        public bool isdefault { get; set; }
        /// <summary>
        /// 添加时间
        /// <summary>
        [HtmlDisplayAttribute("添加时间", "添加时间")]
        public DateTime created { get; set; }

        /// <summary>
        /// 添加时间
        /// <summary>
        [HtmlDisplayAttribute("创建人", "创建人")]
        public long created_by { get; set; }    
    }
}