using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Members;


namespace ManageSystem.Data.Mapping.Members
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：MemberAttestation 
	/// </summary>
	public partial class MemberAttestationMap : ManageSystemEntityTypeConfiguration<MemberAttestation>
	{

		public MemberAttestationMap()
		{
			this.ToTable("MemberAttestation");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.AreaName).HasMaxLength(1000);
            this.Property(p => p.HospitalName).HasMaxLength(1000);
            this.Property(p => p.HospitalDepartmentName).HasMaxLength(1000);
            this.Property(p => p.DoctorTitleName).HasMaxLength(1000);
        }

	}
}
