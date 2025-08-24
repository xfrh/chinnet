using FluentValidation;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Validators.Users
{

    public class LoginValidator : BaseValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.LoginId).NotEmpty().WithMessage("请输入登录帐号")
              .Length(4, 50).WithMessage("登录帐号长度必须在（4-20）字符之间");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请输入密码")
                 .Length(6, 50).WithMessage("密码长度必须在（6-200）字符之间");
            RuleFor(x => x.Captcha).NotEmpty().WithMessage("请输入验证码")
                .Length(4, 4).WithMessage("密码长度必须4个字符");

        }
    }

}