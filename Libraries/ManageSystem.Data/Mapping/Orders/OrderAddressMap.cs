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
	/// 数据库操作类 ，数据库表名：OrderAddress 
	/// </summary>
	public partial class OrderAddressMap : ManageSystemEntityTypeConfiguration<OrderAddress>
	{

		public OrderAddressMap()
		{
			this.ToTable("OrderAddress");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);
            this.Property(p => p.Area).HasMaxLength(1000);
            this.Property(p => p.Address).HasMaxLength(1000);
            this.Property(p => p.Phone).HasMaxLength(1000);
            this.Property(p => p.ZipPostalCode).HasMaxLength(1000);
            this.Property(p => p.Tel).HasMaxLength(1000);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Remark).IsMaxLength();

        }

	}
}
