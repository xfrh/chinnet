using FluentValidation;
using ManageSystem.Web.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MedicalDataItem 
	/// </summary>
	public partial class MedicalDataItemValidator : BaseValidator<MedicalDataItemModel>
	{

		public MedicalDataItemValidator()
		{

		}

	}
}
