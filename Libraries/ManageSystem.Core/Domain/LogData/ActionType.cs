using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Log
{
  

    /// <summary>
     /// 日志类型
     /// </summary>
    public enum ActionType
    {
        [Description("用户登录")]
        Login = 1,
        [Description("添加数据")]
        Create = 2,
        [Description("修改数据")]
        Edit = 3,
        [Description("查看数据")]
        View = 4,
        [Description("删除数据")]
        Delete =5,
        [Description("导出数据")]
        Export = 6,
        [Description("导入数据")]
        Import = 7,
        [Description("发送邮件")]
        Email = 8
    }

   

}
