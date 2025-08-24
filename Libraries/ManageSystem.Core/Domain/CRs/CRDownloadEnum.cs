using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.CRs
{
    public enum CRDownloadEnum : byte
    {
        /// <summary>
        /// 原始文件
        /// </summary>
        [Description("原始文件")]
        Original,
        /// <summary>
        /// 容错文件
        /// </summary>
        [Description("容错文件")]
        FaultTolerant
    }
}
