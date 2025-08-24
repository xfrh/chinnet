using FluentValidation;
using ManageSystem.Admin.Models.Project;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Project
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MedicalData 
	/// </summary>
	public partial class ProjectDataValidator : BaseValidator<ProjectDataModel>
	{

		public ProjectDataValidator()
		{

		}

	}
}
