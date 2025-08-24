using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Documents;

namespace ManageSystem.Services.Documents
{
    /// <summary>
    /// 操作接口类 ，数据库表名：DocumentDownloadLog 
    /// </summary>
    public partial interface IDocumentDownloadLogService : IBaseService<DocumentDownloadLog>
    {
        /// <summary>
        /// 分页查询下载记录
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DocumentDownloadLog> QueryPage(long documentId, string name, int page, int pageSize);
    }
}
