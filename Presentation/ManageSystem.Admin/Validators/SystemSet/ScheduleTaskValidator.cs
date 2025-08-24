using FluentValidation;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.SystemSet
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Role 
	/// </summary>
	public partial class ScheduleTaskValidator : BaseValidator<ScheduleTaskModel>
	{
		public ScheduleTaskValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入任务名称");
        }
	}
}
