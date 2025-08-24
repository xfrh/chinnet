using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Researches
{
    /// <summary>
    /// 实体类 ，数据库表名：ResearchCollect 
    /// </summary>
    public partial class ResearchCollect : BaseEntity
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


    }
}
