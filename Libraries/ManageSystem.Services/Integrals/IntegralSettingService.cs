using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Integrals;
using ManageSystem.Core;

namespace ManageSystem.Services.Integrals
{
	/// <summary>
	/// 操作类 ，数据库表名：IntegralSetting 
	/// </summary>
	public partial class IntegralSettingService :  BaseService<IntegralSetting>, IIntegralSettingService
	{

		public IntegralSettingService(IRepository<IntegralSetting> repository): base(repository)
		{
			
		}

        public IPagedList<IntegralSetting> QueryPage(string name, long type, string status, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (type > 0)
                query = query.Where(m => m.Type == type);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(m => m.Status == status.Equals("1"));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<IntegralSetting>(query, pageIndex, pageSize);
        }
    }
}
