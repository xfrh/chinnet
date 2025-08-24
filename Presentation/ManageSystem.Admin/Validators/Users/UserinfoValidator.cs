using FluentValidation;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Users
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Userinfo 
	/// </summary>
	public partial class UserinfoValidator : BaseValidator<UserinfoModel>
	{

		public UserinfoValidator()
		{
     
            RuleFor(x => x.LoginId).NotEmpty().WithMessage("请输入登录帐号")
                .Length(4,20).WithMessage("登录帐号长度必须在（4-20）字符之间");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请输入密码")
                 .Length(6, 20).WithMessage("密码长度必须在（6-200）字符之间"); 

			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入姓名");

            RuleFor(x => x.State).NotEmpty().WithMessage("请输入帐号状态")
                .GreaterThanOrEqualTo(0).WithMessage("帐号状态必须大于0");

		}

	}
}
