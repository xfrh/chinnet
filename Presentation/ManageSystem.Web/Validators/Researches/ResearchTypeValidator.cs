using FluentValidation;
using ManageSystem.Web.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Researches
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ResearchType 
	/// </summary>
	public partial class ResearchTypeValidator : BaseValidator<ResearchTypeModel>
	{

		public ResearchTypeValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入类型名称");
			RuleFor(x => x.Sort).NotEmpty().WithMessage("请输入排序编号");

		}

	}
}
