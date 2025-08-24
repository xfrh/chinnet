using FluentValidation;
using ManageSystem.Admin.Models.Products;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Products
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ProductBrand 
	/// </summary>
	public partial class ProductBrandValidator : BaseValidator<ProductBrandModel>
	{

		public ProductBrandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入品牌名称");

		}

	}
}
