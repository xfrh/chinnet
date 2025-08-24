using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Weixin
{
    /// <summary>
    /// 微信用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
    /// </summary>
    public enum WeixinUserSex
    {
        [Description("未知")]
        Unknown = 0,
        [Description("男性")]
        Male = 1,
        [Description("女性")]
        Female = 2

    }
}
