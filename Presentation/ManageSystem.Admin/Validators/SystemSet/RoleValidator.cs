using FluentValidation;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.SystemSet
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Role 
	/// </summary>
	public partial class RoleValidator : BaseValidator<RoleModel>
	{
		public RoleValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入角色名称");
            RuleFor(x => x.Sort).NotEmpty().WithMessage("请输入排序编号");
        }
	}
}
