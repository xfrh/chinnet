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
	/// 数据库操作类 ，数据库表名：ProductImage 
	/// </summary>
	public partial class ProductImageMap : ManageSystemEntityTypeConfiguration<ProductImage>
	{
		public ProductImageMap()
		{
			this.ToTable("ProductImage");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.OldFileName).HasMaxLength(1000);
            this.Property(p => p.NewFileName).HasMaxLength(1000);
            this.Property(p => p.Path).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
        }
	}
}
