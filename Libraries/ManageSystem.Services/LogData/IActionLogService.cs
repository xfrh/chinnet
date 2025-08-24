using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core;

namespace ManageSystem.Services.Log
{
	/// <summary>
	/// 操作接口类 ，数据库表名：ActionLog 
	/// </summary>
	public  partial interface IActionLogService : IBaseService<ActionLog>
	{

        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="content"></param>
        void Insert(ActionType type, ActionSource  source , long userId,string userName, string content = "",string detail="");


        IPagedList<ActionLog> QueryPage(string content, int typeId, int source, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 查询指定类型的操作日志
        /// </summary>
        /// <param name="type">日志操作类型</param>
        /// <param name="count">查询总行数</param>
        /// <param name="orderby">排序方式，0表示日期降序，1表示日期升序</param>
        /// <returns></returns>
        List<ActionLog> QueryActionByType(ActionType type, int count=0, int orderby = 0);
        

    }
}
