using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine
{
    public partial interface IHospitalWardLocationService : IBaseService<HospitalWardLocation>
    {

        /// <summary>
        /// 重新更新指定的上传文件科室配置信息
        /// </summary>
        /// <param name="medicalId">所属上传id</param>
        /// <returns></returns>
        string UpdateItem(long medicalId);

        IPagedList<dynamic> QueryPage(string name, int page, int pageSize);


        /// <summary>
        /// 会员中心，医学信息管理的分页数据
        /// </summary>
        /// <param name="HospitalId"></param>      
        /// <returns></returns>
        IQueryable<HospitalWardLocation> Query(long HospitalId);

        /// <summary>
        /// 根据id查询科室
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        HospitalWardLocation QueryEntity(long? Id);

        /// <summary>
        /// 根据条件查询科室
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        HospitalWardLocation QueryWard(long? Id,string ward);

        /// <summary>
        /// 修改科室
        /// </summary>
        /// <param name="hospital"></param>
        void UpdateDepartment(HospitalWardLocation hospital);

        void DeleDepartment(long hospitalId);

    }
}
