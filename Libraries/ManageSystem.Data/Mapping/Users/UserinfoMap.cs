using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.Users
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：Userinfo 
	/// </summary>
	public partial class UserinfoMap : ManageSystemEntityTypeConfiguration<Userinfo>
	{

		public UserinfoMap()
		{
			this.ToTable("Userinfo");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.LoginId).HasMaxLength(1000);
            this.Property(p => p.Password).HasMaxLength(1000);
            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.NickName).HasMaxLength(1000);
            this.Property(p => p.Phone).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);
            this.Property(p => p.Url).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();

            this.Ignore(p => p.UserinfoState);
            this.Ignore(p => p.FunctionList);
            this.Ignore(p => p.RoleList);
            

        }

	}
}
