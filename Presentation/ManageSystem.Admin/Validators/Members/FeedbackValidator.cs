using FluentValidation;
using ManageSystem.Admin.Models.Members;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Members
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Feedback 
	/// </summary>
	public partial class FeedbackValidator : BaseValidator<FeedbackModel>
	{

		public FeedbackValidator()
		{

		}

	}
}
