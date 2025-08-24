using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Meetings;


namespace ManageSystem.Data.Mapping.Meetings
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：Meeting 
	/// </summary>
	public partial class MeetingMap : ManageSystemEntityTypeConfiguration<Meeting>
	{

		public MeetingMap()
		{
			this.ToTable("Meeting");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.CoverImage).HasMaxLength(1000);
            this.Property(p => p.Address).HasMaxLength(1000);
            this.Property(p => p.Contact).HasMaxLength(1000);
            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.AreaName).HasMaxLength(1000);
            this.Property(p => p.AreaFullName).HasMaxLength(1000);
        }

	}
}
