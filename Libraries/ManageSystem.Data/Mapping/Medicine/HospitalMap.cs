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
	/// 数据库操作类 ，数据库表名：Hospital 
	/// </summary>
	public partial class HospitalMap : ManageSystemEntityTypeConfiguration<Hospital>
	{

		public HospitalMap()
		{
			this.ToTable("Hospital");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.Address).HasMaxLength(1000);
            this.Property(p => p.ContactsUser).HasMaxLength(1000);
            this.Property(p => p.Content).IsMaxLength();
            this.Property(p => p.ContactsTel).HasMaxLength(1000);
            this.Property(p => p.Code).HasMaxLength(1000);
            this.Property(p => p.ProvinceName).HasMaxLength(1000);
        }

	}

    /// <summary>
    /// 数据库操作类 ，多中心研究，数据库表名：ProjectHospital 
    /// </summary>
    public partial class ProjectHospitalMap : ManageSystemEntityTypeConfiguration<ProjectHospital>
    {
		public ProjectHospitalMap()
		{
			this.ToTable("ProjectHospital");
			this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        }
    }
}
