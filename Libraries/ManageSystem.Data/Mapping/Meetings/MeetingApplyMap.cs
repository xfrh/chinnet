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
	/// 数据库操作类 ，数据库表名：MeetingApply 
	/// </summary>
	public partial class MeetingApplyMap : ManageSystemEntityTypeConfiguration<MeetingApply>
	{

		public MeetingApplyMap()
		{
			this.ToTable("MeetingApply");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.MeetingName).HasMaxLength(1000);
            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);
            this.Property(p => p.Phone).HasMaxLength(1000);
            this.Property(p => p.OrderSN).HasMaxLength(1000);
            this.Property(p => p.CallBackSN).HasMaxLength(1000);

        }

	}
}
