using FluentValidation;
using ManageSystem.Admin.Models.Survey;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Survey
{
	/// <summary>
	/// 数据验证类 ，数据库表名：MedicalData 
	/// </summary>
	public partial class SurveySurveyValidator : BaseValidator<SurveySurveyModel>
	{

		public SurveySurveyValidator()
		{

		}

	}
}
