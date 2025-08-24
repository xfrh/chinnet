using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Articles;

namespace ManageSystem.Services.Articles
{
	/// <summary>
	/// 操作类 ，数据库表名：ArticleAttachment 
	/// </summary>
	public partial class ArticleAttachmentService :  BaseService<ArticleAttachment>, IArticleAttachmentService
	{

		public ArticleAttachmentService(IRepository<ArticleAttachment> repository): base(repository)
		{
			
		}

	}
}
