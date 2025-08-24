using FluentValidation;
using ManageSystem.Web.Models.Meetings;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Meetings
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingComment 
	/// </summary>
	public partial class MeetingCommentValidator : BaseValidator<MeetingCommentModel>
	{

		public MeetingCommentValidator()
		{
			
		}

	}
}
