using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Services.Members
{
	/// <summary>
	/// 操作接口类 ，数据库表名：MemberAttestation 
	/// </summary>
	public  partial interface IMemberAttestationService : IBaseService<MemberAttestation>
	{

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="state"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MemberAttestation> QueryPage(string memberName,  int state, int pageIndex = 0, int pageSize = int.MaxValue);



        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool FinishCheck(long id, Account account);
        


    }
}
