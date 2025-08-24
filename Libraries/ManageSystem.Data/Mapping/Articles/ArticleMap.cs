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
    /// 数据库操作类 ，数据库表名：Article 
    /// </summary>
    public partial class ArticleMap : ManageSystemEntityTypeConfiguration<Article>
    {

        public ArticleMap()
        {
            this.ToTable("Article");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(s => s.Name).IsRequired().HasMaxLength(1000);
            this.Property(s => s.CoverImage).IsRequired().HasMaxLength(1000);
            this.Property(s => s.Author).IsRequired().HasMaxLength(1000);
            this.Property(s => s.CodeImage).IsRequired().HasMaxLength(1000);
        }

    }
}
