using FluentValidation;
using ManageSystem.Admin.Models.CRE;
using ManageSystem.Framework.Validators;


namespace ManageSystem.Admin.Validators.CRE
{
    public partial class ToconfigureValidator : BaseValidator<ToconfigureModel>
    {
       public ToconfigureValidator()
        {
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("请输入用户手机号码").Length(11, 11).WithMessage("手机号码格式不正确");
        }
    }
}