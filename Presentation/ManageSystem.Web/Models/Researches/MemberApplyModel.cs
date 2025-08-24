using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Researches
{
    /// <summary>
    /// 用户提交科研申请数据实体
    /// </summary>
    public class MemberApplyModel
    {
        /// <summary>
        /// 科研id
        /// </summary>
        public long ResearchId { get; set; }

        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 申请人手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 申请人邮箱
        /// </summary>
        public string Email { get; set; }

    }


}