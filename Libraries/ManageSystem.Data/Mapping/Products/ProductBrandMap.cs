using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Products;


namespace ManageSystem.Data.Mapping.Products
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：ProductBrand 
	/// </summary>
	public partial class ProductBrandMap : ManageSystemEntityTypeConfiguration<ProductBrand>
	{

		public ProductBrandMap()
		{
			this.ToTable("ProductBrand");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
