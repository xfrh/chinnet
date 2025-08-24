using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Members;


namespace ManageSystem.Data.Mapping.Members
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：MemberAddress 
	/// </summary>
	public partial class MemberAddressMap : ManageSystemEntityTypeConfiguration<MemberAddress>
	{

		public MemberAddressMap()
		{
			this.ToTable("MemberAddress");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);
            this.Property(p => p.Address).HasMaxLength(1000);
            this.Property(p => p.Phone).HasMaxLength(1000);
            this.Property(p => p.ZipPostalCode).HasMaxLength(1000);
            this.Property(p => p.Tel).HasMaxLength(1000);
            this.Property(p => p.Remark).IsMaxLength();
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
