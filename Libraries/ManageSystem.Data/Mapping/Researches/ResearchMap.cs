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
	/// 数据库操作类 ，数据库表名：Research 
	/// </summary>
	public partial class ResearchMap : ManageSystemEntityTypeConfiguration<Research>
	{

		public ResearchMap()
		{
			this.ToTable("Research");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Code).HasMaxLength(1000);
            this.Property(p => p.AreaName).HasMaxLength(1000);
            this.Property(p => p.AreaFullName).HasMaxLength(1000);
            this.Property(p => p.Require).HasMaxLength(1000);
            this.Property(p => p.Author).HasMaxLength(1000);
            this.Property(p => p.CoverImage).HasMaxLength(1000);
            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.Remark).HasMaxLength(1000);
        }

	}
}
