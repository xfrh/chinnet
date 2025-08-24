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
	/// 数据库操作类 ，数据库表名：RoleFunction 
	/// </summary>
	public partial class RoleFunctionMap : ManageSystemEntityTypeConfiguration<RoleFunction>
	{

		public RoleFunctionMap()
		{
			this.ToTable("RoleFunction");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.HasRequired(cc => cc.Role)
            //   .WithMany()
            //   .HasForeignKey(cc => cc.RoleId);

            //this.HasRequired(cc => cc.Function)
            // .WithMany()
            // .HasForeignKey(cc => cc.FunctionId);
        }

	}
}
