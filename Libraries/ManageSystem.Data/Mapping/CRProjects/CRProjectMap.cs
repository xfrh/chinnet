using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.CRProjects;

namespace ManageSystem.Data.Mapping.CRProjects
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：MedicalData 
	/// </summary>
	public partial class CRProjectMap : ManageSystemEntityTypeConfiguration<CRProject>
	{

		public CRProjectMap()
		{
			this.ToTable("CRProject");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.HospitalName).HasMaxLength(1000);
            this.Property(p => p.HospitalGrade).HasMaxLength(100);
            this.Property(p => p.Name).HasMaxLength(100);
            this.Property(p => p.UploadFilePath).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(500);
            this.Property(p => p.Area).HasMaxLength(1000);
            this.Property(p => p.Address).HasMaxLength(1000);
            
            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.FileName).HasMaxLength(1000);
            
            
        }

	}
}
