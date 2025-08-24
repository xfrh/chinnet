using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public interface IDataQualityJuryService : IBaseService<DataQuality_Jury>
    {
        /// <summary>
        /// 获取评委名称
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="dataQualityHospitalId"></param>
        /// <returns></returns>
        List<string> GetJuryNames(long dataQualityScoreId, long dataQualityHospitalId);

        /// <summary>
        /// 获取评分的所有评委ID
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <returns></returns>
        IEnumerable<long> GetJuryIdByDataQualityScoreId(long dataQualityScoreId);

        /// <summary>
        /// 获取评委数量
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <returns></returns>
        int GetJuryCountByDataQualityScoreId(long dataQualityScoreId);

    }
}
