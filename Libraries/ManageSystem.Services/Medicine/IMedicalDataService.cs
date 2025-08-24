using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MedicalData 
    /// </summary>
    public partial interface IMedicalDataService : IBaseService<MedicalData>
    {
        /// <summary>
        ///插入数据库  使用服务完成数据分析版本
        /// </summary>
        /// <param name="entity"></param>
        void Insert2(UploadMedicalResult uploadModel);
        /// <summary>
        /// 分页查询数据   前台医学数据首页
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="specimenId"></param>
        /// <param name="bacteriaTypeId"></param>
        /// <param name="departmentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MedicalData> Query(long areaId, long hospitalId, long specimenId, long bacteriaTypeId, long departmentId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 用户中心，项目数据管理
        /// </summary>
        /// <param name="projectItems"></param>
        /// <param name="hospital"></param>
        /// <param name="fileName"></param>
        /// <param name="projectItem"></param>
        /// <param name="year"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IQueryable<MedicalData> Query(List<long> projectItems, string hospital, string fileName, long projectItem, int year, int status);

        MedicalData Query(long id);

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
        IPagedList<MedicalData> QueryPage(string hospitalName, long areaId, long hospitalId, long projectType, int year, int quarter, int pageIndex = 0, int pageSize = int.MaxValue, string satelliteId = "");

        /// <summary>
        /// 会员中心，医学信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IQueryable<MedicalData> Query(long memberId, int year, int quarter, string fileName);

        /// <summary>
        /// Peng测试使用-用于原系统在容错处理失败的情况进行容错处理
        /// </summary>
        /// <param name="medicalId"></param>
        /// <returns></returns>
        string OneAutoDispose(long medicalId = 0);

        /// <summary>
        /// 自动化作业处理数据  
        /// </summary>
        /// <param name="medicalId">医学数据Id，没传递则操作所有</param>
        /// <returns></returns>
        string AutoDispose(long medicalId = 0);

        /// <summary>
        /// 根据Id检查上传数据的状态
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns>true 表示成功  false 表示失败</returns>
        bool CheckStatus(long id);

        /// <summary>
        /// 后台打包下载文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="downloadEnum"></param>
        /// <returns></returns>
        string Download(IEnumerable<long> ids, MedicalDataDownloadEnum downloadEnum);

        /// <summary>
        /// 后台打包下载文件
        /// </summary>
        /// <param name="sqlToIds"></param>
        /// <param name="downloadEnum"></param>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        string Download(String sqlToIds, MedicalDataDownloadEnum downloadEnum, object sqlParam = null);
        /// <summary>
        /// 后台打包下载文件
        /// </summary>
        /// <param name="downloadEnum"></param>
        /// <param name="projectTypes"></param>
        /// <returns></returns>
        string DownloadByProjectType(List<long> projectTypes, MedicalDataDownloadEnum downloadEnum);

        /// <summary>
        /// 单个获取原始文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        string DownloadOriginalData(long id);

        /// <summary>
        /// 生成容错文件，给接口使用的。DownloadNewData 函数则是给前台页面使用的
        /// </summary>
        /// <param name="medicalId">医学数据的id，如果传递了则获取指定的数据，没有传递则系统查询所有数据</param>
        /// <returns></returns>
        string CreateNewFile(long medicalId = 0);

        /// <summary>
        /// 单个获取容错文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        string DownloadNewData(long id);

        /// <summary>
        /// 根据数据上传的年份和季度，获取完整的显示名称
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        string GetDataQuarter(int year, int quarter);

        /// <summary>
        /// 根据项目id获取对应的项目名称
        /// </summary>
        /// <param name="project">项目id</param>
        /// <returns></returns>
        string GetProejctName(long project);

        /// <summary>
        /// 刷新之前老的文件名称
        /// 修改原始文件的路径，原始文件名称_年月日_医院名称.后缀
        /// </summary>
        /// <returns></returns>
        string UpdateDisposeFilePath3();

        /// <summary>
        /// 刷新之前老的文件名称
        /// 修改原始文件的路径，原始文件名称_年月日_医院名称.后缀
        /// </summary>
        /// <returns></returns>
        string UpdateDisposeFilePath4(int type);



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
        IPagedList<MedicalData> QueryPage(long projectType, int year, int quarter, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
