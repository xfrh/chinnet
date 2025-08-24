using FluentValidation;
using ManageSystem.Web.Models.Members;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Members
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MemberAttestation 
	/// </summary>
	public partial class MemberAttestationValidator : BaseValidator<MemberAttestationModel>
	{
		public MemberAttestationValidator()
		{
            RuleFor(x => x.HospitalId).NotEmpty().WithMessage("请选择所属医院").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属医院");
            RuleFor(x => x.AreaId).NotEmpty().WithMessage("请选择所属城市").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属城市");
            RuleFor(x => x.HospitalDepartmentId).NotEmpty().WithMessage("请选择所属科室").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所属科室");
            RuleFor(x => x.DoctorTitleId).NotEmpty().WithMessage("请选择医生职称").InclusiveBetween(1, long.MaxValue).WithMessage("请选择医生职称");

        }

	}
}
