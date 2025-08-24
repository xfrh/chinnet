using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Weixin
{
    /// <summary>
    /// 微信菜单类型，同步官方网站
    /// </summary>
    public enum WeixinMenuType
    {
        [Description("文章推送")]
        click = 1,
        [Description("网页跳转")]
        view = 2,
        [Description("推送文字")]
        text = 3
    }
}
