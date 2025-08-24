using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Meetings
{
    /// <summary>
     /// 用户会议申请的状态
     /// </summary>
    public enum MeetingApplyStatusEnum
    {
        [Description("待支付")]
        Wait = 1,
        [Description("已完成 ")]
        Finish = 2,
        [Description("已取消 ")]
        Cancel = 3
    }
}
