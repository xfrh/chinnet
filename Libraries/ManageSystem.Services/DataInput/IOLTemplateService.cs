using ManageSystem.Core;
using ManageSystem.Core.Domain.DataInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial interface IOLTemplateService : IBaseService<OLTemplate>
    {
        /// <summary>
        /// 添加字段模板表
        /// </summary>
        /// <returns></returns>
        int AddTemplate(OLTemplate model);

        /// <summary>
        /// 删除字段模板表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int delete(long Id);
        
        /// <summary>
        /// 修改字段模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int updateOLTemplate(OLTemplate model);

        /// <summary>
        /// 查询字段模板是否绑定项目
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        int GetLProjects(long templateId);

        /// <summary>
        /// 字段模板列表
        /// </summary>
        /// <param name="createdbyId"></param>
        /// <returns></returns>
        List<OLTemplate> Querylist();
        /// <summary>
        /// 项目添加页面字段模板下拉列表
        /// </summary>
        /// <returns></returns>
        List<OLTemplate> GetLTemplatelist();
        /// <summary>
        /// 查询字段模板数据
        /// </summary>
        /// <returns></returns>
        OLTemplate GetLTemplate(long Id);


    }
}
