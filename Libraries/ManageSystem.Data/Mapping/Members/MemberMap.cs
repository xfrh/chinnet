using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Users;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Data.Mapping.Users
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：Member 
    /// </summary>
    public partial class MemberMap : ManageSystemEntityTypeConfiguration<Member>
	{

		public MemberMap()
		{
			this.ToTable("Member");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.LoginId).HasMaxLength(1000);
            this.Property(p => p.Password).HasMaxLength(1000);
            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.NickName).HasMaxLength(1000);
            this.Property(p => p.Phone).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);
            this.Property(p => p.OpenId).HasMaxLength(1000);

            this.Property(p => p.HeadImage).HasMaxLength(1000);
            this.Property(p => p.InputInviteCode).HasMaxLength(1000);
            this.Property(p => p.InviteCode).HasMaxLength(1000);
            this.Property(p => p.OtherHospital).HasMaxLength(1000);
            this.Property(p => p.MedicineEmail).HasMaxLength(1000);
            
        }

	}
}
