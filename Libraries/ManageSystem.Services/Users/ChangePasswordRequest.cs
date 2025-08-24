using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Users
{
    /// <summary>
    /// Change password requst
    /// </summary>
    public class ChangePasswordRequest
    {
      
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string LoginId { get; set; }


        /// <summary>
        /// 值指示我们是否应该验证请求
        /// </summary>
        public bool ValidateRequest { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 原始密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="validateRequest">A value indicating whether we should validate request</param>
        /// <param name="newPasswordFormat">Password format</param>
        /// <param name="newPassword">New password</param>
        /// <param name="oldPassword">Old password</param>
        public ChangePasswordRequest(string loginId, bool validateRequest,string newPassword, string oldPassword = "")
        {
            this.LoginId = loginId;
            this.ValidateRequest = validateRequest;
            this.NewPassword = newPassword;
            this.OldPassword = oldPassword;
        }
    }
}
