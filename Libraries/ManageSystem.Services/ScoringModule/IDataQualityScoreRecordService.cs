using ManageSystem.Core;
using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public interface IDataQualityScoreRecordService : IBaseService<DataQuality_Score_Record>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="juryName"></param>
        /// <param name="hospital"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DataQuality_Score_Record> QueryPage(long dataQualityScoreId, string juryName, string hospital, DateTime? begin, DateTime? end, int page, int pageSize = int.MaxValue);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        bool Insert(DataQuality_Score_Record model, List<DataQuality_Score_Detail> details);
    }
}
