using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Meetings;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Meetings
{
    /// <summary>
    /// 模型类 ，数据库表名：Meeting 
    /// </summary>
    [Validator(typeof(MeetingValidator))]
    public partial class MeetingModel : BaseEntityModel
    {

        /// <summary>
        /// 会议主题
        /// <summary>
        [HtmlDisplayAttribute("主题", "主题", true)]
        public String Name { get; set; }

        /// <summary>
        /// 会议海报图片地址
        /// <summary>
        [HtmlDisplayAttribute("海报图片", "海报图片")]
        public String CoverImage { get; set; }

        /// <summary>
        /// 会议开始时间
        /// <summary>
        [HtmlDisplayAttribute("会议开始时间", "会议开始时间", false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// <summary>
        [HtmlDisplayAttribute("会议结束时间", "会议结束时间", true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 所在城市
        /// <summary>
        [HtmlDisplayAttribute("所在城市", "所在城市")]
        public long CityId { get; set; }

        /// <summary>
        /// 详细地址
        /// <summary>
        [HtmlDisplayAttribute("详细地址", "详细地址", false)]
        public String Address { get; set; }

        /// <summary>
        /// 会议类型id
        /// <summary>
        [HtmlDisplayAttribute("类型", "类型", false)]
        public long MeetingTypeId { get; set; }


        /// <summary>
        /// 所有会议类型
        /// </summary>
        public IList<SelectListItem> MeetingTypeList { get; set; }


        /// <summary>
        /// 会议详细
        /// <summary>
        [HtmlDisplayAttribute("会议详细", "会议详细")]
        public String Content { get; set; }

        /// <summary>
        /// 活动类型，1：收费活动  2：免费活动
        /// <summary>
        [HtmlDisplayAttribute("活动类型", "活动类型", false)]
        public Int32 Type { get; set; }


        /// 收费金额，如果是收费活动
        /// <summary>
        [HtmlDisplayAttribute("收费金额", "收费金额", false)]
        public decimal? Price { get; set; }

        /// <summary>
        /// 帐号状态枚举列表
        /// </summary>
        [HtmlDisplayAttribute("活动类型", "活动类型")]
        public IList<SelectListItem> TypeEnumList { get; set; }

        /// <summary>
        /// 人数上线，0表示不限制
        /// <summary>
        [HtmlDisplayAttribute("人数上线", "人数上线，0表示不限制", false)]
        public long PersonMaxCount { get; set; }

        /// <summary>
        /// 联系方式
        /// <summary>
        [HtmlDisplayAttribute("联系方式", "联系方式", false)]
        public String Contact { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        [HtmlDisplay("发布人", "发布人")]
        public string Author { get; set; } = "CHINET数据云";
        /// <summary>
        /// 发布用户的id
        /// <summary>
        [HtmlDisplayAttribute("发布用户的id", "发布用户的id")]
        public long MemberId { get; set; }

        /// <summary>
        /// 发布用户的姓名
        /// <summary>
        [HtmlDisplayAttribute("发布用户的姓名", "发布用户的姓名")]
        public String MemberName { get; set; }

        /// <summary>
        ///报名数量，冗余字段，同时更新报名记录表
        /// </summary>
        [HtmlDisplayAttribute("报名数量", "报名数量")]
        public int ApplyCount { get; set; }

        /// <summary>
        /// 查看数量，冗余字段，同时更更新查看记录表
        /// </summary>
        [HtmlDisplayAttribute("查看数量", "查看数量")]
        public int ViewCount { get; set; }

        /// <summary>
        /// 收藏数量，冗余字段，同时更更新收藏记录表
        /// </summary>
        [HtmlDisplayAttribute("收藏数量", "收藏数量")]
        public int CollectCount { get; set; }

        /// <summary>
        /// 评论数量，冗余字段，同时更更新评论记录表
        /// </summary>
        [HtmlDisplayAttribute("评论数量", "评论数量")]
        public int CommentCount { get; set; }

        /// <summary>
        /// 所在区域的完整名称，省市区三级使用空格分割
        /// <summary>
        [HtmlDisplayAttribute("区域的完整名称", "区域的完整名称")]
        public String AreaFullName { get; set; }

        /// <summary>
        /// 所在区域的名称
        /// <summary>
        [HtmlDisplayAttribute("区域的名称", "区域的名称")]
        public String AreaName { get; set; }



    }
}
