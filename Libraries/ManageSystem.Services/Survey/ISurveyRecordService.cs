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
    public partial interface ISurveyRecordService : IBaseService<Survey_Record>
    {

        /// <summary>
        /// 分页查询数据 后台
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Survey_Record> QueryPage(long surveyId, string content29, string content30, string content31, int pageIndex = 0, int pageSize = int.MaxValue);
        
        /// <summary>
        /// 会员中心，CR复敏信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        IQueryable<Survey_Record> Query(long memberId);
        
        /// <summary>
        /// 修改CR数据
        /// </summary>
        /// <param name="entity">CR数据</param>
        /// <param name="member">当前登录用户</param>
        /// <returns>SUCCESS 表示成功，其他则是错误信息</returns>
        string Update(Survey_Record entity, Member member);

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ids">需要导出的项目id集合，为空则导出全部</param>
        /// <param name="ep">excel导出组建</param>
        /// <returns></returns>
        string Export(List<long> ids, ExcelPackage ep);

    }
}
