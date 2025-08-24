using FluentValidation;
using ManageSystem.Web.Models.Members;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Members
{
    /// <summary>
    /// 数据验证类 ，数据库表名：Member 
    /// </summary>
    public partial class MemberValidator : BaseValidator<MemberModel>
    {

        public MemberValidator()
        {
            RuleFor(x => x.LoginId).NotEmpty().WithMessage("请输入用户姓名").Length(3, 20).WithMessage("格式不正确");
            RuleFor(x => x.ValidateCode).NotEmpty().WithMessage("请输入验证码").Length(6, 6).WithMessage("格式不正确");

        }

    }


    /// <summary>
    /// 数据验证类 ，数据库表名：Member 
    /// </summary>
    public partial class CenterIndexMemberValidator : BaseValidator<CenterIndexMemberModel>
    {

        public CenterIndexMemberValidator()
        {
            //RuleFor(x => x.Name).NotEmpty().WithMessage("请输入用户姓名").Length(2, 20).WithMessage("格式不正确");
            //RuleFor(x => x.Sex).NotEmpty().WithMessage("请选择性别").InclusiveBetween(1, int.MaxValue).WithMessage("格式不正确");
            RuleFor(x => x.Phone).Length(11, 11).WithMessage("格式不正确");
            RuleFor(x => x.Email).NotEmpty().WithMessage("请输入邮箱 ").Length(3, 100).WithMessage("格式不正确").EmailAddress().WithMessage("格式不正确");
            RuleFor(x => x.ProvinceId).NotEmpty().WithMessage("请选择所在省份").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所在省份");
            //RuleFor(x => x.HospitalId).NotEmpty().WithMessage("请选择所在医院名称").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所在医院名称");
            //if (RuleFor(x => x.HospitalId).Equals(0))
            //{
            //    RuleFor(x => x.HospitalName).NotEmpty().WithMessage("请填写医院名称").Length()
            //}
        }
    }


}
