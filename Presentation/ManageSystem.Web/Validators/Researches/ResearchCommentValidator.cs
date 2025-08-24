using FluentValidation;
using ManageSystem.Web.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Researches
{
    /// <summary>
    /// 数据验证类 ，数据库表名：ResearchComment 
    /// </summary>
    public partial class ResearchCommentValidator : BaseValidator<ResearchCommentModel>
	{

		public ResearchCommentValidator()
		{
			
		}

	}
}
