using ManageSystem.Core;
using ManageSystem.Core.Domain.ScoringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public interface IDataQualityHospitalService : IBaseService<DataQuality_Hospital>
    {
        /// <summary>
        /// 判断医院是否存在
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        bool CheckHospital(long dataQualityScoreId, long hospitalId);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DataQuality_Hospital> QueryPage(long dataQualityScoreId, int? status, int page, int pageSize = int.MaxValue);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="juryId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DataQuality_Hospital> QueryPage(long dataQualityScoreId, long juryId, int page, int pageSize = int.MaxValue);

        /// <summary>
        /// 物理删除未保存的参与医院
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <returns></returns>
        bool DeleteUnSaveDataQualityHospitalByDataQualityScoreId(long dataQualityScoreId);

        /// <summary>
        /// 获取指定评委在指定评分中分配的医院名称
        /// </summary>
        /// <param name="dataQualityScoreId"></param>
        /// <param name="juryId"></param>
        /// <returns></returns>
        List<string> GetHospitalNameByJuryIdWithDataQualityScore(long dataQualityScoreId, long juryId);
    }
}
