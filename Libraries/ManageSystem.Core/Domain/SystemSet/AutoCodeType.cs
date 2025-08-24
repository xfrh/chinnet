using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SystemSet
{
    /// <summary>
     ///自动获取编码的类型
     /// </summary>
    public enum AutoCodeType
    {
        [Description("信息动态申请订单号")]
        MeetingApply = 1,
        [Description("医学信息数据编码")]
        MedicalData =2,
        [Description("商品订单的订单号")]
        ProductOrder = 3
    }

}
