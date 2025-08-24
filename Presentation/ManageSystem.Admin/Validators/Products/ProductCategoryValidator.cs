using FluentValidation;
using ManageSystem.Admin.Models.Products;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Products
{
	/// <summary>
	/// 数据验证类 ，数据库表名：ProductCategory 
	/// </summary>
	public partial class ProductCategoryValidator : BaseValidator<ProductCategoryModel>
	{

		public ProductCategoryValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("请输入分类名称");

		}

	}
}
