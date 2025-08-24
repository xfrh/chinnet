using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Core.Domain.ScoringModule.Enum;
using ManageSystem.Core.Domain.ScoringModule.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.ScoringModule
{
    public interface IDataQualityScoreService : IBaseService<DataQuality_Score>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="title"></param>
        /// <param name="intro"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DataQuality_Score> QueryPage(string title, string intro, DataQuality_Score_Status? status, int page, int pageSize = int.MaxValue);
        /// <summary>
        /// 移动端分页查询
        /// </summary>
        /// <param name="juryId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<DataQuality_Score> QueryPageWithWap(long juryId, int page, int pageSize = int.MaxValue);

        /// <summary>
        /// 获取某医院最近一次有效上传的数据
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        long GetLastValidMedicalDataId(long hospitalId);

        List<MedicalAntibioticResult> QueryMedicalAntibioticResult(long medicalDataId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organismId">细菌ID</param>
        /// <param name="hospitalId">医院ID</param>
        /// <param name="fields">药物字段</param>
        /// <param name="value">
        /// 此字段取值如下：<br />
        /// 1：敏感<br />
        /// 2：中介<br />
        /// 3：耐药<br />
        /// 其他值无效
        /// </param>
        /// <returns></returns>
        int Count(long organismId, long hospitalId, int value, params string[] fields);

        /// <summary>
        /// 评分页面重点监测耐药菌关键数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        DataQuality_Score_MedicalAntibioticResult_Statistic GetCount(int year, long hospitalId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        int GetTotalCount(int year, long? hospitalId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        int GetMRSACount(int year, long? hospitalId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        int GetVREfmCount(int year, long? hospitalId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        int GetCRKPCount(int year, long? hospitalId = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        int GetCRPACount(int year, long? hospitalId = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        int GetCRABCount(int year, long? hospitalId = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        int GetCTX_CRO_R_EcoCount(int year, long? hospitalId = null);

        /// <summary>
        /// 评分页面自动计算
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void HospitalAutoPageInit(long id);

        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="hid"></param>
        /// <returns></returns>
        IEnumerable<DataQuality_Hospital_PageInit> GetHospitalPageInit(long sid, long hid);
    }
}
