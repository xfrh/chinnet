using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Integrals;


namespace ManageSystem.Data.Mapping.Integrals
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：IntegralSetting 
	/// </summary>
	public partial class IntegralSettingMap : ManageSystemEntityTypeConfiguration<IntegralSetting>
	{

		public IntegralSettingMap()
		{
			this.ToTable("IntegralSetting");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Remark).IsMaxLength();
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
