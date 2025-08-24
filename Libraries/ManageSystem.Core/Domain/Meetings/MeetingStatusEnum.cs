using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Meetings
{
    /// <summary>
     /// 会议状态
     /// </summary>
    public enum MeetingStatusEnum
    {
        [Description("待审核")]
        Wait = 1,
        [Description("已取消")]
        Cancel = 2,
        [Description("通过审核")]
        Finish = 3,
        [Description("拒绝审核")]
        Stop = 4
    }
}
