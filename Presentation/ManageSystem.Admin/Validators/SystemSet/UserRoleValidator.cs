using FluentValidation;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.SystemSet
{
	/// <summary>
	/// 数据验证类 ，数据库表名：UserRole 
	/// </summary>
	public partial class UserRoleValidator : BaseValidator<UserRoleModel>
	{

		public UserRoleValidator()
		{

		}

	}
}
