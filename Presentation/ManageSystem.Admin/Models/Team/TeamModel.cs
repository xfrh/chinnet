using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Team;
using ManageSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Team
{
    [Validator(typeof(TeamValidator))]
    public partial class TeamModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [HtmlDisplayAttribute("主键id", "主键id")]
        public long Id { get; set; }
        /// <summary>
        /// 关联医院id
        /// </summary>
        [HtmlDisplayAttribute("关联医院", "关联医院")]
        public long Hospitalid { get; set; }
        /// <summary>
        /// 医院显示名称
        /// </summary>
        [HtmlDisplayAttribute("显示医院名称", "显示医院名称")]
        public String Title { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [HtmlDisplayAttribute("负责人", "负责人")]
        public String People { get; set; }
        /// <summary>
        /// 简介图片
        /// </summary>
        [HtmlDisplayAttribute("简介图片", "简介图片")]
        public String Images { get; set; }
        /// <summary>
        /// 是否显示简介
        /// </summary>
        [HtmlDisplayAttribute("是否显示简介", "是否显示简介")]
        public bool Displayimages { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        [HtmlDisplayAttribute("所属分类", "所属分类")]
        public long classifyId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [HtmlDisplayAttribute("添加时间", "添加时间")]
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        [HtmlDisplayAttribute("创建人id", "创建人id")]
        public long Createby_Id { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [HtmlDisplayAttribute("修改时间", "修改时间")]
        public DateTime UpdateTime { get; set; }


        /// <summary>
        /// 分类下拉列表
        /// </summary>
        [HtmlDisplayAttribute("分类下拉列表", "医院下拉列表")]
        public IList<SelectListItem> classifyList { get; set; }

        /// <summary>
        /// 省份id
        /// <summary>
        [HtmlDisplayAttribute("所属省份", "所属省份")]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [HtmlDisplayAttribute("省份名称", "省份名称")]
        public String ProvinceName { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [HtmlDisplayAttribute("省份名称", "省份名称")]
        public IList<SelectListItem> ProvinceList { get; set; }

        /// <summary>
        ///  医院下拉列表
        /// </summary>
        [HtmlDisplayAttribute("医院下拉列表", "医院下拉列表")]
        public IList<SelectListItem> hospitalList { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// </summary>
        [HtmlDisplay("上传文件", "上传文件")]
        public HttpPostedFileBase PostFile { get; set; } = null;

        [HtmlDisplay("项目代码", "项目代码")]
        public string project_code { get; set; }
    }
}