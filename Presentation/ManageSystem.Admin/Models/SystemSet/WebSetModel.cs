using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.SystemSet
{
    public class WebSetModel
    {
        /// <summary>
        /// 网站维护状态
        /// </summary>
        public bool WebState { get; set; }

        /// <summary>
        /// 网站维护说明
        /// </summary>
        public string WebMaintain { get; set; }

        /// <summary>
        /// 最新消息
        /// </summary>
        public string NewMessage { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string SystemEmail { get; set; }


        /// <summary>
        /// 版权信息
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// 流量统计代码
        /// </summary>
        public string FlowCount { get; set; }

        /// <summary>
        /// 页面头部公共代码
        /// </summary>
        public string HeadPublicJS { get; set; }

        /// <summary>
        /// 页面底部公共代码
        /// </summary>
        public string FootPublicJS { get; set; }

        /// <summary>
        /// 网站标题
        /// </summary>
        public string SEOTitle { get; set; }

        /// <summary>
        /// 网站关键词
        /// </summary>
        public string SEOKey { get; set; }

        /// <summary>
        /// 网站描述
        /// </summary>
        public string SEODescribe { get; set; }

        /// <summary>
        /// 发件人名称
        /// </summary>
        public string EmailUserName { get; set; }

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string EmailSMTP { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string EmailPassword { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public string EmailPort { get; set; }


        /// <summary>
        /// 通知邮箱
        /// </summary>
        public string NoticeEmail { get; set; }

        /// <summary>
        /// 通知手机
        /// </summary>
        public string NoticePhone { get; set; }


    }
}