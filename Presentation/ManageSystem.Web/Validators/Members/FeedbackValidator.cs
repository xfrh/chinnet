using FluentValidation;
using ManageSystem.Web.Models.Members;
using ManageSystem.Framework.Validators;
using System.Text.RegularExpressions;
using ManageSystem.Core.Utility;

namespace ManageSystem.Web.Validators.Members
{
    /// <summary>
    /// 数据验证类 ，数据库表名：Feedback 
    /// </summary>
    public partial class FeedbackValidator : BaseValidator<FeedbackModel>
    {
        public FeedbackValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入标题").Length(5, 2000).WithMessage("标题长度必须大于5个字符");
            RuleFor(x => x.Email).NotEmpty().WithMessage("请输入邮箱地址");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("请输入手机号码").Must(ValidMobile).WithMessage("请输入正确的手机号码");
        }
        private bool ValidMobile(FeedbackModel model, string mobile)
        {
            if (model == null || string.IsNullOrWhiteSpace(mobile))
            {
                return false;
            }
            return new Regex(ConfigHelper.GetConfigString("regex.mobile")).IsMatch(mobile);
        }
    }
}
