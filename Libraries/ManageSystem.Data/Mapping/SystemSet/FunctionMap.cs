using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.SystemSet
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：Function 
	/// </summary>
	public partial class FunctionMap : ManageSystemEntityTypeConfiguration<Function>
	{

		public FunctionMap()
		{
			this.ToTable("Function");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.CreateName).HasMaxLength(1000);
            this.Property(p => p.Image).HasMaxLength(1000);
            this.Property(p => p.ControlId).HasMaxLength(4000);
            this.Property(p => p.Url).HasMaxLength(4000);
            this.Property(p => p.Name).HasMaxLength(1000);

            this.Ignore(o => o.FunctionType);

        }

	}
}
