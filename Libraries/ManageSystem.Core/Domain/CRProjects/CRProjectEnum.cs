using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.CRProjects
{
    /// <summary>
     /// 医学数据状态
     /// </summary>
    public enum CRProjectEnum
    {
        [Description("未发送")]
        Effective = 1,
        [Description("已发送")]
       Invalid = 2,
        [Description("已确认")]
        Wait = 3
    }
}
