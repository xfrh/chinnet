using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Messages;

namespace ManageSystem.Services.Messages
{
    /// <summary>
    /// 操作接口类 ，数据库表名：MessageEmail 
    /// </summary>
    public partial interface IMessageEmailService : IBaseService<MessageEmail>
	{
    
    }
}
