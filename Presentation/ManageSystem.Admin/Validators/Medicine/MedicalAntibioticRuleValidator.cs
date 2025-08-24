using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MedicalAntibioticRule 
	/// </summary>
	public partial class MedicalAntibioticRuleValidator : BaseValidator<MedicalAntibioticRuleModel>
	{

		public MedicalAntibioticRuleValidator()
		{

		}

	}
}
