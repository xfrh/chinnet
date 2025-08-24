using FluentValidation;
using ManageSystem.Web.Models.Medicine;
using ManageSystem.Framework.Validators;
using ManageSystem.Web.Models.Project;

namespace ManageSystem.Web.Validators.Project
{
    /// <summary>
    /// 数据验证类 ，数据库表名：CRProject
    /// </summary>
    public partial class ProjectValidator : BaseValidator<CRProjectModel>
	{
		public ProjectValidator()
		{

		}

	}

 
}
