using FluentValidation;
using ManageSystem.Core.Domain.MIC;
using ManageSystem.Framework.Validators;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Validators.Members
{
    public partial class MICPermissionapplicationValidator: BaseValidator<MICIndexMemberModel>
    {
        public MICPermissionapplicationValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入用户姓名").Length(2, 20).WithMessage("格式不正确");
            RuleFor(x => x.Phone).Length(11, 11).WithMessage("格式不正确");
            RuleFor(x => x.Email).NotEmpty().WithMessage("请输入邮箱 ").Length(3, 100).WithMessage("格式不正确").EmailAddress().WithMessage("格式不正确");
            //RuleFor(x => x.ProvinceId).NotEmpty().WithMessage("请选择所在省份").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所在省份");
            //RuleFor(x => x.HospitalId).NotEmpty().WithMessage("请选择所在医院名称").InclusiveBetween(1, long.MaxValue).WithMessage("请选择所在医院名称");
            //if (RuleFor(x => x.HospitalId).Equals(0))
            //{
            //    RuleFor(x => x.HospitalName).NotEmpty().WithMessage("请填写医院名称").Length()
            //}
        }
    }
}