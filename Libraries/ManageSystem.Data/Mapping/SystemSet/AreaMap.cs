using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.SystemSet;


namespace ManageSystem.Data.Mapping.SystemSet
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：Area 
    /// </summary>
    public partial class AreaMap : ManageSystemEntityTypeConfiguration<Area>
	{
		public AreaMap()
		{
			this.ToTable("Area");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.ShortName).HasMaxLength(1000);
            this.Property(p => p.Longitude).HasMaxLength(1000);
            this.Property(p => p.Latitude).HasMaxLength(1000);
            this.Property(p => p.Position).HasMaxLength(1000);
        }

	}
}
