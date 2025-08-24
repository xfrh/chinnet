using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Documents;
using ManageSystem.Core;
using System.Linq.Expressions;
using ManageSystem.Core.DynamicLinq;

namespace ManageSystem.Services.Documents
{
    /// <summary>
    /// 操作类 ，数据库表名：DocumentDownloadLog 
    /// </summary>
    public partial class DocumentDownloadLogService : BaseService<DocumentDownloadLog>, IDocumentDownloadLogService
    {

        public DocumentDownloadLogService(IRepository<DocumentDownloadLog> repository) : base(repository)
        {

        }

        public IPagedList<DocumentDownloadLog> QueryPage(long documentId, string name, int page, int pageSize)
        {
            List<int> validMark = new List<int> { 1, 2 };
            Expression<Func<DocumentDownloadLog, bool>> predicate = r => validMark.Contains(r.Mark) && r.DocumentId == documentId;
            if (!string.IsNullOrWhiteSpace(name))
            {
                predicate = predicate.And(r => r.MemberName.Contains(name));
            }

            return QueryPage(predicate, pageIndex: page, pageSize: pageSize);
        }
    }
}
