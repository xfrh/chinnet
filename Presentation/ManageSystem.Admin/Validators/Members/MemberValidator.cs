using FluentValidation;
using ManageSystem.Admin.Models.Members;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Members
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Member 
	/// </summary>
	public partial class MemberValidator : BaseValidator<MemberModel>
	{

		public MemberValidator()
		{
			RuleFor(x => x.LoginId).NotEmpty().WithMessage("请输入用户登录帐号").Length(3, 20).WithMessage("登录帐号格式不正确"); 
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入用户姓名");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请输入用户密码").Length(6, 20).WithMessage("密码格式不正确"); 
            //RuleFor(x => x.Phone).NotEmpty().WithMessage("请输入用户手机号码").Length(11, 11).WithMessage("手机号码格式不正确"); 
            RuleFor(x => x.Phone).Length(11, 11).WithMessage("手机号码格式不正确");
        }

	}
}
