using FluentValidation;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Articles
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ArticlePraise 
	/// </summary>
	public partial class ArticlePraiseValidator : BaseValidator<ArticlePraiseModel>
	{

		public ArticlePraiseValidator()
		{

		}

	}
}
