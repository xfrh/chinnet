using FluentValidation;
using ManageSystem.Admin.Models.Log;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Log
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ActionLog 
	/// </summary>
	public partial class ActionLogValidator : BaseValidator<ActionLogModel>
	{

		public ActionLogValidator()
		{
			RuleFor(x => x.BrowserName).NotEmpty().WithMessage("请输入操作者浏览器名称");
			RuleFor(x => x.IPAddress).NotEmpty().WithMessage("请输入操作的IP地址");
			RuleFor(x => x.UserinfoId).NotEmpty().WithMessage("请输入操作的用户id");
			RuleFor(x => x.UserinfoName).NotEmpty().WithMessage("请输入操作用户的姓名和登录名");
			RuleFor(x => x.Content).NotEmpty().WithMessage("请输入日志内容");
			RuleFor(x => x.Type).NotEmpty().WithMessage("请输入日志类型");

		}

	}
}
