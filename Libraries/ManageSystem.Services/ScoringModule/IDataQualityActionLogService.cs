using ManageSystem.Core;
using ManageSystem.Core.Domain.ScoringModule.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public interface IDataQualityActionLogService : IBaseService<DataQuality_Action_Log>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="userName"></param>
        /// <param name="actionType"></param>
        /// <param name="content"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DataQuality_Action_Log> QueryPage(long dataQualityScoreId, string userName, string actionType, string content, DateTime? begin, DateTime? end, int page, int pageSize = int.MaxValue);
    }
}
