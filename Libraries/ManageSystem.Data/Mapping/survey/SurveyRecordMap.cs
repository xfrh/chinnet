using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Survey;

namespace ManageSystem.Data.Mapping.Survey
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：Survey_Record 
    /// </summary>
    public partial class SurveyRecordMap : ManageSystemEntityTypeConfiguration<Survey_Record>
	{

		public SurveyRecordMap()
		{
			this.ToTable("Survey_Record");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            
            
        }

	}
}
