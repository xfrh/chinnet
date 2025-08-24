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
	public partial class CRProjectItemMap : ManageSystemEntityTypeConfiguration<CRProjectItem>
	{

		public CRProjectItemMap()
		{
			this.ToTable("CRProjectItem");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
         
            this.Property(p => p.Name).HasMaxLength(1000);
            
            
        }

	}
}
