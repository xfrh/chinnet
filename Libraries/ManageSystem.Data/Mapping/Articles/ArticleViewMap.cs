using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Articles;


namespace ManageSystem.Data.Mapping.Articles
{
	/// <summary>
	/// 数据库操作类 ，数据库表名：ArticleView 
	/// </summary>
	public partial class ArticleViewMap : ManageSystemEntityTypeConfiguration<ArticleView>
	{

		public ArticleViewMap()
		{
			this.ToTable("ArticleView");
			this.HasKey(p => p.Id);
			this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(s => s.MemberName).IsRequired().HasMaxLength(1000);
            this.Property(s => s.ArticleName).IsRequired().HasMaxLength(1000);
        }

	}
}
