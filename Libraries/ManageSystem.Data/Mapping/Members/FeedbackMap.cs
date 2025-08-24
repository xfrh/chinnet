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
	/// 数据库操作类 ，数据库表名：Feedback 
	/// </summary>
	public partial class FeedbackMap : ManageSystemEntityTypeConfiguration<Feedback>
	{

		public FeedbackMap()
		{
			this.ToTable("Feedback");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.Content).IsMaxLength();
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
