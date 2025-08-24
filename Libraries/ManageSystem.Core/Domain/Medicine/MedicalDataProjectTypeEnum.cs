using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
     /// 数据上传的项目类型
     /// </summary>
    public enum MedicalDataProjectTypeEnum
    {
        [Description("CHINET中国细菌耐药监测网")]
        CHINET = 1,
        [Description("上海市细菌真菌耐药监测网")]
        Shanghai = 2,
        [Description("浙江省细菌耐药监测网")]
        ZheJiang = 4,
        [Description("河南省细菌耐药监测网")]
        HeNan = 5,
        [Description("其他监测数据")]
        Other = 3,
        [Description("测试数据")]
        Test = 6

    }
}
