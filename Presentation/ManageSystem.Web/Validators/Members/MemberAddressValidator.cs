using FluentValidation;
using ManageSystem.Web.Models.Members;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Members
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MemberAddress 
	/// </summary>
	public partial class MemberAddressValidator : BaseValidator<MemberAddressModel>
	{
		public MemberAddressValidator()
		{
            RuleFor(x => x.ProvinceId).NotEmpty().WithMessage("请选择所属省份").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属省份");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("请选择所属市").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属市");
            RuleFor(x => x.DistrictsId).NotEmpty().WithMessage("请选择所在地区").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所在地区");
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入收货人姓名");
			RuleFor(x => x.Address).NotEmpty().WithMessage("请输入详细地址");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("请输入手机号码").Length(11,11).WithMessage("手机号码格式不正确");

		}
	}
}
