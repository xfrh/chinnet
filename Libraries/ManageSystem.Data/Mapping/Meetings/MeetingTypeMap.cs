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
	/// 数据库操作类 ，数据库表名：MeetingType 
	/// </summary>
	public partial class MeetingTypeMap : ManageSystemEntityTypeConfiguration<MeetingType>
	{

		public MeetingTypeMap()
		{
			this.ToTable("MeetingType");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Name).HasMaxLength(1000);

        }

	}
}
