using FluentValidation;
using ManageSystem.Admin.Models.Chart;
using ManageSystem.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Validators.Chart
{
    public partial class HeatmapValidator : BaseValidator<HeatmapModel>
    {
        public HeatmapValidator()
        {
            RuleFor(x => x.DataSegmentId).NotEmpty().WithMessage("请选择所属数据段").GreaterThan(0).WithMessage("请选择所属数据段");
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入报表名称");
            RuleFor(x => x.Sort).NotEmpty().WithMessage("排序不能为空").GreaterThan(0).WithMessage("排序必须大于0");
            RuleFor(x => x.ItemModel).Must(CheckItemModel).WithMessage("请录入报表数据项");
        }

        private bool CheckItemModel(HeatmapModel model, List<HeatmapItemModel> value)
        {
            return value != null && value.Count > 0;
        }
    }
}