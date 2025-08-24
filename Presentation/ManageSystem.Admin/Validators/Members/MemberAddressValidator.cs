using FluentValidation;
using ManageSystem.Admin.Models.Members;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Members
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MemberAddress 
	/// </summary>
	public partial class MemberAddressValidator : BaseValidator<MemberAddressModel>
	{
		public MemberAddressValidator()
		{
            RuleFor(x => x.MemberId).NotEmpty().WithMessage("请选择所属会员").InclusiveBetween(1,long.MaxValue).WithMessage("请选择所属会员");
            RuleFor(x => x.ProvinceId).NotEmpty().WithMessage("请选择所属省份").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属省份");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("请选择所属市").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属市");
            RuleFor(x => x.DistrictsId).NotEmpty().WithMessage("请选择所属区").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属区");
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入收货人姓名");
			RuleFor(x => x.Address).NotEmpty().WithMessage("请输入详细地址");
			RuleFor(x => x.Phone).NotEmpty().WithMessage("请输入手机号码");
		}
	}
}
