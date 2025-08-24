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
	/// 数据库操作类 ，数据库表名：ResearchType 
	/// </summary>
	public partial class ResearchTypeMap : ManageSystemEntityTypeConfiguration<ResearchType>
	{

		public ResearchTypeMap()
		{
			this.ToTable("ResearchType");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Name).HasMaxLength(1000);
        }

	}
}
