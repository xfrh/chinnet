using FluentValidation;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Articles
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Article 
	/// </summary>
	public partial class ArticleValidator : BaseValidator<ArticleModel>
	{

		public ArticleValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入文章名称");
			RuleFor(x => x.Author).NotEmpty().WithMessage("请输入文章作者");
			RuleFor(x => x.ReleaseTime).NotEmpty().WithMessage("请输入发布时间");
            RuleFor(x => x.ShortContent).NotEmpty().WithMessage("请输入文章简单描述");
        }

	}
}
