using FluentValidation;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Meetings
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingComment 
	/// </summary>
	public partial class MeetingCommentValidator : BaseValidator<MeetingCommentModel>
	{

		public MeetingCommentValidator()
		{
			RuleFor(x => x.Content).NotEmpty().WithMessage("请输入评论内容");

		}

	}
}
