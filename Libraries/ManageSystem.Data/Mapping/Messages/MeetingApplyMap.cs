using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Domain.Messages;

namespace ManageSystem.Data.Mapping.Messages
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：MessageEmail 
    /// </summary>
    public partial class MessageEmailMap : ManageSystemEntityTypeConfiguration<MessageEmail>
	{
		public MessageEmailMap()
		{
			this.ToTable("MessageEmail");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);
            this.Property(p => p.Title).HasMaxLength(1000);
            this.Property(p => p.Content).IsMaxLength();
            this.Property(p => p.SceneType).HasMaxLength(1000);
            this.Property(p => p.Source).HasMaxLength(1000);
            this.Property(p => p.Remark).HasMaxLength(1000);
        }

	}
}
