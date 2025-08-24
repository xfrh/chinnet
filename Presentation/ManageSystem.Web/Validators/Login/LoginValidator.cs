using FluentValidation;
using ManageSystem.Framework.Validators;
using ManageSystem.Web.Models.Login;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Validators.Login
{
    public class LoginValidator : BaseValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.LoginId).NotEmpty().WithMessage("登录帐号不能为空");
            RuleFor(x => x.ValidateCode).NotEmpty().WithMessage("验证码不能为空");
        }
    }

}