using FluentValidation;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Articles
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ArticleType 
	/// </summary>
	public partial class ArticleTypeValidator : BaseValidator<ArticleTypeModel>
	{

		public ArticleTypeValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入类型名称");

		}

	}
}
