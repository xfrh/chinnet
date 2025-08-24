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
	/// 数据库操作类 ，数据库表名：OrderLog 
	/// </summary>
	public partial class OrderLogMap : ManageSystemEntityTypeConfiguration<OrderLog>
	{

		public OrderLogMap()
		{
			this.ToTable("OrderLog");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Type).HasMaxLength(1000);
            this.Property(p => p.Source).HasMaxLength(1000);
            this.Property(p => p.UserName).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Content).IsMaxLength();
            this.Property(p => p.JsonData).IsMaxLength();
        }

	}
}
