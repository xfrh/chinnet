using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Survey;
using System.Web.Mvc;
using ManageSystem.Framework;

namespace ManageSystem.Admin.Models.Survey
{
    /// <summary>
    ///问卷调查管理  问卷调查管理-问卷调查信息  模型类 ，数据库表名：Survey_Survey 
    /// </summary>
    [Validator(typeof(SurveySurveyValidator))]
    public partial class SurveySurveyModel : BaseEntityModel
    {

        /// <summary>
        /// 调查名称
        /// <summary>
        [HtmlDisplayAttribute("调查名称", "调查名称")]
        public String Name { get; set; }

        /// <summary>
        /// 问卷二维码（生成二维码）
        /// <summary>
        [HtmlDisplayAttribute("问卷二维码", "问卷二维码")]
        public String QR { get; set; }

        /// <summary>
        /// 开始时间
        /// <summary>
        [HtmlDisplayAttribute("开始时间", "开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// <summary>
        [HtmlDisplayAttribute("结束时间", "结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 开放权限（1、开放问卷和2、隐私问卷，开放的则不需要登录，隐私的需要登录） 
        /// <summary>
        [HtmlDisplayAttribute("开放权限", "开放权限")]
        public String Rood { get; set; }

        public IList<SelectListItem> RoodList { get; set; }

        /// <summary>
        /// 状态，1：启用  2：禁用
        /// <summary>
        [HtmlDisplayAttribute("状态", "状态")]
        public Int32 State { get; set; }

        /// <summary>
        /// 是否启用
        /// <summary>
        [HtmlDisplayAttribute("状态", "状态")]
        public IList<SelectListItem> StateList { get; set; }

        /// <summary>
        /// 排序编号，正序排列
        /// <summary>
        [HtmlDisplayAttribute("排序编号", "排序编号")]
        public Int32 Sort { get; set; }

        /// <summary>
        /// 说明信息
        /// <summary>
        [HtmlDisplayAttribute("说明信息", "说明信息")]
        public String Remark { get; set; }



    }

    /// <summary>
    /// 详情页面
    /// </summary>
    public partial class SurveyDeteilModel : BaseEntityModel
    {

        /// <summary>
        /// 所属调查Id
        /// <summary>
        public long SurveyId { get; set; }

        /// <summary>
        /// 问卷二维码（生成二维码）
        /// <summary>
        public String Content29 { get; set; }

        /// <summary>
        /// 问卷二维码（生成二维码）
        /// <summary>
        public String Content30 { get; set; }

        /// <summary>
        /// 问卷二维码（生成二维码）
        /// <summary>
        public String Content31 { get; set; }


    }
}
