using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine
{
    public partial interface IMedicalDataHandleLockService
    {
        /// <summary>
        /// 新增处理
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool Insert(long medicalDataId, int status);
        /// <summary>
        /// 出现错误
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        bool Error(long medicalDataId, int status, string remark);
        /// <summary>
        /// 状态更新完成
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool Complete(long medicalDataId, int status);
        /// <summary>
        /// 获取状态<br />
        /// 不存在时返回0
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        int GetStatusByMedicalDataId(long medicalDataId);
    }
}
