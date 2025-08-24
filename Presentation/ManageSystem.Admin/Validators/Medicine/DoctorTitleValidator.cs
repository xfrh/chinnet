using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：DoctorTitle 
	/// </summary>
	public partial class DoctorTitleValidator : BaseValidator<DoctorTitleModel>
	{

		public DoctorTitleValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入职称名称");
		}

	}
}
