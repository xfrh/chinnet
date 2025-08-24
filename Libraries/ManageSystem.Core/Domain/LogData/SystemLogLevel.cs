using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Log
{
  

    /// <summary>
        /// log4net 日志等级类型枚举
     /// </summary>
    public enum SystemLogLevel
    {
        [Description("警告信息")]
        Warn = 1,
        [Description("调试信息")]
        Debug = 2,
        [Description("一般信息")]
        Info = 3,
        [Description("错误信息")]
        Error =4
    }

}
