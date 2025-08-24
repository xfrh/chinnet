using FluentValidation;
using ManageSystem.Admin.Models.Members;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Members
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MemberIntegralLog 
	/// </summary>
	public partial class MemberIntegralLogValidator : BaseValidator<MemberIntegralLogModel>
	{

		public MemberIntegralLogValidator()
		{
            RuleFor(x => x.MemberId).NotEmpty().WithMessage("请选择所属会员").InclusiveBetween(1, long.MaxValue).WithMessage("会员格式不正确");
            RuleFor(x => x.Type).NotEmpty().WithMessage("请输入操作类型");
            RuleFor(x => x.Value).NotEmpty().WithMessage("请输入积分数量").InclusiveBetween(-decimal.MaxValue, decimal.MaxValue).WithMessage("积分数量格式不正确");

        }

	}
}
