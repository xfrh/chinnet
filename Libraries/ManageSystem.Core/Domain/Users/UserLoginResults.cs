using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Users
{
    /// <summary>
    ///代表用户登录结果枚举
    /// </summary>
    public enum UserLoginResults
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        Successful = 1,
        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist = 2,
        /// <summary>
        /// 密码错误
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        ///帐号未启用
        /// </summary>
        NotActive = 4,
        /// <summary>
        ///用户已被删除
        /// </summary>
        Deleted = 5,
        /// <summary>
        ///用户未注册
        /// </summary>
        NotRegistered = 6,

    }
}
