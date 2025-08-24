using ManageSystem.Core;
using ManageSystem.Core.Domain;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Sate;
using System.Collections.Generic;

namespace ManageSystem.Services.Satellites
{
    public partial interface ISatelliteUserService: IBaseService<SatelliteUser>
    {
        /// <summary>
        /// 卫星网UserList列表查询
        /// </summary>
        /// <param name="SatelliteId"></param>
        /// <returns></returns>
        IPagedList<SatelliteUser> QueryPage(long SatelliteId,string UserName,int pageIndex, int pageSize);

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SatelliteUser QueryEntity(string id);

        SatelliteUser QueryEntityByUserName(string userName);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="SatelliteId"></param>
        /// <param name="UserName"></param>
        /// <param name="Id"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        SatelliteUser insert(long SatelliteId, string UserName, string Id, string PassWord, Member member);

   
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="SatelliteId"></param>
        /// <param name="UserName"></param>
        /// <param name="Id"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        void update(long SatelliteId, string UserName, string Id, string PassWord);
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="SatelliteId"></param>
        /// <param name="Status"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        void updateStatus(long SatelliteId, string Status, string Id);
       
    }
}
