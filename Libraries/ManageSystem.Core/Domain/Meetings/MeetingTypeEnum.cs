using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Meetings
{
    /// <summary>
     /// 会议类型
     /// </summary>
    public enum MeetingTypeEnum
    {
        [Description("收费活动")]
        Charge = 1,
        [Description("免费活动")]
        Free = 2
    }
}
