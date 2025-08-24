using FluentValidation;
using ManageSystem.Web.Models.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Medicine
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MedicalData 
	/// </summary>
	public partial class MedicalDataValidator : BaseValidator<MedicalDataModel>
	{
		public MedicalDataValidator()
		{

		}

	}


    /// <summary>
	/// 数据验证类 ，数据库表名：上传 
	/// </summary>
    public partial class UploadMedicalDataValidator : BaseValidator<UploadMedicalDataModel>
    {
        public UploadMedicalDataValidator()
        {
            RuleFor(x => x.Year).NotEmpty().WithMessage("请选择上报数据所属年度").InclusiveBetween(1,int.MaxValue).WithMessage("请选择上报数据所属年度");
            RuleFor(x => x.Quarter).NotEmpty().WithMessage("请选择上报数据所属季度").InclusiveBetween(1, int.MaxValue).WithMessage("请选择上报数据所属季度");
            //RuleFor(x => x.BacteriaIds).NotEmpty().WithMessage("请选择药敏实验方法");
        }

    }
}
