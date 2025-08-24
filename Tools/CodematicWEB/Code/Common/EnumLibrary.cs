using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace CodematicWEB.Common
{

    /// <summary>
    /// 管理员状态
    /// </summary>
    public enum AdminState
    {
        [Description("正常")]
        Normal = 1,
        [Description("停用")]
        Stop = 2
    }

    /// <summary>
    /// 性别状态
    /// </summary>
    public enum SexState
    {
        [Description("女士")]
        Woman = 1,
        [Description("先生")]
        Man = 2
    }

    /// <summary>
    /// 是否显示状态
    /// </summary>
    public enum ShowState
    {
        [Description("显示")]
        Show = 1,
        [Description("不显示")]
        Hide = 2
    }

    /// <summary>
    /// 代码状态
    /// </summary>
    public enum CodeState
    {
        [Description("草稿")]
       Draft = 1,
        [Description("正常")]
        Normal = 2
    }



    /// <summary>
    /// 代码查看权限状态
    /// </summary>
    public enum CodeSeeState
    {
        [Description("所有人看见")]
        Public = 1,
        [Description("自己可见")]
        Private = 2
    }


    /// <summary>
    /// 删除状态
    /// </summary>
    public enum DelteState
    {
        [Description("正常")]
        Normal = 1,
        [Description("删除")]
        Delete = 2
    }



    /// <summary>
    /// 日志操作系统模块467
    /// </summary>
    public enum WebModel
    {
        [Description("后台管理")]
        Admin = 1,
        [Description("前台用户")]
        User = 2
    }


}


