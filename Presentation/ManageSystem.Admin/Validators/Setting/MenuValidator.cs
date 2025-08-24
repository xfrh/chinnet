using FluentValidation;
using ManageSystem.Admin.Models.Setting;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Setting
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Menu 
	/// </summary>
	public partial class MenuValidator : BaseValidator<MenuModel>
	{

		public MenuValidator()
		{
			RuleFor(x => x.Title).NotEmpty().WithMessage("请输入日志标题");
			RuleFor(x => x.Level).NotEmpty().WithMessage("请输入日志等级");
			RuleFor(x => x.Logger).NotEmpty().WithMessage("请输入出错类");
			RuleFor(x => x.Message).NotEmpty().WithMessage("请输入错误详细内容");
			RuleFor(x => x.CreateTime).NotEmpty().WithMessage("请输入日期");

		}

	}
}
