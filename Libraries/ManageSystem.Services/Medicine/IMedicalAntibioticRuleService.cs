using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MedicalAntibioticRule 
    /// </summary>
    public partial interface IMedicalAntibioticRuleService : IBaseService<MedicalAntibioticRule>
    {

        /// <summary>
        /// 根据传入的数据验证规则
        /// </summary>
        /// <param name="uploadModel"></param>
        void ValidateUploadData(UploadMedicalResult uploadModel);
        /// <summary>
        /// 根据细菌编码和字段的名称获取一个唯一的规则
        /// </summary>
        /// <param name="organismCode">细菌的编码</param>
        /// <param name="cod">字段的名称</param>
        ///  <param name="list">验证规则的列表，如果没有则查询数据库</param>
        /// <returns></returns>
        MedicalAntibioticRule QueryEntity(string organismCode, string code, List<MedicalAntibioticRule> list = null);

    }
}
