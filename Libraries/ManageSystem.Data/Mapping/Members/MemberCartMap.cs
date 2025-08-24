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
	/// 数据库操作类 ，数据库表名：MemberCart 
	/// </summary>
	public partial class MemberCartMap : ManageSystemEntityTypeConfiguration<MemberCart>
	{

		public MemberCartMap()
		{
			this.ToTable("MemberCart");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.ProductName).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
