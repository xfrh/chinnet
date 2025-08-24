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
	/// 数据库操作类 ，数据库表名：MedicalDataDisposeLog 
	/// </summary>
	public partial class MedicalDataDisposeLogMap : ManageSystemEntityTypeConfiguration<MedicalDataDisposeLog>
	{

		public MedicalDataDisposeLogMap()
		{
			this.ToTable("MedicalDataDisposeLog");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Data).HasMaxLength(4000);
            this.Property(p => p.Result).HasMaxLength(1000);
        }

	}
}
