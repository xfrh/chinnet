using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Integrals
{
    /// <summary>
     /// 积分制度设置的操作类型（1、上传医学数据   2、创建信息动态  3、创建科研合作）
     /// </summary>
    public enum IntegralSettingTypeEnum
    {
        [Description("上传医学数据")]
        UploadMedicalData = 1,
        [Description("创建信息动态 ")]
        CreateMeeting = 2,
        [Description("创建科研合作 ")]
        CreateResearch = 3
    }
}
