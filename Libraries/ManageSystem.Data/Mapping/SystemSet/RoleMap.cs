using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.SystemSet
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：Role 
	/// </summary>
	public partial class RoleMap : ManageSystemEntityTypeConfiguration<Role>
	{

		public RoleMap()
		{
			this.ToTable("Role");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.CreateName).HasMaxLength(1000);
            this.Property(p => p.Name).HasMaxLength(1000);
        }

	}
}
