using ManageSystem.Core;
using ManageSystem.Core.Domain.DataInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial interface IOLFieldmodelService /*: IBaseService<OLFieldmodel>*/
    {
        /// <summary>
        /// 添加字段模板表
        /// </summary>
        /// <returns></returns>
        int AddFieldmodel(OLFieldmodel model);

        /// <summary>
        /// 删除字段模板表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int delete(long Id);
        /// <summary>
        /// 字段模板返填
        /// </summary>
        /// <returns></returns>
        List<OLFieldmodel> GetLFieldmodels(long Id);

        /// <summary>
        /// 修改字段模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int updateOLTemplate(OLFieldmodel model);

        /// <summary>
        /// 配置数据添加页面字段
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        List<OLFieldmodel> GetLFieldmodellist(long projectid);
        /// <summary>
        /// 根据条件查询模板
        /// </summary>
        /// <returns></returns>
        OLFieldmodel GetLFieldmodel(long templateId, string fieldName);

        /// <summary>
        /// 查找模板code
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        OLFieldmodel GetLFieldfield_code(long templateId);
    }
}
