using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MedicalOrganism 
	/// </summary>
	public partial class MedicalOrganismValidator : BaseValidator<MedicalOrganismModel>
	{

		public MedicalOrganismValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入细菌名称");
			RuleFor(x => x.Type).NotEmpty().WithMessage("请输入细菌类型");
			RuleFor(x => x.Code).NotEmpty().WithMessage("请输入细菌编码");

		}

	}
}
