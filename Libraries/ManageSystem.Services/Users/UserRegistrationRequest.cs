using ManageSystem.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Users
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class UserRegistrationRequest
    {

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="email">Email</param>
        /// <param name="loginId">loginId</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password format</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="isApproved">Is approved</param>
        public UserRegistrationRequest(Userinfo userinfo, string loginId,
            string password,string email,
            bool isApproved = true)
        {
            this.Userinfo = userinfo;
            this.LoginId = loginId;
            this.Password = password;
            this.IsApproved = isApproved;
            this.Email = email;
        }

        /// <summary>
        /// Userinfo
        /// </summary>
        public Userinfo Userinfo { get; set; }

        /// <summary>
        /// LoginId
        /// </summary>
        public string LoginId { get; set; }


        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Is approved
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
