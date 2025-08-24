using FluentValidation;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.SystemSet
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Userinfo 
	/// </summary>
	public partial class UserProfileValidator : BaseValidator<UserProfileModel>
	{

		public UserProfileValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入姓名");
		}

	}
}
