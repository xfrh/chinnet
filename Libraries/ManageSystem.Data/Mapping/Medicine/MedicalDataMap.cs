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
	public partial class MedicalDataMap : ManageSystemEntityTypeConfiguration<MedicalData>
	{

		public MedicalDataMap()
		{
			this.ToTable("MedicalData");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.SN).HasMaxLength(1000);
            this.Property(p => p.HospitalDepartmentName).HasMaxLength(1000);
            this.Property(p => p.HospitalName).HasMaxLength(1000);
            this.Property(p => p.UploadFilePath).HasMaxLength(1000);
            this.Property(p => p.SpecimenName).HasMaxLength(1000);
            this.Property(p => p.BacteriaTypeIds).HasMaxLength(1000);
            this.Property(p => p.BacteriaTypeName).HasMaxLength(1000);
            this.Property(p => p.UploadMessage).IsMaxLength();
            this.Property(p => p.AreName).HasMaxLength(1000);
            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.FileName).HasMaxLength(1000);
            this.Property(p => p.CodeFilePath).HasMaxLength(1000);
            this.Property(p => p.WordFile).HasMaxLength(1000);
            
        }

	}
}
