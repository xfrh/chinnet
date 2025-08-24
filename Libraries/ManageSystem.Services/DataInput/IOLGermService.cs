using ManageSystem.Core.Domain.DataInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial interface IOLGermService
    {
        /// <summary>
        /// 添加细菌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddGerm(OLGermname model);

        /// <summary>
        /// 删除细菌
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int deleteGerm(int Id);
        /// <summary>
        /// 细菌列表
        /// </summary>
        /// <returns></returns>
        List<OLGermname> GetGerm();
    }
}
