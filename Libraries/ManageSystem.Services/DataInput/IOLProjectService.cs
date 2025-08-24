using ManageSystem.Core;
using ManageSystem.Core.Domain.DataInput;
using ManageSystem.Core.Domain.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial interface IOLProjectService /*: IBaseService<OLProject>*/
    {
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <returns></returns>
        int AddOLProject(OLProject model);

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int delete(long Ids);
        
        /// <summary>
        /// 管理员的项目列表
        /// </summary>
        /// <returns></returns>
        List<OLProject> GetOLProjects(long projecId);

        /// <summary>
        /// 成员的项目列表
        /// </summary>
        /// <returns></returns>
        List<OLProject> GetLProjects(long memberId,long projecId);

        /// <summary>
        /// 项目修改返填
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        List<OLProject> UPOLProjects(long Id);
        /// <summary>
        /// 修项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int updateOLProject(OLProject model);
     
       
        /// <summary>
        /// 给项目绑定用户
        /// </summary>
        int CreateProjectMember(OLUserproject model);

        /// <summary>
        /// 查询项目是否有数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        List<OLDataInput> GetOLDataInput(long Id);

        /// <summary>
        /// 修改用户表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int UpdateMember(long Id,string privil);
        /// <summary>
        /// 查询用户绑定项目数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int CountProjectMember(long Id);

        /// <summary>
        /// 删除项目绑定用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int deleteProjectMember(long Id,long userId);

        /// <summary>
        /// 每个项目数据已审核的数量
        /// </summary>
        /// <returns></returns>
        List<OLProject> Projectauditor(long projectid,long createby_Id);

        /// <summary>
        /// 每个项目数据的数量
        /// </summary>
        /// <returns></returns>

        List<OLProject> Project();

        /// <summary>
        /// 查询项目是否已绑定模板
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        OLDataInput GetProjecttemData(long? projectid);
        /// <summary>
        /// 查询项目是否已绑定模板
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        OLProject GetProjecttem(long? projectid);

        /// <summary>
        /// 查询病原菌
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        List<OLDataInput> Getgermname(long projectid, long createdId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempId"></param>
        /// <returns></returns>
        OLProject GetProjecttemp(long? tempId);

    }
}
