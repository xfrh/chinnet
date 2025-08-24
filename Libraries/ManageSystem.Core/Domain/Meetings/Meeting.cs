using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Meetings
{
    /// <summary>
    /// 实体类 ，数据库表名：Meeting 
    /// </summary>
    public partial class Meeting : BaseEntity
    {

        /// <summary>
        /// 会议主题
        /// <summary>
        public String Name { get; set; }
        /// <summary>
        /// 会议海报图片地址
        /// <summary>
        public String CoverImage { get; set; }
        /// <summary>
        /// 会议开始时间
        /// <summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 会议结束时间
        /// <summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 所在区域的id，最后一级区
        /// <summary>
        public long AreaId { get; set; }

        /// <summary>
        /// 所在区域的省份Id，方便前台查询
        /// <summary>
        public long AreaProvinceId { get; set; }

        /// <summary>
        /// 所在区域的名称
        /// <summary>
        public String AreaName { get; set; }

        /// <summary>
        /// 所在区域的完整名称，省市区三级使用空格分割
        /// <summary>
        public String AreaFullName { get; set; }

        /// <summary>
        /// 详细地址
        /// <summary>
        public String Address { get; set; }
        /// <summary>
        /// 会议类型id
        /// <summary>
        public long MeetingTypeId { get; set; }
        /// <summary>
        /// 会议详细
        /// <summary>
        public String Content { get; set; }
        /// <summary>
        /// 活动类型，1：收费活动  2：免费活动
        /// <summary>
        public Int32 Type { get; set; }

        /// <summary>
        /// 收费金额，如果是收费活动
        /// <summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 人数上线，0表示不限制
        /// <summary>
        public long PersonMaxCount { get; set; }
        /// <summary>
        /// 联系方式
        /// <summary>
        public String Contact { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public String Author { get; set; } = "CHINET数据云";
        /// <summary>
        /// 发布用户的id
        /// <summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 发布用户的姓名
        /// <summary>
        public String MemberName { get; set; }

        /// <summary>
        /// 会议状态
        /// </summary>
        public int Status { get; set; }

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
