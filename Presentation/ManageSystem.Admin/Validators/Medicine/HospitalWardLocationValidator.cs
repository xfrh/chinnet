using FluentValidation;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Framework.Validators;
using ManageSystem.Services.Medicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Validators.Medicine
{
    public class HospitalWardLocationValidator : BaseValidator<HospitalWardLocationModel>
    {
        public HospitalWardLocationValidator()
        {
            RuleFor(x => x.HospitalID).NotEmpty().WithMessage("请输入或选择医院名称!");
            RuleFor(x => x.Ward).NotEmpty().WithMessage("Ward不能为空!").Must(Checked).WithMessage("Ward不能为空！");
            RuleFor(x => x.Department).NotEmpty().WithMessage("Department不能为空!").Must(Checked).WithMessage("Department不能为空！");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location不能为空!").Must(Checked).WithMessage("Location不能为空！");
            RuleFor(x => x.LocationType).NotEmpty().WithMessage("Location_Type不能为空!").Must(Checked).WithMessage("Location_Type不能为空！");
        }

        private bool Checked(HospitalWardLocationModel item, string value)
        {
            return !(string.IsNullOrWhiteSpace(item.Ward) && string.IsNullOrWhiteSpace(item.Department) && string.IsNullOrWhiteSpace(item.Location) && string.IsNullOrWhiteSpace(item.LocationType));
        }
    }
}