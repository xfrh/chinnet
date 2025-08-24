using FluentValidation;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Researches
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Research 
	/// </summary>
	public partial class ResearchValidator : BaseValidator<ResearchModel>
	{

		public ResearchValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入研究名称");
			RuleFor(x => x.Code).NotEmpty().WithMessage("请输入研究编码");
			RuleFor(x => x.StartTime).NotEmpty().WithMessage("请输入科研时间");
			RuleFor(x => x.Require).NotEmpty().WithMessage("请输入参与要求");
			RuleFor(x => x.Author).NotEmpty().WithMessage("请输入科研发起人");
			RuleFor(x => x.ResearchTypeId).NotEmpty().WithMessage("请选择科研类型");
            RuleFor(x => x.Remark).NotEmpty().WithMessage("请输入研究简介");
        }

	}
}
