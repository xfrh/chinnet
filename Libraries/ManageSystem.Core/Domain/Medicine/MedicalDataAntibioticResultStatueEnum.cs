using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
    /// 医学数据传数据耐药生成结果状态，1：待处理    2：处理中   3：成功  4：失败
    /// </summary>
    public enum MedicalDataAntibioticResultStatueEnum
    {
        [Description("待处理")]
        Wait = 1,
        [Description("处理中")]
        Processs = 2,
        [Description("成功")]
        Success = 3,
        [Description("失败")]
        Failure = 4
    }
}
