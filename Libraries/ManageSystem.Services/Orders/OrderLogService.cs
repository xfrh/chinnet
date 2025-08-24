using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Orders;

namespace ManageSystem.Services.Orders
{
	/// <summary>
	/// 操作类 ，数据库表名：OrderLog 
	/// </summary>
	public partial class OrderLogService :  BaseService<OrderLog>, IOrderLogService
	{

		public OrderLogService(IRepository<OrderLog> repository): base(repository)
		{
			
		}

	}
}
