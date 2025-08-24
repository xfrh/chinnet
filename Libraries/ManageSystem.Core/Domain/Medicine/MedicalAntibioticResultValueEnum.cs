using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
    /// 医学数据项抗生素检测结果 （0：无效值  1：敏感   2：中介   3：耐药）
    /// </summary>
    public enum MedicalAntibioticResultValueEnum
    {
        [Description("无效值")]
        Invalid = 0,
        [Description("敏感")]
        Sensitive = 1,
        [Description("中介")]
       Intermediary = 2,
        [Description("耐药")]
        Resistance = 3
    }
}
