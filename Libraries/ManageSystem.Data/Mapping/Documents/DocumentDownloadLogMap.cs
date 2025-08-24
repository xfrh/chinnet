using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Documents;


namespace ManageSystem.Data.Mapping.Documents
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：DocumentDownloadLog 
    /// </summary>
    public partial class DocumentDownloadLogMap : ManageSystemEntityTypeConfiguration<DocumentDownloadLog>
	{

		public DocumentDownloadLogMap()
		{
			this.ToTable("DocumentDownloadLog");
			this.HasKey(p => p.Id);

			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.DocumentName).HasMaxLength(1000);
            this.Property(p => p.MemberName).HasMaxLength(1000);
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
