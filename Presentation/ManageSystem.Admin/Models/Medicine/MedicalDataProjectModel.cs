using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.Medicine;
using ManageSystem.Core;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.Medicine
{
    [Validator(typeof(MedicalDataProjectValidator))]
    public class MedicalDataProjectModel : BaseEntityModel
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        [HtmlDisplayAttribute("类型名称", "类型名称")]
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [HtmlDisplayAttribute("序号", "序号")]
        public int Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [HtmlDisplayAttribute("备注", "备注")]
        public string Remark { get; set; }


        /// <summary>
        /// 上传目录
        /// </summary>
        [HtmlDisplayAttribute("上传目录", "上传目录")]
        public string Path { get; set; }
    }
}