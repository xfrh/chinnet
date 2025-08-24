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
	/// 操作类 ，数据库表名：OrderItem 
	/// </summary>
	public partial class OrderItemService :  BaseService<OrderItem>, IOrderItemService
	{

		public OrderItemService(IRepository<OrderItem> repository): base(repository)
		{
			
		}

	}
}
