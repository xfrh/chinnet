using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Log;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.Log
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：SystemLog 
	/// </summary>
	public partial class SystemLogMap : ManageSystemEntityTypeConfiguration<SystemLog>
	{
		public SystemLogMap()
		{
			this.ToTable("SystemLog");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Ignore(o => o.Level);
        }

	}
}
