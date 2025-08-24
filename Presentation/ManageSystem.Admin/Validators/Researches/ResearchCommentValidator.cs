using FluentValidation;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Researches
{
    /// <summary>
    /// 数据验证类 ，数据库表名：ResearchComment 
    /// </summary>
    public partial class ResearchCommentValidator : BaseValidator<ResearchCommentModel>
	{

		public ResearchCommentValidator()
		{
			RuleFor(x => x.Content).NotEmpty().WithMessage("请输入评论内容");

		}

	}
}
