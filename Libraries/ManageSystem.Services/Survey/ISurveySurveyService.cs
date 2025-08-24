using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Members;
using OfficeOpenXml;
using ManageSystem.Core.Domain.Survey;

namespace ManageSystem.Services.Survey
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MedicalData 
    /// </summary>
    public partial interface ISurveySurveyService : IBaseService<Survey_Survey>
    {

        /// <summary>
        /// 分页查询数据 后台
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="name">调查标题</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Survey_Survey> QueryPage(string name, long hospitalId, int pageIndex = 0, int pageSize = int.MaxValue);
        
        /// <summary>
        /// 会员中心，CR复敏信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        IQueryable<Survey_Survey> Query(long memberId);
        
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ep">excel导出组建</param>
        /// <returns></returns>
        string Export(ExcelPackage ep, long surveyId, string content29, string content30, string content31);

    }
}
