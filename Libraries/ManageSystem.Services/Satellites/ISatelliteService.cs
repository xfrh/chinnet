using ManageSystem.Core;
using ManageSystem.Core.Domain;
using ManageSystem.Core.Domain.Sate;
using System.Collections.Generic;

namespace ManageSystem.Services.Satellites
{
    public partial interface ISatelliteService: IBaseService<Satellite>
    {
        /// <summary>
        /// 卫星网列表查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Satellite> QueryPage(string name, string phone, int pageIndex, int pageSize);

        IPagedList<Satellite> QueryPageSa(string name, long said, string phone, int pageIndex, int pageSize);

        /// <summary>
        /// 根据域名判断相应卫星网是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsExist(string id);

        /// <summary>
        /// 获取已经审批通过的卫星网域名
        /// </summary>
        /// <returns></returns>
        IList<Satellite> GetLoginList();

        /// <summary>
        /// 查询用户是否有提交卫星网申请记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int QueryApplyUser(string userId);

        int CreateMeetingType(string sateName);

        int CreateMedicalProjectType(string sateName);
    }
}
