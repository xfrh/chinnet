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
	/// 数据库操作类 ，数据库表名：MemberIntegralLog 
	/// </summary>
	public partial class MemberIntegralLogMap : ManageSystemEntityTypeConfiguration<MemberIntegralLog>
	{

		public MemberIntegralLogMap()
		{
			this.ToTable("MemberIntegralLog");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Type).HasMaxLength(1000);
            this.Property(p => p.Source).HasMaxLength(1000);
            this.Property(p => p.Remark).IsMaxLength();
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
