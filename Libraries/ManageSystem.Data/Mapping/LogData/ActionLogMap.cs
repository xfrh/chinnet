using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Log;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.Log
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：ActionLog 
	/// </summary>
	public partial class ActionLogMap : ManageSystemEntityTypeConfiguration<ActionLog>
	{

		public ActionLogMap()
		{
			this.ToTable("ActionLog");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }

	}
}
