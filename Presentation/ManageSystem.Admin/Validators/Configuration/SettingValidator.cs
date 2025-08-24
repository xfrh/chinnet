using FluentValidation;
using ManageSystem.Admin.Models.Configuration;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Configuration
{
	/// <summary>
	/// 数据验证类 ，数据库表名：SystemConfig 
	/// </summary>
	public partial class SettingValidator : BaseValidator<SettingModel>
	{

		public SettingValidator()
		{
			RuleFor(x => x.Title).NotEmpty().WithMessage("请输入配置名称");
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入配置关键字");
			RuleFor(x => x.Value).NotEmpty().WithMessage("请输入配置的值");

		}

	}
}
