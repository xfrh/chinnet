using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Medicine
{
    /// <summary>
    /// 数据验证类 ，数据库表名：DoctorTitle 
    /// </summary>
    public partial class MedicalDataProjectValidator : BaseValidator<MedicalDataProjectModel>
    {

        public MedicalDataProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入名称");
        }

    }
}
