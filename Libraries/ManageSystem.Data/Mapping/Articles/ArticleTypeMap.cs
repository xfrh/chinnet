using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Articles;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageSystem.Data.Mapping.Articles
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：ArticleType 
	/// </summary>
	public partial class ArticleTypeMap : ManageSystemEntityTypeConfiguration<ArticleType>
	{

		public ArticleTypeMap()
		{
			this.ToTable("ArticleType");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        }

	}
}
