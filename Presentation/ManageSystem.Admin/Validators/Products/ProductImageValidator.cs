using FluentValidation;
using ManageSystem.Admin.Models.Products;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Products
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ProductImage 
	/// </summary>
	public partial class ProductImageValidator : BaseValidator<ProductImageModel>
	{

		public ProductImageValidator()
		{

        }

	}
}
