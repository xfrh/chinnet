using ManageSystem.Core.Domain.DataInput;
using ManageSystem.Core.Domain.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial interface IOLMemberService : IBaseService<MemberHospital>
    {
        /// <summary>
        /// 项目用户
        /// </summary>
        /// <param name="HospitalId"></param>      
        /// <returns></returns>
        List<MemberHospital> Querylist(string LoginId, string phone, string hospital);
        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="LoginId"></param>
        /// <param name="phone"></param>
        /// <param name="hospital"></param>
        /// <returns></returns>

        List<MemberHospital> Queryuserlist1(long? projectid);

        List<MemberHospital> Queryuserlist2(long? projectid);

        /// <summary>
        /// 查询下项目用户关联表
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetOLuserproject(long? projectid,long userId);
    }
}
