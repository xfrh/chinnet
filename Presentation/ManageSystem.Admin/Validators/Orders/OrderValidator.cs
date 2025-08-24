using FluentValidation;
using ManageSystem.Admin.Models.Orders;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Orders
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Order 
	/// </summary>
	public partial class OrderValidator : BaseValidator<OrderModel>
	{

		public OrderValidator()
		{
            RuleFor(x => x.AddressName).NotEmpty().WithMessage("请输入收货人姓名").Length(2, 20).WithMessage("收货人姓名格式不正确");
            RuleFor(x => x.AddressPhone).NotEmpty().WithMessage("请输入手机号码").Length(11, 11).WithMessage("手机号码格式不正确");
            RuleFor(x => x.AddressAddress).NotEmpty().WithMessage("请输入详细地址").Length(2, 100).WithMessage("详细地址格式不正确");
        }

	}
}
