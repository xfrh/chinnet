using FluentValidation;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Researches
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingView 
	/// </summary>
	public partial class ResearchViewValidator : BaseValidator<ResearchViewModel>
	{

		public ResearchViewValidator()
		{

		}

	}
}
