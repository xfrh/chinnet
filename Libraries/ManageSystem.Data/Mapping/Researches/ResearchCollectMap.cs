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
    /// 数据库操作类 ，数据库表名：ResearchCollect 
    /// </summary>
    public partial class ResearchCollectMap : ManageSystemEntityTypeConfiguration<ResearchCollect>
	{

		public ResearchCollectMap()
		{
			this.ToTable("ResearchCollect");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

		}

	}
}
