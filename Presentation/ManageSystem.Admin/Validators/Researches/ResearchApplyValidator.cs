using FluentValidation;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Researches
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingApply 
	/// </summary>
	public partial class ResearchApplyValidator : BaseValidator<ResearchApplyModel>
	{
		public ResearchApplyValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入报名人姓名");
            RuleFor(x => x.Email).NotEmpty().WithMessage("请输入邮箱地址");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("请输入手机号码");
        }

	}
}
