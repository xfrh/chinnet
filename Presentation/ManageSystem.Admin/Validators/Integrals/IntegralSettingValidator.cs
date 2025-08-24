using FluentValidation;
using ManageSystem.Admin.Models.Integrals;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Integrals
{
	/// <summary>
	/// 数据验证类 ，数据库表名：IntegralSetting 
	/// </summary>
	public partial class IntegralSettingValidator : BaseValidator<IntegralSettingModel>
	{

		public IntegralSettingValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入细菌类型名称");
            RuleFor(x => x.Value).NotEmpty().WithMessage("请输入赠送积分数量").InclusiveBetween(0.01M,decimal.MaxValue).WithMessage("赠送积分数量必须大于等于0.01");
            RuleFor(x => x.Type).NotEmpty().WithMessage("请选择操作类型").InclusiveBetween(1,int.MaxValue).WithMessage("操作类型不正确");
        }

	}
}
