using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Weixin;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Weixin
{
    /// <summary>
    /// 模型类 ，数据库表名：WeixinUser 
    /// </summary>
    [Validator(typeof(WeixinUserValidator))]
    public partial class WeixinUserModel : BaseEntityModel
    {

        /// <summary>
        /// 用户昵称
        /// <summary>
        [HtmlDisplayAttribute("用户昵称", "用户昵称")]
        public String Name { get; set; }

        /// <summary>
        /// 用户性别
        /// <summary>
        [HtmlDisplayAttribute("用户性别", "用户性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 性别名称：值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [HtmlDisplayAttribute("用户性别", "用户性别")]
        public string SexName
        {
            get
            {
                if (Sex == 1) return "男性";
                if (Sex == 1) return "女性";
                return "未知";
            }
        }


        /// <summary>
        /// 用户的标识
        /// <summary>
        [HtmlDisplayAttribute("用户OpenId", "用户的标识")]
        public String Openid { get; set; }

        /// <summary>
        /// 关注状态 
        /// <summary>
        [HtmlDisplayAttribute("关注状态 ", "关注状态 ")]
        public bool Subscribe { get; set; }

        /// <summary>
        /// 关注状态名称，用于查询 
        /// <summary>
        [HtmlDisplayAttribute("关注状态 ", "关注状态 ")]
        public string SubscribeValue { get; set; }

        /// <summary>
        /// 关注状态
        /// </summary>
        [HtmlDisplayAttribute("关注状态 ", "关注状态 ")]
        public IList<SelectListItem> SubscribeList { get; set; }


        /// <summary>
        /// 关注时间
        /// <summary>
        [HtmlDisplayAttribute("关注时间", "关注时间")]
        public DateTime SubscribeTime { get; set; }

        /// <summary>
        /// 取消关注时间
        /// <summary>
        [HtmlDisplayAttribute("取消关注时间", "取消关注时间")]
        public DateTime UnsubscribeTime { get; set; }

        /// <summary>
        /// 所在区域
        /// <summary>
        [HtmlDisplayAttribute("所在区域", "所在区域")]
        public String Area { get; set; }

        /// <summary>
        /// 开始时间（用于列表页面的查询）
        /// <summary>
        [HtmlDisplayAttribute("开始时间", "关注时间的开始时间")]
        public string StartTime { get; set; }


        /// <summary>
        /// 结束时间（用于列表页面的查询）
        /// <summary>
        [HtmlDisplayAttribute("结束时间", "关注时间的结束时间")]
        public string EndTime { get; set; }

    }
}
