using ManageSystem.Core;
using ManageSystem.Core.Domain.CRs;
using ManageSystem.Core.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.CRs
{
    public partial interface ICRLogService : IBaseService<CRLog>
    {
        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="content"></param>
        void Insert(ActionType type, ActionSource source, long projectId, long userId, string userName, string content = "", string detail = "");


        IPagedList<CRLog> QueryPage(string content, int typeId, int source, int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<CRLog> QueryProjectPage(long projectId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 查询指定类型的操作日志
        /// </summary>
        /// <param name="type">日志操作类型</param>
        /// <param name="count">查询总行数</param>
        /// <param name="orderby">id排序方式，0表示id倒叙，1表示id正序</param>
        /// <returns></returns>
        List<CRLog> QueryActionByType(ActionType type, int count = 0, int orderby = 0);


        /// <summary>
        /// 根据项目Id查询日志记录
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        List<CRLog> QueryByProject(long projectId);
    }
}
