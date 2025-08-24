using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core;
using System.Linq.Expressions;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.Log
{
	/// <summary>
	/// 操作类 ，数据库表名：SystemLog 
	/// </summary>
	public partial class SystemLogService :  BaseService<SystemLog>, ISystemLogService
	{

		public SystemLogService(IRepository<SystemLog> repository): base(repository)
		{
			
		}
        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="level"></param>
        /// <param name="url"></param>
        public void Insert(Exception exception, SystemLogLevel level,string url="")
        {
            SystemLog log = new SystemLog();
            log.LevelId = (int)level;
            log.Logger = exception.Source;
            log.Message = exception.ToString();
            log.Title = exception.Message;
            log.IPAddress = HttpHelper.GetIp();
            log.Url = url;

            this.Insert(log);
        }

        /// <summary>
        /// 插入系统日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="url"></param>
        public void Insert(string title, string message, SystemLogLevel level, string url = "",string className="")
        {
            SystemLog log = new SystemLog();
            log.LevelId = (int)level;
            log.Logger = className;
            log.Message = message;
            log.Title = title;
            log.Url = url;
            log.IPAddress = HttpHelper.GetIp();

            this.Insert(log);
        }

        public IPagedList<SystemLog> QueryPage(string title, int levelId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table;
            if (!string.IsNullOrEmpty(title)) query = query.Where(m => m.Title.Contains(title));

            if (levelId > 0) query = query.Where(m => m.LevelId == levelId);

            query = query.OrderByDescending(b => b.InsertTime);

            return new PagedList<SystemLog>(query, pageIndex, pageSize);
        }
    }
}
