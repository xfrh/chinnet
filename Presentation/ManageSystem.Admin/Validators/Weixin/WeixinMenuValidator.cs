using FluentValidation;
using ManageSystem.Admin.Models.Weixin;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Weixin
{
	/// <summary>
	/// 数据验证类 ，数据库表名：WeixinMenu 
	/// </summary>
	public partial class WeixinMenuValidator : BaseValidator<WeixinMenuModel>
	{

		public WeixinMenuValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入菜单名称");
            RuleFor(x => x.Key).NotEmpty().WithMessage("请输入菜单关键字且不能重复");

        }

	}
}
