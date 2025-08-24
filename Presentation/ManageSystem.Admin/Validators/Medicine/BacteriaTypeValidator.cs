using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：BacteriaType 
	/// </summary>
	public partial class BacteriaTypeValidator : BaseValidator<BacteriaTypeModel>
	{

		public BacteriaTypeValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入细菌类型名称");
        }

	}
}
