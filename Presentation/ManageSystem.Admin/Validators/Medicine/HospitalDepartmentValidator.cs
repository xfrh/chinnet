using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：HospitalDepartment 
	/// </summary>
	public partial class HospitalDepartmentValidator : BaseValidator<HospitalDepartmentModel>
	{

		public HospitalDepartmentValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入科室名称");
        }

	}
}
