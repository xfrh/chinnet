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
    /// 操作接口类 ，数据库表名：SystemLog 
    /// </summary>
    public partial interface ISystemLogService : IBaseService<SystemLog>
    {

        void Insert(Exception exception, SystemLogLevel level, string url = "");

        void Insert(string title, string message, SystemLogLevel level,string url = "", string className = "");

        IPagedList<SystemLog> QueryPage(string title, int levelId, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
