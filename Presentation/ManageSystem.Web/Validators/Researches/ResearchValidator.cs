using FluentValidation;
using ManageSystem.Web.Models.Researches;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Web.Validators.Researches
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Research 
	/// </summary>
	public partial class ResearchValidator : BaseValidator<ResearchModel>
	{

		public ResearchValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入会议主题").Length(2, 100).WithMessage("会议主题长度必须在2-100个字符之间");
            RuleFor(x => x.CoverImage).NotEmpty().WithMessage("请选择海报图片");
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("请选择科研时间");
            RuleFor(x => x.Code).NotEmpty().WithMessage("请输入研究编码").Length(2, 100).WithMessage("研究编码长度必须在2-100个字符之间");
            RuleFor(x => x.ResearchTypeId).NotEmpty().WithMessage("请选择科研类型").InclusiveBetween(1, long.MaxValue).WithMessage("请选择科研类型");
            RuleFor(x => x.AreaId).NotEmpty().WithMessage("请选择科研区域").InclusiveBetween(1, long.MaxValue).WithMessage("请选择科研区域");
            RuleFor(x => x.Remark).NotEmpty().WithMessage("请输入研究简介").Length(2, 200).WithMessage("研究简介长度必须在2-200个字符之间");
            RuleFor(x => x.Require).NotEmpty().WithMessage("请输入参与要求").Length(2, 200).WithMessage("参与要求长度必须在2-200个字符之间");
            RuleFor(x => x.Author).NotEmpty().WithMessage("请输入科研发起人").Length(2,20).WithMessage("科研发起人长度必须在2-2个字符之间");

		}

	}
}
