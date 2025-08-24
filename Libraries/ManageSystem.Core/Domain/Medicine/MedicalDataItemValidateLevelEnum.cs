using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
     /// 验证上传医学数据信息的等级
     /// </summary>
    public enum MedicalDataItemValidateLevelEnum
    {
        [Description("提示")]
        Hint = 1,
        [Description("警告")]
        Warning = 2,
        [Description("错误")]
        Error = 3
    }
}
