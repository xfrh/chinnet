using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
     /// 医学数据上传处理记录，状态：1  待处理   2：处理失败   3：处理成功
     /// </summary>
    public enum MedicalDataDisposeLogStatusEnum
    {
        [Description("待处理")]
        Wait = 1,
        [Description("处理失败")]
        Failure = 2,
        [Description("处理成功")]
        Success = 3
    }
}
