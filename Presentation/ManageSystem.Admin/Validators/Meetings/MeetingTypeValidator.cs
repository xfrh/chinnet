using FluentValidation;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Meetings
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MeetingType 
	/// </summary>
	public partial class MeetingTypeValidator : BaseValidator<MeetingTypeModel>
	{

		public MeetingTypeValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入会议类型名称");
			RuleFor(x => x.Sort).NotEmpty().WithMessage("请输入排序编号");

		}

	}
}
