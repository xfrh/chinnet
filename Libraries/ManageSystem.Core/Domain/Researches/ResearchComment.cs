using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Researches
{
    /// <summary>
    /// 实体类 ，数据库表名：ResearchComment 
    /// </summary>
    public partial class ResearchComment : BaseEntity
    {
        /// <summary>
        /// 用户的id
        /// <summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 用户的姓名
        /// <summary>
        public String MemberName { get; set; }
        /// <summary>
        /// 科研合作的id
        /// <summary>
        public long ResearchId { get; set; }
        /// <summary>
        /// 科研合作的名称
        /// <summary>
        public String ResearchName { get; set; }
        /// <summary>
        /// Ip地址
        /// <summary>
        public String Ip { get; set; }
        /// <summary>
        /// 操作者浏览器名称
        /// <summary>
        public String BrowserName { get; set; }
        /// <summary>
        /// 评论内容
        /// <summary>
        public String Content { get; set; }
        /// <summary>
        /// 上级评论id
        /// <summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 该评论的下级评论数量，只记录下一级的。多级不处理
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 评论类型，1：普通会员   2：会议主办方  3：后台管理员
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 当前评论所处的深度，0表示顶级评论，1表示1级，以此内推
        /// </summary>
        public int Level { get; set; }

    }
}
