using FluentValidation;
using ManageSystem.Admin.Models.Products;
using ManageSystem.Framework.Validators;

namespace ManageSystem.Admin.Validators.Products
{
	/// <summary>
	/// 数据验证类 ，数据库表名：Product 
	/// </summary>
	public partial class ProductValidator : BaseValidator<ProductModel>
	{

		public ProductValidator()
		{
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入商品名称");
            RuleFor(x => x.Status).NotEmpty().WithMessage("请输入选择商品状态").InclusiveBetween(1,10).WithMessage("请输入选择商品状态");
            RuleFor(x => x.ProductBrandId).NotEmpty().WithMessage("请选择商品品牌").InclusiveBetween(1, long.MaxValue).WithMessage("请选择商品品牌");
            RuleFor(x => x.ProductCategoryId).NotEmpty().WithMessage("请输入选择商品分类").InclusiveBetween(1, long.MaxValue).WithMessage("请输入选择商品分类");
            RuleFor(x => x.MarketPrice).NotEmpty().WithMessage("请输入市场价").InclusiveBetween(0, decimal.MaxValue).WithMessage("请输入市场价");
            RuleFor(x => x.CostPrice).NotEmpty().WithMessage("请输入成本价").InclusiveBetween(0, decimal.MaxValue).WithMessage("请输入成本价");
            RuleFor(x => x.Count).NotEmpty().WithMessage("请输入库存数量").InclusiveBetween(0, int.MaxValue).WithMessage("请输入成本价");
            RuleFor(x => x.ProductImageJson).NotEmpty().WithMessage("请选择商品图片");
            
        }

	}
}
