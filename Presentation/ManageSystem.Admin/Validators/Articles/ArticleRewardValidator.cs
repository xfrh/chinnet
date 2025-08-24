using FluentValidation;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Articles
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ArticleReward 
	/// </summary>
	public partial class ArticleRewardValidator : BaseValidator<ArticleRewardModel>
	{

		public ArticleRewardValidator()
		{

		}

	}
}
