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
	/// 数据库操作类 ，数据库表名：MedicalData 
	/// </summary>
	public partial class MedicalDataProjectMap : ManageSystemEntityTypeConfiguration<MedicalDataProject>
	{

		public MedicalDataProjectMap()
		{
			this.ToTable("MedicalDataProject");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Name).HasMaxLength(500);
            this.Property(p => p.Remark).HasMaxLength(4000);
        }

	}
}
