using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageSystem.Core;
using ManageSystem.Core.Domain.CRs;
using ManageSystem.Core.Domain.Members;
using OfficeOpenXml;

namespace ManageSystem.Services.CRs
{
    public partial interface  ICRService : IBaseService<CRData>
    {
        /// <summary>
        /// 分页查询数据 后台
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="areaId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="projectType"></param>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<CRData> QueryPage(string hospitalName, long hospitalId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 单个获取原始文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        string DownloadOriginalData(long id);

        /// <summary>
        /// 单个获取容错文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        string DownloadNewData(long id);

        /// <summary>
        /// 后台打包下载文件
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="downloadEnum"></param>
        /// <returns></returns>
        string Download(IEnumerable<long> ids, CRDownloadEnum downloadEnum);

        /// <summary>
        /// 会员中心，CR复敏信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        IQueryable<CRData> Query(long memberId);

        /// <summary>
        ///  上传数据 提交
        /// </summary>
        /// <param name="id">CR 的Id</param>
        /// <param name="filePath">上传成功的文件，相对路径</param>
        /// <param name="oldFileName">原始文件名称</param>
        /// <param name="member">当前登录用户</param>
        /// <returns>SUCCESS 表示成功，其他则是错误信息</returns>
        string Upload(long id, string Year, string Quarter, string CREDetectionRate, string CREDrugRate,string filePath, string oldFileName, Member member);

        /// <summary>
        /// 获取指定用户最后的一次提交数据
        /// </summary>
        /// <param name="memberId">所属用户Id</param>
        /// <returns></returns>
        CRData QueryEntityByMember(long memberId);

        /// <summary>
        /// 修改CR数据
        /// </summary>
        /// <param name="entity">CR数据</param>
        /// <param name="member">当前登录用户</param>
        /// <returns>SUCCESS 表示成功，其他则是错误信息</returns>
        string Update(CRData entity, Member member);

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ids">需要导出的项目id集合，为空则导出全部</param>
        /// <param name="ep">excel导出组建</param>
        /// <returns></returns>
        string Export(List<long> ids, ExcelPackage ep);

        /// <summary>
        /// 热图查询
        /// </summary>
        /// <returns></returns>
        List<CRviews> GetCRslist(string year);

        /// <summary>
        /// 热图市区查询
        /// </summary>
        /// <returns></returns>
        List<CRviews> GetCitylist(string year);
        /// <summary>
        /// 查询年份
        /// </summary>
        /// <returns></returns>
        List<CRData> GetyearList();
        /// <summary>
        /// 查询默认显示的年份
        /// </summary>
        /// <returns></returns>
        string GetYear();

        /// <summary>
        /// 根据年份查询热图数据
        /// </summary>
        /// <returns></returns>
        List<CRData> GetDatalist(long Id,string year);
    }
}
