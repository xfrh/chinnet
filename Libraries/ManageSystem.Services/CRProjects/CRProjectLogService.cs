using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using System.Web;
using ManageSystem.Services.Authentication;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Domain.CRProjects;

namespace ManageSystem.Services.CRProjects
{
    /// <summary>
    /// 操作类 ，数据库表名：CRProjectLog 
    /// </summary>
    public partial class CRProjectLogService :  BaseService<CRProjectLog>, ICRProjectLogService
    {
        private readonly HttpContextBase httpContext;
        private readonly IAuthenticationService iauthenticationService;

        public CRProjectLogService(IRepository<CRProjectLog> actionLogRepository,
            HttpContextBase _httpContext,
             IAuthenticationService _iauthenticationService) :
            base(actionLogRepository)
        {
            this.httpContext = _httpContext;
            this.iauthenticationService = _iauthenticationService;
        }


        public void Insert(ActionType type, ActionSource source,long projectId, long userId, string userName, string content = "",string detail="")
        {
            CRProjectLog log = new CRProjectLog();
            log.ProjectId = projectId;
            log.BrowserName = this.httpContext.Request.Browser.Browser;
            log.Content = content;
            log.IPAddress = HttpHelper.GetIp();
            log.UserinfoId = userId;
            log.UserinfoName = userName;
            log.Content = content;
            log.Detail = detail;
            log.Type = this.GetActionType(type);
            log.Source = (int)source;

            this.Insert(log);
        }

        public string GetActionType(ActionType type)
        {
            switch (type)
            {
                case ActionType.Login:
                    return "登录";
                case ActionType.Create:
                    return "添加数据";
                case ActionType.Edit:
                    return "修改数据";
                case ActionType.View:
                    return "查看数据";
                case ActionType.Delete:
                    return "删除数据";
                case ActionType.Export:
                    return "导出数据";
                case ActionType.Import:
                    return "导入数据";
                default:
                    return "";
            }
        }

        public IPagedList<CRProjectLog> QueryPage(string content, int typeId, int source, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table;
            if (!string.IsNullOrEmpty(content)) query = query.Where(m => m.Content.Contains(content));

            if (typeId > 0)
            {
                string typeName = this.GetActionType((ActionType)typeId);
                query = query.Where(m => m.Type.Equals(typeName));
            } 

            if(source >0)
                query = query.Where(m => m.Source == source);

            query = query.OrderByDescending(b => b.InsertTime);

            return new PagedList<CRProjectLog>(query, pageIndex, pageSize);
        }

        //
        public IPagedList<CRProjectLog> QueryProjectPage(long projectId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table;
          

            if (projectId > 0)
                query = query.Where(m => m.ProjectId == projectId);


            query = query.OrderByDescending(b => b.InsertTime);

            return new PagedList<CRProjectLog>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 查询指定类型的操作日志
        /// </summary>
        /// <param name="type">日志操作类型</param>
        /// <param name="count">查询总行数</param>
        /// <param name="orderby">id排序方式，0表示id升序，1表示id降序</param>
        /// <returns></returns>
        public List<CRProjectLog> QueryActionByType(ActionType type, int count = 0, int orderby = 0)
        {
            var query = base._repository.Table;
    
            string typeName = this.GetActionType(type);
            query = query.Where(m => m.Type.Equals(typeName));

            query = orderby > 0 ? query.OrderBy(b => b.Id) : query.OrderByDescending(b => b.Id);
            if (count > 0) query = query.Take(count);

            return query.ToList();
        }

        /// <summary>
        /// 根据项目Id查询日志记录
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public List<CRProjectLog> QueryByProject(long projectId)
        {
            return this.Query(m => m.ProjectId == projectId).OrderByDescending(m => m.InsertTime).ToList();
        }



    }
}
