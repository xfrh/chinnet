using FluentValidation;
using ManageSystem.Admin.Models.Chart;
using ManageSystem.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Validators.Chart
{
    public partial class DataSegmentValidator : BaseValidator<DataSegmentModel>
    {
        public DataSegmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入数据段名称");
            RuleFor(x => x.ProjectType).IsInEnum().WithMessage("值无效");
            RuleFor(x => x.Sort).NotEmpty().WithMessage("排序不能为空").GreaterThan(0).WithMessage("排序必须大于0");
            //RuleFor(x => x.Owners).Must(CheckOwners).WithMessage("请选择资料所属");
        }

        //private bool CheckOwners(DocumentModel model, List<int> value)
        //{
        //    return value != null && value.Count > 0;
        //}
    }
}