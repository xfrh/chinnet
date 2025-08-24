using FluentValidation;
using ManageSystem.Admin.Models.SHChart;
using ManageSystem.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Validators.SHChart
{
    public class SHBarChartTableValidator : BaseValidator<SHBarChartTableModel>
    {
        public SHBarChartTableValidator()
        {
            RuleFor(x => x.DataSegmentId).NotEmpty().WithMessage("请选择所属数据段").GreaterThan(0).WithMessage("请选择所属数据段");
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入报表名称");
            RuleFor(x => x.Title).NotEmpty().WithMessage("请输入报表标题");
            RuleFor(x => x.SubTitle).NotEmpty().WithMessage("请输入报表副标题");
            RuleFor(x => x.MobileDisplayScale).NotEmpty().WithMessage("移动端显示比例").Must(CheckMobileDisplayScale).WithMessage("显示比例取值必须是在1-100之间");
            RuleFor(x => x.Sort).NotEmpty().WithMessage("排序不能为空").GreaterThan(0).WithMessage("排序必须大于0");           
        }
        private bool CheckMobileDisplayScale(SHBarChartTableModel model, double value)
        {
            return value >= 1 && value <= 100;
        }

       
    }
}