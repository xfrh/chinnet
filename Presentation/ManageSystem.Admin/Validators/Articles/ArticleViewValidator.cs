using FluentValidation;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Articles
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ArticleView 
	/// </summary>
	public partial class ArticleViewValidator : BaseValidator<ArticleViewModel>
	{

		public ArticleViewValidator()
		{

		}

	}
}
