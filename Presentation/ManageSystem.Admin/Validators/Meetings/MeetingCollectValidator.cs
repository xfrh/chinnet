using FluentValidation;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Meetings
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingCollect 
	/// </summary>
	public partial class MeetingCollectValidator : BaseValidator<MeetingCollectModel>
	{

		public MeetingCollectValidator()
		{

		}

	}
}
