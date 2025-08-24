using FluentValidation;
using ManageSystem.Admin.Models.Document;
using ManageSystem.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Validators.Document
{
    public partial class DocumentValidator : BaseValidator<DocumentModel>
    {
        public DocumentValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("请输入资料标题");
            RuleFor(x => x.UrlName).NotEmpty().WithMessage("请输入链接文本， 如：《点击下载》");
            RuleFor(x => x.Sort).NotEmpty().WithMessage("排序不能为空");
            RuleFor(x => x.Owners).Must(CheckOwners).WithMessage("请选择资料所属");
        }

        private bool CheckOwners(DocumentModel model, List<int> value)
        {
            return value != null && value.Count > 0;
        }
    }
}