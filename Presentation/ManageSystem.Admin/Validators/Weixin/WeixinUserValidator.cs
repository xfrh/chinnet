using FluentValidation;
using ManageSystem.Admin.Models.Weixin;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Weixin
{
	/// <summary>
	/// 数据验证类 ，数据库表名：WeixinUser 
	/// </summary>
	public partial class WeixinUserValidator : BaseValidator<WeixinUserModel>
	{

		public WeixinUserValidator()
		{

		}

	}
}
