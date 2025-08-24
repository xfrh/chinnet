using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Medicine;


namespace ManageSystem.Data.Mapping.Medicine
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：BacteriaType 
	/// </summary>
	public partial class BacteriaTypeMap : ManageSystemEntityTypeConfiguration<BacteriaType>
	{

		public BacteriaTypeMap()
		{
			this.ToTable("BacteriaType");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
