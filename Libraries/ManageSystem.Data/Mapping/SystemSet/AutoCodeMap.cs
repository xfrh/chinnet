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
	/// 数据库操作类 ，数据库表名：AutoCode 
	/// </summary>
	public partial class AutoCodeMap : ManageSystemEntityTypeConfiguration<AutoCode>
	{

		public AutoCodeMap()
		{
			this.ToTable("AutoCode");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Prefix).HasMaxLength(1000);
            this.Property(p => p.Suffix).HasMaxLength(1000);
        }

	}
}
