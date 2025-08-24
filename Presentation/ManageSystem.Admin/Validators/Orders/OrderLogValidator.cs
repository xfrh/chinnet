using FluentValidation;
using ManageSystem.Admin.Models.Orders;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Orders
{
	/// <summary>
	/// 数据验证类 ，数据库表名：OrderLog 
	/// </summary>
	public partial class OrderLogValidator : BaseValidator<OrderLogModel>
	{

		public OrderLogValidator()
		{

		}

	}
}
