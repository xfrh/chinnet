using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Researches;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Researches
{
    /// <summary>
    /// 模型类 ，数据库表名：ResearchApply 
    /// </summary>
    [Validator(typeof(ResearchApplyValidator))]
    public partial class ResearchApplyModel : BaseEntityModel
    {

        /// <summary>
        /// 报名用户的id
        /// <summary>
        [HtmlDisplayAttribute("会员id", "会员id")]
        public long MemberId { get; set; }

        /// <summary>
        /// 报名用户的姓名
        /// <summary>
        [HtmlDisplayAttribute("会员姓名", "会员姓名")]
        public String MemberName { get; set; }

        /// <summary>
        /// 科研合作的id
        /// <summary>
        [HtmlDisplayAttribute("科研合作的id", "科研合作的id")]
        public long ResearchId { get; set; }

        /// <summary>
        /// 科研合作的名称
        /// <summary>
        [HtmlDisplayAttribute("科研合作的名称", "科研合作的名称")]
        public String ResearchName { get; set; }

        /// <summary>
        /// 科研合作类型
        /// <summary>
        [HtmlDisplayAttribute("科研合作类型", "科研合作类型")]
        public String ResearchTypeName { get; set; }

        /// <summary>
        /// 报名姓名
        /// <summary>
        [HtmlDisplayAttribute("报名人姓名", "报名人姓名", true)]
        public String Name { get; set; }

        /// <summary>
        /// 邮箱地址
        /// <summary>
        [HtmlDisplayAttribute("邮箱地址", "邮箱地址", true)]
        public String Email { get; set; }

        /// <summary>
        /// 手机号码
        /// <summary>
        [HtmlDisplayAttribute("手机号码", "手机号码", true)]
        public String Phone { get; set; }

        /// <summary>
        /// 用户备注
        /// <summary>
        [HtmlDisplayAttribute("用户备注", "用户备注")]
        public String Remark { get; set; }

        /// <summary>
        /// 报名人数
        /// <summary>
        [HtmlDisplayAttribute("报名人数", "报名人数")]
        public Int32 Count { get; set; }

        /// <summary>
        /// 申请状态
        /// <summary>
        [HtmlDisplayAttribute("申请状态", "申请状态")]
        public Int32 Status { get; set; }


        /// <summary>
        /// 申请状态
        /// </summary>
        [HtmlDisplayAttribute("申请状态", "申请状态")]
        public IList<SelectListItem> ResearchApplyStatusList { get; set; }

        /// <summary>
        /// 申请状态名称
        /// <summary>
        [HtmlDisplayAttribute("申请状态", "申请状态")]
        public Int32 StatusName { get; set; }

        /// <summary>
        /// 状态变更时间
        /// <summary>
        [HtmlDisplayAttribute("状态变更时间", "状态变更时间")]
        public DateTime StatusTime { get; set; }

    }


}
