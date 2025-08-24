using ManageSystem.Core;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Users
{

    /// <summary>
    /// Customer registration service
    /// </summary>
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields

        private readonly IUserinfoService userinfoService;
        private readonly IEncryptionService encryptionService;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerService">Customer service</param>
        public UserRegistrationService(IUserinfoService _userinfoService,
            IEncryptionService _encryptionService)
        {
            this.userinfoService = _userinfoService;
            this.encryptionService = _encryptionService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="loginId">登录帐号</param>
        /// <param name="password">登录密码</param>
        /// <returns>Result</returns>
        public virtual UserLoginResults ValidateUser(string loginId, string password)
        {
            var customer = this.userinfoService.QueryModelByLoginId(loginId);

            //管理员自动登成功不验证
            if (loginId == "Chinet")
            {
                var customerPassword = this.encryptionService.DecryptText(customer.Password);
                //ManageSystem.Core.Utility.Utility.WriteCookie(loginId, customerPassword);
                return UserLoginResults.Successful;
            }
            
            if (customer == null)
                return UserLoginResults.UserNotExist;
            if (customer.Mark == 0)
                return UserLoginResults.Deleted;
            if (customer.State != (int)UserinfoState.Normal)
                return UserLoginResults.NotActive;
            string pwd = this.encryptionService.EncryptText(password);

            bool isValid = pwd == customer.Password;

            if (!isValid)
                return UserLoginResults.WrongPassword;

            customer.LastLoginDate = DateTime.UtcNow;

            this.userinfoService.Update(customer);

            return UserLoginResults.Successful;
        }

        /// <summary>
        /// 帐号注册
        /// </summary>
        /// <param name="request">注册信息</param>
        /// <returns>Result</returns>
        public virtual UserRegistrationResult RegisterCustomer(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Userinfo == null)
                throw new ArgumentException("不能加载当前客户");

            var result = new UserRegistrationResult();
            if (String.IsNullOrEmpty(request.LoginId))
            {
                result.AddError("登录帐号不能为空");
                return result;
            }

            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("登录密码不能为空");
                return result;
            }


            if (this.userinfoService.QueryModelByLoginId(request.LoginId) != null)
            {
                result.AddError("登录帐号已经存在");
                return result;
            }

            request.Userinfo.LoginId = request.LoginId;
            request.Userinfo.Email = request.Email;
            request.Userinfo.Password = this.encryptionService.EncryptText(request.Password);

            request.Userinfo.State = request.IsApproved ? 1 : 2;

            this.userinfoService.Update(request.Userinfo);
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var result = new ChangePasswordResult();
            if (String.IsNullOrWhiteSpace(request.LoginId))
            {
                result.AddError("登录帐号不能为空");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("密码不能为空");
                return result;
            }

            var customer = this.userinfoService.QueryModelByLoginId(request.LoginId);
            if (customer == null)
            {
                result.AddError("帐号不存在");
                return result;
            }

            if (string.IsNullOrEmpty(customer.Email))
            {
                result.AddError("帐号未设置邮箱，不能找回密码");
                return result;
            }

            var requestIsValid = false;
            if (request.ValidateRequest)
            {
                //password
                string oldPwd = this.encryptionService.EncryptText(request.OldPassword);

                bool oldPasswordIsValid = oldPwd == customer.Password;
                if (!oldPasswordIsValid)
                    result.AddError("原始密码不正确");

                if (oldPasswordIsValid)
                    requestIsValid = true;
            }
            else
                requestIsValid = true;

            if (requestIsValid)
            {
                customer.Password = this.encryptionService.EncryptText(request.NewPassword);
                this.userinfoService.Update(customer);
            }

            return result;
        }



        /// <summary>
        /// 设置用户登录帐号
        /// </summary>
        /// <param name="userinfo">用户对象</param>
        /// <param name="newLoginId">新的登录帐号</param>
        public virtual void SetUserLoginId(Userinfo userinfo, string newLoginId)
        {
            if (userinfo == null)
                throw new ArgumentNullException("customer");

            newLoginId = newLoginId.Trim();

            if (newLoginId.Length > 100)
                throw new ManageSystemException("登录帐号长度不能大于100");

            var user2 = this.userinfoService.QueryModelByLoginId(newLoginId);
            if (user2 != null && userinfo.Id != user2.Id)
                throw new ManageSystemException("登录帐号已经存在。");

            userinfo.LoginId = newLoginId;
            this.userinfoService.Update(userinfo);
        }


        #endregion
    }
}
