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
    /// 数据库操作类 ，数据库表名：Document 
    /// </summary>
    public partial class DocumentMap : ManageSystemEntityTypeConfiguration<Document>
	{

		public DocumentMap()
		{
			this.ToTable("Document");
			this.HasKey(p => p.Id);

			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Name).HasMaxLength(1000);
            this.Property(p => p.FilePath).HasMaxLength(1000);
            this.Property(p => p.UrlName).HasMaxLength(1000);
            this.Property(p => p.Url).HasMaxLength(1000);
            this.Property(p => p.Remark).IsMaxLength();
            this.Property(p => p.Describe).IsMaxLength();
        }

	}
}
