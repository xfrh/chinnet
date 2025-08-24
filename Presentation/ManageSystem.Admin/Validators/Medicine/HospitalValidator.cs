using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Hospital 
	/// </summary>
	public partial class HospitalValidator : BaseValidator<HospitalModel>
	{

		public HospitalValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入医院名称");
        }

	}
}
