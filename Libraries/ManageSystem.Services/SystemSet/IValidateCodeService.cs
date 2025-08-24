using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Services.SystemSet
{
    /// <summary>
    /// 操作接口类 ，数据库表名：ValidateCode 
    /// </summary>
    public partial interface IValidateCodeService : IBaseService<ValidateCode>
    {
        /// <summary>
        /// 获取一个随机的验证码
        /// </summary>
        /// <param name="type">验证码类型</param>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        string GetCode(ValidateCodeType type, int length = 6);

        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="code">短信编码</param>
        /// <param name="tel">电话号码</param>
        /// <returns></returns>
        string SendPhoneMessage(string type, string code, string tel);

        /// <summary>
        /// 发送验证码至邮箱
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        string SendEmailMessage(string email, string code);
    }
}
