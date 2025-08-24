using FluentValidation;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.SystemSet
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Function 
	/// </summary>
	public partial class FunctionValidator : BaseValidator<FunctionModel>
	{

		public FunctionValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入功能名称");
			RuleFor(x => x.Sort).NotEmpty().WithMessage("请输入排序编号").GreaterThanOrEqualTo(0).WithMessage("排序编号必须大于0");
			RuleFor(x => x.Type).NotEmpty().WithMessage("请输入功能类型");

		}

	}
}
