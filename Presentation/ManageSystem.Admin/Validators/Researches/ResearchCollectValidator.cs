using FluentValidation;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Researches
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingCollect 
	/// </summary>
	public partial class ResearchCollectValidator : BaseValidator<ResearchCollectModel>
	{

		public ResearchCollectValidator()
		{

		}

	}
}
