using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Orders
{
    /// <summary>
    /// 订单状态   1：待处理 、2：待付款、3：待发货  、4：待收货、5：交易取消、6：交易完成
    /// </summary>
    public enum OrderStatusEnum
    {
        //[Description("待处理")]
        //WaitCheck = 1,
        [Description("待付款")]
        WaitPay = 2,
        [Description("待发货")]
        WaitSend = 3,
        [Description("待收货")]
        WaitReceive = 4,
        [Description("交易取消")]
        Cancel = 5,
        [Description("交易完成")]
        Finish = 6
    }

}
