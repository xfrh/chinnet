using ManageSystem.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Users
{
    /// <summary>
    /// 用户注册、登录接口
    /// </summary>
    public partial interface IUserRegistrationService
    {
        /// <summary>
        /// 验证登录帐号
        /// </summary>
        /// <param name="loginId">登录帐号</param>
        /// <param name="password">登录密码</param>
        /// <returns>Result</returns>
        UserLoginResults ValidateUser(string loginId, string password);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        UserRegistrationResult RegisterCustomer(UserRegistrationRequest request);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        ChangePasswordResult ChangePassword(ChangePasswordRequest request);



        /// <summary>
        /// 设置用户登录帐号
        /// </summary>
        /// <param name="userinfo">用户对象</param>
        /// <param name="newLoginId">新的登录帐号</param>
        void SetUserLoginId(Userinfo userinfo, string newLoginId);
    }
}
