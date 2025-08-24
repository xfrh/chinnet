using FluentValidation;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Meetings
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingView 
	/// </summary>
	public partial class MeetingViewValidator : BaseValidator<MeetingViewModel>
	{

		public MeetingViewValidator()
		{

		}

	}
}
