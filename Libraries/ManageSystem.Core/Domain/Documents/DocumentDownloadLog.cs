using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Documents
{
    /// <summary>
    /// 资料下载，文档下载记录
    /// </summary>
    public class DocumentDownloadLog : BaseEntity
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public long DocumentId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// 下载人Id
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 下载人姓名
        /// </summary>
        public string MemberName { get; set; }

    }
}
