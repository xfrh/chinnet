using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Researches
{
    /// <summary>
    /// 评论类型，1：普通会员   2：会议主办方  3：后台管理员
    /// </summary>
    public enum ResearchCommentTypeEnum
    {
        [Description("普通会员")]
        Member = 1,
        [Description("会议主办方")]
        Author = 2,
        [Description("后台管理员")]
        Admin = 3
    }
}
