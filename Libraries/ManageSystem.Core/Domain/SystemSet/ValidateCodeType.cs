using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SystemSet
{
    /// <summa
    /// ry>
     /// 验证码类型
     /// </summary>
    public enum ValidateCodeType
    {
        [Description("短信")]
        Phone = 1,
        [Description("邮件")]
        Email = 2,
        [Description("注册短信验证码")]
        RegistPhone = 3,
        [Description("找回密码短信验证码")]
        FindPasswordPhone = 4
    }


}
