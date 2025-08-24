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
	/// 数据库操作类 ，数据库表名：MeetingCollect 
	/// </summary>
	public partial class MeetingCollectMap : ManageSystemEntityTypeConfiguration<MeetingCollect>
	{

		public MeetingCollectMap()
		{
			this.ToTable("MeetingCollect");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

		}

	}
}
