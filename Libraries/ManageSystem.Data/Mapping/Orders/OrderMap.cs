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
	/// 数据库操作类 ，数据库表名：Order 
	/// </summary>
	public partial class OrderMap : ManageSystemEntityTypeConfiguration<Order>
	{
		public OrderMap()
		{
			this.ToTable("Order");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.SN).HasMaxLength(1000);
            this.Property(p => p.PayType).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Remark).IsMaxLength();
            this.Property(p => p.SystemRemark).IsMaxLength();
            this.Property(p => p.ExpressCompany).HasMaxLength(1000);
            this.Property(p => p.ExpressSN).HasMaxLength(1000);
            this.Property(p => p.ExpressRemark).HasMaxLength(1000);

        }

	}
}
