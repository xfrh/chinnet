using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MedicalAntibiotic 
	/// </summary>
	public partial class MedicalAntibioticValidator : BaseValidator<MedicalAntibioticModel>
	{

		public MedicalAntibioticValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入抗生素名称");
			RuleFor(x => x.Code).NotEmpty().WithMessage("请输入抗生素编码");

		}

	}
}
