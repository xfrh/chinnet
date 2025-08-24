using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.SystemSet
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：ValidateCode 
    /// </summary>
    public partial class ValidateCodeMap : ManageSystemEntityTypeConfiguration<ValidateCode>
	{

		public ValidateCodeMap()
		{
			this.ToTable("ValidateCode");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Code).HasMaxLength(1000);
            this.Property(p => p.Value).HasMaxLength(1000);

        }

	}
}
