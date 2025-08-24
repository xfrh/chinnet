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
	/// 数据库操作类 ，数据库表名：UserRole 
	/// </summary>
	public partial class UserRoleMap : ManageSystemEntityTypeConfiguration<UserRole>
	{

		public UserRoleMap()
		{
			this.ToTable("UserRole");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        }

	}
}
