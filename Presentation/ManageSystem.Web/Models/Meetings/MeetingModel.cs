using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Meetings;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Meetings
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
        [HtmlDisplayAttribute("会议主题", "会议主题", true)]
        public String Name { get; set; }

        /// <summary>
        /// 会议海报图片地址
        /// <summary>
        [HtmlDisplayAttribute("会议海报图片地址", "会议海报图片地址")]
        public String CoverImage { get; set; }

        /// <summary>
        /// 会议开始时间
        /// <summary>
        [HtmlDisplayAttribute("会议开始时间", "会议开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// <summary>
        [HtmlDisplayAttribute("会议结束时间", "会议结束时间")]
        public DateTime EndTime { get; set; }


        /// <summary>
        /// 所在区域的id，最后一级区
        /// <summary>
        [HtmlDisplayAttribute("所在区域", "所在区域")]
        public long AreaId { get; set; }


        /// <summary>
        /// 所在区域的名称，省市区三级使用空格分割
        /// <summary>
        [HtmlDisplayAttribute("区域的名称", "区域的名称")]
        public String AreaName { get; set; }

        /// <summary>
        /// 所在区域的省份Id，方便前台查询
        /// <summary>
        public long AreaProvinceId { get; set; }


        /// <summary>
        /// 所在区域的完整名称，省市区三级使用空格分割
        /// <summary>
        [HtmlDisplayAttribute("区域的完整名称", "区域的完整名称")]
        public String AreaFullName { get; set; }


        /// <summary>
        /// 详细地址
        /// <summary>
        [HtmlDisplayAttribute("详细地址", "详细地址")]
        public String Address { get; set; }


        /// <summary>
        /// 会议类型id  一级
        /// <summary>
        [HtmlDisplayAttribute("会议类型id", "会议类型id")]
        public long MeetingTypeParentId { get; set; }


        /// <summary>
        /// 会议类型id 二级
        /// <summary>
        [HtmlDisplayAttribute("会议类型id", "会议类型id")]
        public long MeetingTypeId { get; set; }

        /// <summary>
        /// 所有会议类型 一级
        /// </summary>
        public IList<SelectListItem> MeetingTypeParentList { get; set; }

        /// <summary>
        /// 所有会议类型 二级
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
        [HtmlDisplayAttribute("活动类型", "活动类型")]
        public Int32 Type { get; set; }

        /// <summary>
        /// 活动类型，1：收费活动  2：免费活动
        /// <summary>
        [HtmlDisplayAttribute("活动类型", "活动类型")]
        public string TypeName { get; set; }

        /// 收费金额，如果是收费活动
        /// <summary>
        [HtmlDisplayAttribute("收费金额", "收费金额")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 帐号状态枚举列表
        /// </summary>
        [HtmlDisplayAttribute("活动类型", "活动类型")]
        public IList<SelectListItem> TypeEnumList { get; set; }

        /// <summary>
        /// 人数上线，0表示不限制
        /// <summary>
        [HtmlDisplayAttribute("人数上线", "人数上线，0表示不限制")]
        public long? PersonMaxCount { get; set; }

        /// <summary>
        /// 联系方式
        /// <summary>
        [HtmlDisplayAttribute("联系方式", "联系方式")]
        public String Contact { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        [HtmlDisplay("发布人", "发布人")]
        public string Author { get; set; }

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
        /// 发布用户的昵称
        /// <summary>
        [HtmlDisplayAttribute("发布用户的昵称", "发布用户的昵称")]
        public String MemberNickName { get; set; }

        /// <summary>
        /// 服务协议
        /// </summary>
        public bool Agreement { get; set; }

        /// <summary>
        /// 会议状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 会议状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        ///报名数量，冗余字段，同时更新报名记录表
        /// </summary>
        public int ApplyCount { get; set; }

        /// <summary>
        /// 查看数量，冗余字段，同时更更新查看记录表
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 收藏数量，冗余字段，同时更更新收藏记录表
        /// </summary>
        public int CollectCount { get; set; }

        /// <summary>
        /// 评论数量，冗余字段，同时更更新评论记录表
        /// </summary>
        public int CommentCount { get; set; }

    }



}
