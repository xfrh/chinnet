using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Integrals;
using ManageSystem.Core.Domain.Datas;

namespace ManageSystem.Data.Mapping.Integrals
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：DataAntibioticDrugFast 
    /// </summary>
    public partial class DataAntibioticDrugFastMap : ManageSystemEntityTypeConfiguration<DataAntibioticDrugFast>
	{

		public DataAntibioticDrugFastMap()
		{
			this.ToTable("DataAntibioticDrugFast");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.AntibioticCode).HasMaxLength(1000);
            this.Property(p => p.AntibioticName).HasMaxLength(1000);
            this.Property(p => p.GermCode).HasMaxLength(1000);
            this.Property(p => p.GermName).HasMaxLength(1000);

            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
