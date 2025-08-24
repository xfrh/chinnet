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
using ManageSystem.Services.Medicine.Model;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MedicalAntibioticResult
    /// </summary>
    public partial interface IMedicalAntibioticResultService : IBaseService<MedicalAntibioticResult>
    {
        /// <summary>
        /// 根据数据获取抗生素对应的检测结果，敏感、中介、耐药的值。服务自动处理预计是5分钟运行一次
        /// </summary>
        /// <param name="member">当前登录用户</param>
        /// <param name="medicalId">医学数据的id，如果传递了则获取指定的数据，没有传递则系统查询所有数据</param>
        string AutoDispose(Member member, long medicalId = 0);

        /// <summary>
        /// 获取耐药性的耐药性数据
        /// </summary>
        /// <param name="medicalId">上传的id</param>
        /// <param name="organismId">指定的细菌id，如果不指定则查询本次上传所有的细菌</param>
        /// <param name="excludeSpecType">需要排除的标本类型，多个采用英文逗号分隔</param>
        /// <param name="otherSqlWhere">其他的sql查询条件，查询的表示：MedicalAntibioticResult，格式：AND ID > 0 </param>
        /// <returns></returns>
        List<GetAntibioticResultModel> GetResult(long medicalId, long organismId = 0, string excludeSpecType = "", string otherSqlWhere = "");
        /// <summary>
        /// 获取耐药性的耐药性数据
        /// </summary>
        /// <param name="medicalId">上传的id</param>
        /// <param name="organismId">指定的细菌id，如果不指定则查询本次上传所有的细菌</param>
        /// <param name="excludeSpecType">需要排除的标本类型，多个采用英文逗号分隔</param>
        /// <param name="otherSqlWhere">其他的sql查询条件，查询的表示：MedicalAntibioticResult，格式：AND ID > 0 </param>
        /// <returns></returns>
        List<GetAntibioticResultModel> GetResult(long medicalId, List<long> organismIds, string excludeSpecType = "", string otherSqlWhere = "");

        /// <summary>
        /// 根据已有的耐药性结果数据，进行耐药性分析
        /// </summary>
        /// <param name="list">耐药性结果的集合数据</param>
        /// <returns></returns>
        List<GetAntibioticResultModel> GetResult(List<MedicalAntibioticResult> list);

        /// <summary>
        /// 根据已有的耐药性结果数据，进行耐药性分析
        /// </summary>
        /// <param name="list">耐药性结果的集合数据</param>
        /// <returns></returns>
        List<GetAntibioticResultModel> GetResultByOrganismIds(List<MedicalAntibioticResult> list, List<long> organismIds);


        /// <summary>
        /// 获取MRSA	MSSA	MRCNS	MSCNS 等的统计数据，专用函数
        /// </summary>
        /// <param name="medicalId">上传id</param>
        /// <param name="organismGroupName">菌属分组名称，区分 MRSA	MSSA	MRCNS	MSCNS </param>
        /// <returns></returns>
        List<MedicalAntibioticResult> QueryMRSAList(long medicalId, string organismGroupName);

    }
}
