using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Specimen 
	/// </summary>
	public partial class SpecimenValidator : BaseValidator<SpecimenModel>
	{

		public SpecimenValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入标本名称");
        }

	}
}
