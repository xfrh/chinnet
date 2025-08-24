using FluentValidation;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.SystemSet
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Function 
	/// </summary>
	public partial class PasswordValidator : BaseValidator<PasswordModel>
	{

		public PasswordValidator()
		{
			RuleFor(x => x.OldPassword).NotEmpty().WithMessage("请输入原始密码").Length(8,20).WithMessage("密码长度必须在（8-20）位之间");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("请输入新密码").Length(8, 20).WithMessage("密码长度必须在（8-20）位之间");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("请输确认新密码").Equal(m=>m.NewPassword).WithMessage("2次输入密码不一致").Length(8, 20).WithMessage("密码长度必须在（8-20）位之间");

        }

	}
}
