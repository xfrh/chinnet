using FluentValidation;
using ManageSystem.Admin.Models.Log;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Log
{
	/// <summary>
	/// 数据验证类 ，数据库表名：SystemLog 
	/// </summary>
	public partial class SystemLogValidator : BaseValidator<SystemLogModel>
	{

		public SystemLogValidator()
		{
			RuleFor(x => x.Title).NotEmpty().WithMessage("请输入日志标题");
			RuleFor(x => x.Logger).NotEmpty().WithMessage("请输入出错类");
			RuleFor(x => x.Message).NotEmpty().WithMessage("请输入错误详细内容");

		}

	}
}
