using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Weixin;


namespace ManageSystem.Data.Mapping.Weixin
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：WeixinUser 
	/// </summary>
	public partial class WeixinUserMap : ManageSystemEntityTypeConfiguration<WeixinUser>
	{

		public WeixinUserMap()
		{
			this.ToTable("WeixinUser");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            this.Property(s => s.Name).IsRequired().HasMaxLength(1000);
            this.Property(s => s.Openid).IsRequired().HasMaxLength(1000);
            this.Property(s => s.Area).IsRequired().HasMaxLength(1000);
        }

	}
}
