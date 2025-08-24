using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Researches
{
    /// <summary>
    /// 实体类 ，数据库表名：ResearchApply 
    /// </summary>
    public partial class ResearchApply : BaseEntity
    {
        /// <summary>
        /// 报名用户的id
        /// <summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 报名用户的姓名
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
        /// 报名姓名
        /// <summary>
        public String Name { get; set; }
        /// <summary>
        /// 邮箱地址
        /// <summary>
        public String Email { get; set; }
        /// <summary>
        /// 手机号码
        /// <summary>
        public String Phone { get; set; }
        /// <summary>
        /// 用户备注
        /// <summary>
        public String Remark { get; set; }
        /// <summary>
        /// 报名人数
        /// <summary>
        public Int32 Count { get; set; }
        /// <summary>
        /// 申请状态
        /// <summary>
        public Int32 Status { get; set; }
        /// <summary>
        /// 状态变更时间
        /// <summary>
        public DateTime StatusTime { get; set; }


    }
}
