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
	/// 数据库操作类 ，数据库表名：MeetingView 
	/// </summary>
	public partial class MeetingViewMap : ManageSystemEntityTypeConfiguration<MeetingView>
	{

		public MeetingViewMap()
		{
			this.ToTable("MeetingView");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.MeetingName).HasMaxLength(1000);
            this.Property(p => p.Ip).HasMaxLength(1000);
            this.Property(p => p.BrowserName).HasMaxLength(1000);
        }

	}
}
