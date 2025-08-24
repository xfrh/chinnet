using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core;
using ManageSystem.Core.Domain.CRProjects;
using ManageSystem.Core.Domain.Log;

namespace ManageSystem.Services.CRProjects
{
    /// <summary>
    /// 操作接口类 ，数据库表名：CRProjectLog 
    /// </summary>
    public partial interface ICRProjectLogService : IBaseService<CRProjectLog>
	{

        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="content"></param>
        void Insert(ActionType type, ActionSource  source, long projectId, long userId,string userName, string content = "",string detail="");


        IPagedList<CRProjectLog> QueryPage(string content, int typeId, int source, int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<CRProjectLog> QueryProjectPage(long projectId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 查询指定类型的操作日志
        /// </summary>
        /// <param name="type">日志操作类型</param>
        /// <param name="count">查询总行数</param>
        /// <param name="orderby">id排序方式，0表示id倒叙，1表示id正序</param>
        /// <returns></returns>
        List<CRProjectLog> QueryActionByType(ActionType type, int count=0, int orderby = 0);


        /// <summary>
        /// 根据项目Id查询日志记录
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        List<CRProjectLog> QueryByProject(long projectId);
        

    }
}
