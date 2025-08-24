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
    /// 数据库操作类 ，数据库表名：Survey_Survey 
    /// </summary>
    public partial class SurveySurveyMap : ManageSystemEntityTypeConfiguration<Survey_Survey>
	{

		public SurveySurveyMap()
		{
			this.ToTable("Survey_Survey");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            
            
        }

	}
}
