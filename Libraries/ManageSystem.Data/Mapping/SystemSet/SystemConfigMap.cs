using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.SystemSet;


namespace ManageSystem.Data.Mapping.SystemSet
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：SystemConfig 
	/// </summary>
	public partial class SystemConfigMap : ManageSystemEntityTypeConfiguration<SystemConfig>
	{

		public SystemConfigMap()
		{
			this.ToTable("SystemConfig");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(s => s.Name).IsRequired().HasMaxLength(1000);
            this.Property(s => s.Key).IsRequired().HasMaxLength(4000);

        }

	}
}
