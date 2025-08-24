using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Orders;


namespace ManageSystem.Data.Mapping.Orders
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：OrderItem 
    /// </summary>
    public partial class OrderItemMap : ManageSystemEntityTypeConfiguration<OrderItem>
    {

        public OrderItemMap()
        {
            this.ToTable("OrderItem");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.ProductCode).HasMaxLength(1000);
            this.Property(p => p.ProductName).HasMaxLength(1000);
            this.Property(p => p.ProductImage).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Remark).IsMaxLength();
        }

    }
}
