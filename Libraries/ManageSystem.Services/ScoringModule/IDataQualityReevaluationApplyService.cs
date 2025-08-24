using ManageSystem.Core;
using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public interface IDataQualityReevaluationApplyService : IBaseService<DataQuality_Reevaluation_Apply>
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="status"></param>
        /// <param name="juryName"></param>
        /// <param name="hospital"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DataQuality_Reevaluation_Apply> QueryPage(string title, string status, string juryName, string hospital, int page, int pageSize);
    }
}
