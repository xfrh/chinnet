using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Meetings
{
    /// <summary>
     /// 用户会议申请的支付类型
     /// </summary>
    public enum MeetingApplyPayMethodEnum
    {
        [Description("支付宝")]
        Alipay = 1,
        [Description("微信")]
        WeixinPay = 2
    }
}
