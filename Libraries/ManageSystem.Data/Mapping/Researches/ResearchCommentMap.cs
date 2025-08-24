using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Researches;


namespace ManageSystem.Data.Mapping.Researches
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：ResearchComment 
    /// </summary>
    public partial class ResearchCommentMap : ManageSystemEntityTypeConfiguration<ResearchComment>
	{

		public ResearchCommentMap()
		{
			this.ToTable("ResearchComment");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.ResearchName).HasMaxLength(1000);
            this.Property(p => p.Ip).HasMaxLength(1000);
            this.Property(p => p.BrowserName).HasMaxLength(1000);
            this.Property(p => p.Content).HasMaxLength(4000);

        }

	}
}
