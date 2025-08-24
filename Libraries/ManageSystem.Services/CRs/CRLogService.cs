using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.CRs;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace ManageSystem.Services.CRs
{
    public partial class CRLogService : BaseService<CRLog>, ICRLogService
    {
        private readonly HttpContextBase httpContext;
        private readonly IAuthenticationService iauthenticationService;

        public CRLogService(IRepository<CRLog> actionLogRepository,
            HttpContextBase _httpContext,
             IAuthenticationService _iauthenticationService) :
            base(actionLogRepository)
        {
            this.httpContext = _httpContext;
            this.iauthenticationService = _iauthenticationService;
        }
        public void Insert(ActionType type, ActionSource source, long projectId, long userId, string userName, string content = "", string detail = "")
        {
            CRLog log = new CRLog();
            log.Id =CommonHelper.GuidToLongID;
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
            log.InsertTime = DateTime.Now;

            this.Insert(log);
            //using (SqlConnection conn = DapperHelper.GetConnection())
            //{
            //    string sql = string.Format("insert into CRLog values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')", log.Id,log.BrowserName,log.IPAddress, log.UserinfoId, log.UserinfoName, log.Content, log.Detail, log.Type, log.Source, log.InsertTime,log.UpdateTime,log.DeleteTime,log.Version,log.Mark,log.Describe,log.ProjectId);
            //    int result = conn.Execute(sql);
            //}
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
        /// <summary>
        /// 查询指定类型的操作日志
        /// </summary>
        /// <param name="type">日志操作类型</param>
        /// <param name="count">查询总行数</param>
        /// <param name="orderby">id排序方式，0表示id升序，1表示id降序</param>
        /// <returns></returns>
        public List<CRLog> QueryActionByType(ActionType type, int count = 0, int orderby = 0)
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
        public List<CRLog> QueryByProject(long projectId)
        {
            return this.Query(m => m.ProjectId == projectId).OrderByDescending(m => m.InsertTime).ToList();
        }

        public IPagedList<CRLog> QueryPage(string content, int typeId, int source, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table;
            if (!string.IsNullOrEmpty(content)) query = query.Where(m => m.Content.Contains(content));

            if (typeId > 0)
            {
                string typeName = this.GetActionType((ActionType)typeId);
                query = query.Where(m => m.Type.Equals(typeName));
            }

            if (source > 0)
                query = query.Where(m => m.Source == source);

            query = query.OrderByDescending(b => b.InsertTime);

            return new PagedList<CRLog>(query, pageIndex, pageSize);
        }

        public IPagedList<CRLog> QueryProjectPage(long projectId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table;


            if (projectId > 0)
                query = query.Where(m => m.ProjectId == projectId);


            query = query.OrderByDescending(b => b.InsertTime);

            return new PagedList<CRLog>(query, pageIndex, pageSize);
        }
    }
}
