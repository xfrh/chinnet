using FluentValidation;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Meetings
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Meeting 
	/// </summary>
	public partial class MeetingValidator : BaseValidator<MeetingModel>
	{

		public MeetingValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入主题");
            //RuleFor(x => x.StartTime).NotEmpty().WithMessage("请选择会议开始时间");
            //RuleFor(x => x.EndTime).NotEmpty().WithMessage("请选择会议结束时间");
            //RuleFor(x => x.Address).NotEmpty().WithMessage("请输入会议地点");
            RuleFor(x => x.MeetingTypeId).NotEmpty().WithMessage("请选择类型").InclusiveBetween(1, long.MaxValue).WithMessage("请选择类型");
            RuleFor(x => x.Content).NotEmpty().WithMessage("请输入详情");
            //RuleFor(x => x.Contact).NotEmpty().WithMessage("请输入主办方联系方式");
            //RuleFor(x => x.PersonMaxCount).NotEmpty().WithMessage("请输入人数限制，0表示不限制").InclusiveBetween(0, long.MaxValue).WithMessage("请输入人数限制，0表示不限制"); ;
            //RuleFor(x => x.Price).NotEmpty().WithMessage("请输入收费金额").InclusiveBetween(0, decimal.MaxValue).WithMessage("请输入收费金额");

        }

    }
}
