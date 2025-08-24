using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Meetings;

namespace ManageSystem.Services.Meetings
{
	/// <summary>
	/// 操作接口类 ，数据库表名：MeetingType 
	/// </summary>
	public  partial interface IMeetingTypeService : IBaseService<MeetingType>
	{

        /// <summary>
        /// 根据id获取类型名称
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        string GetTypeName(long typeId);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<MeetingType> QueryPage(string name ,int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<MeetingType> QueryPageSa(string name, string sateid, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
